using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using System;
using System.Diagnostics;

namespace old_heart
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch sprite_batch;

        readonly ScreenManager screen_manager;
        int eee = 1;

        leukemia test_enemy;
        private ContentManager content;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;
            _graphics.HardwareModeSwitch = false; // _graphics.HardwareModeSwitch = false     to enable alt tap in full screen
            _graphics.ApplyChanges();
            //_graphics.ToggleFullScreen();

            Window.Title = "Chess Battle Advanced" ;
            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            screen_manager = new ScreenManager();

            Components.Add(screen_manager); // auto update screen_manager
        }

        protected override void Initialize()
        {
            base.Initialize();

            screen_manager.ShowScreen(new main_menu(this)); // start in main menu naja
        }

        protected override void LoadContent()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);

            test_enemy = new leukemia(content,max_hp: 5,position: new Vector2(400, 300),speed: 80f); //คำสั่งเรียก leukemia (ถ้าไม่มี script เรียก animation ยังใช้ไม่ได้)
        }

        protected override void Update(GameTime gameTime)
        {
            global.input.update_input_state();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || global.input.keyboard_state.IsKeyDown(Keys.Escape))
                Exit();

            test_enemy.Update(gameTime); //ของ Enemy

            base.Update(gameTime);
        }

        //protected override void Draw(GameTime gameTime)
        //{
        //    GraphicsDevice.Clear(Color.CornflowerBlue);

        //    sprite_batch.Begin(samplerState: SamplerState.PointClamp);
               //test_enemy.Draw(sprite_batch); // Enemy
        //    sprite_batch.End();

        //    base.Draw(gameTime);
        //}
    }
}
