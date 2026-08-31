using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace old_heart
{
    public class main_menu : base_screen
    {
        private SpriteFont font;
        public ui_button start_button;
        public ui_text test_text;
        public main_menu(Game1 game) : base(game)
        {
            game_ref = game;
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("Chess Battle Advanced\nclick or V to play\nEsc to quit", font, new Vector2(100, 50));
            game_manager.add_ui(test_text);
            start_button = new ui_button(Content, new Rectangle(150, 500, 200, 100));
            game_manager.add_ui(start_button);


        }

        public override void Update(GameTime gameTime)
        {
            update_all(gameTime);
            if (global.input.keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new gameplay(game_ref), fade_transition);
            }
            if (start_button.clicked)
            {
                ScreenManager.ReplaceScreen(new gameplay(game_ref), fade_transition);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}