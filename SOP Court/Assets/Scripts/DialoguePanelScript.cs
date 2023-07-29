
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanelScript : MonoBehaviour
{

    public GameObject ContinueButton, pressButton, presentButton, leftArrowButton, rightArrowButton;

    public Text SpeakerName, Dialogue;

    public string StoredDialogue;

    public float textSpeed = 0.2f;

    public int CurrentDialogue = 0;

    public bool isAnimating = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        //Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[0];
        // ContinueButton.gameObject.SetActive(true);
        // StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        // StartCoroutine(AnimateText());
    }

    void OnDisable()
    {
        // CurrentDialogue = 0;
        // StoredDialogue = "";
        // Dialogue.text = "";
        // SpeakerName.text = "";
        //if(CameraMover.instance.Witness.position != null)
        //GameplayControllerScript.instance.MoveToNextSceneDialogue(CameraMover.instance.Witness.position);
    }

    public void OpenDialogueBox(float openDelay)
    {
        StartCoroutine(DelayDialogueBox(openDelay));
    }

    public void OpenCrossExaminationPanel()
    {
        pressButton.SetActive(true);
        presentButton.SetActive(true);
        leftArrowButton.SetActive(true);
        leftArrowButton.GetComponent<Button>().enabled = false;
        rightArrowButton.SetActive(true);
        ContinueButton.SetActive(false);
    }

    private IEnumerator DelayDialogueBox(float t)
    {

        yield return new WaitForSeconds(t);
        //this.gameObject.SetActive(true);

        ContinueButton.gameObject.SetActive(true);
        //SpeakerName.transform.parent.gameObject.SetActive(true);
        StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        StartCoroutine(AnimateText());
    }

    public void CloseDialogueBox()
    {
        CurrentDialogue = 0;
        StoredDialogue = "";
        Dialogue.text = "";
        SpeakerName.text = "";
        gameObject.SetActive(false);
        ContinueButton.SetActive(false);
        SpeakerName.transform.parent.gameObject.SetActive(false);

    }

    public void Continue()
    {
        Debug.Log("Ran in Continue");

        if (!isAnimating && CurrentDialogue == GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1)
        {//where dialogue box closes

            CloseDialogueBox();

            //GameplayControllerScript.instance.MoveToNextSceneDialogue(CameraMover.instance.Witness.position);
            GameplayControllerScript.instance.MoveToNextSceneDialogue();

        }
        else
        {

            if (!isAnimating)
            {
                //Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
                if (CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1)
                {
                    CurrentDialogue++;
                }
                StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
                Dialogue.text = "";
                StartCoroutine(AnimateText());
            }
            else
            {
                StopAllCoroutines();
                Dialogue.text = StoredDialogue;
                isAnimating = false;
            }
        }
    }

    private void CheckForTags()
    {

        if (StoredDialogue.Contains(DocumentTags.Date.ToLower()) || StoredDialogue.Contains(DocumentTags.Date))
        {
            SpeakerName.transform.parent.gameObject.SetActive(false);
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Date.Length);
            //StoredDialogue = System.DateTime.Now.ToString();
            //Debug.Log("change color");
            Dialogue.color = Color.green;
            Dialogue.alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            Dialogue.color = Color.white;
            Dialogue.alignment = TextAnchor.UpperLeft;
        }

        if (StoredDialogue.Contains(DocumentTags.Defence.ToLower()) || StoredDialogue.Contains(DocumentTags.Defence))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Defence.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.DefenseProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Defence.Length);
        }

        if (StoredDialogue.Contains(DocumentTags.Prosecutor.ToLower()) || StoredDialogue.Contains(DocumentTags.Prosecutor))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Prosector.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.ProsecutorProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Prosecutor.Length);
        }

        if (StoredDialogue.Contains(DocumentTags.Judge.ToLower()) || StoredDialogue.Contains(DocumentTags.Judge))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Judge.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.JudgeProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Judge.Length);
        }

        if (StoredDialogue.Contains(DocumentTags.Witness.ToLower()) || StoredDialogue.Contains(DocumentTags.Witness))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = SuspectProfileScript.instance.CurrentSuspectData.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Witness.Length);
        }

        if (StoredDialogue.Contains(DocumentTags.WitnessTestimony.ToLower()) || StoredDialogue.Contains(DocumentTags.WitnessTestimony))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            Dialogue.color = Color.green;
            Dialogue.alignment = TextAnchor.MiddleCenter;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.WitnessTestimony.Length);
        }

        if (StoredDialogue.Contains(DocumentTags.CrossExamination.ToLower()) || StoredDialogue.Contains(DocumentTags.CrossExamination))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            Dialogue.color = Color.red;
            Dialogue.alignment = TextAnchor.MiddleCenter;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.CrossExamination.Length);
            OpenCrossExaminationPanel();
        }

    }

    public IEnumerator AnimateText()
    {

        CheckForTags();

        //for (int t = 0; t < StoredDialogue.Length; t++) {
        //    Dialogue.text += " ";
        //    Debug.Log("t = " + t);
        //    yield return null;
        //}
        //yield return new WaitForSeconds(textSpeed);//delay a bit before animating
        isAnimating = true;
        int i = 0;
        while (i < StoredDialogue.Length && isAnimating)
        {
            yield return new WaitForSeconds(textSpeed);
            //Dialogue.text += StoredDialogue.ToCharArray()[i];

            if (i < StoredDialogue.Length)
            {
                //Dialogue.text = Dialogue.text.Remove(i, 1);
                //Dialogue.text = Dialogue.text.Insert(i, StoredDialogue.ToCharArray()[i].ToString());
                //Debug.Log(Dialogue.text.ToCharArray()[i]);
                Dialogue.text += StoredDialogue.ToCharArray()[i];
            }

            i++;
            yield return null;
        }
        isAnimating = false;

    }

}
