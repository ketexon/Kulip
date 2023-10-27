using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulip.Utility
{
    public static class VectorExtensions
    {
        public static Vector2 XY(this Vector3 vec) => new(vec.x, vec.y);
        public static Vector2 XZ(this Vector3 vec) => new(vec.x, vec.z);
        public static Vector2 YZ(this Vector3 vec) => new(vec.y, vec.z);

        public static Vector2 YX(this Vector3 vec) => new(vec.y, vec.x);
        public static Vector2 ZX(this Vector3 vec) => new(vec.z, vec.x);
        public static Vector2 ZY(this Vector3 vec) => new(vec.z, vec.y);

        public static Vector3 ProjectXY(this Vector3 vec) => new(vec.x, vec.y, 0);
        public static Vector3 ProjectXZ(this Vector3 vec) => new(vec.x, 0, vec.z);
        public static Vector3 ProjectYZ(this Vector3 vec) => new(0, vec.y, vec.z);
    }
}
