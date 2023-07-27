using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public GameObject DialogueBox;

    public ScriptableObjectProfile CurrentWitnessOnStand;

    public string[] CurrentSceneDialogue;

    public int currentSceneIndex = 0, maxFiles = 0;

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
            CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest(currentSceneIndex);
            maxFiles = GetComponent<GetDocumentsScript>().FilesPathCase1Folder.Count;
        }

        OpenDialogueBox(1.5f);

    }

    public void MoveToNextSceneDialogue(Vector3 charPos){

            if(currentSceneIndex < maxFiles - 1){

                if(GetComponent<GetDocumentsScript>()){
                    StartCoroutine(DelayFade(charPos));
                    currentSceneIndex++;
                    CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest(currentSceneIndex);
                    OpenDialogueBox(3f);
                }else{
                    Debug.Log("case File Missing");
                }

            }
        
    }

    public IEnumerator DelayFade(Vector3 pos){
        SceneFaderScript.i.StartFade();
        yield return new WaitForSeconds(1f);
        if(pos != null)
        CameraMover.instance.SnapCamHere(pos);
    }

    public void OpenDialogueBox(float openDelay){
        StartCoroutine(DelayDialogueBox(openDelay));
    }

    private IEnumerator DelayDialogueBox(float t) {

        yield return new WaitForSeconds(t);
        DialogueBox.SetActive(true);
    }
    
}
