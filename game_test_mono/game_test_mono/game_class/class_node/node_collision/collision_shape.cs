using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace old_heart
{
    public abstract class collision_shape :node, ICollisionActor
    {
        public int Id => GetHashCode(); // for collision // id of entity is it's unique hash code
        public CollisionShape2D Shape { get; set;}

        public node owner;
        public override void Update(GameTime gameTime)
        {
            // dont update collision shape using this,       just update Shape when node that use collision update
        }
        public override void Draw(SpriteBatch sprite_batch) // for debug
        {
            BoundingBox2D box = Shape.BoundingBox;
            sprite_batch.DrawRectangle(new RectangleF(box.Min.X, box.Min.Y, box.Width, box.Height), Color.DarkMagenta,1,0);
        }
    }
}