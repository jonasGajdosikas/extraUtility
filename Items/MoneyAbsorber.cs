using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace extraUtility.Items
{
	public class MoneyAbsorber : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("MoneyAbsorber"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Effects of Greedy Ring /nAlso puts all money dropped into your inventory. Instantly");
		}

		public override void SetDefaults() 
		{
			item.width = 28;
			item.height = 20;
			item.accessory = true;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
		}
		public override void UpdateInventory(Player player)
		{
			for (int number = 0; number < 400; ++number)
			{
				if (Main.item[number].active && Main.item[number].type == ItemID.CopperCoin)
				{
					Main.item[number] = player.GetItem(player.whoAmI, Main.item[number], false, false);
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				else if (Main.item[number].active && Main.item[number].type == ItemID.SilverCoin)
				{
					Main.item[number] = player.GetItem(player.whoAmI, Main.item[number], false, false);
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				else if (Main.item[number].active && Main.item[number].type == ItemID.GoldCoin)
				{
					Main.item[number] = player.GetItem(player.whoAmI, Main.item[number], false, false);
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				else if (Main.item[number].active && Main.item[number].type == ItemID.PlatinumCoin)
				{
					Main.item[number] = player.GetItem(player.whoAmI, Main.item[number], false, false);
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.goldRing = true;
			player.coins = true;
			player.discount = true;
		}
		public static void AddRecipeGroups()
        {
			RecipeGroup group = new RecipeGroup(() => "Any Evil Crafting Material", new int[] { ItemID.Ichor, ItemID.CursedFlame });
			RecipeGroup.RegisterGroup("ExUtil:EvilMaterial", group);

			group = new RecipeGroup(() => "Any living fire", new int[] { ItemID.LivingFireBlock, ItemID.LivingCursedFireBlock, 
																		 ItemID.LivingDemonFireBlock, ItemID.LivingFrostFireBlock,
																		 ItemID.LivingIchorBlock, ItemID.LivingUltrabrightFireBlock });
			RecipeGroup.RegisterGroup("ExUtil:AnyLivingFire", group);
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