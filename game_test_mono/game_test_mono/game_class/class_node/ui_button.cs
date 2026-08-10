using Microsoft.Xna.Framework;
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

        public ui_button(GraphicsDevice graphics_device ,Rectangle rect)
        {
            button_rect = rect;

            button_texture = new Texture2D(graphics_device, 1, 1);
            //  Set the single pixel's data to pure white
            button_texture.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            MouseStateExtended mouse_state = MouseExtended.GetState();

            bool mouse_hover = button_rect.Contains(new Point(mouse_state.Position.X, mouse_state.Position.Y));
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
