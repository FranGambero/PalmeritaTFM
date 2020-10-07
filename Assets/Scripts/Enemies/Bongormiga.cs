using System;
using System.Collections;
using System.Collections.Generic;
using ElJardin;
using ElJardin.Movement;
using UnityEngine;

public class Bongormiga : MonoBehaviour, ITurn {
    public int myTurnIndex;
    public Vector2 startingNode;
    public GameObject StunObj;
    
    public Node CurrentNode { get; private set; }

    [Header("Injection")]
    public MovementController Movement;
    public Animator myAnimator;

    int actionTurnIndex;

    public int turnIndex { get => myTurnIndex; set => myTurnIndex = value; }

    private void Awake() {
        myAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        var node = MapManager.Instance.GetNode((int)startingNode.x, (int)startingNode.y);
        CurrentNode = node;
        transform.position = CurrentNode.transform.position + new Vector3(0,0.7f,0);

        actionTurnIndex = 0;
    }

    #region Actions
    IEnumerator Move(Node target)
    {
        myAnimator.SetFloat("Speed", 5);
        yield return new WaitForSeconds(1.25f);
        //TODO: se debería asignar un nodo, no ir directo a por sépalo
        target = GameManager.Instance.Sepalo.CurrentNode;
        Node lastNodeWalked = target;
        yield return StartCoroutine(Movement.MovePartial(CurrentNode, target, 2, value => lastNodeWalked = value));
        CurrentNode = lastNodeWalked;
        myAnimator.SetFloat("Speed", 0);

    }

    IEnumerator PlayBongos()
    {
        Debug.Log("TOCO EL COSO");
        myAnimator.Play("PlayBongo0");
        //AkSoundEngine.PostEvent("Hormiga_Ataque_In", gameObject);

        DestroySurroundingGround(CurrentNode);
        yield return null;
    }

    IEnumerator Stun()
    {
        StunObj.SetActive(true);
        //TODO: lo que sea que haga el stun, animaciones, sonido...
        yield return null;

    }

    void DestroySurroundingGround(Node node)
    {
        var listDestroyNodesCross = new List<Node>();
        
        //North
        var northNode = MapManager.Instance?.GetNode(node.row, node.column + 1);
        if(northNode != null && !northNode.IsGround())
            listDestroyNodesCross.Add(northNode);
        
        //South
        var southNode = MapManager.Instance?.GetNode(node.row, node.column - 1);
        if(southNode != null && !southNode.IsGround())
            listDestroyNodesCross.Add(southNode);
        
        //East
        var eastNode = MapManager.Instance?.GetNode(node.row + 1, node.column);
        if(eastNode != null && !eastNode.IsGround())
            listDestroyNodesCross.Add(eastNode);
        
        //West
        var westNode = MapManager.Instance?.GetNode(node.row - 1,node.column);
        if(westNode != null && !westNode.IsGround())
            listDestroyNodesCross.Add(westNode);

        foreach(var nodeToDestroy in listDestroyNodesCross)
        {
            BuildManager.Instance?.BuildGround(nodeToDestroy);
        }
    }

    IEnumerator DoTurnAction()
    {
        //Este método es un mojon. Para modificarlo:
        //Una nueva accion en cada "ciclo" es un nuevo case con un nuevo int
        //solo el ultimo case resetea el actionTurnIndex a 0, acordarse de ponerlo bien
        //el resto hacen ++ para pasar el index de accion a la siguiente
        switch(actionTurnIndex)
        {
            case 0:
                yield return StartCoroutine(Move(null));
                actionTurnIndex++;
                break;
            case 1:
                yield return StartCoroutine(PlayBongos());
                actionTurnIndex++;
                break;
            case 2:
                yield return StartCoroutine(Stun());
                actionTurnIndex = 0;
                break;
        }
        Invoke(nameof(onTurnFinished), 3f);
    }
    #endregion

    public void onTurnStart(int currentIndex) {
        if (turnIndex == currentIndex)
        {
            StartCoroutine(DoTurnAction());
            //Invoke(nameof(onTurnFinished), 3f);
        }
    }

    public void onTurnFinished() {
        StunObj.SetActive(false);
        Semaphore.Instance.onTurnEnd(turnIndex);
    }




}
