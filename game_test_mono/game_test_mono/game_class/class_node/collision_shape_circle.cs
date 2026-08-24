using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System.Diagnostics;

namespace old_heart
{
    public class collision_shape_circle :collision_shape
    {
        public collision_shape_circle(BoundingCircle2D bounding_circle)
        {
            Shape = new CollisionShape2D(bounding_circle);
        }
        public override void Draw(SpriteBatch sprite_batch) // for debug
        {
            BoundingBox2D box = Shape.BoundingBox;
            sprite_batch.DrawCircle(new CircleF(box.Center, (float)(box.Size.X/2)), 16, Color.Red, 5, 0);
        }
    }
}