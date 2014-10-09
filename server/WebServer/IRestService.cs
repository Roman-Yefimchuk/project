namespace Server
{
    public interface IRestService
    {
        string Path { get; }
        dynamic ResponderMethod(dynamic request);
    }
}
