using System.Collections.Generic;
using System.Linq;
using ElJardin;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BurnController : MonoBehaviour, ITurn
    {
        public bool Turneable;
        
        public int turnIndex { get; set; }
        public bool turneable { get => Turneable; set => Turneable = value; }
        
        public ParticleSystem burning_ps;
        
        List<BurningNode> burningNodes;

        void Start()
        {
            Init();
        }

        void Init()
        {
            burningNodes = new List<BurningNode>();
            
            turneable = true;
            turnIndex = Semaphore.Instance.GetNewIndex();
            Semaphore.Instance.AddTurn(this);
        }
        
        #region Turn
        public void onTurnStart(int currentIndex)
        {
            if(turnIndex == currentIndex)
            {
                SpreadFire();
                
                onTurnFinished();
            }
        }

        public void onTurnFinished()
        {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
        #endregion
        
        #region Burn
        public void BurnNode(Node node)
        {
            //Cambiar para que queme los adyacentes
            AddBurningNode(node);
        }

        void DestroyBush(BurningNode node)
        {
            Destroy(node.particle.gameObject);
            VFXDirector.Instance.Play("AntOut",node.node.GetSurfacePosition());
            node.node.DestroyObstacle();
        }

        void DestroyAllBurnt()
        {
            for(var i = burningNodes.Count - 1; i >= 0; i--)
            {
                var node = burningNodes[i];
                DestroyBush(node);
                burningNodes.RemoveAt(i);
            }
        }

        void SpreadFire()
        {
            var nodesToBurn = new List<Node>();
            for(var i = 0; i < burningNodes.Count; i++)
            {
                nodesToBurn.AddRange(GetAdyacentNodesToBurn(burningNodes[i].node));
            }
            
            DestroyAllBurnt();

            foreach(var node in nodesToBurn)
            {
                AddBurningNode(node);
            }
        }
        
        void AddBurningNode(Node node)
        {
            if(!burningNodes.Any(n => n.node == node))
                burningNodes.Add(new BurningNode(node, burning_ps));
        }

        List<Node> GetAdyacentNodesToBurn(Node target)
        {
            var neighbors = new List<Node>();
            for(var row = target.row - 1; row <= target.row + 1; row++)
            {
                for(var column = target.column - 1; column <= target.column + 1; column++)
                {
                    var possibleNode = MapManager.Instance.GetNode(row, column);
                    if(possibleNode != null && possibleNode != target)
                    {
                        if(possibleNode.HasObstacle && CanBurn(possibleNode))
                        {
                            neighbors.Add(possibleNode);
                        }
                    }
                }
            }

            return neighbors;
        }
        #endregion
        
        #region Support Methods
        bool CanBurn(Node node)
        {
            return node.obstacle.gameObject.CompareTag("obstacle") && node.obstacle.gameObject.transform.GetChild(0).gameObject.CompareTag("Burnable");
        }
        #endregion
        
        #region BurningNode
        public class BurningNode
        {
            public ParticleSystem particle;
            public Node node;
            int turnsLeft;

            public BurningNode(Node node, ParticleSystem particle)
            {
                this.node = node;
                turnsLeft = 1;
                
                InitParticle(particle);
            }

            void InitParticle(ParticleSystem burningPs)
            {
                particle = Instantiate(burningPs, node.gameObject.transform);
                particle.Play();
            }

            public bool CanDestroy()
            {
                if(turnsLeft > 0)
                {
                    turnsLeft--;
                    return false;
                }
                
                return true;
            }
        }
        #endregion
    }
}