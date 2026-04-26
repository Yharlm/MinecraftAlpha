using MinecraftAlpha;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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


    }

    public class Chunk
    {


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

        PerlinNoise Perlin = new PerlinNoise(seed);
        Random random = new Random(seed);


        float scale = 0.05f;
        //Terrain
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < 32; i++)
            {
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
                HeightMap.SetHeight(HeightMap.GetMap(Game.HeightMaps, chunkX), Game._blockManager.GetTile(new Vector3(worldX + 0.2f, Y, z)));




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
                    float spread = 0.02f;


                    float worldX = chunkX * 32 + i;
                    float worldY = j;
                    float worldZ = z;
                    float Coal = Noise.Noise(worldX * spread * 1, worldY * spread * 3);
                    float Iron = Noise.Noise(worldX * spread * 4, worldY * spread * 3);
                    float Gold = Noise.Noise(worldX * spread * 2, worldY * spread * 3);
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


    }



    public void PlaceBlock(Vector2 pos, int z, int id)
    {
        List<Chunk> chunks = Game.Chunks;
        //TileGrid Tile = BlockManager.GetBlockAtPos(pos, z, chunks);
        var chunk = Game._blockManager.GetChunk(pos);
        TileGrid Tile = Game._blockManager.GetTile(new(pos.X, pos.Y, z));



        //if (Tile == null)
        //{

        //    var ChunkNot = BlockManager._blockManager(pos);
        //    chunks.Add(new(ChunkNot[0], ChunkNot[1]));
        //    Tile = BlockManager.GetBlockAtPos(pos, z, chunks);
        //}
        Tile.ID = id;
        //chunk.HeightMap[(int)(pos.X % 32), (int)(pos.Y % 32)] = (int)pos.Y;
    }



    //Terrain algoritms




    public static float[,] GenerateFlat(int Width, int Height, float peak)
    {
        float[,] Noise = new float[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {

                Noise[i, j] = peak;
            }
        }



        return Noise;
    }

    public static float[,] GenerateMaskBlend(int Width, int Height, float Opacity)
    {
        float[,] Map = new float[Height, Width];
        float centerX = Width / 2f;
        float centerY = Height / 2f;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Map[i, j] = float.Abs(centerX - j) / 16;


            }
        }



        return Map;
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

    public static float[,] GeneratePerlinNoise(float[,] Base, int octaves, float persistance)
    {
        int width = Base.GetLength(1);
        int height = Base.GetLength(0);

        var perlin = new float[height, width];

        float[][,] smoothNoise = new float[octaves][,];



        for (int i = 0; i < octaves; i++)
        {
            smoothNoise[i] = GenerateSmoothNoise(Base, i);
        }
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        for (int i = octaves - 1; i >= 0; i--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    perlin[y, x] += smoothNoise[i][y, x] * amplitude;
                }
            }

        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                perlin[y, x] /= totalAmplitude;
            }
        }

        return perlin;

    }

    public static void SumMaps(float[,] baseMap, float[,] addedMap, float weight)
    {
        for (int i = 0; i < addedMap.GetLength(0); i++)
        {
            for (int j = 0; j < addedMap.GetLength(1); j++)
            {
                baseMap[i, j] += addedMap[i, j] * weight;
            }
        }
    }
    public static void SubMaps(float[,] baseMap, float[,] addedMap, float weight)
    {
        for (int i = 0; i < baseMap.GetLength(0); i++)
        {
            for (int j = 0; j < baseMap.GetLength(1); j++)
            {
                baseMap[i, j] -= addedMap[i, j] * weight;
            }
        }
    }

    public static void Mask(float[,] baseMap, float[,] Mask, float weight)
    {
        for (int i = 0; i < baseMap.GetLength(0); i++)
        {
            for (int j = 0; j < baseMap.GetLength(1); j++)
            {
                baseMap[i, j] *= (1 - Mask[i, j] * weight);
            }
        }
    }

}


