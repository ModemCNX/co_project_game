using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoGame.Extended.Input;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace old_heart
{

    public class debug_manager
    {
        public List<node> debug_node_list = new List<node> { };
        public bool debug_enabled = false;
        public debug_manager()
        {

        }
        public void add(node node)
        {
            debug_node_list.Add(node);
        }
        public void remove(node node)
        {
            debug_node_list.Remove(node);
        }
        public void update(GameTime gameTime)
        {
            KeyboardStateExtended keyboard_state = global.input.keyboard_state;
            if (keyboard_state.WasKeyPressed(Keys.P))
            {
                if (debug_enabled) {
                    Debug.WriteLine("hide debug node");
                    debug_enabled = false;
                }else if (debug_enabled == false)
                {
                    Debug.WriteLine("show debug node");
                    debug_enabled = true;
                }

            }
        }

        public void draw(SpriteBatch sprite_batch)
        {
            if (debug_enabled == false)
            {
                return;
            }
            foreach (node node in debug_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
        }
    }
}