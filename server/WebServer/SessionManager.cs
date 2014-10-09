using System;
using System.Collections.Generic;
using Server.Models;

namespace Server
{
    public class Session
    {
        public Session(User user, string token)
        {
            User = user;
            Token = token;
        }

        public User User { get; private set; }
        public string Token { get; private set; }
    }

    public class SessionManager
    {
        private static SessionManager _instance;
        private readonly Dictionary<string, Session> _sessions;

        private SessionManager()
        {
            _sessions = new Dictionary<string, Session>();
        }

        public Session FindByUserId(int userId)
        {
            var keys = _sessions.Keys;
            foreach (var key in keys)
            {
                var session = _sessions[key];
                var user = session.User;

                if (user.Id == userId)
                {
                    return session;
                }
            }

            return null;
        }

        public Session GetSession(string token)
        {
            return _sessions.ContainsKey(token) ? _sessions[token] : null;
        }

        public Session CreateSession(User user)
        {
            string token = GenerateSessionToken();
            var session = new Session(user, token);

            _sessions.Add(token, session);

            return session;
        }

        public void CloseSession(string token)
        {
            _sessions.Remove(token);
        }

        private static string GenerateSessionToken()
        {
            Guid g = Guid.NewGuid();
            byte[] bytes = g.ToByteArray();
            string uuidString = Convert.ToBase64String(bytes);
            return uuidString;
        }

        public static SessionManager GetInstance()
        {
            return _instance ?? (_instance = new SessionManager());
        }
    }
}
