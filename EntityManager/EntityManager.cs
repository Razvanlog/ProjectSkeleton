using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities;
using Silk.NET.SDL;
using System.ComponentModel;
using TheAdventure.Entities.Enemies;
using TheAdventure.Entities.Player;

namespace TheAdventure.EntityManager
{
    internal class EntityManager
    {
        public static EntityManager? Instance { get; private set; }
        private List<Entity> entities;
        

        public EntityManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            entities = new List<Entity>();
        }

        public void add(Entity entity)
        {
            entities.Add(entity);
        }

        public unsafe void render(Sdl sdl, Renderer* renderer, TheAdventure.Camera.Camera camera)
        {
            foreach (Entity it in entities)
            {
                it.Render(sdl, renderer, camera);
            }
        }

        public List<Entity> getEntities()
        {
            return entities;
        }
        
        public bool PlayerIsAlive()
        {
            return entities.OfType<Player>().Any();
        }

        public void CleanDeadEntities()
        {
            var deadEntities = entities.OfType<BasicEnemy>().Where(e => e.isDead()).ToList();
            foreach(Entity entity in deadEntities)
            {
                if (entity is BasicEnemy enemy && entity.isDead())
                {
                    TheAdventure.Program.score += enemy.getScore();
                }
            }
            entities.RemoveAll(entity => entity.isDead());
        }

        public void CheckAndRunInteractions()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = 0; j < entities.Count; j++)
                {
                    if (i != j)
                    {
                        entities[i].interacts(entities[j]);
                        entities[j].interacts(entities[i]);
                    }
                }
            }
        }

        public int getEnemyCount()
        {
            int count = 0;
            for (int i=0; i< entities.Count; i++)
            {
                if (entities[i] is BasicEnemy e)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
