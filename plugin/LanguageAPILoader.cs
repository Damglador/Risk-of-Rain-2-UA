using System.IO;
using R2API;

namespace Risk_of_Rain_2_Ukrainian;

public class LanguageAPILoader
{
    public LanguageAPILoader(string folder)
    {
        if (Directory.Exists(folder))
        {
            var lines = File.ReadLines(Path.Join(folder, "LanguageAPIWhitelist.conf"));
            foreach (var line in lines)
            {
                if (File.Exists(Path.Join(folder, line)))
                {
                    RiskOfRain2UaPlugin.Log.LogInfo($"Loading {Path.Join(folder, line)} for LanguageAPI.");
                    LanguageAPI.AddPath(Path.Join(folder, line));
                }
                else
                {
                    RiskOfRain2UaPlugin.Log.LogWarning($"{Path.Join(folder, line)} is specified in LanguageAPIWhitelist.conf, but does not exist.");
                }
            }
        }
    }
}
