using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public ScriptableObjectProfile CurrentWitnessOnStand;

    public string[] CurrentSceneDialogue;

    public int CurrentDialogue;

    public static GameplayControllerScript instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if (GetComponent<GetDocumentsScript>())
        {
            CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
