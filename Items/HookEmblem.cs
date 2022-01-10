using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace extraUtility.Items
{
    class HookEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hook Enhancer");
            Tooltip.SetDefault("The essence of all early hooks, combined\n"
                             + "Increases your reach by 4 tiles\n"
                             + "Increases hook speed by 50%");
        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.accessory = true;
            item.value = Item.sellPrice(0, 2);
            item.rare = ItemRarityID.Orange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExUtilPlayer>().GrappleAcc = true;
            //player.GetModPlayer<ExUtilPlayer>().grappleRangeIncrease = 64f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GrapplingHook);
            recipe.AddRecipeGroup("ExUtil:AnyGemHook");
            recipe.AddIngredient(ItemID.SlimeHook);
            recipe.AddIngredient(ItemID.IvyWhip);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
