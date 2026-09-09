using Microsoft.AspNetCore.Identity;
using Resend;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Resend;
namespace TermPaper.Services;

public class AutorisationsServices
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IResend _resend;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AutorisationsServices(UserManager<IdentityUser> userManager, IResend resend, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _resend = resend;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterUserAsync( string email, string password, string confirmPassword,string userName)
    {
        var existingUser  = await _userManager.FindByEmailAsync(email);
        if ( existingUser != null){ return  IdentityResult.Failed(); }
        if (password != confirmPassword){return IdentityResult.Failed(); }
        var user = new IdentityUser
        {
            UserName = userName,
            Email = email,
        };
        var result = await _userManager.CreateAsync(user,password);
        if (!result.Succeeded) {return IdentityResult.Failed();}

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var send = await SendEmailAsync(user, token);
        if(!send.Succeeded){ return IdentityResult.Failed(); }  
        return IdentityResult.Success;
    }

    private async Task<IdentityResult> SendEmailAsync(IdentityUser user,string token)
    {
        if (await _userManager.FindByEmailAsync(user.Email) == null)
        {return IdentityResult.Failed();}
        var message = new EmailMessage();
        message.From = "onboarding@resend.dev";
        message.To.Add(user.Email);
        message.Subject = "Confirmation email";
        message.HtmlBody = $"""
                            <h1>Confirmation email</h1>
                            <p>Click to confirm your email; if you received this email by mistake, please ignore it.
                            </p>
                            <a href="https://online-recruiter/confirm-email?email={user.Email}&token={token}">Confirm email</a> 
                            """;
        await _resend.EmailSendAsync(message);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return IdentityResult.Failed(); }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded){ return IdentityResult.Failed(); }

        return IdentityResult.Success;
    }
    public async Task<IdentityResult> LoginUsersAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return IdentityResult.Failed(); }
        var result = await _signInManager.PasswordSignInAsync(user,password, false, false);
        if (!result.Succeeded){ return IdentityResult.Failed(); }
                if (result.IsNotAllowed)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Not allowed to confirm your email"
            });
        }
        if (!result.Succeeded)
        {
            return IdentityResult.Failed();
        }

        return IdentityResult.Success;
    }
    
    
    
}