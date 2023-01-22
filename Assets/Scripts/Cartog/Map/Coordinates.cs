using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartog.Map
{
    public struct Coordinates
    {
        public readonly int x;
        public readonly int y;

        public Coordinates(Vector2 vec)
        {
            x = Mathf.RoundToInt(vec.x);
            y = Mathf.RoundToInt(vec.y);
        }

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinates(float x, float y)
        {
            this.x = Mathf.RoundToInt(x);
            this.y = Mathf.RoundToInt(y);
        }

        public static Coordinates zero {
            get { return new Coordinates(0, 0); }
        }

        public static Coordinates one
        {
            get { return new Coordinates(1, 1); }
        }

        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.x - b.x, a.y - b.y);
        }
    }
}
