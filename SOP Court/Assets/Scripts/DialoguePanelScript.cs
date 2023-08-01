using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanelScript : MonoBehaviour
{

    public GameObject ContinueButton, pressButton, presentButton, leftArrowButton, rightArrowButton;

    public Text SpeakerName;

    public Text[] DialogueLines = new Text[3];

    public Animator currentSubjectanim;

    public string[] StoredDialogue = new string[3];

    public float textSpeed = 0.2f;

    public int CurrentDialogue = 0;

    public bool isAnimating = false;

    public int[] PlayAnimHere = new int[3];
    public string[] PlayThisClip = new string[3];


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
        //StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        StoredDialogue[0] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        StoredDialogue[1] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue + 1];
        StoredDialogue[2] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue + 2];
        StartCoroutine(AnimateText());
    }

    public void CloseDialogueBox()
    {
        CurrentDialogue = 0;
        //StoredDialogue = "";
        StoredDialogue[0] = "";
        StoredDialogue[1] = "";
        StoredDialogue[2] = "";
        DialogueLines[0].text = "";
        DialogueLines[1].text = "";
        DialogueLines[2].text = "";
        SpeakerName.text = "";
        gameObject.SetActive(false);
        ContinueButton.SetActive(false);
        SpeakerName.transform.parent.gameObject.SetActive(false);

    }

    public void Continue()
    {

        if (!isAnimating && CurrentDialogue == GameplayControllerScript.instance.CurrentSceneDialogue.Length - 3)
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
                if (CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 3)
                {
                    CurrentDialogue += 3;
                }
                StoredDialogue[0] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
                StoredDialogue[1] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue + 1];
                StoredDialogue[2] = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue + 2];
                DialogueLines[0].text = "";
                DialogueLines[1].text = "";
                DialogueLines[2].text = "";
                StartCoroutine(AnimateText());
            }
            else
            {
                StopAllCoroutines();
                DialogueLines[0].text = StoredDialogue[0];
                DialogueLines[1].text = StoredDialogue[1];
                DialogueLines[2].text = StoredDialogue[2];
                isAnimating = false;
            }

        }



    }

    public void Press()
    {
        GameplayControllerScript.instance.HoldIt(1f);
    }

    public void Present()
    {//should open the court record instead, before objection image appears
        GameplayControllerScript.instance.Objection(2f);
    }

    public void moveInCrossExamination(bool isLeft)
    {
        if (isLeft)
        {
            Debug.Log("move back");
        }
        else
        {
            Debug.Log("move forward");
        }
    }

    public bool DialogueContainsTag(string dialogueTags, string tag)
    {
        return dialogueTags.Contains(tag.ToLower()) || dialogueTags.Contains(tag);
    }

    private void CheckForTags(int LineIndex)
    {

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.Date))
        {
            SpeakerName.transform.parent.gameObject.SetActive(false);
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.Date.Length);
            //StoredDialogue = System.DateTime.Now.ToString();
            //Debug.Log("change color");
            DialogueLines[LineIndex].color = Color.green;
            DialogueLines[LineIndex].alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            DialogueLines[LineIndex].color = Color.white;
            DialogueLines[LineIndex].alignment = TextAnchor.UpperLeft;
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.Defence))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Defence.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.DefenseProfile.WitnessName;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.Defence.Length);
            currentSubjectanim = CameraMover.instance.Defence.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.Prosecutor))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Prosector.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.ProsecutorProfile.WitnessName;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.Prosecutor.Length);
            currentSubjectanim = CameraMover.instance.Prosector.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.Judge))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Judge.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = GameplayControllerScript.instance.JudgeProfile.WitnessName;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.Judge.Length);
            currentSubjectanim = CameraMover.instance.Judge.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.Witness))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            SpeakerName.transform.parent.gameObject.SetActive(true);
            SpeakerName.text = SuspectProfileScript.instance.CurrentSuspectData.WitnessName;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.Witness.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.WitnessTestimony))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            DialogueLines[LineIndex].color = Color.green;
            DialogueLines[LineIndex].alignment = TextAnchor.MiddleCenter;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.WitnessTestimony.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.CrossExamination))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
            //SpeakerName.transform.parent.gameObject.SetActive(true);
            //SpeakerName.text = DocumentTags.Witness;
            DialogueLines[LineIndex].color = Color.red;
            DialogueLines[LineIndex].alignment = TextAnchor.MiddleCenter;
            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.CrossExamination.Length);
            currentSubjectanim = CameraMover.instance.Witness.GetComponent<Animator>();
            OpenCrossExaminationPanel();
        }


        //animation tags

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimDeskSlam))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimDeskSlam;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimDeskSlam.Length);
            //currentSubjectanim.Play(DocumentTags.AnimDeskSlam);
            //Debug.Log("Slam Desk");
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimSmirk))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimSmirk;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimSmirk.Length);
            //currentSubjectanim.Play(DocumentTags.AnimSmirk);
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimBreakdown))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimBreakdown;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimBreakdown.Length);
            //currentSubjectanim.Play(DocumentTags.AnimBreakdown);
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimRealization))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimRealization;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimRealization.Length);
            //currentSubjectanim.Play(DocumentTags.AnimRealization);
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimShock))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimShock;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimShock.Length);
            //currentSubjectanim.Play(DocumentTags.AnimShock);
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimStandingIdle))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimStandingIdle;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimStandingIdle.Length);
            //currentSubjectanim.Play(DocumentTags.AnimStandingIdle);
        }

        if (DialogueContainsTag(StoredDialogue[LineIndex], DocumentTags.AnimWalkingIdle))
        {
            PlayAnimHere[LineIndex] = StoredDialogue[LineIndex].IndexOf('[');
            PlayThisClip[LineIndex] = DocumentTags.AnimWalkingIdle;

            StoredDialogue[LineIndex] = StoredDialogue[LineIndex].Remove(0, DocumentTags.AnimWalkingIdle.Length);
            //currentSubjectanim.Play(DocumentTags.AnimWalkingIdle);
        }

    }

    public IEnumerator AnimateText()
    {

        //int LineIndex = 0;
        PlayThisClip[0] = "";
        PlayThisClip[1] = "";
        PlayThisClip[2] = "";

        CheckForTags(0);
        CheckForTags(1);
        CheckForTags(2);


        //for (int t = 0; t < StoredDialogue.Length; t++) {
        //    Dialogue.text += " ";
        //    Debug.Log("t = " + t);
        //    yield return null;
        //}
        //yield return new WaitForSeconds(textSpeed);//delay a bit before animating
        isAnimating = true;
        int i = 0;

        int charactersInBox = StoredDialogue[0].Length + StoredDialogue[1].Length + StoredDialogue[2].Length;
        //Debug.Log("character in box: " + charactersInBox);
        //Debug.Log("character in line 1: " + StoredDialogue[0].Length);

        while (i < charactersInBox && isAnimating)
        {
            yield return new WaitForSeconds(textSpeed * 1000 * Time.deltaTime);
            //Dialogue.text += StoredDialogue.ToCharArray()[i];

            if (i < StoredDialogue[0].Length)
            {
                //Dialogue.text = Dialogue.text.Remove(i, 1);
                //Dialogue.text = Dialogue.text.Insert(i, StoredDialogue.ToCharArray()[i].ToString());
                //Debug.Log(Dialogue.text.ToCharArray()[i]);
                if (i > PlayAnimHere[0] && PlayThisClip[0] != "")
                {
                    currentSubjectanim.Play(PlayThisClip[0]);
                }

                DialogueLines[0].text += StoredDialogue[0].ToCharArray()[i];
                //Debug.Log("Line1");
            }
            else
            {
                //Debug.Log("start line 2");
                if (i - StoredDialogue[0].Length < StoredDialogue[1].Length)
                {

                    if (i > PlayAnimHere[1] && PlayThisClip[1] != "")
                    {
                        //Debug.Log("should've played animation");
                        currentSubjectanim.Play(PlayThisClip[1]);
                    }

                    DialogueLines[1].text += StoredDialogue[1].ToCharArray()[i - StoredDialogue[0].Length];

                }
                else
                {

                    if (i > PlayAnimHere[2] && PlayThisClip[2] != "")
                    {
                        currentSubjectanim.Play(PlayThisClip[2]);
                    }
                    //if(i - StoredDialogue[1].Length < StoredDialogue[2].Length){
                    DialogueLines[2].text += StoredDialogue[2].ToCharArray()[i - (StoredDialogue[0].Length + StoredDialogue[1].Length)];
                    //}

                }

            }




            i++;
            yield return null;
        }
        isAnimating = false;

    }

}