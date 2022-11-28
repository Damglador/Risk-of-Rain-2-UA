using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;

namespace Risk_of_Rain_2_Ukrainian
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [R2APISubmoduleDependency(nameof(LanguageAPI))]
    public class RiskOfRain2UaPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Language.collectLanguageRootFolders += LanguageOnCollectLanguageRootFolders;
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void LanguageOnCollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Info.Location)!, "Localization"));
        }
    }
}
