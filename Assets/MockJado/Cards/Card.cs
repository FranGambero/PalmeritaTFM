﻿using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using ElJardin.Data.Cards;

namespace ElJardin
{
    public class Card : MonoBehaviour
    {
        #region SerializedFields
        public TextMeshProUGUI amountText;
        public int transformIndex;
        public bool usedCard;
        public CardData CardData { get; set; }

        ActionCard actionCard;
        #endregion
        
        #region Accessors
        public void Hover(Node targetNode) => actionCard.Hover(targetNode);
        public void UnHover() => actionCard.UnHover();
        public void Action(Node targetNode) => actionCard.Action(targetNode);
        #endregion

        private void Awake()
        {
            usedCard = false;
        }

        private void Start()
        {
            LoadCardData();
        }
        
        #region Load
        public void LoadCardData()
        {
            amountText.text = CardData.amount.ToString();
            GetComponent<ItemDragHandler>().LoadCardData(CardData);
        }
        
        public void LoadCardActions(CardDataModel cardDataModel)
        {
            var aux = cardDataModel.ToCard();
            actionCard = aux;
        }
        #endregion
        
        #region HandMovement
        public IEnumerator ChangeCardTransform(int newIndex, bool wait = true)
        {
            float animTime = .5f;
            transformIndex = newIndex;
            float waitTime = wait ? transformIndex * .5f : 0f;
            yield return new WaitForSeconds(waitTime);
            JumpCard(animTime);
            yield return new WaitForSeconds(animTime);

            transform.SetParent(CardManager.Instance.transformList[transformIndex]);
        }

        void JumpCard(float animTime)
        {
            if(gameObject.activeSelf)
            {
                AkSoundEngine.PostEvent("Carta_Slide_In", gameObject);
            }

            transform.DOJump(CardManager.Instance.transformList[transformIndex].position, 3, 1, animTime);

            GetComponent<ItemDragHandler>().originalHandPosition = CardManager.Instance.transformList[transformIndex].position;
        }
        #endregion
    }
}