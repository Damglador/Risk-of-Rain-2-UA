# Misc Fixes

Designed to comprehensively address the bugs and exceptions that game updates introduce, and attempts to soften the impact that breaking changes have on the existing mod ecosystem, primarily through Harmony based IL hooks. It also applies a small amount of game integrity focused asset updates, and provides tools to assist with safely developing around error prone aspects of the game code.

Note that these changes are NOT intended to modify the vanilla gameplay experience. If it's even debatable on whether or not it's in line with the "intended" vanilla experience, it doesn't belong here.

---

# FOR DEVS:

- ## HUD ChildLocator Entries
  - These are difficult to find normally because they have no identifiable component attached.

### New entries:

> - "SpringCanvas"
> - "UpperRightCluster"
> - "BottomRightCluster"
> - "UpperLeftCluster"
> - "BottomCenterCluster"
> - "LeftCluster"
> - "RightCluster"

> - "NotificationArea"
> - "ScoreboardPanel"
> - "SkillDisplayRoot"
> - "BuffDisplayRoot"
> - "InventoryDisplayRoot"

---

### Existing entries:

> - "BottomLeftCluster"
> - "TopCenterCluster"


> - "RightUtilityArea"
> - "ScopeContainer"
> - "CrosshairExtras"
> - "BossHealthBar"
> - "RightInfoBar" -Always null, kept in for compat

- ## Extension Methods 
  - RiskOfOptions compatible config binding
  - Common ILCursor functions
  - Component removal and cloning
  - EntityStateConfiguration reading and modification

- To gain access, add the MiscFixes.dll as an assembly reference (Nuget package coming soon)
- Methods can be found in the MiscFixes.Modules.Extensions class

---

# Important Changes
### Check the changelog for more info. This list may not include everything.

- Fixes many common Henry prefab creation error
- Fixes many of the issues modded characters and skins have in the recent update.
- Fixes vanilla Asset loading issues introduced in the recent memory optimization update.
- Prevents spam error for various server methods called on clients
- Restores some failing Elder Lemurian footstep sounds
- Fixes an error message for Sale's Star pickup
- Corrects misplaced vermin spawn particle effects
- Restores printer VFX
- Restores scrapper sounds
- Prevents pointless error spam when destroying some objects, e.g., killing a Stone Titan
- Restores backwards compatibilty for temporary overlays

---

## Error handling
- chef RolyPoly achievement NRE
- NRE with Aurelionite affix targeting
- NRE when masterless bodies level up (TryGiveFreeUnlockWhenLevelUp)
- various NREs with VineOrb (dead target on arrival, null dotDef)
- Prevents an error when spawning some projectiles, probably because they lack a model
- Some ProcessGoldEvents error
- an NRE when leaving the stage with drones (MinionLeashBodyBehavior.OnDisable)
- any Antler NREs
- rare FogDamageController NRE when targeting teammates
- Lunar exploder killed by void death error
- the roulette check NRE
- Gilded Coast chest interaction error
- an error for objects that have null HurtBoxes (only seen it for the hanging mushrooms in Golden Dieback)
- Lemurian Eggs constantly creating new lock VFX when charging the teleporter which don't get cleared upon completion
- NRE spam for arrow indicators when playing with the HUD disabled (teleporter boss, void seed, etc)
- Equipment indicator error
- a crosshair error when Seeker respawns
- a Rewired error when quiting the game
- the TestState1 - TestState2 log spam on Prime Meridian
- an error when the Child fails to teleport near your location
- an NRE when spawning close to a chest
- NREs related to TetherVfxOrigin.AddTether and TetherVfxOrigin.RemoveTetherAt for the twisted elite
- Halcyonite's Whirlwind NRE spam when its target is killed during the skill
- Meridian's Will failing to pull monsters in when a stationary target is hit
- The dreadful Facepunch exception that can occur, which randomly prevented loading (3% bug)
- Flicker light error
- an unrecoverable error caused by having multiple event systems (thanks Bubbet)
- Incompletable void seed bug
- (temporarily removed) Fixes ConVars with uppercase letters not working, e.g, egsToggle



---

# _SPECIAL THANKS TO:_

- Chinchi, wouldn't have been possible without ya
