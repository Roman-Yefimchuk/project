using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Services
{
    class SearchService : IWebServerService
    {
        public string Path {
            get { return "/api/search/"; }
        }

        public dynamic ResponderMethod(dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}
