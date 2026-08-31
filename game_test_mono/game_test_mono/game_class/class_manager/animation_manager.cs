using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace old_heart
{

    public class animation_data
    {
        public Dictionary<Enum, animation> data = new Dictionary<Enum, animation>();
        public animation_data()
        {
        }
    }

    public class animation
    {
        public string name; // for debug only 
        public Texture2D sprite_sheet;
        public Point sprite_size = new Point(32,32);
        public Vector2 sprite_scale = new Vector2(4,4);
        public bool loop = true;
        public int frame_count = 1;
        public float frame_time = 0.166f;
        public Vector2 sprite_origin = new Vector2(16,24);
        public animation(Texture2D sprite_sheet,bool loop = true,float frame_per_sec = 4, Point? sprite_size = null )
        {
            this.sprite_sheet = sprite_sheet;
            this.loop = loop;
            this.frame_time = 1f/frame_per_sec;
            if (sprite_size != null)
            {
                this.sprite_size = sprite_size.Value;
                this.sprite_origin = new Vector2(this.sprite_size.X/2, this.sprite_size.Y );
            }

            frame_count = sprite_sheet.Height / this.sprite_size.Y;
        }
    }

    public abstract class animation_player_base
    {
        public animation_data data;
        public animation default_animation;
        public animation current_animation;
        public int current_frame_index = 0;
        public int current_direction = 0;
        public float current_time = 0;

        public bool pause = false;
        public animation_player_base()
        {
        }
        public void update_direction(string direction)
        {
            if (direction == "down") current_direction = 0;
            else if (direction == "up") current_direction = 1;
            else if (direction == "left") current_direction = 2;
            else if (direction == "right") current_direction = 3;
        }
        public void update(float delta_time , string direction )
        {
            if (pause) { return; }

            current_time += delta_time ;

            if(current_time > current_animation.frame_time)  // next frame
            {
                current_time -= current_animation.frame_time ;
                current_frame_index++;

                if(current_frame_index >= current_animation.frame_count )  // end animation
                {
                    current_frame_index = 0;
                    if(current_animation.loop == false)    // if current animation is not loop   play default animation
                    {
                        play(default_animation);
                    }
                }
            }

            update_direction(direction);
        }
        public void play(animation animation)
        {
            if(animation.loop == false)  // start from frame 0 only for not loop animation
            {
                current_frame_index = 0;
            }
            current_animation = animation;
        }
        public void draw(SpriteBatch sprite_batch, Vector2 position)
        {
            Texture2D texture = current_animation.sprite_sheet;
            Rectangle source_rectangle = new Rectangle(current_animation.sprite_size.X * current_direction, current_animation.sprite_size.Y * current_frame_index, current_animation.sprite_size.X, current_animation.sprite_size.Y);
            Vector2 sprite_scale = current_animation.sprite_scale;
            Vector2 sprite_origin = current_animation.sprite_origin;
            float layer_depth = (position.Y + 50000f) / 100000f;

            sprite_batch.Draw(texture, position, source_rectangle,Color.White,0,sprite_origin,sprite_scale,SpriteEffects.None, layer_depth);
        }
    }
}