using System;

namespace JunimoKartPrizes
{
    internal class ModConfig
    {
        public float RareItem { get; set; }
        public float EpicItem { get; set; }
        public float LegendaryItem { get; set; }

        public ModConfig()
        {
            RareItem = 0.25f;
            EpicItem = 0.04f;
            LegendaryItem = 0.01f;

        }
    }
}
