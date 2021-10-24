using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace extraUtility.Items
{
	public class MoneyAbsorber : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Scrooge's Ring"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Effects of Greedy Ring\nPuts all money dropped into your inventory\nHide visual to disable this effect");
		}

		public override void SetDefaults() 
		{
			item.width = 28;
			item.height = 20;
			item.accessory = true;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.goldRing = true;
			player.coins = true;
			player.discount = true;
			if (!hideVisual){
				for (int number = 0; number < 400; ++number)
				{
					if (Main.item[number].active && Main.item[number].type >= ItemID.CopperCoin && Main.item[number].type <= ItemID.PlatinumCoin)
					{
						Main.item[number] = player.GetItem(player.whoAmI, Main.item[number], false, false);
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number);
						}
					}
				}
			}
		}
		
		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GreedyRing);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddRecipeGroup("ExUtil:EvilMaterial", 15);
			recipe.AddRecipeGroup("ExUtil:AnyLivingFire", 100);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}