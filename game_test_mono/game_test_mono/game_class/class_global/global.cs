using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace old_heart
{ 
    public static class global           // This class is the important !!!!!!!!!!!!!!!!!!
    {
        public static Vector2 render_size = new Vector2(960,540);
        public static class input
        {
            public static KeyboardStateExtended keyboard_state;
            public static MouseStateExtended mouse_state;

            public static Point scaled_mouse_position = Point.Zero;
            public static Point scaled_mouse_world_position = Point.Zero;
            public static void update_input_state()
            {
                KeyboardExtended.Update(); // update keyboard input 
                MouseExtended.Update(); // update mouse input 

                keyboard_state = KeyboardExtended.GetState();
                mouse_state = MouseExtended.GetState();
            }

            public static void update_scaled_mouse(BoxingViewportAdapter viewport , OrthographicCamera camera) // call from camera manager
            {
                scaled_mouse_position = viewport.PointToScreen(mouse_state.Position);
                scaled_mouse_world_position = camera.ScreenToWorld(mouse_state.Position.ToVector2()) .ToPoint() ;
            }
        }
        public static class signal
        {
            static event Action<projectile,bool> signal_spawn_projectile;

            public static void spawn_projectile(projectile projectile,Vector2 position, bool player_projectile = false)
            {
                signal_spawn_projectile.Invoke(projectile, player_projectile);
            }
        }
    }
}