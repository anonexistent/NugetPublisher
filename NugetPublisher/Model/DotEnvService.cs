using System.IO;

namespace NugetPublisher.Model;

public static class DotEnvService
{
    public static void LoadEnvironmentFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string[] array = File.ReadAllLines(filePath);
        foreach (string text in array)
        {
            string[] array2 = text.Split(" = ", StringSplitOptions.RemoveEmptyEntries);
            if (array2.Length == 2)
            {
                Environment.SetEnvironmentVariable(array2[0], array2[1]);
            }
        }
    }
}
