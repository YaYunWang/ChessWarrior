using UnityEngine;
using System.Collections;

public static class TrajectoryUtil {

    public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 GetCatmullRomPoint(Vector3 previous, Vector3 start, Vector3 end, Vector3 next,
                            float t, float tension = 0.5f)
    {
        // References used:
        // p.266 GemsV1
        //
        // tension is often set to 0.5 but you can use any reasonable value:
        // http://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
        //
        // bias and tension controls:
        // http://local.wasp.uwa.edu.au/~pbourke/miscellaneous/interpolation/

        float percentComplete = t;
        float percentCompleteSquared = percentComplete * percentComplete;
        float percentCompleteCubed = percentCompleteSquared * percentComplete;

        return previous * (-tension * percentCompleteCubed +
                             2 * tension * percentCompleteSquared +
                              -tension * percentComplete) +
                start * ((2 - tension) * percentCompleteCubed +
                            (tension - 3) * percentCompleteSquared + 1.0f) +
                end * ((tension - 2) * percentCompleteCubed +
                          (3 - 2 * tension) * percentCompleteSquared +
                               tension * percentComplete) +
                next * (tension * percentCompleteCubed +
                              -tension * percentCompleteSquared);
    }

    public static Vector3 GetParabolaPoint(Vector3 start, Vector3 end, float gravity, float elapsed, float duration)
    {
        Vector3 pos = Vector3.Lerp(start, end, elapsed / duration);
        pos.y = 0;

        float squaredElapsed = elapsed * elapsed;
        float squaredTotalTime = duration * duration;
        float halfAcc = gravity * 0.5f;

        // 尝试拆分成为上升和下降，不成功的话直接抛向目标
        // y1 - y0 = (1/2)*a*(t0^2)
        // y1 - y2 = (1/2)*a*(t1^2)
        // t = t0 + t1
        // t0 > 0 and t1 > 0

        float t0 = (2 * (-start.y + end.y) / Mathf.Abs(gravity) + squaredTotalTime) / (2 * duration);
        float t1 = duration - t0;

        if (t0 > 0 && t1 > 0)
        {
            float v0 = -gravity * t0;

            if (elapsed <= t0)
            {
                float y = start.y + v0 * elapsed + halfAcc * squaredElapsed;
                pos.y = y;
            }
            else
            {
                float elapsed2 = elapsed - t0;
                float y1 = start.y + (0 + v0) * t0 * 0.5f;

                float y = y1 + halfAcc * (elapsed2 * elapsed2);
                pos.y = y;
            }
        }
        else
        {
            // 直接投掷向目标点
            float v0 = (end.y - start.y - halfAcc * squaredTotalTime) / duration;
            float y = start.y + v0 * elapsed + halfAcc * squaredElapsed;
            pos.y = y;
        }

        return pos;
    }

    public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3 pos = GetBezierPoint(p0, p1, p2, p3, t);

        return TransformPointFromLocalToWorld(pos, p0, p3, worldStart, worldEnd);
    }

    public static Vector3 GetCatmullRomPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float tension, Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3 pos = GetCatmullRomPoint(p0, p1, p2, p3, t, tension);

        return TransformPointFromLocalToWorld(pos, p0, p3, worldStart, worldEnd);
    }

    private static Vector3 TransformPointFromLocalToWorld(Vector3 pos, Vector3 localP0, Vector3 localP1, Vector3 worldP0, Vector3 worldP1)
    {
        // 获得配置空间以起点终点距离为单位的1坐标
        Vector3 dir = localP1 - localP0;
        float dist = dir.magnitude;
        Vector3 localZ = dir / dist;
        Vector3 localX;
        if (localZ == Vector3.up)
            localX = Vector3.back;      // 垂直时任意选一水平轴
        else
            localX = Vector3.Cross(Vector3.up, localZ);

        Vector3 localY = Vector3.Cross(localZ, localX);

        Matrix4x4 mat = Matrix4x4.identity;
        float oneOverDist = 1 / dist;
        mat.SetColumn(0, localX * oneOverDist);
        mat.SetColumn(1, localY * oneOverDist);
        mat.SetColumn(2, localZ * oneOverDist);

        Matrix4x4.Transpose(mat);           // 正交矩阵求逆直接转置

        Vector3 localPos = mat.MultiplyPoint3x4(pos - localP0);

        // 计算世界空间坐标
        dir = worldP1 - worldP0;
        dist = dir.magnitude;
        localZ = dir / dist;
        if (localZ == Vector3.up)
            localX = Vector3.back;
        else
            localX = Vector3.Cross(Vector3.up, localZ);

        localY = Vector3.Cross(localZ, localX);

        mat = Matrix4x4.identity;
        mat.SetColumn(0, localX * dist);
        mat.SetColumn(1, localY * dist);
        mat.SetColumn(2, localZ * dist);

        pos = mat.MultiplyPoint3x4(localPos) + worldP0;

        return pos;
    }

}
