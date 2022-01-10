using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace extraUtility.Items
{
	public class HomingEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Emblem"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Coats all your bullets with chlorophyte\nThey now home in on enemies\n10% increased ranged damage");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.accessory = true;
			item.value = Item.sellPrice(0, 14, 76);
			item.rare = ItemRarityID.Pink;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rangedDamage += 0.1f;
			player.GetModPlayer<ExUtilPlayer>().HomingBullets = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RangerEmblem);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 16);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
