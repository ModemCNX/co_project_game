using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Diagnostics; // Required for Keyboard input

namespace old_heart
{
    public class player : entity
    {
        public Vector2 input_direction = Vector2.Zero;
        public enum state { idle, walk }
        public state current_state = state.idle;

        // --- combat: melee ---
        public int melee_damage = 1;
        public float melee_range = 40f;   // ระยะยื่นไปด้านหน้า
        public float attack_duration = 20f / 60f; // ~20 frame ที่ 60fps เป็น placeholder ไปก่อน
        public bool is_attacking = false;
        private float attack_timer = 0f;

        // --- combat: head throw ---
        public bool has_head = true;
        public float head_throw_speed = 700f;
        public float pickup_radius = 24f;
        private head_projectile thrown_head;

        // --- aim  ---
        public bool is_aiming = false;
        public float aim_speed_multiplier = 0.2f;

        // --- dash (Space) ---
        public bool is_dashing = false;
        public float dash_speed = 1600f;
        public float dash_timeout = 2f; // ยกเลิก dash ถ้าไปไม่ถึงภายในเวลานี้
        private float dash_timer = 0f;

        private float default_max_velocity;
        // --- headless wobble ---
        public float headless_wobble_max_degrees = 25f;
        private Random rng = new Random();
        public player(ContentManager content, Vector2 position) : base(content, max_hp: 4, position, speed: 5000)
        {
            animation_player = new animation_player_player(content);
            ground_friction = 10f;
            max_velocity = 400;
            default_max_velocity = max_velocity;


        }
        public override void Update(GameTime gameTime)
        {
            if (alive == false) return;
            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardStateExtended keyboard_state = global.input.keyboard_state;

            MouseStateExtended mouse_state = global.input.mouse_state; // TODO: เช็คว่าชื่อ property ตรงกับของจริงในโปรเจกต์ไหม

            // --- attack timer countdown ---
            if (is_attacking)
            {
                attack_timer -= delta_time;
                if (attack_timer <= 0f)
                {
                    is_attacking = false;
                }
            }

            // --- dash overrides ทุกอย่าง ---
            if (is_dashing)
            {
                update_dash(delta_time);
                base.Update(gameTime);
                return;
            }

            // --- pickup head อัตโนมัติเมื่อเดินเข้าใกล้ ---
            if (has_head == false && thrown_head != null && thrown_head.is_resting)
            {
                float distance_to_head = Vector2.Distance(position, thrown_head.position);
                if (distance_to_head <= pickup_radius)
                {
                    reattach_head();
                }
            }

            // --- aiming ---
            is_aiming = mouse_state.IsButtonDown(MouseButton.Right) && has_head;

            input_direction = Vector2.Zero;

            if (is_attacking == false) // ล็อคการเดินระหว่างโจมตี
            {
                if (keyboard_state.IsKeyDown(Keys.D))
                {
                    input_direction += new Vector2(1, 0);
                }
                if (keyboard_state.IsKeyDown(Keys.A))
                {
                    input_direction += new Vector2(-1, 0);
                }
                if (keyboard_state.IsKeyDown(Keys.S))
                {
                    input_direction += new Vector2(0, 1);
                }
                if (keyboard_state.IsKeyDown(Keys.W))
                {
                    input_direction += new Vector2(0, -1);
                }

                if (input_direction != Vector2.Zero)
                {
                    float effective_speed = is_aiming ? speed * aim_speed_multiplier : speed;

                    if (has_head == false) // เดินเซตอนไม่มีหัว: บิดทิศทาง input แบบสุ่มเล็กน้อย
                    {
                        float wobble_angle = MathHelper.ToRadians((float)(rng.NextDouble() * 2 - 1) * headless_wobble_max_degrees);
                        input_direction = Vector2.Transform(input_direction, Matrix.CreateRotationZ(wobble_angle));
                    }

                    input_direction = Vector2.Normalize(input_direction) * effective_speed;
                }
            }

            acceleration = input_direction;

            if (velocity.Length() > 10f)
            {
                current_state = state.walk;
            }
            else
            {
                current_state = state.idle;
            }

            if (keyboard_state.WasKeyPressed(Keys.F))
            {
                take_damage(1);
                Debug.WriteLine("hp left " + hp + " / " + max_hp);
            }
            if (global.input.keyboard_state.WasKeyPressed(Keys.T))
            {
                projectile_test projectile_test = new projectile_test(content, 3, position);
                projectile_test.owner = this;
                projectile_test.velocity = Vector2.Normalize(global.input.scaled_mouse_world_position - position) * 300;
                global.signal.spawn_projectile(projectile_test);
            }

            // --- left click: ขว้างหัว (ถ้ากำลังเล็ง) หรือโจมตีธรรมดา ---
            if (mouse_state.WasButtonPressed(MouseButton.Left))
            {
                if (is_aiming && has_head)
                {
                    throw_head();
                }
                else if (is_attacking == false)
                {
                    start_attack();
                }
            }

            // --- space: dash เข้าหาหัว ---
            if (keyboard_state.WasKeyPressed(Keys.Space) && has_head == false && thrown_head != null)
            {
                is_dashing = true;
                max_velocity = MathF.Max(default_max_velocity, dash_speed); // เปิดเพดานความเร็วให้สูงพอสำหรับ dash

            }


            base.Update(gameTime);
        }

        // ---------------- Melee ----------------

        private void start_attack()
        {
            is_attacking = true;
            attack_timer = attack_duration;
            velocity = Vector2.Zero; // หยุดนิ่งทันทีตอนเริ่มโจมตี

            Vector2 to_cursor = global.input.scaled_mouse_world_position - position;
            Vector2 aim_direction = to_cursor != Vector2.Zero ? Vector2.Normalize(to_cursor) : Vector2.UnitY;

            current_direction = get_cardinal_direction(aim_direction); // ยังใช้ตัวนี้แค่สำหรับเลือก animation/sprite ทิศทาง ไม่เกี่ยวกับ hit detection แล้ว

            melee_projectile punch = new melee_projectile(content, position, aim_direction, melee_range, attack_duration, melee_damage);
            punch.owner = this;
            global.signal.spawn_projectile(punch);
        }

        private direction get_cardinal_direction(Vector2 v)
        {
            float abs_x = MathF.Abs(v.X);
            float abs_y = MathF.Abs(v.Y);
            if (abs_x > abs_y)
                return v.X > 0 ? direction.right : direction.left;
            else
                return v.Y > 0 ? direction.down : direction.up;
        }



        // ---------------- Head throw / dash / pickup ----------------

        private void throw_head()
        {
            has_head = false;

            head_projectile head = new head_projectile(content, position);
            head.owner = this;
            Vector2 to_cursor = global.input.scaled_mouse_world_position - position;
            head.velocity = Vector2.Normalize(to_cursor) * head_throw_speed;

            global.signal.spawn_projectile(head);
            thrown_head = head;
        }

        private void update_dash(float delta_time)
        {
            if (thrown_head == null)
            {
                is_dashing = false;
                max_velocity = default_max_velocity; // คืนค่าเดิม
                return;
            }

            dash_timer += delta_time;
            if (dash_timer >= dash_timeout)
            {
                cancel_dash(); // ไปไม่ถึงภายในเวลาที่กำหนด ยกเลิก dash
                return;
            }

            Vector2 to_head = thrown_head.position - position;
            float distance = to_head.Length();

            if (distance <= pickup_radius)
            {
                is_dashing = false;
                max_velocity = default_max_velocity;
                reattach_head();
                velocity = Vector2.Zero;
                return;
            }

            velocity = Vector2.Normalize(to_head) * dash_speed; // ความเร็วคงที่พุ่งตรงเข้าหาหัว
            acceleration = Vector2.Zero;
        }

        private void cancel_dash()
        {
            is_dashing = false;
            max_velocity = default_max_velocity; // คืนเพดานความเร็วปกติ
            velocity = Vector2.Zero; // หยุดนิ่งทันทีตอนยกเลิก กันพุ่งเลยไปแรงๆ ก่อนกลับสู่ physics ปกติ
            dash_timer = 0f;
        }

        private void reattach_head()
        {
            has_head = true;
            if (thrown_head != null)
            {
                thrown_head.time_out(); // ลบตัวเองออกจาก scene และ collision world
                thrown_head = null;
            }
        }

        public override void update_animation(float delta_time)
        {
            if (current_state == state.walk)
            {
                animation_player.play(animation_player.data.data[animation_player_player.animation_name.walk]);
            }
            else
            {
                animation_player.play(animation_player.default_animation);
            }

            base.update_animation(delta_time);
        }
        public override void Draw(SpriteBatch sprite_batch)
        {
            base.Draw(sprite_batch);
        }


        public class animation_player_player : animation_player_base       // custom animation for this class only
        {
            public enum animation_name { idle, walk }

            public static readonly animation_data animation_data = new animation_data();
            public animation_player_player(ContentManager content) : base()
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
                Texture2D idle_texture = content.Load<Texture2D>("Placeholder/Player/Idle");
                animation idle_animation = new animation(idle_texture, frame_per_sec: 2);
                idle_animation.name = "player idle";
                animation_data.data.Add(animation_name.idle, idle_animation);

                Texture2D walk_texture = content.Load<Texture2D>("Placeholder/Player/Walk");
                animation walk_animation = new animation(walk_texture, frame_per_sec: 8);
                walk_animation.name = "player walk";
                animation_data.data.Add(animation_name.walk, walk_animation);
            }
        }
    }

}
