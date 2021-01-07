using UnityEngine;

namespace ElJardin.Data.HoverDataModels
{
    [CreateAssetMenu(fileName = "New Hover Type", menuName = "ElJardin/Hover/Types")]
    public class HoverTypeDataModelWrapper : ScriptableObject
    {
        [SerializeField]
        HoverTypeDataModel dataModel = new HoverTypeDataModel();
        
        #region Accessors
        public HoverTypeDataModel DataModel => dataModel;
        #endregion
    }
}