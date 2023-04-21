using Unity.Mathematics;

namespace FMP
{
    public static class Util
    {
        public static float GetSimplexNoise(float x, float y, int octaves = 1)
        {
            float2 pos = new(x, y);
            float res = 0f;
            float max = 0f;
            float mult = 1f;
            for (int i = 0; i < octaves; i++)
            {
                res += noise.snoise(pos) * mult;
                max += mult;
                mult *= 0.5f;
                pos /= 0.5f;
            }
            return res / max;
        }
    }
}
