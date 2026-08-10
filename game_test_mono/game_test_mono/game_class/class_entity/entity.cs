using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;

namespace old_heart
{
    public abstract class entity : node ,ICollisionActor
    {
        public ContentManager content;
        public Texture2D texture;
        public int Id => GetHashCode(); // for collision // id of entity is it's unique hash code   
        public BoundingCircle2D hitbox; // for collision
        public CollisionShape2D Shape { get; set; }// for collision

        public int hit_box_radius = 10; // for collision // hit_box is circle (BoundingCircle2D)

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
            Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));
        }
        public override void Update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            if (alive == false) return;
            velocity -= velocity * ground_friction * delta_time;
            if (velocity.Length() > max_velocity)
            {
                velocity = Vector2.Normalize(velocity) * max_velocity;
            }

            velocity += acceleration * delta_time; 
            position += velocity * delta_time; // add collision later
            Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));  // update hit box position
        }
        public void die()
        {
            alive = false;
            // play efx or something
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
        public override void Draw(SpriteBatch sprite_batch)
        {
            Vector2 texture_position = new Vector2(position.X - (texture.Width / 2), position.Y - texture.Height); // position is center X and bottom Y
            sprite_batch.Draw(texture, texture_position, Color.White);
        }
    }
}
