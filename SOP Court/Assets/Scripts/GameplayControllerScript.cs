using System.Collections;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public GameObject DialogueBox, interuptionImage;

    public Sprite[] interuptionSprites;

    public ScriptableObjectProfile JudgeProfile, DefenseProfile, ProsecutorProfile;//CurrentWitnessOnStand

    public string[] CurrentSceneDialogue;

    public int currentSceneIndex = 0, maxFiles = 0;

    public static GameplayControllerScript instance;

    public bool IsInCrossExaminationScene = false;

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

            //CurrentSceneDialogue = GetComponent<GetDocumentsScript>().DialogueFromWebFile;

            //GetComponent<GetDocumentsScript>().StartGetTextFromWebFile();

            CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest();
            // Hard coding for now
            maxFiles = 1;

        }
        if (!IsInCrossExaminationScene)
        {
            _ = StartCoroutine(DelayDialogueBox(0.5f, 0.5f));
        }

    }

    public void HoldIt(float holdItTimer)
    {
        AudioManagerScript.instance.PlaySound(1);
        _ = StartCoroutine(InteruptionTimer(holdItTimer, 0));
    }

    public void Objection(float objectionTimer)
    {
        AudioManagerScript.instance.PlaySound(0);
        _ = StartCoroutine(InteruptionTimer(objectionTimer, 1));
    }

    public void TakeThat(float takeThatTimer)
    {
        AudioManagerScript.instance.PlaySound(2);
        _ = StartCoroutine(InteruptionTimer(takeThatTimer, 2));
    }

    private IEnumerator InteruptionTimer(float t, int spriteIndex)
    {
        interuptionImage.GetComponent<UnityEngine.UI.Image>().sprite = interuptionSprites[spriteIndex];
        interuptionImage.SetActive(true);
        yield return new WaitForSeconds(t);
        interuptionImage.SetActive(false);
    }



    public void MoveToNextSceneDialogue()
    {

        if (currentSceneIndex < maxFiles - 1)
        {

            if (GetComponent<GetDocumentsScript>())
            {
                _ = StartCoroutine(DelayFade());
                currentSceneIndex++;
                CurrentSceneDialogue = GetComponent<GetDocumentsScript>().GetTextFromFileTest();
                //OpenDialogueBox(3f);
                _ = StartCoroutine(DelayDialogueBox(1f, 0));
            }
            else
            {
                Debug.Log("case File Missing");
            }

        }
        _ = StartCoroutine(DelayFade());//delete later

    }

    public IEnumerator DelayFade()
    {
        SceneFaderScript.i.StartFade();
        yield return new WaitForSeconds(1f);

        //if(pos != null)
        //CameraMover.instance.SnapCamHere(pos);
    }

    public void OpenDialogueBox(float TextDelay)
    {
        DialogueBox.SetActive(true);
        DialogueBox.GetComponent<DialoguePanelScript>().OpenDialogueBox(TextDelay);
    }

    public void OpenCrossExaminationBox(float openDelay)
    {

    }

    private IEnumerator DelayDialogueBox(float openDelay, float textD)
    {

        yield return new WaitForSeconds(openDelay);
        //DialogueBox.GetComponent<DialoguePanelScript>().OpenDialogueBox(t);
        OpenDialogueBox(textD);
    }

}