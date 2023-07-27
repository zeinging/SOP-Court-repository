using UnityEngine;
using UnityEngine.UI;

public class DialoguePaneScript : MonoBehaviour
{

    public Text SpeakerName, Dialogue;

    public int CurrentDialogue = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[0];
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (CurrentDialogue < GameplayControllerScript.instance.CurrentSceneDialogue.Length - 1)
            {

                CurrentDialogue++;
                Dialogue.text = GameplayControllerScript.instance.CurrentSceneDialogue[CurrentDialogue];

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
