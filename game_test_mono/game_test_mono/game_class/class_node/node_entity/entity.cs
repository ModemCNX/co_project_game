using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

namespace old_heart
{
    public abstract class entity : node
    {
        public enum direction {down,up,left,right}

        public ContentManager content;
        
        public direction current_direction = direction.down;
        public animation_player_base animation_player;

        public collision_shape collision;
        public float hit_box_radius = 10; // for collision // hit_box is circle (BoundingCircle2D)

        public float ground_friction = 10f;
        public float max_velocity = 400;

        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0,0);
        public Vector2 acceleration = new Vector2(0,0);
        public bool alive = true;
        public int max_hp = 10;
        public int hp = 10;
        public float speed = 100;
        public entity(ContentManager content_set, int max_hp,Vector2 position, float speed)
        {
            content = content_set;
            this.max_hp = max_hp;
            this.hp = max_hp;
            this.position = position;
            this.speed = speed;

            this.collision = new collision_shape_circle(new BoundingCircle2D(position, hit_box_radius));
            collision.owner = this;

        }
        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity -= velocity * ground_friction * delta_time;
            velocity += acceleration * delta_time;

            if (velocity.Length() > max_velocity)
            {
                velocity = Vector2.Normalize(velocity) * max_velocity;
            }

            position += velocity * delta_time;

            collision.Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));  // update collision position

            update_direction(velocity);
            update_animation(delta_time);

            void update_direction(Vector2 velocity)
            {
                float abs_x = MathF.Abs(velocity.X); // for calculate direction
                float abs_y = MathF.Abs(velocity.Y); // for calculate direction

                if (abs_x < 0.01f && abs_y < 0.01f)
                {
                    //current_direction = direction.down;         // stand still will stay the same direction
                }else if(abs_x > abs_y)
                {
                    current_direction = velocity.X > 0 ? direction.right : direction.left;
                }
                else
                {
                    current_direction = velocity.Y > 0 ? direction.down : direction.up;
                }
            }
        }
        public virtual void update_animation(float delta_time)
        {
            animation_player.update(delta_time, current_direction.ToString());
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
            position += pair.FirstResult.MinimumTranslationVector;// / delta_time; // devided by delta_time for wall to push instantly
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            animation_player.draw(sprite_batch, position);
        }
    }
}
