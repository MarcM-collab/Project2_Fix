using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var gO = animator.gameObject;
        var sliderGO = gO.GetComponentInChildren<Slider>().gameObject;

        var gORotationY = gO.transform.rotation.eulerAngles.y == 180 ? -180 : 180;

        gO.transform.Rotate(0, gORotationY, 0);

        sliderGO.transform.Rotate(0, -gORotationY, 0);

        animator.GetComponent<Character>().Turn = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
