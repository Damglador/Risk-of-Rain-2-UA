using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace Risk_of_Rain_2_Ukrainian;

public class GrammaticalGenderManager
{
    public const string FemaleGenderToken = "<жр>";
    public const string UaLangName = "uk";

    public HashSet<string> FemaleGenderTokensCollection = new();

    public GrammaticalGenderManager()
    {
        On.RoR2.Language.LoadAllTokensFromFolder += LanguageOnLoadAllTokensFromFolder;
        On.RoR2.Util.GetBestBodyName += UtilOnGetBestBodyName;
    }
    
    private void LanguageOnLoadAllTokensFromFolder(On.RoR2.Language.orig_LoadAllTokensFromFolder orig, string folder, List<KeyValuePair<string, string>> output)
    {
        // if not UA, don't do anything
        if (!folder.StartsWith(RiskOfRain2UaPlugin.LocationDir))
        {
            orig(folder, output);
            return;
        }

        var tempList = new List<KeyValuePair<string, string>>(3500);
        orig(folder, tempList);

        for (var i = 0; i < tempList.Count; i++)
        {
            var pair = tempList[i];
            if (pair.Value.StartsWith(FemaleGenderToken))
            {
                tempList[i] =
                    new KeyValuePair<string, string>(pair.Key, pair.Value.Substring(FemaleGenderToken.Length));
                this.FemaleGenderTokensCollection.Add(pair.Key);
            }
        }
        RiskOfRain2UaPlugin.Log.LogInfo($"Found {this.FemaleGenderTokensCollection.Count} grammatical female gender tokens.");

        output.AddRange(tempList);
    }

    private string UtilOnGetBestBodyName(On.RoR2.Util.orig_GetBestBodyName orig, GameObject bodyObject)
    {
        if (!Language.currentLanguage.name.Equals(UaLangName, StringComparison.OrdinalIgnoreCase))
        {
            return orig(bodyObject);
        }

        return CustomGetBestBodyNameImpl(bodyObject);
    }

    private string CustomGetBestBodyNameImpl(GameObject bodyObject)
    {
        CharacterBody characterBody = null;
        var displayName = "???";
        var isGrammaticallyFemale = false;
        
        if (bodyObject)
        {
            characterBody = bodyObject.GetComponent<CharacterBody>();
        }
        if (characterBody)
        {
            displayName = characterBody.GetUserName();
            if (this.FemaleGenderTokensCollection.Contains(characterBody.baseNameToken))
            {
                isGrammaticallyFemale = true;
            }
        }
        else
        {
            var component = bodyObject.GetComponent<IDisplayNameProvider>();
            if (component != null)
            {
                displayName = component.GetDisplayName();
            }
        }
        var fullName = displayName;
        if (characterBody)
        {
            if (characterBody.isElite)
            {
                foreach (BuffIndex buffIndex in BuffCatalog.eliteBuffIndices)
                {
                    if (characterBody.HasBuff(buffIndex))
                    {
                        var modifierToken = BuffCatalog.GetBuffDef(buffIndex).eliteDef.modifierToken;
                        fullName = GetStringFormatted(modifierToken, isGrammaticallyFemale, fullName);
                    }
                }
            }
            if (characterBody.inventory)
            {
                if (characterBody.inventory.GetItemCount(RoR2Content.Items.InvadingDoppelganger) > 0)
                {
                    fullName = GetStringFormatted("BODY_MODIFIER_DOPPELGANGER", isGrammaticallyFemale, fullName);
                }
                if (characterBody.inventory.GetItemCount(DLC1Content.Items.GummyCloneIdentifier) > 0)
                {
                    fullName = GetStringFormatted("BODY_MODIFIER_GUMMYCLONE", isGrammaticallyFemale, fullName);
                }
            }
        }
        return fullName;
    }

    private string GetStringFormatted(string token, bool isGrammaticallyFemale, params object[] args)
    {
        if (isGrammaticallyFemale)
        {
            var femaleToken = $"{token}__F";
            if (Language.currentLanguage.TokenIsRegistered(femaleToken))
            {
                token = femaleToken;
            }
        }
            
        return Language.GetStringFormatted(token, args);
    }

}