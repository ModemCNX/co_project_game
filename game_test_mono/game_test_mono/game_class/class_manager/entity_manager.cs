using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace old_heart
{
    public class entity_manager
    {
        public List<entity> entity_list = new List<entity> { }; //  all entity in this scene
        public entity_manager()
        {

        }
        public void add(entity entity)
        {
            entity_list.Add(entity);
        }
        public void remove(entity entity)
        {
            entity_list.Remove(entity);
        }
        public void update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            foreach (entity entity in entity_list) // update world entity
            {
                entity.Update(gameTime);
            }
        }
        public void draw(SpriteBatch sprite_batch)
        {
            foreach (entity entity in entity_list)
            {
                if (entity.visible)
                {
                    entity.Draw(sprite_batch);
                }
            }
        }
    }
}