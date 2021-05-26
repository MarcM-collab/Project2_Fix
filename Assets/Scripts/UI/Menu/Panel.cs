using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Animator))]
public class Panel : MonoBehaviour
{
    private Animator anim;
    private CanvasGroup cg;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        cg = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        SetStats(true);
    }
    public void Hide()
    {
        SetStats(false);
    }

    private void SetStats(bool setter)
    {
        anim.SetBool("Show", setter);
        cg.interactable = setter;
        cg.blocksRaycasts = setter;
    }
}
