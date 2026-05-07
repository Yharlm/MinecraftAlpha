using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MinecraftAlpha
{
    
    public class World
    {
        
        public Player Plr;
        public int Seed;
        public string Name;
        public string Description;
        public List<TileGrid> GameProgress;

        
    }


    internal class FileManager
    {
        public static void Run() // Test
        {
            // Combines the execution path with your specific file name
            //string fileName = "Structures.json";
            //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            //// Use the path to read the file
            //string content = File.ReadAllText(filePath);
            //




        }

        public static void LoadGame(Game1 game,int id)
        {
            var saves = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "World"), "*.json");
            var save = saves[id];
            string content = File.ReadAllText(save);
            World e = JsonConvert.DeserializeObject<World>(content);
            //game.Player = e.Plr;
            List<TileGrid> temp = [.. e.GameProgress];
            e.GameProgress.Clear();
            //Changes
            foreach (var item in temp)
            {
                var tile = game._blockManager.GetTile(item.pos + Vector3.One*0.5f);
                if (tile == null) {
                    if(!game._blockManager.GetChunk(new Vector2(item.pos.X, item.pos.Y) - Vector2.One * 32, 1))//does chunk exist
                    {
                        game.generation.GenerateChunk(new Vector2(item.pos.X, item.pos.Y)-Vector2.One*32);
                    }
                    //var c = game._blockManager.GetChunk(new Vector2(item.pos.X, item.pos.Y));
                    tile = game._blockManager.GetTile(item.pos);
                }
                
                tile.ID = item.ID;
                game._blockManager.Change(tile);
                
            }
            //game.GameProgress.Clear();

        }
        public static void SaveGame(World World)
        {
            string Folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "World\\" + World.Name + ".json"); // folder path
            var e = JsonConvert.SerializeObject(World);

            File.WriteAllText(Folder, e);

        }
        public static void SaveStructure(Structure content)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Structures.json");
            string s = File.ReadAllText(filePath);
            var list = JsonConvert.DeserializeObject<List<Structure>>(s);
            if (list == null)
            {
                list = new List<Structure>();
            }
            //Turn structure to string

            list.Add(content);
            var e = JsonConvert.SerializeObject(list);

            File.WriteAllText(filePath, e);
        }

        public static List<Structure> GetStructures()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Structures.json");
            string content = File.ReadAllText(filePath);
            List<Structure> e = JsonConvert.DeserializeObject<List<Structure>>(content);
            return e;
        }

        
    }
}
