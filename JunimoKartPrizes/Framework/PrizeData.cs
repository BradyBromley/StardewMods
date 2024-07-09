using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunimoKartPrizes.Framework
{
    internal class PrizeData
    {
        public static Prize[] commonItems { get; set; } = new Prize[]
        {
            new Prize { itemID = "(O)368", itemQuantity = 3 }, // Basic Fertilizer
            new Prize { itemID = "(O)370", itemQuantity = 3 }, // Basic Retaining Soil
            new Prize { itemID = "(O)378", itemQuantity = 5 }, // Copper Ore
            new Prize { itemID = "(O)382", itemQuantity = 5 }, // Coal
            new Prize { itemID = "(O)388", itemQuantity = 25 }, // Wood
            new Prize { itemID = "(O)390", itemQuantity = 25 }, // Stone
            new Prize { itemID = "(O)535", itemQuantity = 1 }, // Geode
            new Prize { itemID = "(O)709", itemQuantity = 5 }, // Hardwood

            new Prize { itemID = "(O)495", itemQuantity = 5 }, // Spring Seeds
            new Prize { itemID = "(O)496", itemQuantity = 5 }, // Summer Seeds
            new Prize { itemID = "(O)497", itemQuantity = 5 }, // Fall Seeds
            new Prize { itemID = "(O)498", itemQuantity = 5 } // Winter Seeds
        };

        public static Prize[] rareItems { get; set; } = new Prize[]
        {
            new Prize { itemID = "(O)286", itemQuantity = 5 }, // Cherry Bomb
            new Prize { itemID = "(O)287", itemQuantity = 5 }, // Bomb
            new Prize { itemID = "(O)369", itemQuantity = 3 }, // Quality Fertilizer
            new Prize { itemID = "(O)371", itemQuantity = 3 }, // Quality Retaining Soil
            new Prize { itemID = "(O)380", itemQuantity = 5 }, // Iron Ore
            new Prize { itemID = "(O)536", itemQuantity = 1 }, // Frozen Geode
            new Prize { itemID = "(O)537", itemQuantity = 1 }, // Magma Geode
            new Prize { itemID = "(O)688", itemQuantity = 1 }, // Warp Totem: Farm
            new Prize { itemID = "(O)689", itemQuantity = 1 }, // Warp Totem: Mountains
            new Prize { itemID = "(O)690", itemQuantity = 1 }, // Warp Totem: Beach

            new Prize { itemID = "(O)273", itemQuantity = 3 }, // Rice Shoot
            new Prize { itemID = "(O)299", itemQuantity = 3 }, // Amaranth Seeds
            new Prize { itemID = "(O)301", itemQuantity = 3 }, // Grape Starter
            new Prize { itemID = "(O)302", itemQuantity = 3 }, // Hops Starter
            new Prize { itemID = "(O)425", itemQuantity = 3 }, // Fairy Seeds
            new Prize { itemID = "(O)427", itemQuantity = 3 }, // Tulip Bulb
            new Prize { itemID = "(O)429", itemQuantity = 3 }, // Jazz Seeds
            new Prize { itemID = "(O)431", itemQuantity = 3 }, // Sunflower Seeds
            new Prize { itemID = "(O)453", itemQuantity = 3 }, // Poppy Seeds
            new Prize { itemID = "(O)455", itemQuantity = 3 }, // Spangle Seeds
            new Prize { itemID = "(O)472", itemQuantity = 3 }, // Parsnip Seeds
            new Prize { itemID = "(O)473", itemQuantity = 3 }, // Bean Starter
            new Prize { itemID = "(O)474", itemQuantity = 3 }, // Cauliflower Seeds
            new Prize { itemID = "(O)475", itemQuantity = 3 }, // Potato Seeds
            new Prize { itemID = "(O)477", itemQuantity = 3 }, // Kale Seeds
            new Prize { itemID = "(O)479", itemQuantity = 3 }, // Melon Seeds
            new Prize { itemID = "(O)480", itemQuantity = 3 }, // Tomato Seeds
            new Prize { itemID = "(O)481", itemQuantity = 3 }, // Blueberry Seeds
            new Prize { itemID = "(O)482", itemQuantity = 3 }, // Pepper Seeds
            new Prize { itemID = "(O)483", itemQuantity = 3 }, // Wheat Seeds
            new Prize { itemID = "(O)484", itemQuantity = 3 }, // Radish Seeds
            new Prize { itemID = "(O)487", itemQuantity = 3 }, // Corn Seeds
            new Prize { itemID = "(O)488", itemQuantity = 3 }, // Eggplant Seeds
            new Prize { itemID = "(O)490", itemQuantity = 3 }, // Pumpkin Seeds
            new Prize { itemID = "(O)491", itemQuantity = 3 }, // Bok Choy Seeds
            new Prize { itemID = "(O)492", itemQuantity = 3 }, // Yam Seeds
            new Prize { itemID = "(O)493", itemQuantity = 3 } // Cranberry Seeds
        };

        public static Prize[] epicItems { get; set; } = new Prize[]
        {
            new Prize { itemID = "(O)60", itemQuantity = 1 }, // Emerald
            new Prize { itemID = "(O)62", itemQuantity = 1 }, // Aquamarine
            new Prize { itemID = "(O)64", itemQuantity = 1 }, // Ruby
            new Prize { itemID = "(O)66", itemQuantity = 1 }, // Amethyst
            new Prize { itemID = "(O)68", itemQuantity = 1 }, // Topaz
            new Prize { itemID = "(O)384", itemQuantity = 5 }, // Gold Ore
            new Prize { itemID = "(O)486", itemQuantity = 1 }, // Starfruit Seeds
            new Prize { itemID = "(O)499", itemQuantity = 1 }, // Ancient Seeds
            new Prize { itemID = "(O)517", itemQuantity = 1 }, // Glow Ring
            new Prize { itemID = "(O)519", itemQuantity = 1 }, // Magnet Ring
            new Prize { itemID = "(O)919", itemQuantity = 3 }, // Deluxe Fertilizer
            new Prize { itemID = "(O)920", itemQuantity = 3 }, // Deluxe Retaining Soil

            new Prize { itemID = "(O)176", itemQuantity = 3 }, // White Egg
            new Prize { itemID = "(O)180", itemQuantity = 3 }, // Brown Egg
            new Prize { itemID = "(O)184", itemQuantity = 2 }, // Milk
            new Prize { itemID = "(O)430", itemQuantity = 1 }, // Truffle
            new Prize { itemID = "(O)436", itemQuantity = 2 }, // Goat Milk
            new Prize { itemID = "(O)440", itemQuantity = 1 }, // Wool
            new Prize { itemID = "(O)440", itemQuantity = 3 }, // Duck Egg
            new Prize { itemID = "(O)444", itemQuantity = 1 }, // Duck Feather
            new Prize { itemID = "(O)446", itemQuantity = 1 }, // Rabbit's Foot

            new Prize { itemID = "(B)508", itemQuantity = 1 }, // Combat Boots
            new Prize { itemID = "(B)509", itemQuantity = 1 }, // Tundra Boots
            new Prize { itemID = "(B)510", itemQuantity = 1 }, // Thermal Boots

            new Prize { itemID = "(O)MysteryBox", itemQuantity = 2 } // Mystery Box
        };

        public static Prize[] legendaryItems { get; set; } = new Prize[]
        {
            new Prize { itemID = "(O)72", itemQuantity = 1 }, // Diamond
            new Prize { itemID = "(O)373", itemQuantity = 1 }, // Golden Pumpkin
            new Prize { itemID = "(O)386", itemQuantity = 1 }, // Iridium Ore
            new Prize { itemID = "(O)529", itemQuantity = 1 }, // Amethyst Ring
            new Prize { itemID = "(O)530", itemQuantity = 1 }, // Topaz Ring
            new Prize { itemID = "(O)531", itemQuantity = 1 }, // Aquamarine Ring
            new Prize { itemID = "(O)532", itemQuantity = 1 }, // Jade Ring
            new Prize { itemID = "(O)533", itemQuantity = 1 }, // Emerald Ring
            new Prize { itemID = "(O)534", itemQuantity = 1 }, // Ruby Ring

            new Prize { itemID = "(W)10", itemQuantity = 1 }, // Claymore
            new Prize { itemID = "(W)15", itemQuantity = 1 }, // Forest Sword
            new Prize { itemID = "(W)18", itemQuantity = 1 }, // Burglar's Shank
            new Prize { itemID = "(W)19", itemQuantity = 1 }, // Shadow Dagger
            new Prize { itemID = "(W)27", itemQuantity = 1 }, // Wood Mallet
            new Prize { itemID = "(W)26", itemQuantity = 1 } // Lead Rod
        };
    }
}
