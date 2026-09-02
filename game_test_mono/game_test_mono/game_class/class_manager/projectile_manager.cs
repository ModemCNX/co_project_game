using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace old_heart
{
    public class projectile_manager
    {

        public List<projectile> projectile_list = new List<projectile> { }; //  all projectile in this scene
        public projectile_manager()
        {

        }
        public void add(projectile projectile)
        {
            projectile_list.Add(projectile);
        }
        public void remove(projectile projectile)
        {
            projectile_list.Remove(projectile);
        }
        public void update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (projectile projectile in projectile_list) // update world projectile
            {
                projectile.Update(gameTime);
            }
        }
        public void draw(SpriteBatch sprite_batch)
        {
            foreach (projectile projectile in projectile_list)
            {
                if (projectile.visible)
                {
                    projectile.Draw(sprite_batch);
                }
            }
        }
    }
}