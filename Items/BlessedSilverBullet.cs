using Terraria.ModLoader;
using Terraria.ID;

namespace extraUtility.Items
{
	public class BlessedSilverBullet : ModItem
	{
		private readonly Mod Fargowiltas = ModLoader.GetMod("Fargowiltas");
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blessed Silver Bullet");
            Tooltip.SetDefault("It always crits and deals triple damage to werewolves");
        }
		public override void SetDefaults()
		{
			item.damage = 9;
			item.ranged = true;
			item.width = 12;
			item.height = 12;
			item.maxStack = 9999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 3f;
			item.value = 15;
			item.rare = ItemRarityID.White;
			item.shoot = ModContent.ProjectileType<Projectiles.SilverBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;              //The ammo class this ammo belongs to.
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilverBullet, 70);
			recipe.SetResult(this, 70);
			recipe.AddRecipe();

			if (ModLoader.GetMod("Fargowiltas") != null)
            {
				recipe = new ModRecipe(mod);
				recipe.AddRecipeGroup("ExUtil:AnySilverBullet", 3996);
				recipe.AddTile(TileID.CrystalBall);
				recipe.SetResult(Fargowiltas , "SilverPouch", 1);
				recipe.AddRecipe();
            }
		}
	}
}