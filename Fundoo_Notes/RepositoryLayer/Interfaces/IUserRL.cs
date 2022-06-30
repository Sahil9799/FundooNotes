using DatabasLayer.User;


namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
      public void AddUser(UserPostModel userPostModel);
      public string LogInUser(string Email, string Password);
    }
}
