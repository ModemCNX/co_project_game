using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace old_heart
{

    public class ui_manager
    {
        public List<node> ui_node_list = new List<node> { }; //  ui
        public ui_manager()
        {

        }
        public void add(node node)
        {
            ui_node_list.Add(node);
        }
        public void remove(node node)
        {
            ui_node_list.Remove(node);
        }
        public void update(GameTime gameTime)
        {
            foreach (node node in ui_node_list) // update ui
            {
                node.Update(gameTime);
            }
        }

        public void draw(SpriteBatch sprite_batch)
        {
            foreach (node node in ui_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
        }
    }
}