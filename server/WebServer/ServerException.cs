using System;

namespace Server
{
    public class ServerException : Exception
    {
        public ServerException(String message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
