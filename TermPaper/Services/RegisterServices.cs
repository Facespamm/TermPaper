using Microsoft.AspNetCore.Identity;

namespace TermPaper.Services;

public class RegisterServices
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public RegisterServices(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    private async Task<IdentityResult> RegisterUserAsync( string email, string password, string confirmPassword,string userName)
    {
        var existingUser  = await _userManager.FindByEmailAsync(email);
        if ( existingUser != null){ return  IdentityResult.Failed(); }
        if (password != confirmPassword){return IdentityResult.Failed(); }
        var user = new IdentityUser
        {
        UserName = userName,
        Email = email,
        };
     return await _userManager.CreateAsync(user,password);
    }
    
}  