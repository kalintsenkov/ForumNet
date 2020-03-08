namespace ForumNet.Web.Areas.Identity.Pages.Account
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Data.Models;

    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ForumUser> signInManager;

        public LogoutModel(SignInManager<ForumUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            await this.signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }

        public void OnPost()
        {
        }
    }
}
