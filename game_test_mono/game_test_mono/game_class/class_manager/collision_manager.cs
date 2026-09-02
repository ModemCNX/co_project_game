using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using System;
using System.Diagnostics;

namespace old_heart
{
    public class collision_manager
    {
        Layer default_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); 
        public CollisionWorld2D collision_world ; // for collision detecting
        public collision_manager()
        {
            collision_world = new CollisionWorld2D(default_layer);
            // make collision system layer
            Layer player_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for player 
            collision_world.AddLayer("player",player_layer);

            Layer player_hitbox_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for player _hitbox
            collision_world.AddLayer("player_hitbox", player_hitbox_layer);

            Layer enemy_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for enemy 
            collision_world.AddLayer("enemy", enemy_layer);

            Layer enemy_hitbox_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for enemy _hitbox
            collision_world.AddLayer("enemy_hitbox", enemy_hitbox_layer);

            Layer wall_layer = new Layer(new SpatialHash(new SizeF(128f, 128f))); // for wall that can't move
            wall_layer.IsDynamic = false;                     // not dynamic mean it will not move
            collision_world.AddLayer("wall", wall_layer);

            collision_world.EnableCollisionBetweenLayers("player", "wall");
            collision_world.EnableCollisionBetweenLayers("player_hitbox", "wall");

            collision_world.EnableCollisionBetweenLayers("enemy", "wall");
            collision_world.EnableCollisionBetweenLayers("enemy_hitbox", "wall");
        }
        public void add(ICollisionActor collision_object ,String collision_layer_name)
        {
            collision_world.Insert(collision_object, collision_layer_name);
            //Debug.WriteLine("add " + collision_object + " to " +  collision_layer_name);
        }
        public void remove(ICollisionActor collision_object)
        {
            collision_world.Remove(collision_object);
        }
        public void update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            collision_world.RebuildDynamicLayers();

            resolve_wall_collision("player", delta_time);
            resolve_wall_collision("player_hitbox", delta_time);

            resolve_wall_collision("enemy", delta_time);
            resolve_wall_collision("enemy_hitbox", delta_time);
        }

        public void resolve_wall_collision(string layer_that_collide_with_wall ,float delta_time) // use in update only
        {
            var collisionPairs = collision_world.QueryCollisionPairs(layer_that_collide_with_wall, "wall"); // when collision between default and wall layer happen
            foreach (var pair in collisionPairs)
            {
                if (pair.First is collision_shape collision_shape) // if any entity in default collision layer hit wall layer
                {
                    if (collision_shape.owner is entity entity)
                    {
                        entity.collide_wall(pair, delta_time);
                    }
                    else if (collision_shape.owner is projectile projectile)
                    {
                        projectile.collide_wall(pair, delta_time);
                    }else
                    {
                        Debug.WriteLine("collision_shape.owner is not entity nor projectile : " + collision_shape);
                    }
                }
                //Debug.WriteLine(pair.First.GetType().Name);
            }
        }

    }
}