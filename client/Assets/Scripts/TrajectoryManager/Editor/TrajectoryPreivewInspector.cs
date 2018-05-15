using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TrajectoryPreview))]
public class TrajectoryPreivewInspector : Editor {

    TrajectoryPreview targetScript;
    SerializedProperty from;
    SerializedProperty to;
    SerializedProperty go;

    SerializedProperty data;

    TrajectoryCfg config;


    public void OnEnable()
    {
        targetScript = target as TrajectoryPreview;

        from = serializedObject.FindProperty("from");
        to = serializedObject.FindProperty("to");
        go = serializedObject.FindProperty("go");

        data = serializedObject.FindProperty("data");
        config = new TrajectoryCfg();
        config.Data = data.stringValue;
        TrajectoryCfgLoader.InitRuntimeData(config);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(from, new GUIContent("Trajectory From"));
        EditorGUILayout.PropertyField(to, new GUIContent("Trajectory To"));
        EditorGUILayout.PropertyField(go, new GUIContent("Preview Object"));

        {
            EditorGUI.BeginChangeCheck();

            config.trajectoryType = (TrajectoryTypes)EditorGUILayout.EnumPopup("Trajectory Type", config.trajectoryType);

            if (EditorGUI.EndChangeCheck())
            {
                data.stringValue = "";
            }

            EditorGUI.BeginChangeCheck();

            switch (config.trajectoryType)
            {
                case TrajectoryTypes.Line:
                    {
                        TrajectoryLineParams lineParams;
                        lineParams = config.trajectoryParameters as TrajectoryLineParams;

                        if (lineParams == null)
                        {
                            lineParams = new TrajectoryLineParams();
                            config.trajectoryParameters = lineParams;
                        }

                        lineParams.Speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("Speed", lineParams.Speed));
                    }
                    break;

                case TrajectoryTypes.Parabola:
                    {
                        TrajectoryParabolaParams parabolaParams;
                        parabolaParams = config.trajectoryParameters as TrajectoryParabolaParams;

                        if (parabolaParams == null)
                        {
                            parabolaParams = new TrajectoryParabolaParams();
                            config.trajectoryParameters = parabolaParams;
                        }

                        parabolaParams.Speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("Speed", parabolaParams.Speed));
                        parabolaParams.VAcceleration = Mathf.Min(-0.01f, EditorGUILayout.FloatField("Vertical Acceleration", parabolaParams.VAcceleration));
                    }
                    break;

                case TrajectoryTypes.Bezier:
                    {
                        TrajectoryBezierParams bezierParams;
                        bezierParams = config.trajectoryParameters as TrajectoryBezierParams;

                        if (bezierParams == null)
                        {
                            bezierParams = new TrajectoryBezierParams();
                            config.trajectoryParameters = bezierParams;
                        }

                        bezierParams.Speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("Speed", bezierParams.Speed));
                        //bezierParams.p0 = EditorGUILayout.Vector3Field("Control Point0", bezierParams.p0);
                        if (from.objectReferenceValue != null)
                        {
                            Vector3 p0 = (from.objectReferenceValue as Transform).position;
                            if (bezierParams.p0 != p0)
                            {
                                bezierParams.p0 = p0;
                                GUI.changed = true;
                            }
                        }

                        if (to.objectReferenceValue != null)
                        {
                            Vector3 p3 = (to.objectReferenceValue as Transform).position;
                            if (bezierParams.p3 != p3)
                            {
                                bezierParams.p3 = p3;
                                GUI.changed = true;
                            }
                        }

                        bezierParams.p1 = EditorGUILayout.Vector3Field("Control Point1", bezierParams.p1);
                        bezierParams.p2 = EditorGUILayout.Vector3Field("Control Point2", bezierParams.p2);
                        //bezierParams.p3 = EditorGUILayout.Vector3Field("Control Point3", bezierParams.p3);
                    }
                    break;

                //case TrajectoryTypes.CatmullRom:
                //    {
                //        TrajectoryCatmullRomParams catmullRomParams;
                //        catmullRomParams = config.trajectoryParameters as TrajectoryCatmullRomParams;

                //        if (catmullRomParams == null)
                //        {
                //            catmullRomParams = new TrajectoryCatmullRomParams();
                //            config.trajectoryParameters = catmullRomParams;
                //        }

                //        catmullRomParams.Speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("Speed", catmullRomParams.Speed));
                //        //catmullRomParams.p0 = EditorGUILayout.Vector3Field("Control Point0", catmullRomParams.p0);
                //        if (from.objectReferenceValue != null)
                //        {
                //            Vector3 p0 = (from.objectReferenceValue as Transform).position;
                //            if (catmullRomParams.p0 != p0)
                //            {
                //                catmullRomParams.p0 = p0;
                //                GUI.changed = true;
                //            }
                //        }

                //        if (to.objectReferenceValue != null)
                //        {
                //            Vector3 p3 = (to.objectReferenceValue as Transform).position;
                //            if (catmullRomParams.p3 != p3)
                //            {
                //                catmullRomParams.p3 = p3;
                //                GUI.changed = true;
                //            }
                //        }

                //        catmullRomParams.tension = EditorGUILayout.Slider(catmullRomParams.tension, 0, 1.0f);

                //        catmullRomParams.p1 = EditorGUILayout.Vector3Field("Control Point1", catmullRomParams.p1);
                //        catmullRomParams.p2 = EditorGUILayout.Vector3Field("Control Point2", catmullRomParams.p2);
                //        //catmullRomParams.p3 = EditorGUILayout.Vector3Field("Control Point3", catmullRomParams.p3);
                //    }
                //    break;

                default:
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                data.stringValue = string.Format("{0}={1}", (int)config.trajectoryType, JsonUtility.ToJson(config.trajectoryParameters));
            }
        }

        EditorGUI.BeginChangeCheck();
        data.stringValue = EditorGUILayout.TextArea(data.stringValue);
        if (EditorGUI.EndChangeCheck())
        {
            config.Data = data.stringValue;
            try
            {
                TrajectoryCfgLoader.InitRuntimeData(config);

                if (config.trajectoryType == TrajectoryTypes.Bezier)
                {
                    TrajectoryBezierParams bezierParams = config.trajectoryParameters as TrajectoryBezierParams;
                    if (bezierParams != null)
                    {
                        if (from.objectReferenceValue != null)
                        {
                            Transform fromTrans = from.objectReferenceValue as Transform;
                            fromTrans.position = bezierParams.p0;
                        }

                        if (to.objectReferenceValue != null)
                        {
                            Transform toTrans = to.objectReferenceValue as Transform;
                            toTrans.position = bezierParams.p3;
                        }

                    }
                }
                //else if (config.trajectoryType == TrajectoryTypes.CatmullRom)
                //{
                //    TrajectoryCatmullRomParams catmullRomParams = config.trajectoryParameters as TrajectoryCatmullRomParams;
                //    if (catmullRomParams != null)
                //    {
                //        if (from.objectReferenceValue != null)
                //        {
                //            Transform fromTrans = from.objectReferenceValue as Transform;
                //            fromTrans.position = catmullRomParams.p0;
                //        }

                //        if (to.objectReferenceValue != null)
                //        {
                //            Transform toTrans = to.objectReferenceValue as Transform;
                //            toTrans.position = catmullRomParams.p3;
                //        }
                //    }
                //}
            }
            catch
            {
                config.trajectoryType = TrajectoryTypes.Invalid;
                config.trajectoryParameters = null;
            }
            
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        
        if (GUILayout.Button("Copy Data"))
        {
            EditorGUIUtility.systemCopyBuffer = data.stringValue;
        }

        EditorGUILayout.Separator();

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Play"))
            {
                targetScript.Play();
            }
        }
    }

    public void OnSceneGUI()
    {
        if (config == null)
            return;

        Handles.color = Color.cyan;
        const int SEGMENTS = 50;

        if (from.objectReferenceValue == null || to.objectReferenceValue == null)
            return;

        Vector3 p0 = (from.objectReferenceValue as Transform).position;
        Vector3 p1 = (to.objectReferenceValue as Transform).position;

        switch (config.trajectoryType)
        {
            case TrajectoryTypes.Line:
                Handles.DrawLine(p0, p1);
                break;

            case TrajectoryTypes.Parabola:

                TrajectoryParabolaParams parabolaParams;
                parabolaParams = config.trajectoryParameters as TrajectoryParabolaParams;

                if (parabolaParams != null)
                {
                    List<Vector3> lines = new List<Vector3>();

                    Vector3 dir = p1 - p0;
                    dir.y = 0;
                    float dist = dir.magnitude;
                    float totalTime = dist / parabolaParams.Speed;

                    Vector3 last = p0;

                    for (int i = 1; i < SEGMENTS; i++)
                    {
                        float elapsed = (float)i / SEGMENTS * totalTime;
                        Vector3 cur = TrajectoryUtil.GetParabolaPoint(p0, p1, parabolaParams.VAcceleration, elapsed, totalTime);
                        //Handles.DrawLine(cur, cur + Vector3.right);

                        lines.Add(last);
                        lines.Add(cur);
                        last = cur;
                    }

                    lines.Add(last);
                    lines.Add(p1);

                    Handles.DrawLines(lines.ToArray());
                }
                break;

            case TrajectoryTypes.Bezier:

                TrajectoryBezierParams bezierParams;
                bezierParams = config.trajectoryParameters as TrajectoryBezierParams;

                if (bezierParams != null)
                {
                    List<Vector3> lines = new List<Vector3>();

                    EditorGUI.BeginChangeCheck();

                    bezierParams.p1 = Handles.PositionHandle(bezierParams.p1, Quaternion.identity);
                    Handles.SphereHandleCap(0, bezierParams.p1, Quaternion.identity, HandleUtility.GetHandleSize(bezierParams.p1)* 0.2f, EventType.Layout);
                    bezierParams.p2 = Handles.PositionHandle(bezierParams.p2, Quaternion.identity);
                    Handles.SphereHandleCap(0, bezierParams.p2, Quaternion.identity, HandleUtility.GetHandleSize(bezierParams.p1)* 0.2f, EventType.Layout);

                    if (EditorGUI.EndChangeCheck())
                    {
                        data.stringValue = string.Format("{0}={1}", (int)config.trajectoryType, JsonUtility.ToJson(config.trajectoryParameters));
                        serializedObject.ApplyModifiedProperties();
                        Repaint();
                    }


                    Vector3 last = p0;

                    for (int i = 1; i <= SEGMENTS; i++)
                    {
                        float t = (float)i / SEGMENTS;

                        Vector3 cur = TrajectoryUtil.GetBezierPoint(p0, bezierParams.p1, bezierParams.p2, p1, t);

                        lines.Add(last);
                        lines.Add(cur);
                        last = cur;
                    }

                    lines.Add(last);
                    lines.Add(p1);

                    Handles.DrawLines(lines.ToArray());
                }

                break;

            //case TrajectoryTypes.CatmullRom:

            //    TrajectoryCatmullRomParams catmullRomParams;
            //    catmullRomParams = config.trajectoryParameters as TrajectoryCatmullRomParams;

            //    if (catmullRomParams != null)
            //    {
            //        List<Vector3> lines = new List<Vector3>();

            //        EditorGUI.BeginChangeCheck();

            //        catmullRomParams.p1 = Handles.PositionHandle(catmullRomParams.p1, Quaternion.identity);
            //        Handles.SphereCap(0, catmullRomParams.p1, Quaternion.identity, HandleUtility.GetHandleSize(catmullRomParams.p1) * 0.2f);
            //        catmullRomParams.p2 = Handles.PositionHandle(catmullRomParams.p2, Quaternion.identity);
            //        Handles.SphereCap(0, catmullRomParams.p2, Quaternion.identity, HandleUtility.GetHandleSize(catmullRomParams.p1) * 0.2f);

            //        if (EditorGUI.EndChangeCheck())
            //        {
            //            Repaint();
            //        }


            //        Vector3 last = p0;

            //        for (int i = 1; i <= SEGMENTS; i++)
            //        {
            //            float t = (float)i / SEGMENTS;

            //            Vector3 cur = TrajectoryUtil.GetCatmullRomPoint(p0, catmullRomParams.p1, catmullRomParams.p2, p1, t, catmullRomParams.tension);

            //            lines.Add(last);
            //            lines.Add(cur);
            //            last = cur;
            //        }

            //        lines.Add(last);
            //        lines.Add(p1);

            //        Handles.DrawLines(lines.ToArray());
            //    }


            //    break;
        }

    }

}
