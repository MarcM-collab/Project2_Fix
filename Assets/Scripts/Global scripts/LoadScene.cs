using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public int buildIndex;

    public void OnClick()
    {
        CustomSceneManager.LoadScene(buildIndex);
    }
}
