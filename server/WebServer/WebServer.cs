using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace WebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener;
        private readonly IWebServerService[] _services;

        public WebServer(string host, params IWebServerService[] services)
        {

            if (HttpListener.IsSupported)
            {

                _listener = new HttpListener();
                _services = services;

                foreach (IWebServerService service in services)
                {
                    _listener.Prefixes.Add(host + service.Path);
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
            foreach (IWebServerService service in _services)
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
                Console.WriteLine("WebServer running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var context = c as HttpListenerContext;
                            if (context != null)
                            {
                                try
                                {
                                    HttpListenerRequest request = context.Request;
                                    HttpListenerResponse response = context.Response;

                                    dynamic data = GetData(request);

                                    dynamic serviceResult = GetResponderMethod(request.RawUrl)(data);
                                    dynamic json = JsonConvert.SerializeObject(serviceResult);

                                    byte[] buffer = Encoding.UTF8.GetBytes(json);

                                    response.ContentType = "application/json";
                                    response.AddHeader("Access-Control-Allow-Origin", "*");
                                    response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                                    response.ContentLength64 = buffer.Length;
                                    response.OutputStream.Write(buffer, 0, buffer.Length);
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
