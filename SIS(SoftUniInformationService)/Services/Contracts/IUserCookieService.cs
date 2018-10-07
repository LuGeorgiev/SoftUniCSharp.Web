namespace Services.Contracts
{
    public interface IUserCookieService
    {
        string GetUserCookieContent(string username);
        string GetUserDate(string cookieContent);
    }
}