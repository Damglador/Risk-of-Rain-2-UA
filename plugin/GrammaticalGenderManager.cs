using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace Risk_of_Rain_2_Ukrainian;

public class GrammaticalGenderManager
{
    public HashSet<string> FemGenderTokensCollection = new();
    public HashSet<string> NeuGenderTokensCollection = new();

    public GrammaticalGenderManager()
    {
        On.RoR2.Language.LoadAllTokensFromFolder += LanguageOnLoadAllTokensFromFolder;
        On.RoR2.Util.GetBestBodyName += UtilOnGetBestBodyName;
    }

    private void LanguageOnLoadAllTokensFromFolder(On.RoR2.Language.orig_LoadAllTokensFromFolder orig, string folder, List<KeyValuePair<string, string>> output)
    {
        // if not uk, don't do anything
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
            if (pair.Value.StartsWith(Consts.FemGenderToken))
            {
                tempList[i] =
                    new KeyValuePair<string, string>(pair.Key, pair.Value.Substring(Consts.FemGenderToken.Length));
                FemGenderTokensCollection.Add(pair.Key);
            }
            if (pair.Value.StartsWith(Consts.NeuGenderToken))
            {
                tempList[i] =
                    new KeyValuePair<string, string>(pair.Key, pair.Value.Substring(Consts.NeuGenderToken.Length));
                NeuGenderTokensCollection.Add(pair.Key);
            }
        }
        RiskOfRain2UaPlugin.Log.LogInfo($"Found {FemGenderTokensCollection.Count} feminine tokens.");
        RiskOfRain2UaPlugin.Log.LogInfo($"Found {NeuGenderTokensCollection.Count} neutral tokens.");
        output.AddRange(tempList);
    }

    private string UtilOnGetBestBodyName(On.RoR2.Util.orig_GetBestBodyName orig, GameObject bodyObject)
    {
        if (!Language.currentLanguage.name.Equals(Consts.MyLangName, StringComparison.OrdinalIgnoreCase))
        {
            return orig(bodyObject);
        }

        return CustomGetBestBodyNameImpl(bodyObject);
    }

    private string CustomGetBestBodyNameImpl(GameObject bodyObject)
    {
        CharacterBody characterBody = null;
        var displayName = "???";
        var gender = Gender.Default;

        if (bodyObject)
        {
            characterBody = bodyObject.GetComponent<CharacterBody>();
        }
        if (characterBody)
        {
            displayName = characterBody.GetUserName();
            if (FemGenderTokensCollection.Contains(characterBody.baseNameToken))
            {
                gender = Gender.Fem;
            }
            else if (NeuGenderTokensCollection.Contains(characterBody.baseNameToken))
            {
                gender = Gender.Neu;
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
                        fullName = GetStringFormatted(modifierToken, gender, fullName);
                    }
                }
            }
            if (characterBody.inventory)
            {
                if (characterBody.inventory.GetItemCount(RoR2Content.Items.InvadingDoppelganger) > 0)
                {
                    fullName = GetStringFormatted("BODY_MODIFIER_DOPPELGANGER", gender, fullName);
                }
                if (characterBody.inventory.GetItemCount(DLC1Content.Items.GummyCloneIdentifier) > 0)
                {
                    fullName = GetStringFormatted("BODY_MODIFIER_GUMMYCLONE", gender, fullName);
                }
            }
        }
        return fullName;
    }

    private string GetStringFormatted(string token, Gender gender, params object[] args)
    {
        string genderToken;
        switch (gender)
        {
            case Gender.Fem:
                genderToken = $"{token}__F";
                if (Language.currentLanguage.TokenIsRegistered(genderToken))
                {
                    token = genderToken;
                }
                break;
            case Gender.Neu:
                genderToken = $"{token}__N";
                if (Language.currentLanguage.TokenIsRegistered(genderToken))
                {
                    token = genderToken;
                }
                break;
        }
        return Language.GetStringFormatted(token, args);
    }
}
