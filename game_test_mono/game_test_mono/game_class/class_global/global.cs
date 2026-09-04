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
            public static Vector2 scaled_mouse_world_position = Vector2.Zero;
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
                scaled_mouse_world_position = camera.ScreenToWorld(mouse_state.Position.ToVector2());
            }
        }
        public static class signal
        {
            public static event Action<projectile> signal_spawn_projectile;
            public static event Action<entity> signal_spawn_entity;
            public static event Action<Enum,Vector2,bool> signal_spawn_particle;
            public static void spawn_projectile(projectile projectile)
            {
                signal_spawn_projectile.Invoke(projectile);
            }
            public static void spawn_entity(entity entity)
            {
                signal_spawn_entity.Invoke(entity);
            }
            public static void spawn_particle(Enum particle_name, Vector2 position, bool high_layer = false)
            {
                signal_spawn_particle.Invoke(particle_name,position,high_layer);
            }
        }
    }
}