using UnityEngine;

public class NodeDataModel : MonoBehaviour
{
    public enum ObstacleType
    {
        None, Rock
    }
    
    [SerializeField] public GameObject rockPrefab;
    [SerializeField] public ObstacleType obstacle = ObstacleType.None;

    #region Node Setters
    [HideInInspector] public int Row;
    [HideInInspector] public int Column;
    [HideInInspector] public bool isRiverStart;
    [HideInInspector] public bool isRiverEnd;
    #endregion
}
