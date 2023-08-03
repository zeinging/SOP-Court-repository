using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtRecordManager : MonoBehaviour
{

    public Image ProfileImage;

    public Text ProfileName, ProfileDescription;

    public int SelectedProfile;

    public GameObject EvidenceButnParent, ProfileButnParent, EvidenceSectionButn, ProfileSectionButn;
    public List<Button> EvidenceButtons, ProfileButtons;

    public Button LeftArrowButn, RightArrowButn;

    public ScriptableObjectProfile CurrentlySelectedEvidence;

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
        if (ProfileButtons.Count == 0)
        {
            SuspectProfileScript.instance.GenerateListTest();
            GetButtons();
        }
        SelectEvidence(0);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(EvidenceButtons[0].gameObject);
    }


    private void GetButtons()
    {

        for (int i = 0; i < ProfileButnParent.transform.childCount; i++)
        {
            ProfileButtons.Add(ProfileButnParent.transform.GetChild(i).GetComponent<Button>());
            EvidenceButtons.Add(EvidenceButnParent.transform.GetChild(i).GetComponent<Button>());

            int profileIndex = i;

            if (i < SuspectProfileScript.instance.SuspectsInOrder.Length)
            {
                ProfileButtons[i].GetComponent<Image>().sprite = SuspectProfileScript.instance.SuspectsEncounteredOrder[i].ProfileImage;
                ProfileButtons[i].onClick.AddListener(() => SelectProfile(profileIndex));
            }
            else
            {
                ProfileButtons[i].interactable = false;
                ProfileButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            if (i < SuspectProfileScript.instance.Evidence.Length)
            {
                EvidenceButtons[i].GetComponent<Image>().sprite = SuspectProfileScript.instance.Evidence[i].ProfileImage;
                EvidenceButtons[i].onClick.AddListener(() => SelectEvidence(profileIndex));
            }
            else
            {
                EvidenceButtons[i].interactable = false;
                EvidenceButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            if (SuspectProfileScript.instance.SuspectsEncounteredOrder.Count > 10 || SuspectProfileScript.instance.Evidence.Length > 10)
            {
                LeftArrowButn.gameObject.SetActive(true);
                RightArrowButn.gameObject.SetActive(true);
            }
            //ProfileButtons[i].onClick.AddListener(delegate {SelectProfile(i);});
            //Debug.Log(ProfileButtons[i].name);

        }

    }


    public void SelectProfile(int profileIndex)
    {
        //SelectedProfile = i;
        //Debug.Log("" + profileIndex);
        ProfileImage.sprite = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].ProfileImage;
        ProfileName.text = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessName;
        ProfileDescription.text = SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessProfesion;
        //Debug.Log("selected " + SuspectProfileScript.instance.SuspectsEncounteredOrder[profileIndex].WitnessName);
        //ProfileButtons[profileIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SelectEvidence(int evidenceIndex)
    {

        CurrentlySelectedEvidence = SuspectProfileScript.instance.Evidence[evidenceIndex];

        ProfileImage.sprite = CurrentlySelectedEvidence.ProfileImage;
        ProfileName.text = CurrentlySelectedEvidence.WitnessName;
        ProfileDescription.text = CurrentlySelectedEvidence.WitnessProfesion;
    }

    public void ProfileSectionButton()
    {
        ProfileSectionButn.SetActive(false);
        EvidenceSectionButn.SetActive(true);
        EvidenceButnParent.SetActive(false);
        ProfileButnParent.SetActive(true);
        SelectProfile(0);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(ProfileButtons[0].gameObject);
    }

    public void EvidenceSectionButton()
    {
        EvidenceSectionButn.SetActive(false);
        ProfileSectionButn.SetActive(true);
        ProfileButnParent.SetActive(false);
        EvidenceButnParent.SetActive(true);
        SelectEvidence(0);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(EvidenceButtons[0].gameObject);
    }

    public void BackButton()
    {
        EvidenceSectionButn.SetActive(false);
        ProfileSectionButn.SetActive(true);
        ProfileButnParent.SetActive(false);
        EvidenceButnParent.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
