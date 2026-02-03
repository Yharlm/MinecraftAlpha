using System;

public static class PerlinNoise
{
    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private static float Grad(int hash, float x, float y)
    {
        switch (hash & 3)
        {
            case 0: return x + y;
            case 1: return -x + y;
            case 2: return x - y;
            default: return -x - y;
        }
    }

    public static float[,] GenerateSeamless(
        int width,
        int height,
        int periodX,
        int periodY,
        int seed = 0)
    {
        Random rng = new Random(seed);

        int[,] perm = new int[periodX + 1, periodY + 1];

        for (int x = 0; x <= periodX; x++)
            for (int y = 0; y <= periodY; y++)
                perm[x, y] = rng.Next(256);

        float[,] noise = new float[height, width];

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int y = 0; y < height; y++)
        {
            float fy = (float)y / height * periodY;
            int y0 = (int)Math.Floor(fy);
            int y1 = (y0 + 1) % periodY;
            float ty = fy - y0;

            float v = Fade(ty);

            for (int x = 0; x < width; x++)
            {
                float fx = (float)x / width * periodX;
                int x0 = (int)Math.Floor(fx);
                int x1 = (x0 + 1) % periodX;
                float tx = fx - x0;

                float u = Fade(tx);

                float g00 = Grad(perm[x0, y0], tx, ty);
                float g10 = Grad(perm[x1, y0], tx - 1, ty);
                float g01 = Grad(perm[x0, y1], tx, ty - 1);
                float g11 = Grad(perm[x1, y1], tx - 1, ty - 1);

                float nx0 = Lerp(g00, g10, u);
                float nx1 = Lerp(g01, g11, u);
                float value = Lerp(nx0, nx1, v);

                noise[y, x] = value;

                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
        }

        // Normalize to 0–1
        float range = max - min;
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                noise[y, x] = (noise[y, x] - min) / range;

        return noise;
    }
}