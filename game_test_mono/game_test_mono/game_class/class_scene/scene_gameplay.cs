using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace old_heart
{
    public class gameplay : base_screen
    {
        private Vector2 _titlePosition;
        private SpriteFont font;

        

        public ui_button return_button;
        public ui_text test_text;
        public ui_text test_text2;
        public player player;
        public gameplay(Game1 game) : base(game)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("gameplay scene\nclick or V to go back to title bruh", font, new Vector2(100, 50),Color.DarkGreen);
            game_manager.add_ui(test_text);
            test_text2 = new ui_text("press P to view hitbox testttttttt  Chess Battle Advanced", font, new Vector2(256f, 600f), Color.DarkOrange);
            game_manager.add_map(test_text2);
            return_button = new ui_button(Content , new Rectangle(150, 600, 200, 100));
            game_manager.add_ui(return_button);

            player = new player(Content,new Vector2(200,200));
            game_manager.add_entity(player);

            collision_shape_box wall = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(500f, 100f), new Vector2(64f, 500f)));
            game_manager.add_map_collision(wall);
            collision_shape_box wall2 = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(100f, 500f), new Vector2(500f, 64f)));
            game_manager.add_map_collision(wall2);
        }
        public override void Update(GameTime gameTime)
        {
            update_all(gameTime);

            if (global.input.keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }
            if (return_button.clicked)
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }

            test_text.text_string = $"gameplay scene\nclick or V to go back to title bruh\nplayer acc : {player.acceleration}\nvelocity : {player.velocity.X:F2} , {player.velocity.Y:F2}" +
            $"\nw speed : {player.velocity.Length():F2} \nposition : {player.position.X:F2} , {player.position.Y:F2} \nmouse_pos : {global.input.mouse_state.Position}" +
            $"\nplayer animation : {player.animation_player.current_animation.name} [{player.animation_player.current_frame_index}]";

        }
        public void OnCollision()
        {

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}