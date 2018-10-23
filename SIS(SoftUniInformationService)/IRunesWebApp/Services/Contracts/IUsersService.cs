namespace IRunesWebApp.Services.Contracts
{
    public interface IUsersService
    {
        bool ExistsByUsernameAndPassword(string username, string password);

        bool RegisterUser(string username, string password);
    }
}
