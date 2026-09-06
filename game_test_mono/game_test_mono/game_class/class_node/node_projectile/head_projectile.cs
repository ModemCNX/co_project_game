using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace old_heart
{
    // หัวที่ผู้เล่นขว้างออกไป: บินไปตาม velocity แล้วค่อยๆ ช้าลงด้วย drag จนหยุด (ไม่ time_out หายไปเอง รอผู้เล่นมาเก็บ)
    public class head_projectile : projectile
    {
        public bool is_resting = false;
        public float drag = 3f;                    // ยิ่งมากยิ่งหยุดเร็ว
        private const float stop_velocity_threshold = 15f;

        public head_projectile(ContentManager content_set, Vector2 position)
            : base(content_set, time_left: 9999f, position) // time_left ไม่ได้ใช้จริงเพราะ override Update ทั้งหมด
        {
            texture = content.Load<Texture2D>("Placeholder/Weapons/Head");
            sprite_origin = new Vector2(texture.Width / 2, texture.Height); // position คือกึ่งกลาง X, ล่างสุด Y
            sprite_scale = new Vector2(2, 2);

            register_collision("player_hitbox");
        }

        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            if (is_resting) return;

            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity -= velocity * drag * delta_time;
            position += velocity * delta_time;
            collision.Shape = new CollisionShape2D(new BoundingCircle2D(position, hit_box_radius));


            if (velocity.Length() < stop_velocity_threshold)
            {
                velocity = Vector2.Zero;
                is_resting = true;
            }
        }

        public override void on_hit_entity(entity target_entity)
        {
            if (target_entity is enemy target_enemy)
            {
                target_enemy.on_hit_by_projectile(this);
            }
            velocity = Vector2.Zero;
            is_resting = true; // หัวหยุดตรงจุดที่โดน enemy ทันที
        }

        public override void collide_wall(CollisionPair2D pair, float delta_time)
        {
            velocity = Vector2.Zero;
            is_resting = true; // ชนกำแพงก็หยุดตรงนั้นเลย ไม่ time_out
        }
    }
}