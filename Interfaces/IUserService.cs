namespace LR4.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(Models.User user);
        bool ValidateUser(Models.User user);
        bool AuthenticateAdmin(string passport, string password);
        bool AuthenticateUser(string passport, string password);
        void ActivateUser(string userLine);
    }
}
