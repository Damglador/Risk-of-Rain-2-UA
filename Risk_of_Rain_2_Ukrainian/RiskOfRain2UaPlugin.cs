using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using RoR2;

namespace Risk_of_Rain_2_Ukrainian
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [R2APISubmoduleDependency(nameof(LanguageAPI))]
    public class RiskOfRain2UaPlugin : BaseUnityPlugin
    {
        public static RiskOfRain2UaPlugin Instance;
        public static ManualLogSource Log => Instance.Logger;

        public static string LocationDir;

        public GrammaticalGenderManager GrammaticalGenderManager;
        
        private void Awake()
        {
            Instance = this;
            LocationDir = System.IO.Path.GetDirectoryName(Info.Location);
            Language.collectLanguageRootFolders += LanguageOnCollectLanguageRootFolders;
            GrammaticalGenderManager = new GrammaticalGenderManager();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void LanguageOnCollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(LocationDir!, "Localization"));
        }
    }
}
