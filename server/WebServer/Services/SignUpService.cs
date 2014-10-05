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

            var name = (string)request.name;
            var password = (string)request.password;

            Database database = Database.GetInstance();
            SessionManager sessionManager = SessionManager.GetInstance();

            User user = database.CreateUser(name, password);
            Session session = sessionManager.CreateSession(user);

            return new
            {
                id = user.Id,
                role = user.Role,
                name = user.Name,
                token = session.Token
            };
        }
    }
}
