using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using extraUtility.Items;

namespace extraUtility
{
    public class ExUtilPlayer : ModPlayer
    {
        public ExUtilPlayer()
        {
        }
        public override void PreUpdate()
        {
            base.PreUpdate();

            Item item = player.inventory[player.selectedItem];
            if (item.type == ModContent.ItemType<DeathMirror>() && player.itemAnimation > 0)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, 0f, 0f, 150, new Color(), 1.1f);
                }
                if (player.itemTime == 0) player.itemTime = 90;
                else if (player.itemTime == 45)
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
                }
            }
            if (item.type == ModContent.ItemType<DeathRecallPot>() && player.itemAnimation > 0)
            {
                if (player.itemTime == 0)
                {
                    player.itemTime = 30;
                }
                else if (player.itemTime == 2)
                {
                    for (int index = 0; index < 70; ++index)
                    {
                        int d = Dust.NewDust(player.position, player.width, player.height, DustID.AmethystBolt, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, new Color(), 1.5f);
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
                        int d = Dust.NewDust(player.position, player.width, player.height, DustID.AmethystBolt, 0.0f, 0.0f, 150, new Color(), 1.5f);
                        Main.dust[d].velocity *= 4f;
                        Main.dust[d].noGravity = true;
                    }
                    if (ItemLoader.ConsumeItem(item, player) && item.stack > 0)
                    {
                        item.stack--;
                    }
                }
            }
        }
    }
}
