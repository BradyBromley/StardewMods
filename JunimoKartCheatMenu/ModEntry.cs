using System;
using JunimoKartCheatMenu.Framework.GMCM;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace JunimoKartCheatMenu
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            // Read or create config file
            Config = Helper.ReadConfig<ModConfig>();

            // Setup events
            helper.Events.GameLoop.GameLaunched += SetupConfig;
        }

        // Set up the GenericModConfigMenu
        private void SetupConfig (object? sender, EventArgs e)
        {
            // Get GenericModConfigMenu API
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }

            // Register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config.EnableCheats = false,
                save: () => Helper.WriteConfig(Config)
            );

            // Add options
            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config.EnableCheats,
                setValue: value => Config.EnableCheats = value,
                name: () => "Enable Cheats",
                tooltip: () => "Enable Cheats when playing Junimo Kart"
            );
        }
    }


}
