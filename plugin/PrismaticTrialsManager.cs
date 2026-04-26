using System.Collections.Generic;

namespace Risk_of_Rain_2_Ukrainian;

public class PrismaticTrialsManager
{
    string[] modWhiteList =
    {
        "___0pseudopulse.__SeekersPatcherDLL",
        "___riskofthunder.RoR2BepInExPack",
        "_score.MiscFixes",
        "com.bepis.r2api.language",
        "com.bepis.r2api",
        "Risk_of_Rain_2_Ukrainian",
    };
    public PrismaticTrialsManager()
    {
        if (OnlyWhiteListInstalled())
        {
            // Enable prismatic trials
            On.RoR2.DisableIfGameModded.OnEnable += (orig, self) =>
            {
                return;
            };
        }
    }

    bool OnlyWhiteListInstalled()
    {
        var allowedSet = new HashSet<string>(modWhiteList);

        foreach (var guid in BepInEx.Bootstrap.Chainloader.PluginInfos.Keys)
        {
            if (!allowedSet.Contains(guid))
                return false;
        }

        return true;
    }
}
