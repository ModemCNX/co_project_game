using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using System;
using System.Diagnostics;

namespace old_heart
{

    public class camera_manager
    {
        public OrthographicCamera camera;

        private float Trauma;
        private float Trauma_decay = 1.5f;
        private float max_Offset = 20f;
        private float max_angle = 8f;
        private Random rngShake = new Random();
        public camera_manager(GraphicsDevice graphics_device)
        {
            camera = new OrthographicCamera(graphics_device);
        }
        public void shake_screen(float amount = 0.4f) // chess battle advanced
        {
            Trauma = MathHelper.Clamp(Trauma + amount, 0f, 1f);

        }
        public void update(GameTime gameTime ,Vector2 target_position)
        {
            camera.LookAt(target_position); // maybe use gametime to tween camera to target position in future

            if(Trauma > 0f)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float shake = Trauma * Trauma;

                float offset_x = max_Offset * shake * (float)(rngShake.NextDouble() * 2 - 1);
                float offset_Y = max_Offset * shake * (float)(rngShake.NextDouble() * 2 - 1);
                float angle = max_angle * shake * (float)(rngShake.NextDouble() * 2 - 1);

                camera.Position += new Vector2(offset_x, offset_Y);

                Trauma = MathHelper.Clamp(Trauma - Trauma_decay * dt, 0f, 1f);

               
            }
            KeyboardStateExtended keyboard_state = global.input.keyboard_state;
            if (keyboard_state.IsKeyDown(Keys.E)) //camera shake Test
            {
                Debug.WriteLine("Shaked");
                shake_screen(0.5f);
            }
        }

    }
}