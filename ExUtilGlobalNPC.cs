using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using extraUtility.Items;
using Microsoft.Xna.Framework;

namespace extraUtility
{
	public class ExUtilGlobalNPC : GlobalNPC
	{
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            
            if (type == NPCID.WitchDoctor)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<DeathRecallPot>());
                shop.item[nextSlot].shopCustomPrice = 25000;
                ++nextSlot;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }
        
    }

    class ModGlobalProjectile : GlobalProjectile
    {
		private bool firstTick = true;
		public override bool InstancePerEntity => true;
		public override void AI(Projectile projectile)
		{
			if (!projectile.npcProj)
			{
				var modPlayer = Main.player[projectile.owner]?.GetModPlayer<ExUtilPlayer>();
				#region [homing_bullet]
				if (modPlayer.HomingBullets && projectile.ranged && IsBullet(projectile))
				{
					float projVel = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
					float homeVel = projectile.localAI[0];          //apparently localAI[0] holds the homing velocity of the projectile
					if (homeVel == 0f)
					{
						projectile.localAI[0] = projVel;
						homeVel = projVel;
					}
					if (projectile.alpha > 0)
					{
						projectile.alpha -= 25;
					}
					if (projectile.alpha < 0)
					{
						projectile.alpha = 0;
					}
					float posX = projectile.position.X;
					float posY = projectile.position.Y;
					float maxChaseDist = 300f;
					bool foundTarget = false;
					int targetIndex = 0;
					if (projectile.ai[1] == 0f)                     //projectile.ai[0] holds the target index
					{
						for (int npcIDiterator = 0; npcIDiterator < 200; npcIDiterator++)
						{
							if (Main.npc[npcIDiterator].CanBeChasedBy(projectile, false) && (projectile.ai[1] == 0f || projectile.ai[1] == (float)(npcIDiterator + 1)))
							{
								float targetX = Main.npc[npcIDiterator].position.X + (float)(Main.npc[npcIDiterator].width / 2);
								float targetY = Main.npc[npcIDiterator].position.Y + (float)(Main.npc[npcIDiterator].height / 2);
								float distToTarget = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - targetX) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - targetY);
								if (distToTarget < maxChaseDist && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[npcIDiterator].position, Main.npc[npcIDiterator].width, Main.npc[npcIDiterator].height))
								{
									maxChaseDist = distToTarget;
									posX = targetX;
									posY = targetY;
									foundTarget = true;
									targetIndex = npcIDiterator;
								}
							}
						}
						if (foundTarget)
						{
							projectile.ai[1] = (float)(targetIndex + 1);
						}
						foundTarget = false;
					}
					if (projectile.ai[1] > 0f)
					{
						int targetNPCIndex = (int)(projectile.ai[1] - 1f);
						if (Main.npc[targetNPCIndex].active && Main.npc[targetNPCIndex].CanBeChasedBy(projectile, true) && !Main.npc[targetNPCIndex].dontTakeDamage)
						{
							float targetX = Main.npc[targetNPCIndex].position.X + (float)(Main.npc[targetNPCIndex].width / 2);
							float targetY = Main.npc[targetNPCIndex].position.Y + (float)(Main.npc[targetNPCIndex].height / 2);
							if (Math.Abs(projectile.position.X + (float)(projectile.width / 2) - targetX) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - targetY) < 1000f)
							{
								foundTarget = true;
								posX = Main.npc[targetNPCIndex].position.X + (float)(Main.npc[targetNPCIndex].width / 2);
								posY = Main.npc[targetNPCIndex].position.Y + (float)(Main.npc[targetNPCIndex].height / 2);
							}
						}
						else
						{
							projectile.ai[1] = 0f;
						}
					}
					if (!projectile.friendly)
						foundTarget = false;
					if (foundTarget)
					{
						Vector2 projectileCenter = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
						float distX = posX - projectileCenter.X;
						float distY = posY - projectileCenter.Y;
						float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));
						dist = homeVel / dist;
						distX *= dist;
						distY *= dist;
						projectile.velocity.X = (projectile.velocity.X * 7f + distX) / 8f;
						projectile.velocity.Y = (projectile.velocity.Y * 7f + distY) / 8f;
					}
				}
				#endregion
				#region [hook range extension]
				/**
				else if (projectile.aiStyle == 7)
				{
                    if (Main.player[projectile.owner].dead ||
						Main.player[projectile.owner].stoned ||
						Main.player[projectile.owner].webbed ||
						Main.player[projectile.owner].frozen)
					{
						projectile.Kill();
						return;
					}
					Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
					Vector2 projectileCenter = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float distanceX = mountedCenter.X - projectileCenter.X;
					float distanceY = mountedCenter.Y - projectileCenter.Y;
					float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
					projectile.rotation = (float)Math.Atan2(distanceY, distanceX) - 1.57f;
					#region [hook graphics]
					if (projectile.type == ProjectileID.SkeletronHand)
					{
						projectile.rotation = (float)Math.Atan2(distanceY, distanceX) + 3.92500019f;
					}
					if (projectile.type == ProjectileID.AntiGravityHook)
					{
						Lighting.AddLight(mountedCenter, 0f, 0.4f, 0.3f);
						projectile.localAI[0]++;
						if (projectile.localAI[0] >= 28f)
						{
							projectile.localAI[0] = 0f;
						}
						DelegateMethods.v3_1 = new Vector3(0f, 0.4f, 0.3f);
						Utils.PlotTileLine(projectile.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
					}
					if (projectile.type == ProjectileID.StaticHook && ++projectile.frameCounter >= 7)
					{
						projectile.frameCounter = 0;
						if (++projectile.frame >= Main.projFrames[projectile.type])
						{
							projectile.frame = 0;
						}
					}
                    if (projectile.type >= ProjectileID.LunarHookSolar && projectile.type <= ProjectileID.LunarHookStardust)
					{
						Vector3 projectileColor = Vector3.Zero;
						switch (projectile.type)
						{
							case ProjectileID.LunarHookSolar :
								projectileColor = new Vector3(0.7f, 0.5f, 0.1f);
								break;
							case ProjectileID.LunarHookVortex :
								projectileColor = new Vector3(0f, 0.6f, 0.7f);
								break;
							case ProjectileID.LunarHookNebula :
								projectileColor = new Vector3(0.6f, 0.2f, 0.6f);
								break;
							case ProjectileID.LunarHookStardust :
								projectileColor = new Vector3(0.6f, 0.6f, 0.9f);
								break;
						}
						Lighting.AddLight(mountedCenter, projectileColor);
						Lighting.AddLight(projectile.Center, projectileColor);
						DelegateMethods.v3_1 = projectileColor;
						Utils.PlotTileLine(projectile.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
					}
					#endregion
					// ai[0] is 0 when the hook is extending
					if (projectile.ai[0] == 0f)
					{
						distance -= modPlayer.grappleRangeIncrease;
						Main.NewText("distance is " + distance.ToString());
						//Main.NewText("decreased distance by " + modPlayer.grappleRangeIncrease.ToString()+ " units");
						// sets ai[0] t0 1f if out of range
						#region [check for max hook distance]
						if ((distance > 300f && projectile.type == ProjectileID.Hook ) ||
							(distance > 400f && projectile.type == ProjectileID.IvyWhip ) ||
							(distance > 440f && projectile.type == ProjectileID.DualHookBlue ) ||
							(distance > 440f && projectile.type == ProjectileID.DualHookRed ) ||
							(distance > 250f && projectile.type == ProjectileID.Web ) ||
							(distance > 350f && projectile.type == ProjectileID.SkeletronHand ) ||
							(distance > 500f && projectile.type == ProjectileID.BatHook ) ||
							(distance > 550f && projectile.type == ProjectileID.WoodHook ) ||
							(distance > 400f && projectile.type == ProjectileID.CandyCaneHook ) ||
							(distance > 550f && projectile.type == ProjectileID.ChristmasHook ) ||
							(distance > 400f && projectile.type == ProjectileID.FishHook ) ||
							(distance > 300f && projectile.type == ProjectileID.SlimeHook ) ||
							(distance > 550f && projectile.type >= ProjectileID.LunarHookSolar  && projectile.type <= ProjectileID.LunarHookStardust ) ||
							(distance > 600f && projectile.type == ProjectileID.StaticHook ) ||
							(distance > 480f && projectile.type >= ProjectileID.TendonHook  && projectile.type <= ProjectileID.WormHook ) ||
							(distance > 500f && projectile.type == ProjectileID.AntiGravityHook ))
						{
							projectile.ai[0] = 1f;
						}
						else if (projectile.type >= ProjectileID.GemHookAmethyst  && projectile.type <= ProjectileID.GemHookDiamond )
						{
							int maxHookDist = 300 + (projectile.type - 230) * 30;
							if (distance > (float)maxHookDist)
							{
								projectile.ai[0] = 1f;
							}
						}
						else if (ProjectileLoader.GrappleOutOfRange(distance, projectile))
						{
							projectile.ai[0] = 1f;
						}
						distance += modPlayer.grappleRangeIncrease;
						#endregion

						Vector2 topLeftCorner = projectile.Center - new Vector2(5f);
						Vector2 botRightCorner = projectile.Center + new Vector2(5f);
						Point topLeftPoint = (topLeftCorner - new Vector2(16f)).ToTileCoordinates();
						Point botRightPoint = (botRightCorner + new Vector2(32f)).ToTileCoordinates();

						int left = topLeftPoint.X;
						int right = botRightPoint.X;
						int top = topLeftPoint.Y;
						int bot = botRightPoint.Y;
						if (left < 0)
							left = 0;
						if (right > Main.maxTilesX)
							right = Main.maxTilesX;
						if (top < 0)
							top = 0;
						if (bot > Main.maxTilesY)
							bot = Main.maxTilesY;
						Vector2 pos = default(Vector2);
						// checks for solid tiles within distance
						for (int xPos = left; xPos < right; xPos++)
						{
							for (int yPos = top; yPos < bot; yPos++)
							{
								if (Main.tile[xPos, yPos] == null)
								{
									Main.tile[xPos, yPos] = new Tile();
								}
								pos.X = xPos * 16;
								pos.Y = yPos * 16;
								if (!(topLeftCorner.X + 10f > pos.X) ||
									!(topLeftCorner.X < pos.X + 16f) ||
									!(topLeftCorner.Y + 10f > pos.Y) ||
									!(topLeftCorner.Y < pos.Y + 16f) ||
									!Main.tile[xPos, yPos].nactive() ||
									(!Main.tileSolid[Main.tile[xPos, yPos].type] &&
										Main.tile[xPos, yPos].type != TileID.MinecartTrack ) || //minecart track is the only non-solid tile to grapple to
									(projectile.type == 403 &&
										Main.tile[xPos, yPos].type != TileID.MinecartTrack ))

								{
									continue;
								}
								if (Main.player[projectile.owner].grapCount < 10)
								{
									Main.player[projectile.owner].grappling[Main.player[projectile.owner].grapCount] = projectile.whoAmI;
									Main.player[projectile.owner].grapCount++;
									//
								}
								if (Main.myPlayer == projectile.owner)
								{
									int numLiveHooks = 0;
									int oldestHookID = -1;
									int oldestHookTimeLeft = 100000;
									// dual hook kills older hook upon latching 
									if (projectile.type == ProjectileID.DualHookBlue  || projectile.type == ProjectileID.DualHookRed )
									{
										for (int hookProjIDIterator = 0; hookProjIDIterator < 1000; hookProjIDIterator++)
										{
											if (hookProjIDIterator != projectile.whoAmI && Main.projectile[hookProjIDIterator].active && Main.projectile[hookProjIDIterator].owner == projectile.owner && Main.projectile[hookProjIDIterator].aiStyle == 7 && Main.projectile[hookProjIDIterator].ai[0] == 2f)
											{
												Main.projectile[hookProjIDIterator].Kill();
											}
										}
									}
									else
									{
										int maxHooks = 3;
										if (projectile.type == ProjectileID.Web )
											maxHooks = 8;
										if (projectile.type == ProjectileID.SkeletronHand )
											maxHooks = 2;
										if (projectile.type == ProjectileID.FishHook )
											maxHooks = 2;
										if (projectile.type == ProjectileID.StaticHook )
											maxHooks = 1;
										if (projectile.type >= ProjectileID.LunarHookSolar  && projectile.type <= ProjectileID.LunarHookStardust )
											maxHooks = 4;
										ProjectileLoader.NumGrappleHooks(projectile, Main.player[projectile.owner], ref maxHooks);
										for (int projIterator = 0; projIterator < 1000; projIterator++)
										{
											if (Main.projectile[projIterator].active && Main.projectile[projIterator].owner == projectile.owner && Main.projectile[projIterator].aiStyle == 7)
											{
												if (Main.projectile[projIterator].timeLeft < oldestHookTimeLeft)
												{
													oldestHookID = projIterator;
													oldestHookTimeLeft = Main.projectile[projIterator].timeLeft;
												}
												numLiveHooks++;
											}
										}
										if (numLiveHooks > maxHooks)
										{
											Main.projectile[oldestHookID].Kill();
										}
									}
								}
								WorldGen.KillTile(xPos, yPos, fail: true, effectOnly: true);
								Main.PlaySound(SoundID.Dig, xPos * 16, yPos * 16);
								projectile.velocity.X = 0f;
								projectile.velocity.Y = 0f;
								projectile.ai[0] = 2f;
								projectile.position.X = xPos * 16 + 8 - projectile.width / 2;
								projectile.position.Y = yPos * 16 + 8 - projectile.height / 2;
								projectile.damage = 0;
								projectile.netUpdate = true;
								if (Main.myPlayer == projectile.owner)
								{
									NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, projectile.owner);
								}
								break;
							}
							if (projectile.ai[0] == 2f)
							{
								break;
							}
						}
					}
					// ai[0] is 1 if the hook is retreating
					else if (projectile.ai[0] == 1f)
					{
						float retreatSpeed = 11f;
						if (projectile.type == ProjectileID.IvyWhip )
							retreatSpeed = 15f;
						if (projectile.type == ProjectileID.DualHookBlue || projectile.type == ProjectileID.DualHookRed)
							retreatSpeed = 17f;
						if (projectile.type == ProjectileID.BatHook )
							retreatSpeed = 20f;
						if (projectile.type == ProjectileID.WoodHook )
							retreatSpeed = 22f;
						if (projectile.type >= ProjectileID.GemHookAmethyst  && projectile.type <= ProjectileID.GemHookDiamond )
							retreatSpeed = 11f + (float)(projectile.type - 230) * 0.75f;
						if (projectile.type == ProjectileID.AntiGravityHook )
							retreatSpeed = 20f;
						if (projectile.type >= ProjectileID.TendonHook  && projectile.type <= ProjectileID.WormHook )
							retreatSpeed = 18f;
						if (projectile.type >= ProjectileID.LunarHookSolar  && projectile.type <= ProjectileID.LunarHookStardust)
							retreatSpeed = 24f;
						if (projectile.type == ProjectileID.StaticHook )
							retreatSpeed = 24f;
						if (projectile.type == ProjectileID.ChristmasHook )
							retreatSpeed = 17f;

						ProjectileLoader.GrappleRetreatSpeed(projectile, Main.player[projectile.owner], ref retreatSpeed);
						if (distance < 24f)
						{
							projectile.Kill();
						}
						distance = retreatSpeed / distance; //inverse of return time
						distanceX *= distance; // same as dividing by return time
						distanceY *= distance;
						projectile.velocity.X = distanceX;
						projectile.velocity.Y = distanceY;
					}
					// ai[0] is 2 when grappled
					else
					{
						if (projectile.ai[0] != 2f) return;
						int left = (int)(projectile.position.X / 16f) - 1;
						int right = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
						int top = (int)(projectile.position.Y / 16f) - 1;
						int bottom = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
						if (left < 0) left = 0;
						if (right > Main.maxTilesX) right = Main.maxTilesX;
						if (top < 0) top = 0;
						if (bottom > Main.maxTilesY) bottom = Main.maxTilesY;
						bool illegalGrapple = true;
						Vector2 tilePos = default(Vector2);
						for (int xPos = left; xPos < right; xPos++)
						{
							for (int yPos = top; yPos < bottom; yPos++)
							{
								if (Main.tile[xPos, yPos] == null)
								{
									Main.tile[xPos, yPos] = new Tile();
								}
								tilePos.X = xPos * 16;
								tilePos.Y = yPos * 16;
								if (projectile.position.X + (float)(projectile.width / 2) > tilePos.X &&
									projectile.position.X + (float)(projectile.width / 2) < tilePos.X + 16f &&
									projectile.position.Y + (float)(projectile.height / 2) > tilePos.Y &&
									projectile.position.Y + (float)(projectile.height / 2) < tilePos.Y + 16f &&
									Main.tile[xPos, yPos].nactive() &&
									(Main.tileSolid[Main.tile[xPos, yPos].type] ||
									 Main.tile[xPos, yPos].type == TileID.MinecartTrack  ||
									 Main.tile[xPos, yPos].type == TileID.Trees ))
									illegalGrapple = false;
							}
						}
						if (illegalGrapple)
						{
							projectile.ai[0] = 1f;
						}
						else if (Main.player[projectile.owner].grapCount < 10)
						{
							Main.player[projectile.owner].grappling[Main.player[projectile.owner].grapCount] = projectile.whoAmI;
							Main.player[projectile.owner].grapCount++;
						}
					}
				}
				*/
				#endregion
			}

		}
        public override bool PreAI(Projectile projectile)
        {
			ExUtilPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<ExUtilPlayer>();
			if (projectile.owner == Main.myPlayer)
            {
				if (firstTick)
                {
					if (modPlayer.GrappleAcc && projectile.aiStyle == 7)
                    {
						projectile.extraUpdates = 1;
                    }
                }
            }
            return true;
        }
        bool IsBullet(Projectile projectile)
		{

			return (projectile.friendly && projectile.ranged && !projectile.arrow)
				&& Main.player[projectile.owner].HeldItem.useAmmo == AmmoID.Bullet;     // am using projectile instead of ⟨projectile.AIStyle == 1⟩ because of modded bullets
		}
		public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
		{
			ExUtilPlayer utilPlayer = player.GetModPlayer<ExUtilPlayer>();
			if (utilPlayer.GrappleAcc)
			{
				speed *= 1.5f;
			}
		}
		public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
		{
			ExUtilPlayer utilPlayer = player.GetModPlayer<ExUtilPlayer>();
			if (utilPlayer.GrappleAcc)
			{
				speed *= 1.5f;
			}
		}
	}
}
