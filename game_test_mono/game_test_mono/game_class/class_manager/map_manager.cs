using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace old_heart
{
    public class map_manager
    {
        public List<node> map_node_list = new List<node> { }; //  world object (wall)
        public List<node> high_map_node_list = new List<node> { }; //  world object but draw above (player enemy and all particle)
        public map_manager()
        {

        }
        public void add(node node , bool high_ground = false)
        {
            if (high_ground) { 
                high_map_node_list.Add(node);
            }
            else
            {
                map_node_list.Add(node);
            }
        }
        public void update(GameTime gameTime)  
        {
            Debug.WriteLine("error map_manager got updated tell modem to fix this it is not suppose to update");
            // map will not update lol
        }
        public void draw_low(SpriteBatch sprite_batch) //  world object (wall) 
        {
            foreach (node node in map_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
        }
        public void draw_high(SpriteBatch sprite_batch) //  world object but draw above (player enemy and all particle)
        {
            foreach (node node in high_map_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
        }
    }
}