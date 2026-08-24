using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;

namespace old_heart
{
    public abstract class entity : node
    {
        public ContentManager content;
        public Texture2D texture;
        public collision_shape collision;

        public float hit_box_radius = 15; // for collision // hit_box is circle (BoundingCircle2D)
        public Vector2 sprite_origin;

        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0,0);
        public Vector2 acceleration = new Vector2(0,0);
        public bool alive = true;
        public int max_hp = 10;
        public int hp = 10;
        public float speed = 100;

        public float ground_friction = 5f;
        public float max_velocity = 400;
        public entity(ContentManager content_set, int max_hp,Vector2 position, float speed)
        {
            content = content_set;
            this.max_hp = max_hp;
            this.hp = max_hp;
            this.position = position;
            this.speed = speed;
            this.collision = new collision_shape_circle(new BoundingCircle2D(position, hit_box_radius));
            collision.owner = this;
            texture = content.Load<Texture2D>("image/player");
            sprite_origin = new Vector2((texture.Width / 2), texture.Height); // origin position is center X and bottom Y
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
            position += velocity * delta_time;

            collision.Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));  // update collision position
        }
        public void die()
        {
            alive = false;
            // play efx or something
            active = false; // active = false make this get instant delete
        }
        public void take_damage(int damage_taken)
        {
            hp -= damage_taken;
            if(hp  <= 0)
            {
                hp = 0;
                die();
            }
        }
        public void collide_wall(CollisionPair2D pair , float delta_time) // wall collision get call from collision_manager
        {
            velocity += pair.FirstResult.MinimumTranslationVector / delta_time; // devided by delta_time for wall to push instantly
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            float layer_depth = (position.Y + 50000f) / 100000f;
            sprite_batch.Draw(texture, position,null,Color.White,0, sprite_origin, 1,SpriteEffects.None, layer_depth);
        }
    }
}
