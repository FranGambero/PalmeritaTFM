using UnityEngine;

namespace ElJardin.Data.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "ElJardin/Card")]
    public class CardDataModelWrapper : ScriptableObject
    {
        [SerializeField]
        CardDataModel dataModel = new CardDataModel();
        [SerializeField]
        CardData cardData = new CardData();
        
        #region Accessors
        public CardDataModel DataModel => dataModel;
        public CardData CardData => cardData;
        #endregion
    }
}