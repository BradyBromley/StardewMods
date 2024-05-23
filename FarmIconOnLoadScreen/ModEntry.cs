using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData;

namespace FarmIconOnLoadScreen
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveCreating += this.SaveFarmType;
            helper.Events.GameLoop.Saving += this.SaveFarmType;

            FarmIconPatch.Initialize(this);

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Menus.LoadGameMenu.SaveFileSlot), nameof(StardewValley.Menus.LoadGameMenu.SaveFileSlot.Draw)),
               postfix: new HarmonyMethod(typeof(FarmIconPatch), nameof(FarmIconPatch.DrawFarmIconPostfix))
            );
        }

        // Save the farm type in farmer modData after creating a farmer so that it can be easily accessed later
        private void SaveFarmType(object? sender, EventArgs e)
        {
            try
            {
                StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                if (!modData.ContainsKey("FarmIconOnLoadScreen"))
                {
                    modData.Add("FarmIconOnLoadScreen", (Game1.whichFarm).ToString());
                }
                
                if ((Game1.whichFarm == 7) && (!modData.ContainsKey("ModFarmIconOnLoadScreen")))
                {
                    ModFarmType farmType = Game1.whichModFarm;
                    modData.Add("ModFarmIconOnLoadScreen", farmType.IconTexture);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(SaveFarmType)}:\n{ex}", LogLevel.Error);
            }
        }
    }


}
