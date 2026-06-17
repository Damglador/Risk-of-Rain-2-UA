using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using RoR2;

namespace Risk_of_Rain_2_Ukrainian
{
    [BepInPlugin("Risk_of_Rain_2_Ukrainian", "Risk_of_Rain_2_Ukrainian", "1.6.0")]
    public class RiskOfRain2UaPlugin : BaseUnityPlugin
    {
        public static RiskOfRain2UaPlugin Instance;
        public static ManualLogSource Log => Instance.Logger;

        public static string LocationDir;

        public GrammaticalGenderManager GrammaticalGenderManager;
        public FontManager FontManager;
        public PrismaticTrialsManager PrismaticTrialsManager;

        private void Awake()
        {
            Instance = this;
            LocationDir = System.IO.Path.GetDirectoryName(Info.Location);
            Language.collectLanguageRootFolders += LanguageOnCollectLanguageRootFolders;
            GrammaticalGenderManager = new GrammaticalGenderManager();
            FontManager = new FontManager();
            PrismaticTrialsManager = new PrismaticTrialsManager();
            Logger.LogInfo($"Plugin Risk_of_Rain_2_Ukrainian is loaded!");

            // string path = System.IO.Path.Combine(Paths.BepInExRootPath, "installed_mods.txt");
            // File.WriteAllLines(path, BepInEx.Bootstrap.Chainloader.PluginInfos.Keys);
        }

        private void LanguageOnCollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(LocationDir!, "lang"));
            folders.Add(System.IO.Path.Combine(LocationDir!, "lang_mods"));
        }
    }
}
