using System;

namespace JunimoKartCheatMenu
{
    internal class ModConfig
    {
        public bool InfiniteLives { get; set; }
        public bool Invincibility { get; set; }
        public string JumpGravity { get; set; }
        public string FallGravity { get; set; }

        public ModConfig()
        {
            InfiniteLives = false;
            Invincibility = false;
            JumpGravity = "Default";
            FallGravity = "Default";
        }
    }
}
