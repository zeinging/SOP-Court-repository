using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public Animator WitnessAnim, JudgeAnim, DefenceAnim, ProsectorAnim;

    // Start is called before the first frame update
    void Start()
    {
        WitnessAnim.runtimeAnimatorController = SuspectProfileScript.instance.CurrentSuspectData.myAnim;
        //WitnessAnim.runtimeAnimatorController = null;
    }

}
