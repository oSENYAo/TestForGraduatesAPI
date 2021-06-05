using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TestForGraduates.Data.Repository;
using TestForGraduates.Interfaces;
using TestForGraduates.Models;
using TestForGraduates.Services;
using TestForGraduates.ViewModels;

namespace TestForGraduates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMessageEmailService message;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMessageEmailService message)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.message = message;
        }


        // список всех аккаунтов
        [HttpGet("users")]
        public async Task<IEnumerable<User>> GetAllUser() => await userManager.Users.ToListAsync();

        
        // регистрация нового пользователя
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // проверка email
                var isValidEmail = CheckEmail.IsValidEmail(model.Email);
                
                if (isValidEmail)
                {
                    User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name };
                    try
                    {
                        var result = await userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                "Account",
                                new { userId = user.Id, code = code },
                                protocol: HttpContext.Request.Scheme);
                            EmailService emailService = new EmailService();
                            await message.SendEmailMessage(model.Email, "Confirm your account",
                                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                            return Ok("Регистрация успешна");
                        }
                    }
                    catch (Exception)
                    {
                        await userManager.DeleteAsync(user);

                        return NotFound("Почта ненайдена");
                    }
                }
                else
                {
                    return Content("email некорректен");
                }
            }
            return ValidationProblem("Ты лошара");
        }
        
        // вход в существующий аккаунт
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                return Ok(result);
            }
            return this.StatusCode((int)HttpStatusCode.NetworkAuthenticationRequired);
        }
        
        // выход из аккаунта
        [HttpPost("logout")]
        public async Task<ActionResult<User>> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }


        #region Первый вариант регистрации
        //[HttpPost("register")]
        //public async Task<ActionResult<User>> Register([FromBody] RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var _user = await userRepository.GetByName(model.Name);
        //        CheckUserName checkUserName = new CheckUserName();
        //        var IsCheckUserName = checkUserName.CheckName(model.Name);

        //        if (IsCheckUserName)
        //        {
        //            if (_user == null)
        //            {
        //                if (model.Password == model.PasswordConfirm)
        //                {
        //                    User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name };
        //                    try
        //                    {
        //                        var result = await userManager.CreateAsync(user, model.Password);

        //                        if (result.Succeeded)
        //                        {
        //                            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        //                            var callbackUrl = Url.Action(
        //                                "ConfirmEmail",
        //                                "Account",
        //                                new { userId = user.Id, code = code },
        //                                protocol: HttpContext.Request.Scheme);
        //                            EmailService emailService = new EmailService();
        //                            await message.SendEmailMessage(model.Email, "Confirm your account",
        //                                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

        //                            IdentityRole getRoleById = await userRepository.GetRole(role);

        //                            var userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = getRoleById.Id };

        //                            await userRepository.AddRole(userRole);
        //                            await userRepository.Save();

        //                            return Ok("Регистрация успешна");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        await userRepository.Delete(user.Id);
        //                        await userRepository.Save();

        //                        return NotFound("Почта ненайдена");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return Content("Пользователь с таким ником уже есть");
        //            }
        //        }
        //        else
        //        {
        //            return Content("Никнейм не должен содержать цифры");
        //        }
        //    }
        //    return ValidationProblem("Ты лошара");
        //}
        #endregion
    }
}
