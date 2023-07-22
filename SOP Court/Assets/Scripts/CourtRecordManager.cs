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

    // Start is called before the first frame update
    void Start()
    {
        // ProfileImage.sprite = ProfileData[SelectedProfile].ProfileImage;
        // ProfileName.text = ProfileData[SelectedProfile].WitnessName;
        // ProfileDescription.text = ProfileData[SelectedProfile].WitnessProfesion;
        //SelectProfile(GameplayControllerScript.instance.CurrentSuspect);
        
        GameplayControllerScript.instance.GenerateListTest();
        


        SelectProfile(0);

        GetButtons();
    }


    private void GetButtons(){

        for(int i = 0; i < ButtonsParent.transform.childCount; i++){
            ProfileButtons.Add(ButtonsParent.transform.GetChild(i).GetComponent<Button>());

            int profileIndex = i;

            ProfileButtons[i].image.sprite = GameplayControllerScript.instance.SuspectsEncounteredOrder[i].ProfileImage;
            ProfileButtons[i].onClick.AddListener(() => SelectProfile(profileIndex));
            //ProfileButtons[i].onClick.AddListener(delegate {SelectProfile(i);});
            //Debug.Log(ProfileButtons[i].name);

        }

    }


    public void SelectProfile(int profileIndex){
        //SelectedProfile = i;
        //Debug.Log("" + profileIndex);
        ProfileImage.sprite = GameplayControllerScript.instance.SuspectsEncounteredOrder[profileIndex].ProfileImage;
        ProfileName.text = GameplayControllerScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessName;
        ProfileDescription.text = GameplayControllerScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessProfesion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
