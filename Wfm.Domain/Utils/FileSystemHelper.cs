namespace Wfm.Domain.Utils;

public static class FileSystemHelper
{
    public static List<KeyValuePair<string, string>> ConvertPathToList(string path)
    {
        var result = new List<KeyValuePair<string, string>>();

        if (string.IsNullOrEmpty(path))
            return result;

        string[] directories = path.Split('/', '\\');
        string currentPath = "";

        for (int i = 0; i < directories.Length; i++)
        {
            currentPath += (i == 0 ? "" : "/") + directories[i];
            result.Add(new KeyValuePair<string, string>(directories[i], currentPath));
        }

        return result;
    }

    public static string FormatBytes(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        int i = 0;
        double dblBytes = bytes;

        if (bytes >= 1024)
        {
            for (i = 0; (bytes / 1024) > 0; i++, bytes /= 1024)
            {
                dblBytes = bytes / 1024.0;
            }
        }

        return String.Format("{0:0.##} {1}", dblBytes, suffixes[i]);
    }
}
