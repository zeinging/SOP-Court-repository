using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectProfile", menuName = "ScriptableObjects/ScriptableObjectProfile")]
public class ScriptableObjectProfile : ScriptableObject
{
    public string WitnessName;
    public Sprite ProfileImage;

    public RuntimeAnimatorController myAnim;

    public string WitnessProfesion;

    public Sprite[] WitnessEmoteFrames;


    public string Confession, DodgeTatic;
    public string[] funWittySaying;
}
