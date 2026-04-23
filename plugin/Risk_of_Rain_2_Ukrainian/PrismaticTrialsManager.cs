using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using RoR2;
using UnityEngine;

namespace Risk_of_Rain_2_Ukrainian;

public class PrismaticTrialsManager
{
    public void EnableTrials()
    {
        On.RoR2.DisableIfGameModded.OnEnable += (orig, self) =>
        {
            return;
        };
    }
}