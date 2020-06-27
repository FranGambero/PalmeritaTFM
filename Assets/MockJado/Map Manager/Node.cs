using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin
{
    public class Node : MonoBehaviour
    {
        #region Variables

        private Material currentMaterial;
        private NodeType nodeType;

        private MeshRenderer _mr;

        #endregion

        private void Awake()
        {
            nodeType = NodeType.Undefined;

            _mr = GetComponent<MeshRenderer>();
        }

        public void ChangeNodeType(NodeType newType, Material newMaterial)
        {
            nodeType = newType;
            _mr.material = newMaterial;
        }
    }
}
