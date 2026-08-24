using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Data;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;

namespace old_heart
{

    public class particle_manager
    {
        public ParticleEffect low_particle_effect;
        public ParticleEffect high_particle_effect;

        public Texture2D default_particle_texture;


        public int particle_limit = 670;

        public particle_manager(ContentManager content)
        {
            default_particle_texture = content.Load<Texture2D>("image/white_pixel");

            create_particle_effect();
        }

        public void create_particle_effect()
        {
            low_particle_effect = new ParticleEffect("particle_effect")
            {
                Position = new Vector2(0,0),
                AutoTrigger = true,                     // Automatically trigger particle emitters
                AutoTriggerFrequency = 0.5f              // Emit particles every 0.1 seconds
            };
            high_particle_effect = new ParticleEffect("particle_effect")
            {
                Position = new Vector2(0, 0),
                AutoTrigger = true,                     // Automatically trigger particle emitters
                AutoTriggerFrequency = 0.5f              // Emit particles every 0.1 seconds
            };
            ParticleEmitter emitter = new ParticleEmitter(2000)
            {
                Name = "Fire Emitter",

                Offset = new Vector2(200,200),
                // Each particle created by this emitter lives for 2 seconds
                LifeSpan = 2.0f,
                TextureRegion = new Texture2DRegion(default_particle_texture),

                // Use a spray profile - particles emit in a directional cone
                Profile = Profile.Spray(-Vector2.UnitY, 2.0f),
                
                // Set up how particles look when they're created
                Parameters = new ParticleReleaseParameters
                { 

                    // Release 10-20 particles each time
                    Quantity = new ParticleInt32Parameter(10, 20),

                    // Random speed between 10-40
                    Speed = new ParticleFloatParameter(10.0f, 40.0f),

                    // Red color using HSL values (Hue=0°, Saturation = 100%, Lightness=60%)
                    Color = new ParticleColorParameter(new Vector3(0.0f, 1.0f, 0.6f)),

                    // Make them 10x bigger
                    Scale = new ParticleVector2Parameter(new Vector2(10f, 10f))
                }
            };

            // Add fire-like behavior
            emitter.Modifiers.Add(new LinearGravityModifier
            {
                // Point upward (negative Y)
                Direction = -Vector2.UnitY,

                // Make fire rise with this much force
                Strength = 100f
            });

            // Make particles fade out as they age
            emitter.Modifiers.Add(new AgeModifier
            {
                Interpolators =
                {
                    new OpacityInterpolator
                    {
                        // Start fully visible
                        StartValue = 1.0f,

                        // Fade to transparent over lifetime
                        EndValue = 0.0f
                    }
                }
            });

            // Add the emitter to our effect
            low_particle_effect.Emitters.Add(emitter);
        }
        public void update(GameTime gameTime)
        {
            float delta_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            low_particle_effect.Update(delta_time);
        }
        public void draw_low(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(low_particle_effect);
        }
        public void draw_high(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(high_particle_effect);
        }
    }
}