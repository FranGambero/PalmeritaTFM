using System;
using ElJardin.CardActions;
using ElJardin.Hover;
using ElJardin.Util;

namespace ElJardin.Data.Cards
{
    [Serializable]
    public class CardDataModel
    {
        #region SerializeFields
        public int size;
        [ClassImplements(typeof(IHover))] public ClassTypeReference hoverType;
        [ClassImplements(typeof(ICardAction))] public ClassTypeReference actionType;
        
        readonly CardDataModelConverter converter = new CardDataModelConverter();
        #endregion
        
        #region Converters
        public ActionCard ToCard() => converter.Convert(this);
        #endregion
    }
}