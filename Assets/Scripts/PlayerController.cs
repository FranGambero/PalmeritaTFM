using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables

    public float movSpeed = 10;
    
    public float DownRayDistance = .5f;
    public Transform detectionOrigin;
    public LayerMask terrainLayer;
    public Vector3 DiagRayOffSet = new Vector3(0,-.5f,0);
    public float DiagRayDistance = .3f;
    public float overlappingDistance;

    public float floorSize = 10;

    public bool nFloorContact;
    protected bool sFloorContact;
    protected bool eFloorContact;
    protected bool wFloorContact;

    #endregion

    private void Update()
    {
        CheckFloor();
        Move();
        SolveFloors();
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        transform.Translate(((Vector3.forward * v) + (Vector3.right * h)) * movSpeed * Time.deltaTime);
    }

    private void CheckFloor()
    {
        RaycastHit hit;
        if(!Physics.Raycast(transform.position, transform.position + Vector3.down * DownRayDistance, out hit, terrainLayer))
        {
            Debug.LogError("No hay suelo");
        }
    }

    private void SolveFloors()
    {
        nFloorContact = SolveFloor(transform.forward);
        sFloorContact = SolveFloor(-transform.forward);
        eFloorContact = SolveFloor(transform.right);
        wFloorContact = SolveFloor(-transform.right);
    }

    private bool SolveFloor(Vector3 direction)
    {
        RaycastHit hit;
        Physics.Raycast(detectionOrigin.position, direction + DiagRayOffSet, out hit, terrainLayer);
        Debug.DrawLine(detectionOrigin.position, transform.position + direction + DiagRayOffSet, Color.red);
        //Physics.Raycast(transform.position, direction + DiagRayOffSet, out hit, terrainLayer);
        //Debug.DrawLine(transform.position, transform.position + direction + DiagRayOffSet, Color.red);
        if (hit.collider == null)
        {
            Debug.LogError("Detectado fin suelo");
            bool isOverlapping = hit.distance < overlappingDistance;
            if (isOverlapping)
            {
                float overlapDistance = overlappingDistance - hit.distance;
                Vector3 overlapDirection = -direction;
                //transform.position += overlapDirection * overlapDistance;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Posicion nueva instancia = "+direction*floorSize);
                }
            }
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * DownRayDistance);
    }
}
