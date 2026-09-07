using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace old_heart
{
    public class image : node
    {
        public Texture2D texture;
        public Vector2 position;

        public image(ContentManager content, Vector2 position, string file_path)
        {
            this.position = position;
            this.texture = content.Load<Texture2D>(file_path);
            this.texture.Name = file_path;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(texture,position,Color.White);
        }

    }
}
