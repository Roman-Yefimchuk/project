using Server.Models;

namespace Server.Services
{
    class SignUpService : IRestService
    {
        public string Path {
            get { return "/api/sign-up/"; }
        }

        public dynamic ResponderMethod(dynamic request)
        {

            var name = (string)request.name;
            var password = (string)request.password;

            User user = User.Create(name, password);
            SessionManager sessionManager = SessionManager.GetInstance();
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
