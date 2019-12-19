using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebFinalProject.Data;
using WebFinalProject.Models;
using WebFinalProject.Models.SchoolViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{   
    public class AccountController : Controller
    {
        // GET: /<controller>/
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public AccountController(
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager
          
            )
        {
            
            _userManager = userManager;
            _signInManager = signInManager;
           
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "UTEUniversity");
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if(user == null)
            {
                return Json(true);
            }
            return Json($"Email {email} already in use");
           
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = register.Email, Email = register.Email };
                IdentityResult result = await _userManager.CreateAsync(user, register.Password);
                if(result.Succeeded)
                {
                    /*
                    //Token to confirm email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Link Confirmation
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, HttpContext.Request.Scheme);

                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = @"/Users/nam/Projects";
                    client.Send("test@localhost",user.Email,"Confirm your email",confirmationLink);

                    if(_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    ViewBag.ErrorTitle = "Registration Successfull";
                    ViewBag.ErrorMessage = "Before you can login, please confirm your" +""+
                        "email by clicking on the cofirmation link that we have emailed you";
                    return View("Error1");*/
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","UTEUniversity");
                }
                
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(register);  
        }
        //Method : Confirm Email
        [HttpGet]
        public IActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "UTEUniversity");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "The email is not confirmed ";
            return View("Error1");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login login,string ReturnUrl )
        {
            if(ModelState.IsValid)
            {
               /* var user = await _userManager.FindByEmailAsync(login.Email);
                if(user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user,login.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email is not confirmed yet");
                    return View(login);
                }*/

                    var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.Rememberme, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                       
                       return RedirectToAction("Index", "UTEUniversity");
                    }
                
                    ModelState.AddModelError("", "Invalid login attepmt");
                
            }
            return View(login);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        //Method : ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
               
                bool confirmed = await _userManager.IsEmailConfirmedAsync(user);
                if(user!= null && !confirmed)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.ActionLink("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);

                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = @"/Users/nam/Projects";
                    client.Send("test@localhost", user.Email, "Confirm your password", passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        //Method : Reset Password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token,string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Reset Password");

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.token, model.Password);
                    if(result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
    }
}
