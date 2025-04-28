//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SimpleIL))]
//public class SimpleIKSolverEditor : Editor
//{
//    static GUIStyle errorBox;

//    void OnEnable()
//    {
//        errorBox = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).box);
//        errorBox.normal.textColor = Color.red;
//    }

//    [DrawGizmo(GizmoType.Selected)]
//    static void OnDrawGizmosSelected(SimpleIL siks, GizmoType gizmoType)
//    {
//        Gizmos.color = Color.magenta;
//        Handles.color = Color.blue;
//        Gizmos.color = Color.red;

//        if (siks.pivot == null)
//        {
//            Handles.Label(siks.transform.position, "Pivot is not assigned", errorBox);
//            return;
//        }
//        if (siks.upper == null)
//        {
//            Handles.Label(siks.pivot.position, "Upper is not assigned", errorBox);
//            return;
//        }
//        if (siks.lower == null)
//        {
//            Handles.Label(siks.upper.position, "Lower is not assigned", errorBox);
//            return;
//        }
//        if (siks.effector == null)
//        {
//            Handles.Label(siks.lower.position, "Effector is not assigned", errorBox);
//            return;
//        }
//        if (siks.tip == null)
//        {
//            Handles.Label(siks.effector.position, "Tip is not assigned", errorBox);
//            return;
//        }

//        Handles.DrawPolyLine(siks.pivot.position, siks.upper.position, siks.lower.position, siks.effector.position, siks.tip.position);

//        if (siks.targetTransform != null)
//        {
//            Handles.DrawDottedLine(siks.tip.position, siks.targetTransform.position, 3);
//            Handles.Label(siks.targetTransform.position, "Target");
//            var distanceToTarget = Vector3.Distance(siks.targetTransform.position, siks.tip.position);
//            var midPoint = Vector3.Lerp(siks.targetTransform.position, siks.tip.position, 0.5f);
//            Handles.Label(midPoint, string.Format("Distance to Target: {0:0.00}", distanceToTarget));
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        var s = target as SimpleIL;
//        if (s.pivot == null || s.upper == null || s.lower == null || s.effector == null || s.tip == null)
//            EditorGUILayout.HelpBox("Please assign Pivot, Upper, Lower, Effector and Tip transforms.", MessageType.Error);
//        base.OnInspectorGUI();
//    }

//    public void OnSceneGUI()
//    {
//        var siks = target as SimpleIL;

//        RotationHandle(siks.effector);
//        RotationHandle(siks.lower);
//        RotationHandle(siks.upper);

//        if (siks.targetTransform != null)
//        {
//            EditorGUI.BeginChangeCheck();
//            Vector3 newTargetPosition = Handles.PositionHandle(siks.targetTransform.position, Quaternion.identity);
//            if (EditorGUI.EndChangeCheck())
//            {
//                Undo.RecordObject(siks.targetTransform, "Move Target");
//                siks.targetTransform.position = newTargetPosition;
//            }
//        }

//        var normalRotation = Quaternion.LookRotation(Vector3.forward, siks.normal);
//        normalRotation = Handles.RotationHandle(normalRotation, siks.tip.position);
//        siks.normal = normalRotation * Vector3.up;
//    }

//    void RotationHandle(Transform transform)
//    {
//        if (transform != null)
//        {
//            EditorGUI.BeginChangeCheck();
//            var rotation = Handles.RotationHandle(transform.rotation, transform.position);
//            if (EditorGUI.EndChangeCheck())
//            {
//                Undo.RecordObject(transform, "Rotate");
//                transform.rotation = rotation;
//            }
//        }
//    }
//}
