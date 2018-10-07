namespace SIS.Http.Session
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class HttpSession : IHttpSession
    {
        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        private IDictionary<string, object> parameters { get; }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            if (this.ContainsParameters(name))
            {
                throw new ArgumentException();
            }

            this.parameters[name]=parameter;
        }


        public void ClearParameters()
            => this.parameters.Clear();

        public bool ContainsParameters(string name)
            => this.parameters.ContainsKey(name);

        public object GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }
            if (!this.ContainsParameters(name))
            {
                return null;

            }
            return this.parameters[name];
        }
    }
}
