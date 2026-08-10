using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using System.Diagnostics;

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
            camera = new OrthographicCamera(GraphicsDevice);
            camera.Zoom = 1;

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("gameplay scene\nclick or V to go back to title bruh", font, new Vector2(100, 50),Color.DarkGreen);
            ui_node_list.Add(test_text);
            test_text2 = new ui_text("testttttttt text on floor to see if player actually move Chess Battle Advanced", font, new Vector2(256f, 600f), Color.DarkOrange);
            world_node_list.Add(test_text2);
            return_button = new ui_button(GraphicsDevice, new Rectangle(150, 600, 200, 100));
            ui_node_list.Add(return_button);

            player = new player(Content,new Vector2(200,100));
            world_entity_list.Add(player);
            collision_world.Insert(player);

            wall_collision wall = new wall_collision(GraphicsDevice, BoundingBox2D.CreateFromPositionAndSize(new Vector2(1500f, 100f), new Vector2(64f, 500f)));
            collision_world.Insert(wall, "wall");
            world_node_list.Add(wall);
            wall_collision wall2 = new wall_collision(GraphicsDevice, BoundingBox2D.CreateFromPositionAndSize(new Vector2(1100f, 500f), new Vector2(500f, 64f)));
            collision_world.Insert(wall2, "wall");
            world_node_list.Add(wall2);
        }
        public void debug_draw(SpriteBatch sprite_batch, CollisionWorld2D collision_world, string target_layer_name)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            update_node(gameTime);

            if (KeyboardExtended.GetState().WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }
            if (return_button.clicked)
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }

            test_text.text_string = $"gameplay scene\nclick or V to go back to title bruh\nplayer acc : {player.acceleration}\nvelocity : {player.velocity.X:F2} , {player.velocity.Y:F2}" +
            $"\nw speed : {player.velocity.Length():F2}\nmouse_pos : {MouseExtended.GetState().Position}";

            camera.LookAt(player.position); // update camera
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