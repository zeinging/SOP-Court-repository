using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanelScript : MonoBehaviour
{

    public GameObject ContinueButton, pressButton, presentButton, leftArrowButton, rightArrowButton;

    public Text SpeakerName, Dialogue;

    public Animator currentSubjectanim;

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

    void OnDisable(){
        // CurrentDialogue = 0;
        // StoredDialogue = "";
        // Dialogue.text = "";
        // SpeakerName.text = "";
        //if(CameraMover.instance.Witness.position != null)
        //GameplayControllerScript.instance.MoveToNextSceneDialogue(CameraMover.instance.Witness.position);
    }

    public void OpenDialogueBox(float openDelay){
        StartCoroutine(DelayDialogueBox(openDelay));
    }

    public void OpenCrossExaminationPanel(){
        pressButton.SetActive(true);
        presentButton.SetActive(true);
        leftArrowButton.SetActive(true);
        leftArrowButton.GetComponent<Button>().enabled = false;
        rightArrowButton.SetActive(true);
        ContinueButton.SetActive(false);
    }

    private IEnumerator DelayDialogueBox(float t) {

        yield return new WaitForSeconds(t);
        //this.gameObject.SetActive(true);

        ContinueButton.gameObject.SetActive(true);
        //SpeakerName.transform.parent.gameObject.SetActive(true);
        StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        StartCoroutine(AnimateText());
    }

    public void CloseDialogueBox(){
        CurrentDialogue = 0;
        StoredDialogue = "";
        Dialogue.text = "";
        SpeakerName.text = "";
        this.gameObject.SetActive(false);
        ContinueButton.SetActive(false);
        SpeakerName.transform.parent.gameObject.SetActive(false);

    }

    public void Continue() {

            if(!isAnimating && CurrentDialogue == GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1){//where dialogue box closes

                CloseDialogueBox();
                
                //GameplayControllerScript.instance.MoveToNextSceneDialogue(CameraMover.instance.Witness.position);
                GameplayControllerScript.instance.MoveToNextSceneDialogue();
                
            }else{

                if (!isAnimating)
                {
                    //Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
                    if(CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1){
                        CurrentDialogue++;
                    }
                    StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
                    Dialogue.text = "";
                    StartCoroutine(AnimateText());
                }
                else { 
                    StopAllCoroutines();
                    Dialogue.text = StoredDialogue;
                    isAnimating = false;
                }

            }



    }

    public bool DialogueContainsTag(string dialogueTags,string tag){
        if(dialogueTags.Contains(tag.ToLower()) || dialogueTags.Contains(tag)){
            return true;
        }else{
            return false;
        }
    }

    private void CheckForTags() {

        if (DialogueContainsTag(StoredDialogue, DocumentTags.Date))
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

        if (DialogueContainsTag(StoredDialogue, DocumentTags.Defence))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Defence.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.DefenseProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Defence.Length);
            currentSubjectanim = CameraMover.instance.Defence.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue, DocumentTags.Prosecutor))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Prosector.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.ProsecutorProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Prosecutor.Length);
            currentSubjectanim = CameraMover.instance.Prosector.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue, DocumentTags.Judge))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Judge.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.JudgeProfile.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Judge.Length);
            currentSubjectanim = CameraMover.instance.Judge.GetComponent<Animator>();
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.Witness)){
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = SuspectProfileScript.instance.CurrentSuspectData.WitnessName;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.Witness.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.WitnessTestimony)){
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            Dialogue.color = Color.green;
            Dialogue.alignment = TextAnchor.MiddleCenter;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.WitnessTestimony.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.CrossExamination)){
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            Dialogue.color = Color.red;
            Dialogue.alignment = TextAnchor.MiddleCenter;
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.CrossExamination.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
            OpenCrossExaminationPanel();
        }


        //animation tags

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimDeskSlam)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimDeskSlam.Length);
            currentSubjectanim.Play(DocumentTags.AnimDeskSlam);
            //Debug.Log("Slam Desk");
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimSmirk)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimSmirk.Length);
            currentSubjectanim.Play(DocumentTags.AnimSmirk);
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimBreakdown)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimBreakdown.Length);
            currentSubjectanim.Play(DocumentTags.AnimBreakdown);
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimRealization)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimRealization.Length);
            currentSubjectanim.Play(DocumentTags.AnimRealization);
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimShock)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimShock.Length);
            currentSubjectanim.Play(DocumentTags.AnimShock);
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimStandingIdle)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimStandingIdle.Length);
            currentSubjectanim.Play(DocumentTags.AnimStandingIdle);
        }

        if(DialogueContainsTag(StoredDialogue, DocumentTags.AnimWalkingIdle)){
            StoredDialogue = StoredDialogue.Remove(0, DocumentTags.AnimWalkingIdle.Length);
            currentSubjectanim.Play(DocumentTags.AnimWalkingIdle);
        }

    }

    public IEnumerator AnimateText() {

        CheckForTags();

        //for (int t = 0; t < StoredDialogue.Length; t++) {
        //    Dialogue.text += " ";
        //    Debug.Log("t = " + t);
        //    yield return null;
        //}
        //yield return new WaitForSeconds(textSpeed);//delay a bit before animating
        isAnimating = true;
        int i = 0;
        while (i < StoredDialogue.Length && isAnimating) {
            yield return new WaitForSeconds(textSpeed);
            //Dialogue.text += StoredDialogue.ToCharArray()[i];

            if (i < StoredDialogue.Length) {
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
