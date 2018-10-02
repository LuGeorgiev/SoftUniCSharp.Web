namespace SIS.Http.Session
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpSession : IHttpSession
    {
        public HttpSession(string id)
        {
            this.Id = id;
            this.Parameters = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Parameters { get; }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
            => this.Parameters.Add(name, parameter);


        public void ClearParameters()
            => this.Parameters.Clear();

        public bool ContainsParameters(string name)
            => this.Parameters.ContainsKey(name);

        public object GetParameter(string name)
            => this.Parameters[name];
    }
}
