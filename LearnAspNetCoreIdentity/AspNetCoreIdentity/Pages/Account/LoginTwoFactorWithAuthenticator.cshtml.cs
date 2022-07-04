using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentity.Pages.Account
{
    public class LoginTwoFactorWithAuthenticatorModel : PageModel
    {
        private readonly SignInManager<User> signInManager;

        public LoginTwoFactorWithAuthenticatorModel(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty]
        public LoginTwoFactorViewModel Vm { get; set; } = new LoginTwoFactorViewModel();

        public void OnGet(bool rememberMe)
        {
            Vm.VerifyForm.SecurityCode = "";
            Vm.VerifyForm.RememberMe = rememberMe;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // You can see here noway that the app can verify the security code come from which user.
                // So a cookie called Identity.TwoFactorUserId is saved so that the app can verify the next factor
                var result = await signInManager.TwoFactorAuthenticatorSignInAsync(
                    code: Vm.VerifyForm.SecurityCode,
                    isPersistent: Vm.VerifyForm.RememberMe,
                    rememberClient: false); // Indicate the browser is remembered and do not ask for two factor next time login

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("Authenticator2FA", "You are locked out.");
                    }
                    else
                    {
                        ModelState.AddModelError("Authenticator2FA", "Failed to login.");
                    }
                }
            }

            return Page();
        }
    }

    public class LoginTwoFactorWithAuthenticatorViewModel
    {
        public string? FallbackSendEmailDisplaySecurityCode { get; set; }

        public VerifyFormViewModel VerifyForm { get; set; } = new VerifyFormViewModel();

        public class VerifyFormViewModel
        {
            [Required]
            [Display(Name = "Security Code")]
            public string SecurityCode { get; set; } = "";

            public bool RememberMe { get; set; }
        }
    }
}
