namespace SIS.Http.Session
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            if (this.parameters.ContainsKey(name))
            {
                return;
                //TODO is it correct
                throw new ArgumentException("Parameter already exists.");
            }

            this.parameters[name] = parameter;
        }

        public void ClearParameters()
        {
            this.parameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Parameter name cannot be null.");
            }

            if (!this.parameters.ContainsKey(name))
            {
                return null;
            }

            return this.parameters[name];
        }
    }
}
