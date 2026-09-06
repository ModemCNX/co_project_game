using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace old_heart
{
    // hitbox ของท่าโจมตีปกติ: ล่องหน วิ่งออกไปตามทิศ cursor ระยะสั้นๆ แล้วหายไป
    public class melee_projectile : projectile
    {
        public int damage;
        private HashSet<enemy> hit_enemies = new HashSet<enemy>(); // กันโดนดาเมจซ้ำจาก swing เดียวกัน

        public melee_projectile(ContentManager content_set, Vector2 position, Vector2 aim_direction, float travel_distance, float travel_time, int damage)
            : base(content_set, time_left: travel_time, position)
        {
            this.damage = damage;
            visible = false; // ล่องหน ไม่ต้องมี texture เลย
            velocity = aim_direction * (travel_distance / travel_time); // วิ่งให้ได้ระยะ travel_distance พอดีตอน time_left หมด

            register_collision("player_hitbox");
        }

        public override void on_hit_entity(entity target_entity)
        {
            if (target_entity is enemy target_enemy && target_enemy.alive)
            {
                target_enemy.take_damage(damage);
                time_out(); // โดน enemy แล้วหายทันที ไม่ต้องรอ travel_time หมด
            }
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            // ล่องหน ไม่วาดอะไรเลย (ไม่เรียก base.Draw เพราะไม่มี texture โหลดไว้)
        }
    }
}