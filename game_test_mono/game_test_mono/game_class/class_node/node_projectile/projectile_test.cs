using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace old_heart
{
    public class projectile_test : projectile
    {
        public projectile_test(ContentManager content_set, float time_left,Vector2 position) : base(content_set,time_left,position)
        {
            texture = content.Load<Texture2D>("Placeholder/Weapons/Head");
            sprite_origin = new Vector2((texture.Width / 2), (texture.Height)); // position is center X and bottom Y
            sprite_scale = new Vector2(2, 2);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        //public override void collide_wall(CollisionPair2D pair , float delta_time) // wall collision get call from collision_manager
        //{
        //    velocity += pair.FirstResult.MinimumTranslationVector / delta_time; // devided by delta_time for wall to push instantly
        //}
        public override void Draw(SpriteBatch sprite_batch)
        {
            base.Draw(sprite_batch);
        }
    }
}
