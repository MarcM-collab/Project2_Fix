using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDrawableCards : StateMachineBehaviour
{
    public List<Card> Cards; //Baraja elegida por el player (8 cartas)

    List<Card> twoCardsRandom = new List<Card>(); //lista para las dos cartas aleatorias.

    public Image[] buttons;

    public HandManager Hand;
    [SerializeField]
    private int maxCardInHand = 6;
    private bool currentTurn = false;

    public bool PressedFirst;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var IsBelowHandMaxSize = Hand.hand.Count < maxCardInHand;
        if (IsBelowHandMaxSize)
            ShowRandomCards(ChooseRandom());
    }

    private List<Card> ChooseRandom() //salen dos cartas random y las guarda en una lista.
    {
        int random1 = UnityEngine.Random.Range(0, Cards.Count);
        int random2 = UnityEngine.Random.Range(0, Cards.Count);

        while (random1 == random2)
        {
            random2 = UnityEngine.Random.Range(0, Cards.Count);
        }
        twoCardsRandom.Add(Cards[random1]);
        twoCardsRandom.Add(Cards[random2]);

        return twoCardsRandom;
    }
    private void ShowRandomCards(List<Card> _twoCardsRandom) //muestra las dos cartas random
    {
        for (int i = 0; i < _twoCardsRandom.Count; i++)
        {
            Sprite sprite;
            if (TurnManager.TeamTurn == Team.TeamAI)
                sprite = _twoCardsRandom[i].Back;
            else
                sprite = _twoCardsRandom[i].GetComponent<Image>().sprite;

            buttons[i].sprite = sprite;
            buttons[i].gameObject.SetActive(true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
