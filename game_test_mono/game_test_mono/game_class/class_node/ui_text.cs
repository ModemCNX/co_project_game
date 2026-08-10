using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace old_heart
{
    public class ui_text : node
    {
        public SpriteFont text_font;
        public Color text_color = Color.Black;
        public Vector2 text_position;
        public string text_string;

        public bool clicked = false;

        public ui_text(string text_string_set,SpriteFont font, Vector2 position)
        {
            text_string = text_string_set;
            text_font = font;
            text_font.Spacing = 1f;
            text_position = position;
        }
        public ui_text(string text_string_set, SpriteFont font, Vector2 position,Color color)
        {
            text_string = text_string_set;
            text_font = font;
            text_position = position;
            text_color = color;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.DrawString(text_font,text_string, text_position, text_color);
        }

    }
}
