using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input; // Required for Keyboard input


namespace old_heart
{
    public class player : entity
    {
        public Vector2 input_direction = Vector2.Zero;
        
        public player(ContentManager content, Vector2 position) : base(content, max_hp:4, position, speed:2000)
        {
            texture = content.Load<Texture2D>("image/Player");
            ground_friction = 5f;
            max_velocity = 600;
        }
        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            KeyboardStateExtended keyboard_state = global.input.keyboard_state;
            input_direction = Vector2.Zero;

            if (keyboard_state.IsKeyDown(Keys.D))
            {
                input_direction += new Vector2(1,0);
            }
            if (keyboard_state.IsKeyDown(Keys.A))
            {
                input_direction += new Vector2(-1,0);
            }
            if (keyboard_state.IsKeyDown(Keys.S))
            {
                input_direction += new Vector2(0,1);
            }
            if (keyboard_state.IsKeyDown(Keys.W))
            {
                input_direction += new Vector2(0,-1);
            }

            if (input_direction != Vector2.Zero)
            {
                input_direction = Vector2.Normalize(input_direction) * speed;
            }

            acceleration = input_direction;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            base.Draw(sprite_batch);
            sprite_batch.DrawCircle(new CircleF(new Vector2(position.X, position.Y),(float)hit_box_radius),16,Color.Red,5,0);
        }
    }
}
