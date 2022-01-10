using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace extraUtility.Projectiles
{
    public class SilverBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Bullet");                    //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;                 //The ai style of the projectile, please reference the source code of Terraria
            projectile.width = 4;                   //The width of projectile hitbox
            projectile.height = 4;                  //The height of projectile hitbox
            projectile.friendly = true;             //Can the projectile deal damage to enemies?
            projectile.timeLeft = 600;              //Lifwspan of projectile; 600 = 10s
            projectile.ranged = true;               //Is the projectile shoot by a ranged weapon?
            projectile.alpha = 255;                 //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            projectile.extraUpdates = 1;            //bullet moves fast
            projectile.scale = 1.3f;
            projectile.light = 0.5f;                //How much light emit around the projectile
            aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
        }
        /**public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type == NPCID.Werewolf)
            {
                Main.NewText("A Werewolf has been hit");

                //NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, target.netID, (float)damage * 3, knockback, projectile.direction, 1, 0, 0);
            }
            else base.OnHitNPC(target, damage, knockback, crit);
        }/**/
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.type == NPCID.Werewolf)
            {
                damage *= 3;
                crit = true;
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
