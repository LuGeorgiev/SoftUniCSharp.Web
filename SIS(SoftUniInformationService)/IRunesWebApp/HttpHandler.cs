using SIS.WebServer.Routing;

namespace IRunesWebApp
{
    public class HttpHandler
    {
        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }
    }
}