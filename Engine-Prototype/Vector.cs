using System;

namespace Engine_Protoype
{
    public struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        public static Vector operator +(Vector v, Vector v2)
        {
            return new Vector(v.x + v2.x, v.y + v2.y, v.z + v2.z);
        }

        public static Vector operator -(Vector v, Vector v2)
        {
            return new Vector(v.x - v2.x, v.y - v2.y, v.z - v2.z);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public static Vector Max(Vector a, Vector b)
        {
            return new Vector(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }

        public static Vector Min(Vector a, Vector b)
        {
            return new Vector(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }

        public static float Distance(Vector a, Vector b)
        {
            float e1 = Max(a, b).x - Min(a, b).x;
            float e2 = Max(a, b).y - Min(a, b).y;

            return (float) Math.Sqrt(Math.Pow(e1, 2) + Math.Pow(e2, 2));
        }
    }
}