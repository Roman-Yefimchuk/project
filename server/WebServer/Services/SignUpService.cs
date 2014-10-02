using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Services
{
    class SignUpService : IWebServerService
    {
        public string Path {
            get { return "/api/sign-up/"; }
        }

        public dynamic ResponderMethod(dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}
