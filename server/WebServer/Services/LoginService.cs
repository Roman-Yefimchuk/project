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

            var name = (string)request.name;
            var password = (string)request.password;

            Database database = Database.GetInstance();
            SessionManager sessionManager = SessionManager.GetInstance();

            User user = database.FindUser(name, password);

            if (sessionManager.FindByUserId(user.Id) != null)
            {
                throw new Exception("Ви вже авторизовані");
            }

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
