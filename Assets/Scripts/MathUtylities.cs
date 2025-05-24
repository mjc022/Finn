using UnityEngine;

public class MathUtylities : MonoBehaviour
{
    // esta clase es una coleccion de funciones matematicas utiles para el juego
    public static class MathUtils
    {
        public static bool IsFinite(Vector3 v)
        {
            return IsFinite(v.x) && IsFinite(v.y) && IsFinite(v.z);
        }

        public static bool IsFinite(float f)
        {
            return !float.IsNaN(f) && !float.IsInfinity(f);
        }
    }

}
