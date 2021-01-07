using System;
using ElJardin.Hover;
using ElJardin.Util.Patterns;

namespace ElJardin.Data.HoverDataModels
{
    public class HoverDataModelConverter : IConverter<HoverTypeDataModel, IHover>
    {
        public IHover Convert(HoverTypeDataModel source)
        {
            return Activator.CreateInstance(source.type) as IHover;
        }
    }
}