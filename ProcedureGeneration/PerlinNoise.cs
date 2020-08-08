using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureGeneration
{
    class PerlinNoise
    {
        static private byte[] PermTable;
        static private float[][] vectors;

        public PerlinNoise()
        {
            PermTable = new byte[2048];
            new Random().NextBytes(PermTable);
            vectors = new float[4][];
            for (int i = 0; i < 4; i++)
            {
                vectors[i] = new float[2] {(float) new Random().NextDouble() * 2 - 1, (float) new Random().NextDouble() * 2 - 1};
            }
        }

        // QuinticCurve
        public static float QuinticCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static float SmoothedNoise2D(float x, float y)
        {
            float corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) +
                 Noise(x - 1, y + 1) + Noise(x + 1, y + 1)) / 16;
            float sides = (Noise(x - 1, y) + Noise(x + 1, y) +
                 Noise(x, y - 1) + Noise(x, y + 1)) / 8;
            float center = Noise(x, y) / 4;
            return corners + sides + center;
        }

        public static float Lerp(float a, float b, float t, Lerps lerp)
        {
            if (lerp == Lerps.CosLerp)
            {
                float ft = t * 3.1415927f;
                float f = (1 - (float) Math.Cos(ft)) * 0.5f;
                return a * (1 - f) + b * f;
            }
            if (lerp == Lerps.LinLerpWithQuintic)
            {
                return a + (b - a) * QuinticCurve(t);
            }
            return a + (b - a) * t;

        }
        public static float Dot(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }

        static float[] GetPseudoRandomGradientVector(int x, int y, PRNG rand)
        {
            int v = rand.GetRandIntFromTwoNumbers(x, y, PermTable) % 4; // Псевдослучайное число от 0 до 3, зависящее от координат x и y

            switch (v)
            {
                case 0: return new float[] { 1, 0 };
                case 1: return new float[] { -1, 0 };
                case 2: return new float[] { 0, 1 };
                default: return new float[] { 0, -1 };
            }
        }

        //static float[] GetPseudoRandomGradientVector(int x, int y)
        //{
        //    float[] randomPoint = new float[] { (float)(new Random().NextDouble() * 2 - 1), (float)(new Random().NextDouble() * 2 - 1) };
        //    randomPoint[0] /=  (float)Math.Sqrt(2); // normalize gradient 
        //    randomPoint[1] /= (float)Math.Sqrt(2); // normalize gradient 
        //}

        public enum Lerps
        {
            LinearLerp,
            LinLerpWithQuintic,
            CosLerp,
        }

        public static float Noise(float fx, float fy, Lerps lerp = Lerps.CosLerp)
        {
            PRNG rand = new PRNG();

            int left = (int)System.Math.Floor(fx);
            int top = (int)System.Math.Floor(fy);
            float pointInQuadX = fx - left;
            float pointInQuadY = fy - top;

            //float[] topLeftGradient = GetPseudoRandomGradientVector(left, top, rand);
            //float[] topRightGradient = GetPseudoRandomGradientVector(left + 1, top,rand);
            //float[] bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1,rand);
            //float[] bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1,rand);

            float[] topLeftGradient = GetPseudoRandomGradientVector(left, top, rand);
            float[] topRightGradient = GetPseudoRandomGradientVector(left + 1, top, rand);
            float[] bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1, rand);
            float[] bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1, rand);

            float[] distanceToTopLeft = { pointInQuadX, pointInQuadY };
            float[] distanceToTopRight = { pointInQuadX - 1, pointInQuadY };
            float[] distanceToBottomLeft = { pointInQuadX, pointInQuadY - 1 };
            float[] distanceToBottomRight = { pointInQuadX - 1, pointInQuadY - 1 };

            float tx1 = Dot(distanceToTopLeft, topLeftGradient);
            float tx2 = Dot(distanceToTopRight, topRightGradient);
            float bx1 = Dot(distanceToBottomLeft, bottomLeftGradient);
            float bx2 = Dot(distanceToBottomRight, bottomRightGradient);

            pointInQuadX = QuinticCurve(pointInQuadX);
            pointInQuadY = QuinticCurve(pointInQuadY);

            float tx = Lerp(tx1, tx2, pointInQuadX, lerp);
            float bx = Lerp(bx1, bx2, pointInQuadX, lerp);
            float tb = Lerp(tx, bx, pointInQuadY, lerp);

            return tb;
        }

        public static float Noise(float fx, float fy, int octaves, float persistence = 0.5f, float frequency = 0.7f, Lerps lerp = Lerps.LinearLerp)
        {
            float amplitude = 1;
            float max = 0;
            float result = 0;

            while (octaves-- > 0)
            {
                max += amplitude;
                result += Noise(fx*frequency, fy*frequency, lerp) * amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }

            return result / max;
        }

    }
}
