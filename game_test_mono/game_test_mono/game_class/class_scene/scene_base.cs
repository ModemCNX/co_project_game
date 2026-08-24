using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System.Collections.Generic;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended;

namespace old_heart
{
    public abstract class base_screen : GameScreen
    {
        public Game1 game_ref = null;
        public FadeTransition fade_transition;
        public SpriteBatch sprite_batch;

        public game_manager game_manager;

        public CollisionWorld2D collision_world; // for collision detecting

        public base_screen(Game1 game) : base(game)
        {
            game_ref = game;
            sprite_batch = game.sprite_batch;
        }

        public override void LoadContent()
        {
            fade_transition = new FadeTransition(game_ref.GraphicsDevice, Color.Black, 0.5f); // setup transition screen for all inheried scene to use
            game_manager = new game_manager(Content, GraphicsDevice);
        }

        public void update_all(GameTime gameTime)
        {
            game_manager.update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            game_manager.draw(sprite_batch);
        }
        public override void UnloadContent()
        {
            // call unscribe function of the game manager
        }
    }
}