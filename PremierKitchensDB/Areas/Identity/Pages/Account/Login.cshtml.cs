using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PremierKitchensDB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace PremierKitchensDB.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private HttpContext _httpContext;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public Dictionary<string, string> Database { get; set; }
        public ApplicationUser applicationUser { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Display(Name = "System")]
            public string Database { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //Check which database was last selected:
            var systemDatabase = "";

            if (_httpContext.Request.Cookies["SystemDatabase"] != null)
            {
                systemDatabase = _httpContext.Request.Cookies["SystemDatabase"].ToString();
            }
            else
            {
                systemDatabase = "Live";

                //If no cookie then set one for live system
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(14);
                option.HttpOnly = false;
                option.IsEssential = true;
                Response.Cookies.Append("SystemDatabase", systemDatabase, option);
            }

            //Get list of databases to connect to
            Database = new Dictionary<string, string>();
            Database.Add("Live", "Live System");
            Database.Add("Training", "Training System");

            ViewData["Database"] = new SelectList(Database, "Key", "Value", systemDatabase);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //Record the database the user has connected to
            //CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddDays(14);
            //option.HttpOnly = true;
            //option.IsEssential = true;
            //Response.Cookies.Append("SystemDatabase", Input.Database, option);

            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
