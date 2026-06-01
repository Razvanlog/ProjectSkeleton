using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities.Player;

namespace TheAdventure.Entities.Gun.Shotgun
{
    internal class Shotgun : Gun
    {
        public Shotgun(Player.Player player) : base(player)
        {
        }
        override public List<Projectile.Projectile> Shoot()
        {
            var bullets = new List<Projectile.Projectile>();

            if (cooldown <= 0f)
            {
                cooldown = 0.85f;

                for (int i = 0; i < 8; i++)
                {
                    float spawnX = player.X + (player.W / 2);
                    float spawnY = player.Y + (player.H / 2);

                    double spread = 30.0 * (Math.PI / 180f);
                    double angle = player.angle + spread*(Random.Shared.NextDouble()*2.0-1.0);
                    Projectile.Projectile bullet = new Projectile.Projectile(spawnX, spawnY, angle);
                    bullet.setDamage(6);
                    bullets.Add(bullet);
                }
            }
            return bullets;
        }
    }
}
