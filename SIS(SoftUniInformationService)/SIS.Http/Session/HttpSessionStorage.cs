namespace SIS.Http.Session
{
    using Contracts;
    using System.Collections.Concurrent;

    public class HttpSessionStorage
    {
        public const string Session_Cookie_Key = "SIS_ID";

        private static readonly ConcurrentDictionary<string, HttpSession> sessions =
            new ConcurrentDictionary<string, HttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return sessions.GetOrAdd(id, _ => new HttpSession(id));
        }
    }
}
