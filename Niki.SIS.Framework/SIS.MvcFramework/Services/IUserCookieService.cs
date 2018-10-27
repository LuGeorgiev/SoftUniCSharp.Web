namespace SIS.MvcFramework.Services
{
    public interface IUserCookieService
    {
        string GetUserCookie(MvcUserInfo userName);

        MvcUserInfo GetUserData(string cookieContent);
    }
}
