using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace old_heart
{
    public class test_level : base_screen
    {
        private SpriteFont font;

        public ui_text test_text;
        public ui_text test_text_2;

        public test_level(Game1 game) : base(game)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("this text get replace in update function anyway", font, new Vector2(10, 5));
            test_text.text_color = Color.DarkRed;
            test_text.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text); 

            test_text_2 = new ui_text("[V] to main_menu [B] to level_editor \nChess Battle Advanced", font, new Vector2(10, 490));
            test_text_2.text_color = Color.DarkRed;
            test_text_2.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text_2);

            game_manager.level_manager.set_level_file("test_1.json");
            game_manager.level_manager.load_level();
        }
        public override void Update(GameTime gameTime)
        {
            if (global.input.keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }
            if (global.input.keyboard_state.WasKeyPressed(Keys.B))
            {
                ScreenManager.ReplaceScreen(new level_editor(game_ref), fade_transition);
            }


            test_text.text_string = $"test_level scene fps [{(1/ gameTime.ElapsedGameTime.TotalSeconds):F2}]" +
            $"\nmouse_pos : {global.input.scaled_mouse_position}\nworld_mouse_pos : {global.input.scaled_mouse_world_position}";

            if (game_manager.player != null)
            {
                test_text.text_string += $"\nplayer acc : {game_manager.player.acceleration}\nvelocity : {game_manager.player.velocity.X:F2} , {game_manager.player.velocity.Y:F2}" +
                    $"\nw speed : {game_manager.player.velocity.Length():F2} \nposition : {game_manager.player.position.X:F2} , {game_manager.player.position.Y:F2}" +
                    $"\nplayer animation : {game_manager.player.animation_player.current_animation.name} [{game_manager.player.animation_player.current_frame_index}]";
            }


            update_all(gameTime);
        }
    }
}