using MinecraftAlpha;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Color = Microsoft.Xna.Framework.Color;

namespace MinecraftAlpha
{

    public class Structure
    {

        

        public string Name;
        public int id;
        public Vector3 position = new Vector3(0, 0, 0); //Finding structures.
        
        public int[,,] Grid;
        public List<TileGrid> Special;

        public void SaveStruct(Vector3 a,Vector3 b,Game1 game)
        {
            Vector3 A = a;
            Vector3 B = b;
            var t = game._blockManager.GetTile(A);

            var structure = this;
            structure.id = t.ID;
            int x = Math.Abs((int)(B.X - A.X) + 1);
            int y = Math.Abs((int)(B.Y - A.Y) + 1);
            int z = Math.Abs((int)(B.Z - A.Z) + 1);
            structure.Grid = new int[z, y, x];
            ////For loop for every block bettween,
            for (float i = 0; i < x; i++)
            {
                for (float j = 0; j < y; j++)
                {
                    for (float k = 0; k < z; k++)
                    {
                        Vector3 p = new Vector3(i, j, k);
                        t = game._blockManager.GetTile(p + A);
                        if (t == null) continue;
                        structure.Grid[(int)p.Z, (int)p.Y, (int)p.X] = t.ID;
                        game._blockManager.SetTile(t, "Air");
                    }
                }

            }
        }

        public void Build(Vector3 A,Game1 game)
        {
            var str = this;
            
            int x = str.Grid.GetLength(2);
            int y = str.Grid.GetLength(1);
            int z = str.Grid.GetLength(0);
            Vector3 pos1 = A - new Vector3(x/2,0,z/2);
            for (float i = 0; i < x; i++)
            {
                for (float j = 0; j < y; j++)
                {
                    for (float k = 0; k < z; k++)
                    {
                        Vector3 p = new Vector3(i, j, k);
                        var t = game._blockManager.GetTile(p + pos1);
                        int id = str.Grid[(int)p.Z, (int)p.Y, (int)p.X];
                        if (t == null || id == 0) continue;
                        var block = game._blockManager.Blocks[id];
                        game._blockManager.SetTile(t, block.Name);


                    }
                }

            }
            //Creates special tiles
            foreach (var tile in str.Special)
            {
                var t = game._blockManager.GetTile(tile.pos + pos1);
            }
        }

    }

    public class HeightMap
    {
        public int x = 0;

        public int[,] Map = new int[10, 32];
        public static void SetHeight(HeightMap heightMap, TileGrid tile)
        {
            return;
            int x = (int)tile.pos.X % 32;
            int y = (int)tile.pos.Y;
            int z = (int)tile.pos.Z;


            heightMap.Map[z, x] = y;



        }

        public static HeightMap GetMap(List<HeightMap> heightMap, int x)
        {
            HeightMap m = heightMap.Find(m => m.x == x);
            if (m == null)
            {
                m = new() { x = x };
                heightMap.Add(m);
            }
            return m;
        }

        public float GetHeight(TileGrid tile)
        {
            return Map[(int)tile.pos.Z % 32, (int)tile.pos.X % 32];
        }

        

    }

    public class Chunk
    {

        public bool Finished = false;
        public int x;
        public int y;

        public TileGrid[,,] Tiles = new TileGrid[10, 32, 32];

        public Chunk(int x, int y, TileGrid[,,] Grid)
        {
            this.x = x;
            this.y = y;
            Tiles = Grid;
        }
        public Chunk(int x, int y)
        {
            this.Finished = true;
            this.x = x;
            this.y = y;

            for (int i = 0; i < Tiles.GetLength(1); i++)
            {
                for (int j = 0; j < Tiles.GetLength(2); j++)
                {
                    for (int z = 0; z < Tiles.GetLength(0); z++)
                    {
                        Vector3 pos = new Vector3(j, i, z);

                        pos.X += (this.x - 1) * 32;
                        pos.Y += (this.y - 1) * 32;




                        Tiles[z, i, j] = new TileGrid()
                        {
                            //brightness = 0,
                            ID = 0,
                            pos = new Vector3(pos.X, pos.Y, pos.Z),
                            Parent = this
                        };
                    }
                }
            }
        }
    }


    //public bool isinchunk(Vector2 pos)
    //{
    //    float ChunkSize = Tiles.GetLength(0) * 32f;
    //    Vector2 chunkPos = new Vector2(x,y) * ChunkSize;
    //    if ()
    //}

}



public class Generation
{

    public Game1 Game;
    public static List<Vector2> CaveGenerate(Vector2 position, int seed, int length)
    {
        Random random = new Random(seed);
        float angle = 0f;
        Vector2 Lastpos = position;
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < length; i++)
        {
            angle += (float)(random.NextDouble() * 2 - 1) * 50; // 10 / -10*
            var rot = Matrix4x4.CreateRotationZ(angle * float.Pi / 180);
            Vector2 dir = Vector2.Transform(new Vector2(2, 0), rot);
            Vector2 point = Lastpos + dir;

            Lastpos = point;





            points.Add(point);

        }
        return points;
    }

    int seed;


    public Generation(int Seed)
    {
        seed = Seed;
    }


    public void GenerateChunk(Vector2 Pos)
    {
        //Overworld
        var chunks = Game.Chunks;
        int chunkX = Game._blockManager.GetChunk(Pos).x;
        int chunkY = Game._blockManager.GetChunk(Pos).y;

        PerlinNoise Perlin = new PerlinNoise(seed);
        Random random = new Random(seed);
        float scale = 0.05f;
        PerlinNoise Ore = new PerlinNoise(seed + 1); // separate noise for ores
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    float worldX = chunkX * 32 + j;
                    float WorldY = chunkY * 32 + i;
                    float worldZ = z;

                    float noise = Perlin.Noise(worldX * scale, worldZ * scale);
                    float Mountains = Perlin.Noise(worldX * scale / 4, z * scale);
                    Mountains -= Perlin.Noise(worldX * scale * 0.2f, z * scale) * 2f;


                    float Y = (noise + Mountains) * 35f;
                    
                    var Tile = Game._blockManager.GetTile(new Vector3(j + chunkX * 32 + 0.2f, i + 0.2f + chunkY * 32, z));
                    //if (Tile == null) continue; // shouldnt happen. 

                    if (Y < Tile.pos.Y) // if the noise value is less than the current height, place a block
                    {
                        Game._blockManager.SetTile(Tile, "Grass");
                    }
                    if (Y + 1 < Tile.pos.Y)
                    {
                        Game._blockManager.SetTile(Tile, "Dirt");
                    }
                    if (Y + 4 < Tile.pos.Y)
                    {
                        Game._blockManager.SetTile(Tile, "Stone");
                    }
                    //if(WorldY > Y)
                    float Distribution = 0.1f;
                    
                    float Coal = Ore.Noise(worldX * Distribution * 1, WorldY * Distribution * 1);
                    float Iron = Ore.Noise(worldX * Distribution * 1 + 1, WorldY * Distribution * 1);
                    float Gold = Ore.Noise(worldX * Distribution * 1 + 2, WorldY * Distribution * 1);
                    
                    if(Coal - float.Abs(z - Perlin.Noise(worldX * scale, worldZ * scale) * 8) / 10f > 0.78f && Y + 4 < Tile.pos.Y)
                    {
                        Game._blockManager.SetTile(Tile, "Coal Ore");
                    }
                }
            }
        }

        return;
            
        //Terrain
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < 32; i++)
            {
                //Perlin for top, 
                //if under top stone
                //ore distirbution
                //use layers to pick what a block will be, perlin for height, stone or ore.




                float worldX = chunkX * 32 + i;

                float worldZ = z;

                float noise = Perlin.Noise(worldX * scale, worldZ * scale);
                float n = Perlin.Noise(worldX * scale / 4, z * scale);
                n += Perlin.Noise(worldX * scale * 2, z * scale) * 0.1f;
                n += Perlin.Noise(worldX * scale * 3, z * scale) * 0.2f;
                noise += n;
                //terrain basic




                float Y = noise * 20f;

                Vector2 placement = new Vector2(worldX + 0.2f, Y) + Vector2.One * 0.2f;
                //PlaceBlock(placement, z, 2);
                Game._blockManager.SetTile(new Vector3(worldX + 0.2f, Y + 0.2f, z), 2, "");

                for (int j = 1; j < 5; j++)
                {
                    //PlaceBlock(placement + new Vector2(0, j), z, 1);
                    Game._blockManager.SetTile(new Vector3(worldX + 0.2f, Y + j + 0.2f, z), 1, "");

                }
                for (int j = 5; j < 52; j++)
                {
                    Game._blockManager.SetTile(new Vector3(worldX + 0.2f, Y + j + 0.2f, z), 4, "");
                    //PlaceBlock(placement + new Vector2(0, j), z, 4);
                }
                




            }
        }

        // --------------------------------------
        // ORE GENERATION
        // --------------------------------------

        PerlinNoise Noise = new PerlinNoise(seed + 1); // separate noise for ores
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 62; j++)
                {
                    float spread = 0.22f;
                    float worldX = chunkX * 32 + i;
                    float worldY = j;
                    float worldZ = z;
                    float Coal = Noise.Noise(worldX * spread * 1, worldY * spread * 1);
                    float Iron = Noise.Noise(worldX * spread * 1, worldY * spread * 1);
                    float Gold = Noise.Noise(worldX * spread * 1, worldY * spread * 1);
                    if (Coal - float.Abs(z - Perlin.Noise(worldX * scale, worldZ * scale) * 8) / 20f > 0.78f)
                    {
                        var tile = Game._blockManager.GetTile(new Vector3(worldX + 0.2f, worldY + 0.2f, worldZ));
                        if (Game._blockManager.getBlock(tile).Name == "Stone")
                        {
                            Game._blockManager.SetTile(tile, "Coal Ore");
                        }
                    }
                    if (Iron - float.Abs(z - Perlin.Noise(worldX * scale + 30, worldZ * scale) * 8) / 20f > 0.79f)
                    {
                        var tile = Game._blockManager.GetTile(new Vector3(worldX + 0.2f, worldY + 0.2f, worldZ));
                        if (Game._blockManager.getBlock(tile).Name == "Stone")
                        {
                            Game._blockManager.SetTile(tile, "Iron Ore");
                        }
                    }
                }
            }
        }

        //Cave generation
        
        //Sky light check
        var heightMap = HeightMap.GetMap(Game.HeightMaps, chunkX);
        SetHeights(heightMap);


    }

    public void SetHeights(HeightMap Map)
    {
        var c = GetTallest(Map.x);
        Vector3 Chunk = new Vector3((c.x) * 32, (c.y-1) * 32, 0);
        for (int i = 0; i < Map.Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.Map.GetLength(1); j++)
            {
                float height = 0;
                for (int k = 0; k < 32; k++)
                {
                    
                    var tile = Game._blockManager.GetTile(new Vector3(j, k, i) + Chunk + new Vector3(0.5f));
                    if (tile == null) continue;
                    if (Game._blockManager.getBlock(tile).Name != "Air" && Game._blockManager.getBlock(tile).Solid)
                    {
                        height = k;
                        //tile.Color = Color.Blue;
                        break;
                    }
                }
                Map.Map[i, j] = (int)height;
            }
        }
    }

    

    public Chunk GetTallest(int x)
    {

        Chunk tallest = null;
        foreach (var chunk in Game.Chunks)
        {
            if (chunk.x == x)
            {
                if (tallest == null || chunk.y < tallest.y)
                {
                    tallest = chunk;
                }
            }
        }
        return tallest;

    }



    public static float[,] GenerateWhiteNoise(int Width, int Height, int seed, int Area)
    {
        float[,] Noise = new float[Height, Width];
        Random random = new Random(seed);
        for (int i = 0; i < Height; i++)
        {


            for (int j = 0; j < Width; j++)
            {

                if (random.Next(0, Area + 1) != Area / 2)
                {
                    continue;
                }
                Noise[i, j] = (float)random.NextDouble();
            }
        }



        return Noise;
    }

    public static float Lerp(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1;
    }

    public static float[,] GenerateSmoothNoise(float[,] Grid, int octave)
    {


        int width = Grid.GetLength(1);
        int height = Grid.GetLength(0);
        float[,] smoothed = new float[height, width];

        int samplePeriod = 1 << octave; // calculates 2 ^ k
        float sampleFrequency = 1.0f / samplePeriod;

        for (int y = 0; y < height; y++)
        {
            int sample_y0 = (y / samplePeriod) * samplePeriod;
            int sample_y1 = (sample_y0 + samplePeriod) % height; //wrap around
            float vertical_blend = (y - sample_y0) * sampleFrequency;
            for (int x = 0; x < width; x++)
            {
                int sample_x0 = (x / samplePeriod) * samplePeriod;
                int sample_x1 = (sample_x0 + samplePeriod) % width; //wrap around
                float horizontal_blend = (x - sample_x0) * sampleFrequency;

                float top = Lerp(Grid[sample_y0, sample_x0], Grid[sample_y0, sample_x1], horizontal_blend);
                float bottom = Lerp(Grid[sample_y1, sample_x0], Grid[sample_y1, sample_x1], horizontal_blend);

                smoothed[y, x] = Lerp(top, bottom, vertical_blend);
            }
        }





        return smoothed;
    }

    


}


