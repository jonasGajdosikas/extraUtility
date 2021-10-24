using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace extraUtility.Items
{
	public class DeathMirror : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grave Mirror"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Gaze in the mirror to return to your last death point");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MagicMirror);
            item.useAnimation = 90;
            item.useTime = 90;
			item.maxStack = 1;
			item.consumable = false;
			return;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("ExUtil:AnyMirror");
			recipe.AddRecipeGroup("ExUtil:AnyRichTomb", 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            return player.lastDeathPostion != Vector2.Zero;
        }

        public override bool UseItem(Player player)
        {
            /**
            if (Main.rand.Next(2) == 0)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, 0f, 0f, 150, new Color(), 1.1f);
            }
            if (player.itemTime == 0) player.itemTime = 90;
            //else if (player.itemTime == 45)
            {
                for (int index = 0; index < 70; ++index)
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, new Color(), 1.5f);
                    Main.dust[d].velocity *= 4f;
                    Main.dust[d].noGravity = true;
                }
                player.grappling[0] = -1;
                player.grapCount = 0;
                for (int index = 0; index < 1000; ++index)
                {
                    if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI && Main.projectile[index].aiStyle == 7)
                        Main.projectile[index].Kill();
                }

                if (player.whoAmI == Main.myPlayer)
                {
                    player.Teleport(player.lastDeathPostion, 1);
                    player.velocity = Vector2.Zero;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y - 16, 1);
                }

                for (int index = 0; index < 70; ++index)
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, 0.0f, 0.0f, 150, new Color(), 1.5f);
                    Main.dust[d].velocity *= 4f;
                    Main.dust[d].noGravity = true;
                }
            } /**/
            return true;
        }
    }
}
