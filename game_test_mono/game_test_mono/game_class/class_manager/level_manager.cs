using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Threading;

namespace old_heart
{
    public class level_manager
    {
        public game_manager game_manager;

        public string current_level_file;

        public List<entity> entity_list; //  all entity in this level
        public List<collision_shape> wall_collision_list;
        public List<node> map_node_list; //  world object (wall)
        public List<node> high_map_node_list; //  world object but draw above (player enemy and all particle)
        public level_manager(game_manager game_manager)
        {
            this.game_manager = game_manager;

            entity_list = game_manager.entity_manager.entity_list;
            wall_collision_list = game_manager.collision_manager.wall_list;
            map_node_list = game_manager.map_manager.map_node_list;
            high_map_node_list = game_manager.map_manager.high_map_node_list;

        }
        public void set_level_file(string level_file)
        {
            string file_directory = AppDomain.CurrentDomain.BaseDirectory;
            string game_root_file = Path.GetFullPath(Path.Combine(file_directory, "..", "..", ".."));  // get true game sorce code file not temporary
            string level_folder = Path.Combine(game_root_file, "Content", "level");
            current_level_file = Path.Combine(level_folder, level_file);
        }
        public void clear_level()  //not true clear all  (still have collision in collision world)            use for level editor only
        {
            entity_list.Clear();
            wall_collision_list.Clear();
            map_node_list.Clear();
            high_map_node_list.Clear();
            game_manager.debug_manager.debug_node_list.Clear();

            Debug.WriteLine("-------- Cleared level -------");
        }
        public void save_level()
        {
            if (current_level_file == null)
            {
                Debug.WriteLine("level file is null error");
            }
            level_data level_data = new level_data(current_level_file);


            foreach (collision_shape collision in wall_collision_list)
            {
                if(collision is collision_shape_box box)
                {
                    level_object wall = new level_object("wall_collision_rectangle",box.Shape.BoundingBox.Min.X, box.Shape.BoundingBox.Min.Y);
                    wall.data["size_x"] = box.Shape.BoundingBox.Size.X.ToString();
                    wall.data["size_y"] = box.Shape.BoundingBox.Size.Y.ToString();
                    level_data.level_object_list.Add(wall);
                }
            }
            

            string json_string = JsonSerializer.Serialize(level_data, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(current_level_file, json_string);

            Debug.WriteLine("Saved level : " + current_level_file);
        }
        public void load_level()
        {
            clear_level();
            if (current_level_file == null)
            {
                Debug.WriteLine("level file is null error");
            }

            level_data level_data_in_file = JsonSerializer.Deserialize<level_data>(File.ReadAllText(current_level_file));

            foreach(level_object level_object in level_data_in_file.level_object_list)
            {
                if (level_object.type == "wall_collision_rectangle")
                {
                    Debug.WriteLine("Load  : " + level_object.type);
                    float size_x = 0;
                    float size_y = 0;

                    float.TryParse(level_object.data["size_x"], out size_x);
                    float.TryParse(level_object.data["size_y"], out size_y);
                    game_manager.add_map_collision(new collision_shape_box((BoundingBox2D.CreateFromPositionAndSize(new Vector2(level_object.position_x,level_object.position_y) , new Vector2(size_x,size_y) ))));
                }
                else
                {
                    Debug.WriteLine("ERROR cant load type : " + level_object.type);
                }
            }
            Debug.WriteLine("Loaded level : " + level_data_in_file.level_object_list);
        }

        public class level_data
        {
            public string name { get; set; }
            public List<level_object> level_object_list { get; set; } = new List<level_object>();
            public level_data(string name)
            {
                this.name = Path.GetFileName(name);
            }
        }
        public class level_object
        {
            public string type { get; set; }
            public float position_x { get; set; }
            public float position_y { get; set; }
            public Dictionary<string, string> data { get; set; } = new Dictionary<string, string>();

            public level_object(string type, float position_x, float position_y)
            {
                this.type = type;
                this.position_x = position_x;
                this.position_y = position_y;
            }
        }
    }
}