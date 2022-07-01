using BusinessLayer.Interfaces;
using DatabasLayer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using System;
using System.Linq;

namespace Fundoo_Notes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        IUserBL userBL;
        FundooContext fundooContext;

        public UserController(IUserBL userBL, FundooContext fundooContext)
        {
            this.userBL = userBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == userPostModel.Email);
                if (user != null)
                {
                    return this.BadRequest(new { success = false, message = "Email Already Exits" });
                }
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, message = "Registration Successfull" });
                //when request get succeded we get 2oo 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("Login /{email}/{password}")]
        public IActionResult LogIn(string email, string password)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == email);
                string Password = PwdEncryptDecryptService.DecryptPassword(user.Password);
                if (user == null)
                {
                    return this.BadRequest(new { success = false, message = "Email doesn't Exits" });
                }

                var userdata1 = fundooContext.Users.FirstOrDefault(u => u.Email == email && Password == password);
                if (userdata1 == null)
                {
                    return this.BadRequest(new { success = false, message = "Password is Invalid" });
                }

                string token = this.userBL.LogInUser(email, password);
                return this.Ok(new { success = true, message = "Log Successfull" });
                //when request get succeded we get 2oo 
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpPost("ForgotPassword/{email}")]
        public IActionResult Forgot_Password(string email)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == email);
                //string Password = PwdEncryptDecryptService.DecryptPassword(user.Password);
                if (user == null)
                {
                    return this.BadRequest(new { success = false, message = "Email doesn't Exits" });
                }


                bool token = this.userBL.ForgetPassword(email);
                return this.Ok(new { success = true, message = "Forgot password" });


                //when request get succeded we get 2oo 
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Authorize]
        [HttpPut("Resetpassword")]
        public IActionResult Resetpassword(UserPasswordModel userPasswordModel)
        {
            try
            {

                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userid", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var result = fundooContext.Users.Where(u => u.UserId == UserID).FirstOrDefault();
                string Email = result.Email.ToString();

                if (userPasswordModel.Password != userPasswordModel.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Password and ConfirmPassword must be same" });
                }

                bool res = this.userBL.ResetPassword(Email, userPasswordModel);

                if (res == false)
                {
                    return this.BadRequest(new { success = false, message = "Enter the valid Email" });
                }
                return this.Ok(new { success = true, message = "Password Update Successfull" });
                //when request get succeded we get 2oo 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
