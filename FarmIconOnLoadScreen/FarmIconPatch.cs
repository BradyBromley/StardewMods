using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.GameData;

namespace FarmIconOnLoadScreen
{
    internal class FarmIconPatch
    {

        private static IMonitor Monitor;

        internal static void Initialize(Mod mod)
        {
            Monitor = mod.Monitor;
        }

        // Draw the farm type on the load screen
        internal static void DrawFarmIconPostfix(LoadGameMenu.SaveFileSlot __instance, SpriteBatch b, int i)
        {
            try
            {
                Vector2 vector = new Vector2((TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 8, (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 78);

                StardewValley.Mods.ModDataDictionary modData = __instance.Farmer.modData;
                if (modData.ContainsKey("FarmIconOnLoadScreen"))
                {
                    int whichFarm;
                    bool success = int.TryParse(modData["FarmIconOnLoadScreen"], out whichFarm);
                    if (success)
                    {
                        switch (whichFarm)
                        {
                            case 0:
                                // Standard
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 1:
                                // Riverland
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(22, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 2:
                                // Forest
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(44, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 3:
                                // Hills
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(66, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 4:
                                // Wilderness
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(88, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 5:
                                // Four Corners
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 345, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 6:
                                // Beach
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(22, 345, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                break;
                            case 7:
                                if (modData["ModFarmIconOnLoadScreen"] != null)
                                {
                                    // Mod Farm
                                    Texture2D modFarmTexture = Game1.content.Load<Texture2D>(modData["ModFarmIconOnLoadScreen"]);
                                    b.Draw(modFarmTexture, vector, new Rectangle(0, 0, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                } else
                                {
                                    // Meadowlands
                                    b.Draw(Game1.mouseCursors, vector, new Rectangle(1, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawFarmIconPostfix)}:\n{ex}", LogLevel.Error);
            }
            
        }
    }
}
