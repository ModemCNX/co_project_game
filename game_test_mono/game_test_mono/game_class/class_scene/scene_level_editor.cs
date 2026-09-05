using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace old_heart
{
    public class level_editor : base_screen
    {
        private SpriteFont font;

        public string current_level_file;

        public ui_text test_text;
        public level_editor(Game1 game) : base(game)
        {
            string file_directory = AppDomain.CurrentDomain.BaseDirectory;
            string game_root_file = Path.GetFullPath(Path.Combine(file_directory, "..", "..", ".."));  // get true game sorce code file
            string level_folder = Path.Combine(game_root_file, "Content", "level");
            current_level_file = Path.Combine(level_folder, "test_1.json");
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("this text get replace in update function anyway", font, new Vector2(10, 5));
            test_text.text_color = Color.DarkRed;
            test_text.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text);

            collision_shape_box wall = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(500f, 100f), new Vector2(64f, 500f)));
            //game_manager.add_map_collision(wall);
            collision_shape_box wall2 = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(100f, 500f), new Vector2(500f, 64f)));
            game_manager.add_map_collision(wall2);
        }
        public override void Update(GameTime gameTime)
        {
            if (global.input.keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }
            else if (global.input.keyboard_state.WasKeyPressed(Keys.Q))
            {
                save_level();
            }

            test_text.text_string = $"level_editor scene fps [{(1/ gameTime.ElapsedGameTime.TotalSeconds):F2}]                                                                       V to go back to title bruh" +
            $"\ncurrent_level_file : {Path.GetFileName(current_level_file)}" +
            $"\nmouse_pos : {global.input.scaled_mouse_position}\nworld_mouse_pos : {global.input.scaled_mouse_world_position}";

            if (game_manager.player != null)
            {
                test_text.text_string += $"\nplayer acc : {game_manager.player.acceleration}\nvelocity : {game_manager.player.velocity.X:F2} , {game_manager.player.velocity.Y:F2}" +
                    $"\nw speed : {game_manager.player.velocity.Length():F2} \nposition : {game_manager.player.position.X:F2} , {game_manager.player.position.Y:F2}" +
                    $"\nplayer animation : {game_manager.player.animation_player.current_animation.name} [{game_manager.player.animation_player.current_frame_index}]";
            }


            update_all(gameTime);
        }
        public void save_level()
        {
            if (current_level_file == null)
            {
                Debug.WriteLine("level file is null error");
            }
            level_data level_data = new level_data();
            level_data.name = "test";
            level_object wall = new level_object();
            wall.type = "test";
            level_data.level_object_list.Add(wall);
            string json_string = JsonSerializer.Serialize(level_data, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(current_level_file, json_string);

            Debug.WriteLine("Saved level : " + current_level_file);
        }

        public class level_data
        {
            public string name { get; set; }
            public List<level_object> level_object_list { get; set; } = new List<level_object>();
        }
        public class level_object
        {
            public string type { get; set; }
            public List<int> data { get; set; } = new List<int> { 3, 2, 1, };
        }
    }
    
}