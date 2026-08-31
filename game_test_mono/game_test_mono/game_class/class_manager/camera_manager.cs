using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace old_heart
{

    public class camera_manager
    {
        public OrthographicCamera camera;
        public camera_manager(GraphicsDevice graphics_device)
        {
            camera = new OrthographicCamera(graphics_device);
        }
        public void shake_screen() // chess battle advanced
        {

        }
        public void update(GameTime gameTime ,Vector2 target_position)
        {
            camera.LookAt(target_position); // maybe use gametime to tween camera to target position in future
        }

    }
}