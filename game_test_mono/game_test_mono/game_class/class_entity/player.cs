using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System.Diagnostics; // Required for Keyboard input

namespace old_heart
{
    public class player : entity
    {
        public Vector2 input_direction = Vector2.Zero;
        public enum state { idle , walk}
        public state current_state = state.idle;
        //private camera_manager camManage; //Camera test
        public player(ContentManager content, Vector2 position) : base(content, max_hp:4, position, speed:5000)
        {
            animation_player = new animation_player_player(content);
            ground_friction = 10f;
            max_velocity = 400;


        }
        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

           // if (keyboard_state.IsKeyDown(Keys.E)) //camera shake Test
            //{
             //   camManage.shake_screen(0.5f); 
            //}

            if (input_direction != Vector2.Zero)
            {
                input_direction = Vector2.Normalize(input_direction) * speed;
            }

            acceleration = input_direction;

            if (velocity.Length() > 10f)
            {
                current_state = state.walk;
            }
            else
            {
                current_state = state.idle;
            }
            
                base.Update(gameTime);
        }
        public override void update_animation(float delta_time)
        {
            if (current_state == state.walk)
            {
                animation_player.play(animation_player.data.data[animation_player_player.animation_name.walk]);
            }
            else
            {
                animation_player.play(animation_player.default_animation);
            }


            animation_player.update(delta_time, current_direction.ToString());
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            base.Draw(sprite_batch);
        }


        public class animation_player_player : animation_player_base       // custom animation for this class only
        {
            public enum animation_name { idle,walk }

            public static readonly animation_data animation_data = new animation_data();
            public animation_player_player(ContentManager content) : base()
            {
                if (animation_data.data.Count == 0)
                {
                    load(content);
                }

                base.data = animation_data;

                default_animation = animation_data.data[animation_name.idle];
                current_animation = default_animation;
            }
            public void load(ContentManager content)
            {
                Texture2D idle_texture = content.Load<Texture2D>("Placeholder/Player/Idle");
                animation idle_animation = new animation(idle_texture, frame_per_sec: 2);
                idle_animation.name = "player idle";
                animation_data.data.Add(animation_name.idle, idle_animation);

                Texture2D walk_texture = content.Load<Texture2D>("Placeholder/Player/Walk");
                animation walk_animation = new animation(walk_texture, frame_per_sec: 8);
                walk_animation.name = "player walk";
                animation_data.data.Add(animation_name.walk, walk_animation);
            }
        }
    }
    
}
