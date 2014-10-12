using System.IO;

namespace Server
{
    public class ResourcesManager
    {
        public static Stream GetResourceAsStream(string path)
        {
            return File.Exists(path) ? new FileStream(path, FileMode.Open) : null;
        }

        public static string GetStringFromStream(Stream stream)
        {
            return stream == null ? null : new StreamReader(stream).ReadToEnd();
        }

        public static string GetStringFromFile(string path)
        {
            Stream fileStream = GetResourceAsStream(path);
            return GetStringFromStream(fileStream);
        }
    }
}
