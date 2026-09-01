using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

namespace old_heart
{
    public class ui_button : node
    {
        Texture2D button_texture;
        Rectangle button_rect = new Rectangle(0,0,200,100);
        Color button_color = Color.White;
        Color base_color = Color.White;
        Color hover_color = Color.Gray;
        Color hold_color = Color.Black;
        public bool clicked = false;

        public ui_button(ContentManager content ,Rectangle rect)
        {
            button_rect = rect;

            button_texture = content.Load<Texture2D>("image/white_pixel");
        }

        public override void Update(GameTime gameTime)
        {
            if (!visible) return; // make it not work when it is invisible

            MouseStateExtended mouse_state = global.input.mouse_state;
            Point mouse_position = global.input.scaled_mouse_position;
            bool mouse_hover = button_rect.Contains(mouse_position);

            clicked = false; // reset clicked every frame

            if (mouse_hover) // mouse inside button rect
            {
                button_color = hover_color;
                if (mouse_state.WasButtonPressed(MouseButton.Left))  // left click detected XD
                {
                    clicked = true;
                }else if (mouse_state.IsButtonDown(MouseButton.Left))
                {
                    button_color = hold_color;
                }
            }
            else
            {
                button_color = base_color;
            }

        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(button_texture,button_rect,button_color);
        }

    }
}
