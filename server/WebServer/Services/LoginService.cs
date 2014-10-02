using System;

namespace WebServer.Services
{
    class LoginService : IWebServerService
    {
        public string Path {
            get { return "/api/login/"; } 
        }

        public dynamic ResponderMethod(dynamic request)
        {
            return new
            {
                status = "success"
            };
        }
    }
}
