using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System.Collections.Generic;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended;
using System.Diagnostics;

namespace old_heart
{
    public abstract class base_screen : GameScreen
    {
        public Game1 game_ref = null;
        public FadeTransition fade_transition;
        public SpriteBatch sprite_batch;

        public OrthographicCamera camera;

        public List<node> ui_node_list = new List<node> { }; //  ui
        public List<node> world_node_list = new List<node> { }; //  world (anything in scene not entity ,move when camera move)
        public List<entity> world_entity_list = new List<entity> { }; //  all entity in this scene

        public CollisionWorld2D collision_world; // for collision detecting

        public base_screen(Game1 game) : base(game)
        {
            game_ref = game;
            sprite_batch = game.sprite_batch;
        }

        public override void LoadContent()
        {
            fade_transition = new FadeTransition(game_ref.GraphicsDevice, Color.Black, 0.5f); // setup transition screen for all inheried scene to use

            // collision system
            Layer default_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for entity that can move
            collision_world = new CollisionWorld2D(default_layer);

            Layer wall_layer = new Layer(new SpatialHash(new SizeF(128f, 128f)));
            wall_layer.IsDynamic = false; // not dynamic mean it will not move
            collision_world.AddLayer("wall", wall_layer);
        }

        public void update_node(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            foreach (node node in ui_node_list) // update ui
            {
                if (node.active)
                {
                    node.Update(gameTime);
                }
            }
            foreach (node node in world_node_list) // update world node
            {
                if (node.active)
                {
                    node.Update(gameTime);
                }
                else
                {
                    world_node_list.Remove(node);
                }
            }
            foreach (entity entity in world_entity_list) // update world entity
            {
                if (entity.active)
                {
                    entity.Update(gameTime);
                }
                else
                {
                    world_node_list.Remove(entity);
                    collision_world.Remove(entity);
                }
            }

            collision_world.RebuildDynamicLayers();

            var collisionPairs = collision_world.QueryCollisionPairs(default, "wall"); // when collision between default and wall layer happen
            foreach (var pair in collisionPairs)
            {
                if (pair.First is entity entity) // if any entity in default collision layer hit wall layer
                {
                    entity.velocity += pair.FirstResult.MinimumTranslationVector / delta_time ; // devided by delta_time for wall to push instantly
                }
                //Debug.WriteLine(pair.First.GetType().Name);
            }

        }
        public void draw_ui(SpriteBatch sprite_batch) // draw ui
        {
            foreach (node node in ui_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
        }
        public void draw_world(SpriteBatch sprite_batch) // draw world (anything that not ui ,move when camera move)
        {
            foreach (node node in world_node_list)
            {
                if (node.visible)
                {
                    node.Draw(sprite_batch);
                }
            }
            foreach (entity entity in world_entity_list)
            {
                if (entity.visible)
                {
                    entity.Draw(sprite_batch);
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw world node and entity
            if (camera != null) // has camera
            {
                game_ref.sprite_batch.Begin(transformMatrix: camera.GetViewMatrix());
            }
            else
            {
                game_ref.sprite_batch.Begin();
            }

            draw_world(sprite_batch);

            game_ref.sprite_batch.End();


            // draw ui node
            game_ref.sprite_batch.Begin();

            draw_ui(sprite_batch);

            game_ref.sprite_batch.End();
        }
    }
}