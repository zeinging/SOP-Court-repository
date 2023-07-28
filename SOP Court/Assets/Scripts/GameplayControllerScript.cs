using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public GameObject DialogueBox;

    public ScriptableObjectProfile JudgeProfile, DefenseProfile, ProsecutorProfile;//CurrentWitnessOnStand

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

        // if(GetComponent<GetDocumentsScript>()){
        //     CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest(currentSceneIndex);
        //     maxFiles = GetComponent<GetDocumentsScript>().FilesPathCase1Folder.Count;
        // }

        CurrentSceneDialogue = GetComponent<DialogueHolderTestScript>().DialogueHolder;//delete later

        //OpenDialogueBox(1.5f);
        StartCoroutine(DelayDialogueBox(0.5f, 0.5f));

    }

    public void MoveToNextSceneDialogue(){

            if(currentSceneIndex < maxFiles - 1){

                if(GetComponent<GetDocumentsScript>()){
                    StartCoroutine(DelayFade());
                    currentSceneIndex++;
                    CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest(currentSceneIndex);
                    //OpenDialogueBox(3f);
                    StartCoroutine(DelayDialogueBox(1f, 0));
                }else{
                    Debug.Log("case File Missing");
                }

            }
            StartCoroutine(DelayFade());//delete later
        
    }

    public IEnumerator DelayFade(){
        SceneFaderScript.i.StartFade();
        yield return new WaitForSeconds(1f);

        //if(pos != null)
        //CameraMover.instance.SnapCamHere(pos);
    }

    public void OpenDialogueBox(float TextDelay){
        DialogueBox.SetActive(true);
        DialogueBox.GetComponent<DialoguePanelScript>().OpenDialogueBox(TextDelay);
    }

    public void OpenCrossExaminationBox(float openDelay){
        
    }

    private IEnumerator DelayDialogueBox(float openDelay, float textD) {

        yield return new WaitForSeconds(openDelay);
        //DialogueBox.GetComponent<DialoguePanelScript>().OpenDialogueBox(t);
        OpenDialogueBox(textD);
    }
    
}
