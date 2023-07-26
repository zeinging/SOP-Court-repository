using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectProfile", menuName = "ScriptableObjects/ScriptableObjectProfile")]
public class ScriptableObjectProfile : ScriptableObject
{
    public string WitnessName;
    public Sprite ProfileImage;

    public string WitnessProfesion;

    public Sprite[] WitnessEmoteFrames;


    public string Confession, DodgeTatic;
    public string[] funWittySaying;
}
