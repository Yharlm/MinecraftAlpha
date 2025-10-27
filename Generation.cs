using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace MinecraftAlpha
{

    public class Structure
    {

        public static List<Structure> LoadStructures()
        {
            var list = new List<Structure>()
            {
                new Structure()
                {
                    Name = "Tree",
                    id = 0,
                    BluePrint = GetBluePrint(new int[,]
                    {
                        {0,8,8,8,0},
                        {0,8,7,8,0},
                        {8,8,7,8,8},
                        {8,8,7,8,8},
                        {0,0,7,0,0},
                    }
                    )
                }
            };

            return list;
        }

        public string Name;
        public int id;
        public Vector2 position = new Vector2(0, 0);
        public TileGrid[,] BluePrint;

        static public TileGrid[,] GetBluePrint(int[,] structure)
        {
            var tilegrid = new TileGrid[structure.GetLength(0), structure.GetLength(1)];
            for (int y = 0; y < structure.GetLength(0); y++)
            {
                for (int x = 0; x < structure.GetLength(1); x++)
                {
                    tilegrid[y, x] = new TileGrid() { ID = structure[y, x] };
                }
            }
            return tilegrid;
        }

        public void GenerateStructure(TileGrid[,] World, Vector2 position, bool Replace)
        {
            for (int y = 0; y < BluePrint.GetLength(0); y++)
            {
                for (int x = 0; x < BluePrint.GetLength(1); x++)
                {
                    var grid = World[(int)(position.Y + y), (int)(position.X + x)];

                    var blueprintGrid = BluePrint[y,x];

                    grid.ID = blueprintGrid.ID;


                }
            }
        }



    }

    public class Chunk
    {


        public int x;
        public int y;    
        public TileGrid[,] Tiles = new TileGrid[32, 32];
        public Chunk(int x, int y, TileGrid[,] Grid) 
        {
            this.x = x;
            this.y = y;
            Tiles = Grid;
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




        int seed;


        public Generation(int Seed)
        {
            seed = Seed;
        }

        //Generate a layer of terrain at position (x, y)
        public float[,] GenerateGround(int x, int y)
        {
            int Width = 32;
            int Height = 5;
            float[,] Layer = new float[Height, Width]; //The layer to be generated

            // Flat terrain

            // Hills

            // Mountains





            return Layer;
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
            for (int i = 0; i < baseMap.GetLength(0); i++)
            {
                for (int j = 0; j < baseMap.GetLength(1); j++)
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
}
