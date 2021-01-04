using ElJardin;
using System.Collections;
using System.Collections.Generic;
using ElJardin.Data.Cards;
using UnityEngine;

namespace ElJardin {

    [System.Serializable]
    public class CardData {

        public enum CardType { Undefined, Construction, Summon, Buff}

        public int amount;
        public DirectionType direction;
        public CardType cardType;
        public Sprite sprite;
    }
}