using MonoGame.Extended.Input;
namespace old_heart
{ 
    public static class global           // This class is the important!!!!!!!!!!!!!!!!!!
    {
        public static class input
        {
            public static KeyboardStateExtended keyboard_state;
            public static MouseStateExtended mouse_state;

            public static void update_input_state()
            {
                KeyboardExtended.Update(); // update keyboard input 
                MouseExtended.Update(); // update mouse input 

                keyboard_state = KeyboardExtended.GetState();
                mouse_state = MouseExtended.GetState();
            }
        }
    }
}