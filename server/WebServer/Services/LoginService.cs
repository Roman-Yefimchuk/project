using System;
using Server.Models;

namespace Server.Services
{
    class LoginService : IRestService
    {
        public string Path {
            get { return "/api/login/"; } 
        }

        public dynamic ResponderMethod(dynamic request)
        {

            var name = (string)request.name;
            var password = (string)request.password;

            User user = User.Find(name, password);
            SessionManager sessionManager = SessionManager.GetInstance();

            if (sessionManager.FindByUserId(user.Id) != null)
            {
                throw new ServerException("Ви вже авторизовані");
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
