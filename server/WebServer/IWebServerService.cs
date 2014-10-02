namespace WebServer
{
    public interface IWebServerService
    {

        string Path { get; }

        dynamic ResponderMethod(dynamic request);
    }
}
