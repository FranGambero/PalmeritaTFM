using UnityEngine;

namespace ElJardin.Data.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "ElJardin/Card")]
    public class CardDataModelWrapper : ScriptableObject
    {
        [SerializeField]
        CardDataModel dataModel = new CardDataModel();
        
        #region Accessors
        public CardDataModel DataModel => dataModel;
        #endregion
    }
}