namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Data.Models;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;

        public PersonalDataModel(UserManager<ForumUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}