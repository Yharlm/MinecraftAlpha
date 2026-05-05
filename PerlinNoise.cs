using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

public class PerlinNoise
{
    private int[] p; // permutation table

    public PerlinNoise(int seed)
    {
        p = new int[512];
        Random rand = new Random(seed);

        // generate a shuffled array 0..255
        int[] permutation = new int[256];
        for (int i = 0; i < 256; i++)
            permutation[i] = i;

        // Fisher-Yates shuffle
        for (int i = 255; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = permutation[i];
            permutation[i] = permutation[j];
            permutation[j] = temp;
        }

        // duplicate the array
        for (int i = 0; i < 512; i++)
            p[i] = permutation[i % 256];
    }

    // 2D Perlin Noise
    public float Noise(float x, float y)
    {
        int X = (int)MathF.Floor(x) & 255;
        int Y = (int)MathF.Floor(y) & 255;

        x -= MathF.Floor(x);
        y -= MathF.Floor(y);

        float u = Fade(x);
        float v = Fade(y);

        int A = p[X] + Y;
        int B = p[X + 1] + Y;

        float res = Lerp(v,
            Lerp(u, Grad(p[A], x, y), Grad(p[B], x - 1, y)),
            Lerp(u, Grad(p[A + 1], x, y - 1), Grad(p[B + 1], x - 1, y - 1))
        );

        return (res + 1f) / 2f; // normalize to 0..1
    }

    private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
    private static float Lerp(float t, float a, float b) => a + t * (b - a);

    private static float Grad(int hash, float x, float y)
    {
        int h = hash & 7; // use only 3 bits
        float u = h < 4 ? x : y;
        float v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    public class PerlinWorm
    {
        private Vector2 _position;
        private Vector2 _direction;
        private float _angle;
        private Random _random;
        private float _noiseOffset;

        // Settings
        public float Radius { get; set; } = 2.0f;
        public int Steps { get; set; } = 500;
        public float Speed { get; set; } = 1.0f;
        public List<Vector2> Path { get; private set; }

        public PerlinWorm(Vector2 startPos, int seed)
        {
            _position = startPos;
            _random = new Random(seed);
            _noiseOffset = (float)_random.NextDouble() * 1000f; // Unique noise space
            Path = new List<Vector2>();

            // Initial random direction
            double startAngle = _random.NextDouble() * Math.PI * 2;
            _direction = new Vector2((float)Math.Cos(startAngle), (float)Math.Sin(startAngle));
        }

        public void Dig(int[,] map, int mapWidth, int mapHeight)
        {
            for (int i = 0; i < Steps; i++)
            {
                // 1. Sample 3D noise (using time/step as Z) to get smooth direction changes
                // Using FastNoiseLite or similar is recommended here. 
                // For brevity, using basic Perlin simulation logic:
                float n = GetNoise(_position.X * 0.05f, _position.Y * 0.05f, i * 0.01f + _noiseOffset);

                // 2. Change angle based on noise
                _angle += (n - 0.5f) * 0.5f;
                _direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));

                // 3. Move
                _position += _direction * Speed;

                // 4. Carve (Mark as air/0, assuming 1 is solid)
                CarveCircle(map, mapWidth, mapHeight, (int)_position.X, (int)_position.Y, Radius);
                Path.Add(_position);

                // Boundary check
                if (_position.X < Radius || _position.X > mapWidth - Radius ||
                    _position.Y < Radius || _position.Y > mapHeight - Radius)
                    break;
            }
        }

        // Basic 2D distance calculation to carve a tunnel
        private void CarveCircle(int[,] map, int w, int h, int cx, int cy, float r)
        {
            int rad = (int)Math.Ceiling(r);
            for (int y = cy - rad; y <= cy + rad; y++)
            {
                for (int x = cx - rad; x <= cx + rad; x++)
                {
                    if (x >= 0 && x < w && y >= 0 && y < h)
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy)) <= r)
                        {
                            map[x, y] = 0; // Dig!
                        }
                    }
                }
            }
        }

        // Placeholder for a proper Perlin Noise function (e.g., FastNoiseLite)
        private float GetNoise(float x, float y, float z)
        {
            // Implementation of Perlin noise or mapping to a library like FastNoiseLite
            // For now returning pseudo-random for structure
            return (float)_random.NextDouble();
        }
    }

}
public static class TerrainGenerator
{
    /// <summary>
    /// Generates a terrain heightmap between 0–1.
    /// worldOffsetX/Y = distance traveled in world space
    /// </summary>
    public static float[,] GenerateTerrain(
        int width,
        int height,
        float worldOffsetX,
        float worldOffsetY,
        float scale = 438f,
        int octaves = 4,
        float persistence = 0.5f,
        float lacunarity = 2f,
        int seed = 1337
    )
    {
        float[,] map = new float[height, width];

        Random rand = new Random(seed);

        // offsets per octave
        float[] octaveOffsetX = new float[octaves];
        float[] octaveOffsetY = new float[octaves];

        for (int i = 0; i < octaves; i++)
        {
            octaveOffsetX[i] = rand.Next(-100000, 100000) + worldOffsetX;
            octaveOffsetY[i] = rand.Next(-100000, 100000) + worldOffsetY;
        }

        if (scale <= 0) scale = 0.0001f;

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (x + octaveOffsetX[o]) / scale * frequency;
                    float sampleY = (y + octaveOffsetY[o]) / scale * frequency;

                    float perlin = Perlin.Noise(sampleY, sampleX) * 2f - 1f;
                    noiseHeight += perlin * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxHeight) maxHeight = noiseHeight;
                if (noiseHeight < minHeight) minHeight = noiseHeight;

                map[y, x] = noiseHeight;
            }
        }

        // normalize 0–1
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = (map[y, x] - minHeight) / (maxHeight - minHeight);

        return map;
    }
}

public static class Perlin
{
    static int[] permutation = {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,
        103,30,69,142,8,99,37,240,21,10,23,
        190,6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,
        35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,
        168,68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,
        92,41,55,46,245,40,244,102,143,54,65,25,63,161,
        1,216,80,73,209,76,132,187,208,89,18,169,
        200,196,135,130,116,188,159,86,164,100,109,
        198,173,186,3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,
        206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152,2,44,154,163,
        70,221,153,101,155,167,43,172,9,
        129,22,39,253,19,98,108,110,79,113,
        224,232,178,185,112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,
        162,241,81,51,145,235,249,14,239,107,
        49,192,214,31,181,199,106,157,184,
        84,204,176,115,121,50,45,127,4,150,
        254,138,236,205,93,222,114,67,29,24,
        72,243,141,128,195,78,66,215,61,156,180
    };

    static int[] p;

    static Perlin()
    {
        p = new int[512];
        for (int i = 0; i < 512; i++)
            p[i] = permutation[i % 256];
    }
    public static float Noise(float x, float y)
    {
       return Noise(x, y, 0);
    }
    public static float Noise(float x, float y,int Seed)
    {
        int X = (int)MathF.Floor(x) & 255 + Seed;
        int Y = (int)MathF.Floor(y) & 255 + Seed;

        x -= MathF.Floor(x);
        y -= MathF.Floor(y);

        float u = Fade(x);
        float v = Fade(y);

        int A = p[X] + Y;
        int B = p[X + 1] + Y;

        float res = Lerp(v,
            Lerp(u, Grad(p[A], x, y), Grad(p[B], x - 1, y)),
            Lerp(u, Grad(p[A + 1], x, y - 1), Grad(p[B + 1], x - 1, y - 1))
        );

        return (res + 1f) / 2f; // 0–1
    }

    static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
    static float Lerp(float t, float a, float b) => a + t * (b - a);

    static float Grad(int hash, float x, float y)
    {
        int h = hash & 7;
        float u = h < 4 ? x : y;
        float v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}



public static class BlueNoise
{
    /// <summary>
    /// Generates blue noise points in a 2D rectangle.
    /// </summary>
    /// <param name="width">Width of the area</param>
    /// <param name="height">Height of the area</param>
    /// <param name="minDistance">Minimum distance between points</param>
    /// <param name="seed">Seed for reproducibility</param>
    /// <returns>List of Vector2 points</returns>
    public static List<Vector2> Generate(int width, int height, float minDistance, int seed = 1337)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        Random rand = new Random(seed);

        // start with one random point
        spawnPoints.Add(new Vector2((float)(rand.NextDouble() * width), (float)(rand.NextDouble() * height)));

        int k = 30; // tries per point

        while (spawnPoints.Count > 0)
        {
            int index = rand.Next(spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[index];
            bool candidateAccepted = false;

            for (int i = 0; i < k; i++)
            {
                double angle = rand.NextDouble() * Math.PI * 2;
                double radius = minDistance * (1 + rand.NextDouble());
                Vector2 candidate = spawnCenter + new Vector2((float)(Math.Cos(angle) * radius), (float)(Math.Sin(angle) * radius));

                if (candidate.X >= 0 && candidate.X < width && candidate.Y >= 0 && candidate.Y < height)
                {
                    bool ok = true;
                    foreach (var p in points)
                    {
                        if (Vector2.DistanceSquared(p, candidate) < minDistance * minDistance)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        candidateAccepted = true;
                        break;
                    }
                }
            }

            if (!candidateAccepted)
                spawnPoints.RemoveAt(index);
        }

        return points;
    }
}