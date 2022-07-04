using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentity.Entities;
using AspNetCoreIdentity.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;

namespace AspNetCoreIdentity.Pages.Account
{
    [Authorize]
    public class AuthenticatorWithMFASetupModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public SetupMFAViewModel Vm { get; set; } = new SetupMFAViewModel();

        public AuthenticatorWithMFASetupModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserEnsureNotNullAsync(User);

            // Re-Generate the MFA authentication token and also save it into the database, so that it can get and generate code again to verify. If the key is existed, get again
            // will return the existing key. So that if you want it to be changed because you lost your phone or something, you could call reset the token before get again
            if (await userManager.GetTwoFactorEnabledAsync(user))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                Vm.IsPrevAuthenticatorKeyHasBeenReset = true;
            }
            var key = await userManager.GetAuthenticatorKeyAsync(user);

            Vm.Key = key;
            Vm.QRCodeBytes = GenerateQRCodeBytes(provider: "My Web App", key, user.Email);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserEnsureNotNullAsync(User);

            await DoVerify();
            Vm.QRCodeBytes = GenerateQRCodeBytes(provider: "My Web App", Vm.Key, user.Email);

            return Page();

            async Task DoVerify()
            {
                if (ModelState.IsValid)
                {
                    if (await userManager.VerifyTwoFactorTokenAsync(
                            user,
                            userManager.Options.Tokens.AuthenticatorTokenProvider,
                            Vm.VerifyForm.SecurityCode))
                    {
                        await userManager.SetTwoFactorEnabledAsync(user, true);

                        Vm.VerifySucceeded = true;
                    }
                    else
                    {
                        ModelState.AddModelError("AuthenticatorSetup", "Something went wrong with authenticator setup");
                    }
                }
            }
        }

        /// <summary>
        /// GenerateQRCodeBytes
        /// </summary>
        /// <param name="provider">Our application name. It could be anything name, will be used as display name in the authenticator app. EX format: [Provider] (userEmail)</param>
        /// <param name="key"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        private Byte[] GenerateQRCodeBytes(string provider, string key, string userEmail)
        {
            var qrCoderGenerator = new QRCodeGenerator();

            //topt: time-based one time password
            var qrCodeData = qrCoderGenerator.CreateQrCode(
                plainText: $"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}",
                eccLevel: QRCodeGenerator.ECCLevel.Q);

            var qrCode = new QRCoder.BitmapByteQRCode(qrCodeData);

            var qrCodeAsBitmapByteArr = qrCode.GetGraphic(pixelsPerModule: 20);

            return qrCodeAsBitmapByteArr;
        }
    }

    public class SetupMFAViewModel
    {
        public string Key { get; set; } = "";

        public LoginTwoFactorViewModel.VerifyFormViewModel VerifyForm { get; set; } = new LoginTwoFactorViewModel.VerifyFormViewModel();

        public bool VerifySucceeded { get; set; }

        public Byte[] QRCodeBytes { get; set; } = new byte[] { };
        public bool IsPrevAuthenticatorKeyHasBeenReset { get; set; }

        public class VerifyFormViewModel
        {
            [Required]
            [Display(Name = "Security Code")]
            public string SecurityCode { get; set; } = "";
        }
    }
}
