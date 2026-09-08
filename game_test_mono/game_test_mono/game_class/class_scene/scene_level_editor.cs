using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using System.IO;

namespace old_heart
{
    public class level_editor : base_screen
    {
        private SpriteFont font;

        public node selecting_node;

        public Vector2 camera_position = new Vector2 (0, 0);
        public int camera_speed = 300; // pixel per sec
        public int camera_speed_fast = 1000; // pixel per sec  // when press shift

        public ui_text test_text;
        public ui_text test_text_2;
        public ui_text test_text_3;
        public level_editor(Game1 game) : base(game)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            game_manager.level_manager.set_level_file("test_1.json");         // exact JSON file name
            game_manager.pause = true;

            test_text = new ui_text("this text get replace in update function anyway", font, new Vector2(10, 5));
            test_text.text_color = Color.DarkRed;
            test_text.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text);

            test_text_2 = new ui_text("[K] save [L] load [WASD] move [Shift] move faster [P] toggle collision [V] return [B] test level", font, new Vector2(10, 510));
            test_text_2.text_color = Color.DarkRed;
            test_text_2.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text_2);

            test_text_3 = new ui_text("current_state = none (not finidsh)", font, new Vector2(10, 490));
            test_text_3.text_color = Color.DarkRed;
            test_text_3.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text_3);

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardStateExtended keyboard_state = global.input.keyboard_state;
            if (keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }else if (keyboard_state.WasKeyPressed(Keys.B))
            {
                ScreenManager.ReplaceScreen(new test_level(game_ref), fade_transition);
            }
            else if (keyboard_state.WasKeyPressed(Keys.K))
            {
                game_manager.level_manager.save_level();
            }
            else if (keyboard_state.WasKeyPressed(Keys.L))
            {
                game_manager.level_manager.load_level();
            }else if (keyboard_state.WasKeyPressed(Keys.D1))
            {
                game_manager.add_entity(new player(game_manager.content, global.input.scaled_mouse_world_position));
            }


            move_camera(gameTime);

            test_text.text_string = $"level_editor scene fps [{(1/ gameTime.ElapsedGameTime.TotalSeconds):F2}]" +
            $"\ncurrent_level_file : {Path.GetFileName(game_manager.level_manager.current_level_file)}" +
            $"\nworld_mouse_pos : {global.input.scaled_mouse_world_position}" +
            $"\nobject count bellow\nentity :{game_manager.entity_manager.entity_list.Count}\nwall collision :{game_manager.collision_manager.wall_list.Count} \nmap_low :{game_manager.map_manager.map_node_list.Count} \nmap_high :{game_manager.map_manager.high_map_node_list.Count}";


            update_all(gameTime);

            void move_camera(GameTime game_time)
            {
                float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                KeyboardStateExtended keyboard_state = global.input.keyboard_state;
                Vector2 input_direction = Vector2.Zero;

                if (keyboard_state.IsKeyDown(Keys.D))
                {
                    input_direction += new Vector2(1, 0);
                }
                if (keyboard_state.IsKeyDown(Keys.A))
                {
                    input_direction += new Vector2(-1, 0);
                }
                if (keyboard_state.IsKeyDown(Keys.S))
                {
                    input_direction += new Vector2(0, 1);
                }
                if (keyboard_state.IsKeyDown(Keys.W))
                {
                    input_direction += new Vector2(0, -1);
                }
                if (input_direction != Vector2.Zero)
                {
                    if (keyboard_state.IsKeyDown(Keys.LeftShift))
                    {
                        input_direction = Vector2.Normalize(input_direction) * camera_speed_fast;
                    }
                    else
                    {
                        input_direction = Vector2.Normalize(input_direction) * camera_speed;
                    }
                }
                camera_position += input_direction * delta_time;
                game_manager.camera_manager.camera.LookAt(camera_position.ToPoint().ToVector2());
            }

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
    
}