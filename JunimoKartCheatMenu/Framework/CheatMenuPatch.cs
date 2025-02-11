using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using static StardewValley.Minigames.MineCart;

namespace JunimoKartCheatMenu.Framework
{
    internal class CheatMenuPatch
    {
        private static IMonitor Monitor;
        private static ModConfig Config;

        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }

        // Check for invincibility and infinite lives
        internal static bool DiePrefix(int ___currentTheme, MineCartCharacter ___player, int ___screenHeight, ref int ___livesLeft)
        {
            try
            {
                // Prevent the player from dying when hitting an obstacle
                if (Config.Invincibility)
                {
                    float death_buffer = 3f;
                    if (___currentTheme == 9)
                    {
                        death_buffer = 32f;
                    }
                    if (___player.position.Y <= (float)___screenHeight + death_buffer)
                    {
                        return false;
                    }
                }

                // Prevent the player from losing a life when they die
                if (Config.InfiniteLives)
                {
                    ___livesLeft++;
                }

                return true;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DiePrefix)}:\n{ex}", LogLevel.Error);

                return false;
            }
        }


        // Change the jump and fall gravity in Junimo Kart
        internal static void GetMaxFallSpeedPostfix(MineCartCharacter __instance)
        {
            try
            {
                // Jump height
                switch (Config.JumpGravity)
                {
                    case "Very Small":
                        __instance.jumpGravity = 12000;
                        break;
                    case "Small":
                        __instance.jumpGravity = 7200;
                        break;
                    case "Default":
                        __instance.jumpGravity = 3400;
                        break;
                    case "Large":
                        __instance.jumpGravity = 2400;
                        break;
                    case "Very Large":
                        __instance.jumpGravity = 1000;
                        break;
                }

                // Gravity
                switch (Config.FallGravity)
                {
                    case "Very Low":
                        __instance.fallGravity = 300;
                        break;
                    case "Low":
                        __instance.fallGravity = 900;
                        break;
                    case "Default":
                        __instance.fallGravity = 3000;
                        break;
                    case "High":
                        __instance.fallGravity = 6800;
                        break;
                    case "Very High":
                        __instance.fallGravity = 10000;
                        break;
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(GetMaxFallSpeedPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
