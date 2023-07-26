using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtRecordManager : MonoBehaviour
{

    public Image ProfileImage;

    public Text ProfileName, ProfileDescription;

    public int SelectedProfile;

    public GameObject ButtonsParent;
    public List<Button> ProfileButtons;

    public Button LeftArrowButn, RightArrowButn;

    // Start is called before the first frame update
    void Start()
    {
        // ProfileImage.sprite = ProfileData[SelectedProfile].ProfileImage;
        // ProfileName.text = ProfileData[SelectedProfile].WitnessName;
        // ProfileDescription.text = ProfileData[SelectedProfile].WitnessProfesion;
        //SelectProfile(GameplayControllerScript.instance.CurrentSuspect);
        
        //SuspectProfileScript.instance.GenerateListTest();
        


        //SelectProfile(0);
        //UnityEngine.EventSystems.EventSystem.current = null;

        //GetButtons();
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(ProfileButtons[0].gameObject);
    }

    private void OnEnable()
    {
        if (ProfileButtons.Count == 0) {
            SuspectProfileScript.instance.GenerateListTest();
            GetButtons();
        }
        SelectProfile(0);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(ProfileButtons[0].gameObject);
    }


    private void GetButtons(){

        for(int i = 0; i < ButtonsParent.transform.childCount; i++){
            ProfileButtons.Add(ButtonsParent.transform.GetChild(i).GetComponent<Button>());

            int profileIndex = i;

            ProfileButtons[i].GetComponent<Image>().sprite = SuspectProfileScript.instance.SuspectsEncounteredOrder[i].ProfileImage;
            ProfileButtons[i].onClick.AddListener(() => SelectProfile(profileIndex));

            if(SuspectProfileScript.instance.SuspectsEncounteredOrder.Count > 10){
                LeftArrowButn.gameObject.SetActive(true);
                RightArrowButn.gameObject.SetActive(true);
            }
            //ProfileButtons[i].onClick.AddListener(delegate {SelectProfile(i);});
            //Debug.Log(ProfileButtons[i].name);

        }

    }


    public void SelectProfile(int profileIndex){
        //SelectedProfile = i;
        //Debug.Log("" + profileIndex);
        ProfileImage.sprite = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].ProfileImage;
        ProfileName.text = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessName;
        ProfileDescription.text = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessProfesion;
        //Debug.Log("selected " + SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessName);
        //ProfileButtons[profileIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
