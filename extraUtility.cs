using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace extraUtility
{
	public class extraUtility : Mod
	{
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => "Any Evil Crafting Material", new int[] { ItemID.Ichor, ItemID.CursedFlame });
			RecipeGroup.RegisterGroup("ExUtil:EvilMaterial", group);

			group = new RecipeGroup(() => "Any living fire", new int[] { ItemID.LivingFireBlock, ItemID.LivingCursedFireBlock,
																		 ItemID.LivingDemonFireBlock, ItemID.LivingFrostFireBlock,
																		 ItemID.LivingIchorBlock, ItemID.LivingUltrabrightFireBlock });
			RecipeGroup.RegisterGroup("ExUtil:AnyLivingFire", group);

			int[] graves = new int[]
			{
				ItemID.Tombstone,
				ItemID.GraveMarker,
				ItemID.CrossGraveMarker,
				ItemID.Headstone,
				ItemID.Gravestone,
				ItemID.Obelisk
			};
			group = new RecipeGroup(() => "Any gravestone", graves);
			RecipeGroup.RegisterGroup("ExUtil:AnyTomb", group);

			int[] richGraves = new int[]
			{
				ItemID.RichGravestone1,
				ItemID.RichGravestone2,
				ItemID.RichGravestone3,
				ItemID.RichGravestone4,
				ItemID.RichGravestone5
			};
			group = new RecipeGroup(() => "Any gilded gravestone", richGraves);
			RecipeGroup.RegisterGroup("ExUtil:AnyRichTomb", group);
		}
	}
}