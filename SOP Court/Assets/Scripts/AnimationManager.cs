using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public Animator WitnessAnim, JudgeAnim, DefenceAnim, ProsectorAnim;

    // Start is called before the first frame update

    public static AnimationManager instance;




    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        WitnessAnim.runtimeAnimatorController = SuspectProfileScript.instance.CurrentSuspectData.myAnim;
        //WitnessAnim.runtimeAnimatorController = null;
    }

}
