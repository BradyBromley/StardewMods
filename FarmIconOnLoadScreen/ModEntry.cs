using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace FarmIconOnLoadScreen
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            FarmIconPatch.Initialize(this);

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Menus.LoadGameMenu.SaveFileSlot), nameof(StardewValley.Menus.LoadGameMenu.SaveFileSlot.Draw)),
               postfix: new HarmonyMethod(typeof(FarmIconPatch), nameof(FarmIconPatch.DrawFarmIconPostfix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Menus.TitleMenu), nameof(StardewValley.Menus.TitleMenu.createdNewCharacter)),
               postfix: new HarmonyMethod(typeof(FarmIconPatch), nameof(FarmIconPatch.SaveFarmTypePostfix))
            );
        }
    }


}
