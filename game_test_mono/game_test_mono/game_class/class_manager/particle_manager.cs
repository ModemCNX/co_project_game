using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Data;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using System;
using System.Collections.Generic;

namespace old_heart
{
    public class particle_manager
    {
        static public Dictionary<Enum, ParticleEmitter> data = new Dictionary<Enum, ParticleEmitter>();
        enum particle_name { test1 }

        public ParticleEffect low_particle_effect;
        public ParticleEffect high_particle_effect;

        public ContentManager content;
        public Texture2D default_particle_texture;


        public int particle_limit = 670;

        public particle_manager(ContentManager content)
        {
            this.content = content;
            default_particle_texture = content.Load<Texture2D>("image/white_pixel");
            low_particle_effect = new ParticleEffect("low_particle_effect")
            {
                Position = new Vector2(0, 0),
                AutoTrigger = false
            };
            high_particle_effect = new ParticleEffect("high_particle_effect")
            {
                Position = new Vector2(0, 0),
                AutoTrigger = false
            };
            if(data.Count == 0)
            {
                load();
            }
        }

        public void add(Enum particle_name, Vector2 position, bool high_layer)
        {
            ParticleEmitter saved_particle = data[particle_name];
            ParticleEmitter new_particle = new ParticleEmitter(saved_particle.Capacity)
            {
                Name = saved_particle.Name,
                LifeSpan = saved_particle.LifeSpan,
                TextureRegion = saved_particle.TextureRegion,
                Profile = saved_particle.Profile,
                ModifierExecutionStrategy = ModifierExecutionStrategy.Serial,
                Offset = saved_particle.Offset,

                Parameters = new ParticleReleaseParameters
                {
                    Quantity = saved_particle.Parameters.Quantity,
                    Speed = saved_particle.Parameters.Speed,
                    Color = saved_particle.Parameters.Color,
                    Scale = saved_particle.Parameters.Scale,
                    Opacity = saved_particle.Parameters.Opacity,
                    Rotation = saved_particle.Parameters.Rotation
                }
            };
            foreach (Modifier modifier in saved_particle.Modifiers)
            {
                new_particle.Modifiers.Add(modifier);
            }

            if (high_layer) 
            { 
                high_particle_effect.Emitters.Add(new_particle);
            }
            else 
            {
                low_particle_effect.Emitters.Add(new_particle);
            }

            new_particle.Trigger(position);
        }
        public void update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            KeyboardStateExtended keyboard_state = global.input.keyboard_state;
            if (keyboard_state.IsKeyDown(Keys.Z))
            {
                global.signal.spawn_particle(particle_name.test1, global.input.scaled_mouse_world_position);
            }

            low_particle_effect.Update(delta_time);
            high_particle_effect.Update(delta_time);
        }
        public void draw_low(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(low_particle_effect);
        }
        public void draw_high(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(high_particle_effect);
        }

        public void load()
        {
            ParticleEmitter emitter = new ParticleEmitter(20)
            {
                Name = "fire_efx",
                LifeSpan = 2.0f,
                TextureRegion = new Texture2DRegion(content.Load<Texture2D>("Placeholder/Weapons/Head")),
                Profile = Profile.Spray(-Vector2.UnitY, 2.0f),
                Parameters = new ParticleReleaseParameters
                {
                    Quantity = new ParticleInt32Parameter(10, 20),
                    Speed = new ParticleFloatParameter(10.0f, 40.0f),
                    Color = new ParticleColorParameter(new Vector3(0.0f, 1.0f, 0.6f)),
                    Scale = new ParticleVector2Parameter(new Vector2(1f, 1f))
                }
            };

            emitter.Modifiers.Add(new LinearGravityModifier
            {
                Direction = -Vector2.UnitY,
                Strength = 100f
            });
            emitter.Modifiers.Add(new AgeModifier
            {
                Interpolators = { new OpacityInterpolator { StartValue = 1.0f, EndValue = 0.0f } }
            });

            data.Add(particle_name.test1,emitter);
        }
    }
}