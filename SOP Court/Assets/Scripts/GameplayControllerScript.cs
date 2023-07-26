using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public GameObject DialogueBox;

    public ScriptableObjectProfile CurrentWitnessOnStand;

    public string[] CurrentSceneDialogue;

    public int CurrentDialogue;

    public static GameplayControllerScript instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
        }

        if(GetComponent<GetDocumentsScript>()){
            CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest();
        }

        StartCoroutine(OpenDialogueBox());

    }

    private IEnumerator OpenDialogueBox() {

        yield return new WaitForSeconds(1.5f);
        DialogueBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
