using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanelScript : MonoBehaviour
{

    public GameObject ContinueButton;

    public Text SpeakerName, Dialogue;

    public string StoredDialogue;

    public float textSpeed = 0.2f;

    public int CurrentDialogue = 0;

    public bool isAnimating = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        //Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[0];
        ContinueButton.gameObject.SetActive(true);
        StoredDialogue = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
        StartCoroutine(AnimateText());
    }

    public void Continue() {

        if (CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1)
        {

            if (!isAnimating)
            {
                CurrentDialogue++;
                //Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];
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
        else
        {
            gameObject.SetActive(false);
            ContinueButton.SetActive(false);
            SceneFaderScript.i.StartFade();
        }

    }

    private void CheckForTags() {

        if (StoredDialogue.ToLower().Contains("[date]"))
        {
            SpeakerName.transform.parent.gameObject.SetActive(false);
            StoredDialogue = StoredDialogue.Remove(0, 6);
            //StoredDialogue = System.DateTime.Now.ToString();
            //Debug.Log("change color");
            Dialogue.color = Color.green;
            Dialogue.alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            SpeakerName.transform.parent.gameObject.SetActive(true);
            Dialogue.color = Color.white;
            Dialogue.alignment = TextAnchor.UpperLeft;
        }

        if (StoredDialogue.ToLower().Contains("[defence]"))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Defence.position);
            SpeakerName.text = "Defence";
            StoredDialogue = StoredDialogue.Remove(0, 10);
        }

        if (StoredDialogue.ToLower().Contains("[prosecutor]"))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Prosector.position);
            SpeakerName.text = "Prosecutor";
            StoredDialogue = StoredDialogue.Remove(0, 13);
        }

        if (StoredDialogue.ToLower().Contains("[judge]"))
        {
            CameraMover.instance.SnapCamHere(CameraMover.instance.Judge.position);
            SpeakerName.text = "Judge";
            StoredDialogue = StoredDialogue.Remove(0, 7);
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

    // Update is called once per frame
    void Update()
    {

            //if(Input.GetKeyDown(KeyCode.KeypadEnter)){
            //    if(CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1){

            //        CurrentDialogue++;
            //        Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];

            //    }else{
            //        gameObject.SetActive(false);
            //    }
            //}
    }
}
