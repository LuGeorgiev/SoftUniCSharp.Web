namespace SIS.Http.Session.Contracts
{
    public interface IHttpSession
    {
        string Id { get; }

        object GetParameter(string name);

        bool ContainsParameters(string name);

        void AddParameter(string name, object parameter);

        void ClearParameters();
    }
}
