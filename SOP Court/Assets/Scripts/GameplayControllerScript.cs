using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{

    public GameObject DialogueBox;

    public ScriptableObjectProfile JudgeProfile, DefenseProfile, ProsecutorProfile;//CurrentWitnessOnStand

    public XElement CurrentXMLData;

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
            CurrentXMLData = GetComponent<GetDocumentsScript>().GetXmlFromFile(currentSceneIndex);
        }

        //OpenDialogueBox(1.5f);
        StartCoroutine(DelayDialogueBox(0.5f, 0.5f));

    }

    public void MoveToNextSceneDialogue()
    {

        if (currentSceneIndex < maxFiles - 1)
        {

            if (GetComponent<GetDocumentsScript>())
            {
                StartCoroutine(DelayFade());
                currentSceneIndex++;
                CurrentXMLData = GetComponent<GetDocumentsScript>().GetXmlFromFile(currentSceneIndex);
                //OpenDialogueBox(3f);
                StartCoroutine(DelayDialogueBox(1f, 0));
            }
            else
            {
                Debug.Log("case File Missing");
            }

        }

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
