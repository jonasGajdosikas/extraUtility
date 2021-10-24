using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using extraUtility.Items;

namespace extraUtility
{
	public class ExUtilGlobalNPC : GlobalNPC
	{
		public ExUtilGlobalNPC()
		{
		}
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            
            if (type == NPCID.WitchDoctor && (ModLoader.GetMod("AlchemistNPC") != null || ModLoader.GetMod("AlchemistNPCLite") != null))
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<DeathRecallPot>());
                shop.item[nextSlot].shopCustomPrice = 25000;
                ++nextSlot;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }
        
    }
}
