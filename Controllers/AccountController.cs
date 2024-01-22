
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using Newtonsoft.Json;
using UserApplication.Models;


namespace UserApplication.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AccountController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }


        //================================= Registration=========================================
        public IActionResult Registor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registor(UserModel model) 
        {
            if (ModelState.IsValid) 
            {
                IdentityUser user = new IdentityUser { UserName = model.Name, Email = model.Email,PhoneNumber=model.Mobile };

                var result=await userManager.CreateAsync(user,model.Password);

                if (result.Succeeded) 
                {
                    await signInManager.SignInAsync(user, false);
                    
                    
                    if(model.UserRole=="Admin")
                    {
                        if(await roleManager.FindByNameAsync("Admin") is null) 
                        {
                            IdentityRole role = new IdentityRole { Name="Admin"};
                           await roleManager.CreateAsync(role);
                        }

                    }
                    if (model.UserRole == "User")
                    {
                        if (await roleManager.FindByNameAsync("User") is null)
                        {
                            IdentityRole role = new IdentityRole { Name = "User" };
                            await roleManager.CreateAsync(role);
                        }
                    }
                   await userManager.AddToRoleAsync(user, model.UserRole);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        //=====================================Login=========================================
        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                
                var result=await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password,true,false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("ApplicationUser", loginModel.Name);
                    HttpContext.Session.SetString("ApplicationSessionStarted", DateTime.Now.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginModel);
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("ApplicationUser");
            await signInManager.SignOutAsync();
            return RedirectToAction( "Login");
        }
        
        public  IActionResult CookiesSet() 
        {
            HttpContext.Session.SetString("ApplicationSessionStarted", DateTime.Now.ToString());
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
