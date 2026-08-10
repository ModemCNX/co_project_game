using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace old_heart
{
    public sealed class wall_collision :node, ICollisionActor
    {
        public int Id => GetHashCode(); // for collision // id of entity is it's unique hash code
        public CollisionShape2D Shape { get; }
        GraphicsDevice graphics_device;

        public wall_collision(GraphicsDevice graphics_device, BoundingBox2D bounds)
        {
            this.graphics_device = graphics_device;
            Shape = new CollisionShape2D(bounds);
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch sprite_batch) // for debug
        {
            Texture2D wall_texture = new Texture2D(graphics_device, 1, 1);
            //  Set the single pixel's data to pure white
            wall_texture.SetData(new[] { Color.DarkRed });
            BoundingBox2D box = Shape.BoundingBox;
            sprite_batch.DrawRectangle(new RectangleF(box.Min.X, box.Min.Y, box.Width, box.Height), Color.DarkMagenta,5,0);
        }
    }
}