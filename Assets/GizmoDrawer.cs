using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GizmoType {WireCube, Cube, Sphere, WireSphere}
public enum GizmoColor {Black, Grey, White, Red, Green, Blue, Cyan, Magenta, Yellow}
public class GizmoDrawer : MonoBehaviour
{
    [SerializeField] private GizmoType type;
    [SerializeField] private float size;
    [SerializeField] private GizmoColor color;
    public void OnDrawGizmos(){
        Color gizmoColor = Color.white;
        switch (color)
        {
            case GizmoColor.Red:
                gizmoColor = Color.red;
                break;
            case GizmoColor.Green:
                gizmoColor = Color.green;
                break;
            case GizmoColor.Yellow:
                gizmoColor = Color.yellow;
                break;
            case GizmoColor.Blue:
                gizmoColor = Color.blue;
                break;
            case GizmoColor.Magenta:
                gizmoColor = Color.magenta;
                break;
            case GizmoColor.White:
                gizmoColor = Color.white;
                break;
            case GizmoColor.Black:
                gizmoColor = Color.black;
                break;
            case GizmoColor.Grey:
                gizmoColor = Color.grey;
                break;
            case GizmoColor.Cyan:
                gizmoColor = Color.cyan;
                break;
        }

        Gizmos.color = gizmoColor;
        switch (type)
        {
            case GizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position,Vector3.one*size);
                break;
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position,Vector3.one*size);
                break;
            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position,size);
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position,size);
                break;
        }
    }
}
