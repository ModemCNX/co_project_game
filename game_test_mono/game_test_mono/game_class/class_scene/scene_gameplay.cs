using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace old_heart
{
    public class gameplay : base_screen
    {
        private SpriteFont font;



        public ui_button return_button;
        public ui_text test_text;
        public ui_text test_text2;

        leukemia test_enemy;
        private ContentManager content;
        public gameplay(Game1 game) : base(game)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("font/test_font");

            test_text = new ui_text("this text get replace in update function anyway", font, new Vector2(10, 5));
            test_text.text_color = Color.DarkRed;
            test_text.text_scale = new Vector2(0.5f, 0.5f);
            game_manager.add_ui(test_text);
            test_text2 = new ui_text("press P to view hitbox testttttttt  Chess Battle Advanced\nE shake\nF player take 1 damage\nT spawn test projectile\nZ spawn test particle", font, new Vector2(25f, 0f));
            test_text2.text_color = Color.DarkOrange;
            game_manager.add_map(test_text2);
            return_button = new ui_button(Content , new Rectangle(15, 400, 100, 50));
            game_manager.add_ui(return_button);

            player player = new player(Content,new Vector2(200,200));
            game_manager.add_entity(player);

            collision_shape_box wall = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(500f, 100f), new Vector2(64f, 500f)));
            game_manager.add_map_collision(wall);
            collision_shape_box wall2 = new collision_shape_box(BoundingBox2D.CreateFromPositionAndSize(new Vector2(100f, 500f), new Vector2(500f, 64f)));
            game_manager.add_map_collision(wall2);

            test_enemy = new leukemia(Content, max_hp: 5, position: new Vector2(400, 300), speed: 600f); // ใช้ Content (ตัวใหญ่) ไม่ใช่ content
            test_enemy.target = player; // ให้ enemy รู้จัก player เพื่อเช็คระยะ dangerous_rad/safe_rad
            game_manager.add_entity(test_enemy);
           

        }
        public override void Update(GameTime gameTime)
        {
            if (global.input.keyboard_state.WasKeyPressed(Keys.V))
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }
            if (return_button.clicked)
            {
                ScreenManager.ReplaceScreen(new main_menu(game_ref), fade_transition);
            }

            test_text.text_string = $"gameplay scene fps [{(1/ gameTime.ElapsedGameTime.TotalSeconds):F2}]\nclick or V to go back to title bruh" +
            $"\nmouse_pos : {global.input.scaled_mouse_position}\nworld_mouse_pos : {global.input.scaled_mouse_world_position}";

            if (game_manager.player != null)
            {
                test_text.text_string += $"\nplayer acc : {game_manager.player.acceleration}\nvelocity : {game_manager.player.velocity.X:F2} , {game_manager.player.velocity.Y:F2}" +
                    $"\nw speed : {game_manager.player.velocity.Length():F2} \nposition : {game_manager.player.position.X:F2} , {game_manager.player.position.Y:F2}" +
                    $"\nplayer animation : {game_manager.player.animation_player.current_animation.name} [{game_manager.player.animation_player.current_frame_index}]";
            }


            update_all(gameTime);
        }
        public void OnCollision()
        {

        }
        
    }
    
}