using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;

namespace old_heart
{
    public abstract class projectile : node ,ICollisionActor
    {
        public ContentManager content;
        public Texture2D texture;
        public int Id => GetHashCode(); // for collision // id of projectile is it's unique hash code   
        public BoundingCircle2D hitbox; // for collision
        public string collision_layer_name;  // for collision
        public CollisionShape2D Shape { get; set; }// for collision

        public int hit_box_radius = 10; // for collision

        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0,0);
        public Vector2 acceleration = new Vector2(0,0);

        public bool alive = true;
        public float time_left = 0;

        public float ground_friction = 5f;
        public float max_velocity = 400;
        public projectile(ContentManager content_set, float time_left,Vector2 position)
        {
            content = content_set;
            this.time_left = time_left;
            this.position = position;
            Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));
        }
        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity -= velocity * ground_friction * delta_time;
            if (velocity.Length() > max_velocity)
            {
                velocity = Vector2.Normalize(velocity) * max_velocity;
            }

            velocity += acceleration * delta_time; 
            position += velocity * delta_time; // add collision later
            Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));  // update hit box position
        }
        public void time_out()
        {
            alive = false;
            // play efx or something
            active = false; // active = false make this get instant delete
        }

        public void collide_wall(CollisionPair2D pair , float delta_time) // wall collision get call from collision_manager
        {
            velocity += pair.FirstResult.MinimumTranslationVector / delta_time; // devided by delta_time for wall to push instantly
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            Vector2 texture_origin = new Vector2((texture.Width / 2), texture.Height); // position is center X and bottom Y
            float layerDepth = (position.Y + 50000f) / 100000f;
            sprite_batch.Draw(texture, position,null,Color.White,0, texture_origin,1,SpriteEffects.None, layerDepth);
        }
    }
}
