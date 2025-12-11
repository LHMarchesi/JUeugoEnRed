using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTutorialScene : MonoBehaviour
{
    public void loadtutorial()
    {
        TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 1);
    }
}
