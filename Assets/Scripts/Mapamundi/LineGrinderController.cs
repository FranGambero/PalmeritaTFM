using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LineGrinderController : MonoBehaviour {
    public Transform point1, point2, point3;
    public LineRenderer lineRenderer;
    public List<Vector3> pointList;
    public int vertexCount = 12;
    public GameObject point;
    List<GameObject> points;
    private void Start() {
        points = new List<GameObject>();
        GeneratePoints();
        for (int i = 0; i < pointList.Count; i++) {
            points.Add(Instantiate(point, pointList[i], Quaternion.identity));
        }
    }
    private void Update() {
        GeneratePoints();
        points.ForEach(p => p.transform.position = pointList[points.IndexOf(p)]);
    }
    public List<Vector3> GeneratePoints() {
        if (vertexCount > 0) {
            pointList = new List<Vector3>();
            for (float ratio = 0; ratio <= 1; ratio += 1f / vertexCount) {
                Vector3 tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
                Vector3 tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
                Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                pointList.Add(bezierPoint);
            }
            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }
        return pointList;
    }
}
