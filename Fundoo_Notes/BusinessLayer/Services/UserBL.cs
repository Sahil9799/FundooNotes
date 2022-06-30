using BusinessLayer.Interfaces;
using DatabasLayer.User;
using RepositoryLayer.Interfaces;
using System;


namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                userRL.AddUser(userPostModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string LogInUser(string Email, string Password)
        {
            try
            {
                return this.userRL.LogInUser(Email, Password);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

