﻿using System.Collections.Generic;
using ElJardin.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ElJardin
{
    public class Node : MonoBehaviour, IPathable
    {
        #region Variables
        [Header("Hover")] public Color hoverColor;

        [Header("Position")] public int row, column;

        //[HideInInspector]
        public List<Node> neighbors;

        Material currentMaterial;
        NodeType nodeType;

        MeshRenderer _mr;
        MeshFilter _mf;
        Color baseColor;

        bool hovering;

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public Node CameFromNode { get; set; }
        #endregion

        private void Awake()
        {
            nodeType = NodeType.Undefined;

            _mr = GetComponentInChildren<MeshRenderer>();
            _mf = GetComponentInChildren<MeshFilter>();

            hovering = false;

            baseColor = _mr.material.color;
        }

        public void ChangeNodeType(NodeType newType, Material newMaterial)
        {
            nodeType = newType;
            _mr.material = newMaterial;
        }

        public void ChangeNodeType(NodeType newType, Mesh newMesh)
        {
            nodeType = newType;
            _mf.mesh = newMesh;
            CalculateNeighbors();
        }

        #region Setters
        public void SetPosition(Vector2 pos)
        {
            row = (int) pos.x;
            column = (int) pos.y;
        }

        public void SetColor(Color cl)
        {
            _mr.material.color = cl;
        }
        #endregion

        #region Getters
        public Vector2 GetPosition()
        {
            return new Vector2(row, column);
        }

        public NodeType GetNodeType()
        {
            return nodeType;
        }

        public Mesh GetMesh()
        {
            return _mf.mesh;
        }
        #endregion

        #region Hover
        public void HoverOn()
        {
            baseColor = _mr.material.color;
            _mr.material.color = hoverColor;
            hovering = true;
        }

        public void HoverOff()
        {
            _mr.material.color = baseColor;
            hovering = false;
        }

        private void OnMouseEnter()
        {
            BuildManager.Instance.GetSurroundingsByCard(this);
            BuildManager.Instance.HoverNodesInList();
            //BuildManager.Instance.CalculateMeshToBuild(this);
        }

        private void OnMouseExit()
        {
            BuildManager.Instance.UnHoverNodesInList();
        }
        #endregion

        #region Builder
        private void OnMouseUp()
        {
            Debug.LogError("Me han llamao");
            if(EventSystem.current.IsPointerOverGameObject())
                return;

            //if (hovering)
            //{
            //    //BuildManager.Instance.BuildGroove(this);
            //    BuildManager.Instance.ChangeNodesInList();
            //    MapManager.Instance.CheckFullRiver();
            //}
        }
        #endregion

        #region
        public bool IsGround()
        {
            return nodeType == NodeType.Ground ? true : false;
        }

        public void CalculateNeighbors()
        {
            this.neighbors = new List<Node>();

            List<Vector2> positionList = new List<Vector2>();
            List<Node> neighbors = new List<Node>();
            //Sacamos lista de posibles vecinos en cruz (i+1,j // i-1,j // i,j+1 // i,j-1)

            positionList.Add(new Vector2(row + 1, column));
            positionList.Add(new Vector2(row - 1, column));
            positionList.Add(new Vector2(row, column + 1));
            positionList.Add(new Vector2(row, column - 1));


            //Comprobamos que los vecinos sean validos
            foreach(Vector2 pos in positionList)
            {
                if(BuildManager.Instance.CheckValidNode((int) pos.x, (int) pos.y))
                {
                    neighbors.Add(MapManager.Instance.GetNode((int) pos.x, (int) pos.y));
                    this.neighbors.Add(MapManager.Instance.GetNode((int) pos.x, (int) pos.y));
                    foreach(Node neighbor in neighbors)
                    {
                        if(!neighbor.neighbors.Contains(this))
                            neighbor.neighbors.Add(this);
                    }
                }
            }
        }
        #endregion
        
        #region Pathfinding

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }

        #endregion
    }
}