using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Server
{
    public class Server
    {
        private readonly HttpListener _listener;
        private readonly IRestService[] _services;

        public Server(string host, params IRestService[] services)
        {
            if (HttpListener.IsSupported)
            {

                _listener = new HttpListener();
                _services = services;

                HttpListenerPrefixCollection prefixCollection = _listener.Prefixes;
                foreach (IRestService service in services)
                {
                    prefixCollection.Add(host + service.Path);
                }

                _listener.Start();
            }
            else
            {
                throw new NotSupportedException("HttpListener not supported");
            }
        }

        private Func<dynamic, dynamic> GetResponderMethod(string path)
        {
            foreach (IRestService service in _services)
            {
                string servicePath = service.Path;
                if (servicePath.Equals(path))
                {
                    return service.ResponderMethod;
                }
            }
            return null;
        }


        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var context = c as HttpListenerContext;
                            if (context == null)
                            {
                                return;
                            }

                            try
                            {
                                HttpListenerRequest request = context.Request;
                                HttpListenerResponse response = context.Response;

                                if ("OPTIONS".Equals(request.HttpMethod))
                                {
                                    WriteHeader(response);
                                    response.OutputStream.Close();
                                    return;
                                }

                                var relativeUrl = request.RawUrl;

                                try
                                {
                                    dynamic data = GetData(request);

                                    SendResponse(new
                                    {
                                        status = true,
                                        data = GetResponderMethod(request.RawUrl)(data)
                                    }, response, relativeUrl);
                                }
                                catch (Exception e)
                                {
                                    string error = e.ToString();
                                    Console.WriteLine(error);

                                    string errorMessage = e.Message;
                                    SendResponse(new
                                    {
                                        status = false,
                                        error = errorMessage
                                    }, response, relativeUrl);
                                }
                            }
                            catch (Exception e)
                            {
                                string error = e.ToString();
                                Console.WriteLine(error);
                            }
                            finally
                            {
                                context.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                    Console.WriteLine(error);
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        private static void SendResponse(dynamic serviceResult, HttpListenerResponse response, string relativeUrl)
        {
            string json = JsonConvert.SerializeObject(serviceResult);

            byte[] buffer = Encoding.UTF8.GetBytes(json);

            WriteHeader(response);

            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);

            Console.WriteLine("Url: '{0}', {1} byte(s)", relativeUrl, buffer.Length);
        }

        private static void WriteHeader(HttpListenerResponse response)
        {
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
        }

        private static dynamic GetData(HttpListenerRequest request)
        {
            if (request.HasEntityBody)
            {
                using (Stream body = request.InputStream)
                {
                    using (var reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string json = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<dynamic>(json);
                    }
                }
            }
            return null;
        }
    }
}
