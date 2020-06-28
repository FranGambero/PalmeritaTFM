using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ElJardin
{
    public class Node : MonoBehaviour
    {
        #region Variables

        [Header("Hover")]
        public Color hoverColor;

        [Header("Position")]
        public int row, column;

        private Material currentMaterial;
        private NodeType nodeType;

        private MeshRenderer _mr;
        private Color baseColor;

        private bool hovering;

        #endregion

        private void Awake()
        {
            nodeType = NodeType.Undefined;

            _mr = GetComponent<MeshRenderer>();

            hovering = false;

            baseColor = _mr.material.color;
        }

        public void ChangeNodeType(NodeType newType, Material newMaterial)
        {
            nodeType = newType;
            _mr.material = newMaterial;
        }

        #region Setters

        public void SetPosition(Vector2 pos)
        {
            row = (int)pos.x;
            column = (int)pos.y;
        }

        #endregion

        #region Getters

        public Vector2 GetPosition()
        {
            return new Vector2(row, column);
        }

        #endregion

        #region Hover

        private void OnMouseEnter()
        {
            _mr.material.color = hoverColor;
            hovering = true;
        }

        private void OnMouseExit()
        {
            _mr.material.color = baseColor;
            hovering = false;
        }

        #endregion

        #region Builder

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (hovering)
            {
                //BuildManager.Instance.BuildGroove(this);
                BuildManager.Instance.BuildByCard(this);
            }
        }

        #endregion

        #region 

        public bool IsGround()
        {
            return nodeType == NodeType.Ground ? true : false;
        }

        #endregion
    }
}
