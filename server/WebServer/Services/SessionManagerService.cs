﻿using System;

namespace Server.Services
{
    class SessionManagerService : IRestService
    {
        public string Path {
            get { return "/api/session-manager/"; }
        }

        public dynamic ResponderMethod(dynamic request)
        {
            var action = (string) request.action;
            var token = (string) request.token;

            SessionManager sessionManager = SessionManager.GetInstance();
            Session session = sessionManager.GetSession(token);

            switch (action)
            {
                case "is_authenticated":
                {
                    return new
                    {
                        isAuthenticated = session != null
                    };
                }
                case "get_user_data":
                {
                    if (session == null)
                    {
                        throw new ServerException("Некоректний токен: " + token);
                    }

                    var user = session.User;
                    return new
                    {
                        id = user.Id,
                        role = user.Role,
                        name = user.Name
                    };
                }
                case "logout":
                {
                    if (session == null)
                    {
                        throw new ServerException("Некоректний токен: " + token);
                    }

                    sessionManager.CloseSession(token);
                    return new { };
                }
                default:
                {
                    throw new ServerException("Невідома дія: " + action);
                }
            }
        }
    }
}
