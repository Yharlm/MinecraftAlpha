using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            //Turn structure to string



            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Structures.json");
            File.WriteAllText(filePath, "E");
        }
    }
}
