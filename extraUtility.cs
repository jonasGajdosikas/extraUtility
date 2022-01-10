using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using extraUtility.Items;

namespace extraUtility
{
	public class extraUtility : Mod
	{
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Crafting Material", new int[] { ItemID.Ichor, ItemID.CursedFlame });
			RecipeGroup.RegisterGroup("ExUtil:EvilMaterial", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Living Fire", new int[] { 
				ItemID.LivingFireBlock, ItemID.LivingCursedFireBlock, ItemID.LivingDemonFireBlock,
				ItemID.LivingFrostFireBlock, ItemID.LivingIchorBlock, ItemID.LivingUltrabrightFireBlock });
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
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Non-Gilded Gravestone", graves);
			RecipeGroup.RegisterGroup("ExUtil:AnyPoorTomb", group);

			int[] richGraves = new int[]
			{
				ItemID.RichGravestone1,
				ItemID.RichGravestone2,
				ItemID.RichGravestone3,
				ItemID.RichGravestone4,
				ItemID.RichGravestone5
			};
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gilded Gravestone", richGraves);
			RecipeGroup.RegisterGroup("ExUtil:AnyRichTomb", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Magic Mirror", new int[] { ItemID.MagicMirror, ItemID.IceMirror });
			RecipeGroup.RegisterGroup("ExUtil:AnyMirror", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bullet", new int[] { ItemID.SilverBullet, ModContent.ItemType<BlessedSilverBullet>() });
			RecipeGroup.RegisterGroup("ExUtil:AnySilverBullet", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gem Hook", new int[] { ItemID.AmethystHook, ItemID.TopazHook, ItemID.SapphireHook, ItemID.EmeraldHook, ItemID.RubyHook, ItemID.DiamondHook });
			RecipeGroup.RegisterGroup("ExUtil:AnyGemHook", group);

		}

        public override void Load()
        {
            base.Load();
        }
    }
}