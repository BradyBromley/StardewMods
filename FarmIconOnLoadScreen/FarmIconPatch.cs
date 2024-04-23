using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.GameData;
using StardewValley.GameData.Pets;

namespace FarmIconOnLoadScreen
{
    internal class FarmIconPatch
    {

        private static IMonitor Monitor;

        internal static void Initialize(Mod mod)
        {
            Monitor = mod.Monitor;
        }

        internal static void DrawFarmIconPostfix(LoadGameMenu.SaveFileSlot __instance, SpriteBatch b, int i)
        {
            try
            {
                Vector2 vector = new Vector2((TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 8, (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 78);
                b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
            }
            catch (Exception e)
            {
                Monitor.Log($"Failed in {nameof(DrawFarmIconPostfix)}:\n{e}", LogLevel.Error);
            }
            
        }
    }
}
