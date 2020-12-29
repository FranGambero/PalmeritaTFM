using System;
using ElJardin.CardActions;
using ElJardin.Hover;
using ElJardin.Util.Patterns;

namespace ElJardin.Data.Cards
{
    public class CardDataModelConverter : IConverter<CardDataModel, ActionCard>
    {
        public ActionCard Convert(CardDataModel source)
        {
            var action = Activator.CreateInstance(source.actionType, source.size) as ICardAction;
            var hover = Activator.CreateInstance(source.hoverType, source.size) as IHover;

            return new ActionCard(hover, action);
        }
    }
}