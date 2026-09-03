using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_heart
{
    public class leukemia : entity
    {
        // state 
        public const string state_normal = "Normal";
        public const string state_frightened = "Frightened";
        public const string state_dizzy = "Dizzy";
        public const string state_died = "Died";
        public string _state = state_normal;

        // target (player)
        public node target;

        // radii 
        public float dangerous_rad = 150f;
        public float safe_rad = 250f;

        // shield
        public bool shield = true;
        public float shield_timer = 3f; // ตั้งเท่ากับ dizzy_timer ไว้ก่อน ปรับแยกได้ทีหลัง
        private float shield_timer_current = 0f;

        // dizzy 
        public float dizzy_timer = 3f;
        private float dizzy_timer_current = 0f;

        //  clone 
        public float clone_timer = 15f; // TODO: ปรับค่าตามความยากง่ายที่ต้องการ
        private float clone_timer_current;
        public event Action<leukemia> on_request_clone; // scene/spawner ต้อง subscribe เพื่อ instantiate enemy ตัวใหม่จริงๆ

        // frightened 
        public float frightened_speed_multiplier = 2f;
        private float frightened_exit_timer = 0f;
        private const float frightened_exit_delay = 1f;

        // patrol (square)
        public float patrol_square_size = 100f;
        private Vector2 patrol_origin;
        private int patrol_index = 0;
        private const float patrol_point_threshold = 8f;

        //Animation
        public enum state { idle, walk }

        public leukemia(ContentManager content_set, int max_hp, Vector2 position, float speed)
            : base(content_set, max_hp, position, speed)
        {
            patrol_origin = position;
            clone_timer_current = clone_timer;
        }

        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // shield regen ทำงานอิสระจาก state ตามที่ต้องการ
            if (shield == false && _state != state_died)
            {
                shield_timer_current -= delta_time;
                if (shield_timer_current <= 0f)
                {
                    shield = true;
                }
            }

            switch (_state)
            {
                case state_normal:
                    update_clone_timer(delta_time);
                    update_patrol();
                    check_player_distance();
                    break;

                case state_frightened:
                    update_clone_timer(delta_time);
                    update_frightened(delta_time);
                    break;

                case state_dizzy:
                    update_dizzy(delta_time);
                    break;

                case state_died:
                    break;
            }

            base.Update(gameTime); // ให้ entity จัดการ velocity/position/animation/collision ตามปกติ
        }

        // Normal 

        private Vector2[] get_patrol_points()
        {
            float half = patrol_square_size / 2f;
            return new Vector2[]
            {
                patrol_origin + new Vector2(-half, -half),
                patrol_origin + new Vector2(half, -half),
                patrol_origin + new Vector2(half, half),
                patrol_origin + new Vector2(-half, half),
            };
        }

        private void update_patrol()
        {
            Vector2[] points = get_patrol_points();
            Vector2 to_point = points[patrol_index] - position;

            if (to_point.Length() <= patrol_point_threshold)
            {
                patrol_index = (patrol_index + 1) % points.Length;
            }
            else
            {
                acceleration = Vector2.Normalize(to_point) * speed;
            }
        }

        private void check_player_distance()
        {
            if (target == null) return;
            float distance = Vector2.Distance(position, target.position);
            if (distance <= dangerous_rad)
            {
                enter_frightened();
            }
        }

        // Frightened

        private void enter_frightened()
        {
            _state = state_frightened;
            frightened_exit_timer = 0f;
        }

        private void update_frightened(float delta_time)
        {
            if (target == null) return;

            Vector2 away_direction = position - target.position;
            float distance = away_direction.Length();
            away_direction = distance > 0.001f ? Vector2.Normalize(away_direction) : Vector2.UnitY;

            acceleration = away_direction * speed * frightened_speed_multiplier;

            if (distance >= safe_rad)
            {
                frightened_exit_timer += delta_time;
                if (frightened_exit_timer >= frightened_exit_delay)
                {
                    _state = state_normal;
                    patrol_origin = position; // จุดเริ่ม patrol ใหม่ตามตำแหน่งปัจจุบัน
                    patrol_index = 0;
                    frightened_exit_timer = 0f;
                }
            }
            else
            {
                frightened_exit_timer = 0f; // ยังไม่พ้น safe_rad ให้รีเซ็ต delay
            }
        }

        // Dizzy 

        public void on_hit_by_projectile(projectile proj) // เรียกจาก collision manager ตอน projectile ชน enemy
        {
            if (alive == false) return;
            if (shield && (_state == state_normal || _state == state_frightened))
            {
                enter_dizzy();
            }
        }

        private void enter_dizzy()
        {
            _state = state_dizzy;
            shield = false;
            dizzy_timer_current = dizzy_timer;
            shield_timer_current = shield_timer;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        private void update_dizzy(float delta_time)
        {
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

            dizzy_timer_current -= delta_time;
            if (dizzy_timer_current <= 0f)
            {
                _state = state_normal;
                patrol_origin = position;
                patrol_index = 0;
            }
        }

        // Clone

        private void update_clone_timer(float delta_time)
        {
            clone_timer_current -= delta_time;
            if (clone_timer_current <= 0f)
            {
                clone_timer_current = clone_timer;
                on_request_clone?.Invoke(this); // scene/spawner จะ subscribe event นี้เพื่อสร้าง enemy ตัวใหม่จริงๆ
            }
        }

        //Damage / Death 

        public override void take_damage(int damage_taken)
        {
            if (alive == false) return;
            if (shield) return; // มี shield อยู่ โจมตีธรรมดาไม่เข้า

            base.take_damage(damage_taken);

            if (alive == false)
            {
                _state = state_died;
            }
        }
        // animation

        public class animation_player_enemy : animation_player_base
        {
            public enum animation_name { idle, walk, } 

            public static readonly animation_data animation_data = new animation_data();

            public animation_player_enemy(ContentManager content) : base()
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
                // TODO: เปลี่ยน path ให้ตรงกับ sprite sheet จริงของ enemy ตัวนี้
                Texture2D idle_texture = content.Load<Texture2D>("Placeholder/Enemies/Beast");
                animation idle_animation = new animation(idle_texture, frame_per_sec: 2);
                idle_animation.name = "enemy idle";
                animation_data.data.Add(animation_name.idle, idle_animation);

                Texture2D walk_texture = content.Load<Texture2D>("Placeholder/Enemies/Beast");
                animation walk_animation = new animation(walk_texture, frame_per_sec: 8);
                walk_animation.name = "enemy walk";
                animation_data.data.Add(animation_name.walk, walk_animation);

                // TODO: ใส่ dizzy sprite sheet จริงตอนมี asset แล้ว ตอนนี้ใช้ idle ไปพลางๆ กัน error
                //Texture2D dizzy_texture = content.Load<Texture2D>("Placeholder/Enemies/Beast");
                //animation dizzy_animation = new animation(dizzy_texture, frame_per_sec: 2);
                //dizzy_animation.name = "enemy dizzy";
                //animation_data.data.Add(animation_name.dizzy, dizzy_animation);
            }



        }
    }
}
