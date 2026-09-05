using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace old_heart
{
    public class leukemia : enemy
    {
        public leukemia(ContentManager content_set, int max_hp, Vector2 position, float speed)
            : base(content_set, max_hp, position, speed)
        {
            animation_player = new animation_player_leukemia(content_set);
        }

        public class animation_player_leukemia : animation_player_base
        {
            public static readonly animation_data animation_data = new animation_data();

            public animation_player_leukemia(ContentManager content) : base()
            {
                if (animation_data.data.Count == 0)
                {
                    load(content);
                }

                base.data = animation_data;

                default_animation = animation_data.data[animation_name.idle];
                current_animation = default_animation;
            }

            public void load(ContentManager content)
            {
                //Point placeholder_sprite_size = new Point(16, 16); // sprite sheet ของ Beast.png คือ 4x4 ช่อง ช่องละ 16x16

                // TODO: Beast.png เป็น placeholder แทน Leukemia ไปก่อน เปลี่ยน path เมื่อมีภาพจริงของ leukemia
                Texture2D placeholder_texture = content.Load<Texture2D>("Placeholder/Player/Walk"); //ใช้รูป player ไปก่อนเพราะ Beast ยังใส่ไม่ได้

                animation idle_animation = new animation(placeholder_texture, frame_per_sec: 2); // (ใส่ , sprite_size: placeholder_sprite_size ไว้หลัง frame per sec  ถ้าใส่ beast ได้แล้ว)
                idle_animation.name = "leukemia idle";
                animation_data.data.Add(animation_name.idle, idle_animation);

                animation walk_animation = new animation(placeholder_texture, frame_per_sec: 8);
                walk_animation.name = "leukemia walk";
                animation_data.data.Add(animation_name.walk, walk_animation);

                animation dizzy_animation = new animation(placeholder_texture, frame_per_sec: 2);
                dizzy_animation.name = "leukemia dizzy";
                animation_data.data.Add(animation_name.dizzy, dizzy_animation);
            }
        }
    }
}