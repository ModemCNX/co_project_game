using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace old_heart
{
    public sealed class collision_shape_box :collision_shape
    {
        public collision_shape_box(BoundingBox2D bounding_box)
        {
            Shape = new CollisionShape2D(bounding_box);
        }
        public override void Draw(SpriteBatch sprite_batch) // for debug
        {
            BoundingBox2D box = Shape.BoundingBox;
            sprite_batch.DrawRectangle(new RectangleF(box.Min.X, box.Min.Y, box.Width, box.Height), Color.DarkMagenta,1,0);
        }
    }
}