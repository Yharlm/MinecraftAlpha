using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MinecraftAlpha
{
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
        

        
        public static void SaveStructure(Structure content)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Structures.json");
            string s = File.ReadAllText(filePath);
            var list = JsonConvert.DeserializeObject<List<Structure>>(s);
            
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
