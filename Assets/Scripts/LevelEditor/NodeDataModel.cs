using UnityEngine;

public class NodeDataModel : MonoBehaviour
{
    public enum ObstacleType
    {
        None, Rock
    }
    
    [SerializeField] public GameObject rockPrefab;
    
    [SerializeField] public ObstacleType obstacle = ObstacleType.None;
    [SerializeField] public bool isRiverStart;
    [SerializeField] public bool isRiverEnd;
    [SerializeField] [HideInInspector] public GameObject obstacleGameObject;

    #region Node Setters
    [HideInInspector] public int Row;
    [HideInInspector] public int Column;
    #endregion

    public void SetPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
