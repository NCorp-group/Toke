using UnityEngine;

public class Util
{
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;
    }
    
    public static bool IsFieldMissing(GameObject obj) => ReferenceEquals(obj, null) ? false : (obj ? false : true);
}