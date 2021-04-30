using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    //Future anim usage//private Animator _animator;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        //Future anim usage// _animator = GetComponent<Animator>();
        Hide();
    }

    public virtual void Show()
    {
        Interact();
        _canvasGroup.alpha = 1;
        //Future anim usage//_animator.SetBool("Show", true);
    }

    public void Hide()
    {
        StopInteract();
        _canvasGroup.alpha = 0;

        //Future anim usage//_animator.SetBool("Show", false);

    }
    private void Interact()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void StopInteract()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
