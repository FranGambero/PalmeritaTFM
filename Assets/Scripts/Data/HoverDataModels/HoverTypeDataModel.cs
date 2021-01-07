using System;
using ElJardin.Hover;
using ElJardin.Util;
using ElJardin.Util.Patterns;

namespace ElJardin.Data.HoverDataModels
{
    [Serializable]
    public class HoverTypeDataModel
    {
        #region SerializedFields
        [ClassImplements(typeof(IHover))] public ClassTypeReference type;
        
        readonly IConverter<HoverTypeDataModel, IHover> converter = new HoverDataModelConverter();
        #endregion
        
        #region Converter
        public IHover ToHover() => converter.Convert(this);
        #endregion
    }
}