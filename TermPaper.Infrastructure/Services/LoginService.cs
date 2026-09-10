using Microsoft.AspNetCore.Identity;
using Resend;

namespace TermPaper.Infrastructure.Services;

public class LoginService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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