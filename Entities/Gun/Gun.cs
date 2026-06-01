using System;
using System.Collections.Generic;
using System.Text;

namespace TheAdventure.Entities.Gun
{
    internal class Gun
    {
        protected float cooldown = 0f;
        protected readonly Player.Player player;
        protected float amount = 1;

        public Gun(Player.Player player)
        {
            this.player = player;
        }

        virtual public List<Projectile.Projectile> Shoot()
        {
            var bullets = new List<Projectile.Projectile>();

            if (cooldown <= 0f)
            {
                cooldown = 0.65f;

                float spawnX = player.X + (player.W / 2);
                float spawnY = player.Y + (player.H / 2);

                Projectile.Projectile bullet = new Projectile.Projectile(spawnX, spawnY, player.angle);
                bullets.Add(bullet);
            }
            return bullets;
        }

        public void Update(float delta)
        {
            if (cooldown > 0f)
            {
                cooldown -= delta;
            }
        }
    }
}
