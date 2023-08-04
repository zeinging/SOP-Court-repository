using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

    private XElement NotImportantPressedInteractions;

    private static readonly string CROSS_EXAMINATION_XML_TAG = "CrossExamination";
    private static readonly string TESTIMONY_SERIES_XML_TAG = "TestimonySeries";
    private static readonly string TESTIMONY_PARAGRAPH_XML_TAG = "TestimonyParagraph";
    private static readonly string LINE_XML_TAG = "Line";
    private static readonly string PRESSED_INTERACTIONS_XML_TAG = "PressedInteractions";
    private static readonly string PRESSED_INTERACTION_XML_TAG = "PressedInteraction";
    private static readonly string CHARACTER_XML_TAG = "Character";
    private static readonly string PARAGRAPH_XML_TAG = "Paragraph";


    private static readonly string ON_STAND_XML_ATTRIBUTE = "onStand";

    private static readonly string ITEM_XML_ATTRIBUTE = "item";
    private static readonly string IS_CONFLICIN_XML_ATTRIBUTE = "isConflicting";



    private int currentFileNumber = 1;

    private int currentTestimonySeriesIndex = 0;

    private bool inCrossExaminationMode = false;

    private int numberOfSeries = -1;





    private XElement crossExamination;

    private string OnStandCharacter;

    public GameObject CourtRecordManager;

    public GameObject dialogPanel;
    public GameObject crossExaminationPanel;

    public GameObject CrossExaminationContinueButton;
    public GameObject DialogPanelContinueButton;

    public Text[] displayTexts;
    public Text characterText;

    private bool progressingThroughPressedInteraction = false;
    private IEnumerator<(List<string>, string)> ParagraphAndCharacterFromPressedInteractionInterator;

    private bool progressingThroughNotImportantDialog = false;
    private bool isPress = false;

    private void MoveCameraToCharacter(string character)
    {


        switch (character.ToLower())
        {
            case "phoenix wright":
                {
                    CameraMover.instance.SnapCamHere(CameraMover.instance.Defence.position);
                    break;
                }
            case "judge":
                {

                    CameraMover.instance.SnapCamHere(CameraMover.instance.Judge.position);
                    break;
                }
            case "sahwit":
                {
                    CameraMover.instance.SnapCamHere(CameraMover.instance.Witness.position);
                    break;
                }
            case "payne":
                {
                    CameraMover.instance.SnapCamHere(CameraMover.instance.Prosector.position);
                    break;
                }
            default:
                {
                    throw new IOException("Unexpected value of " + character + "! Doesn't fit expected characters!");
                }
        }


    }

    public void StartCrossExamination()
    {

        dialogPanel.SetActive(false);
        DialogPanelContinueButton.SetActive(false);
        crossExaminationPanel.SetActive(true);
        CrossExaminationContinueButton.SetActive(true);

        List<string> firstMessage = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

        int index = 0;

        foreach (string line in firstMessage)
        {
            displayTexts[index++].text = line;
        }

        characterText.text = OnStandCharacter;
        MoveCameraToCharacter(OnStandCharacter);

    }

    private void ResetDisplayTexts()
    {
        displayTexts[0].text = "";
        displayTexts[1].text = "";
        displayTexts[2].text = "";
    }

    private IEnumerator<(List<string>, string)> GetParagraphAndCharacterFromPressedInteractionForCurrentStep(bool forPressed, string item = null)
    {
        //int dg = currentTestimonySeriesIndex;

        //IEnumerable<XElement> series = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG);
        //List<XElement> testSeries = series.ToList();

        //XElement specificSeries = series.ElementAt(currentTestimonySeriesIndex);

        //IEnumerable<XElement> pressedInteractions = specificSeries.Elements(PRESSED_INTERACTIONS_XML_TAG);

        //List<XElement> testPressedInteractions = pressedInteractions.ToList();

        //XElement filteredPressedInteractions = pressedInteractions.Where(pressedInteraction => pressedInteraction != null && (forPressed ? pressedInteraction.Attribute(ITEM_XML_ATTRIBUTE) == null : pressedInteraction.Attribute(ITEM_XML_ATTRIBUTE) != null)).First();




        XElement pressedInteractionsComplete = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Elements(PRESSED_INTERACTIONS_XML_TAG).Where(pressedInteraction => pressedInteraction != null && (forPressed ? pressedInteraction.Attribute(ITEM_XML_ATTRIBUTE) == null : pressedInteraction.Attribute(ITEM_XML_ATTRIBUTE) != null)).First();

        if (!forPressed)
        {

            XAttribute neededItem = pressedInteractionsComplete.Attribute(ITEM_XML_ATTRIBUTE);

            if (neededItem == null)
            {
                Debug.LogError("Retreving item from pressedInterations is not working!");
                throw new IOException("Couldn't find pressed Interations item tag when should've been there");
            }

            if (item == null)
            {
                Debug.LogError("Item paramter needs to be given!");
                throw new IOException("Item parameter needs to be given!");
            }

            if (item.ToLower() != neededItem.Value.ToLower())
            {





                // Provided item but 

                // Matching

            }

        }


        IEnumerable<XElement> pressedInteractionList = pressedInteractionsComplete.Elements(PRESSED_INTERACTION_XML_TAG);

        foreach (XElement pressedInteraction in pressedInteractionList)
        {

            List<string> paragraphStringList = pressedInteraction.Element(PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(x => x.Value).ToList();


            string character = pressedInteraction.Element(CHARACTER_XML_TAG).Value;

            yield return (paragraphStringList, character);

        }




    }

    // 0 base.
    private IEnumerable<XElement> getElementAtSlot(IEnumerable<XElement> query, int slot)
    {
        return query.Skip(slot).Take(1);
    }


    public void Previous()
    {

        if (!inCrossExaminationMode || progressingThroughPressedInteraction)
        {
            return;
        }


        Debug.Log("Previous!");

        if (currentTestimonySeriesIndex <= 0)
        {
            Debug.Log("User attempted to go to previous item when on the first item.");
            return;
        }


        ResetDisplayTexts();

        currentTestimonySeriesIndex--;

        IEnumerable<string> lines = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(line => line.Value);

        int index = 0;
        foreach (string line in lines)
        {
            displayTexts[index].text = line;
            index++;
        }

        characterText.text = OnStandCharacter;
        MoveCameraToCharacter(OnStandCharacter);

    }
    public void Next()
    {
        if (!inCrossExaminationMode || progressingThroughPressedInteraction)
        {
            return;
        }


        Debug.Log("Next!");
        if (currentTestimonySeriesIndex >= numberOfSeries - 1)
        {
            Debug.Log("User attempted to go to next item when on the last item.");
            return;
        }

        ResetDisplayTexts();

        currentTestimonySeriesIndex++;

        IEnumerable<string> lines = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(line => line.Value);

        int index = 0;

        foreach (string line in lines)
        {
            displayTexts[index].text = line;
            index++;
        }

    }

    public void Continue()
    {

        if (!progressingThroughPressedInteraction && inCrossExaminationMode)
        {
            // don't want to use it in this case
            return;
        }

        ResetDisplayTexts();



        if (progressingThroughPressedInteraction)
        {

            bool result = ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();

            if (!result)
            {
                if (isPress)
                {

                    if (currentTestimonySeriesIndex >= numberOfSeries - 1)
                    {
                        // Reached the end.
                        currentTestimonySeriesIndex = 0;
                    }
                    else
                    {
                        currentTestimonySeriesIndex++;
                    }

                }
                else
                {

                    // Presenting

                    if (progressingThroughNotImportantDialog)
                    {
                        // Not successful
                        // Don't need to do anything
                    }
                    else
                    {
                        // Successful

                        Debug.Log("Need to implement switch to next case");
                    }


                }

                // last pressed interation. Go to next block
                CrossExaminationContinueButton.SetActive(false);

                progressingThroughPressedInteraction = false;
                progressingThroughNotImportantDialog = false;


                IEnumerable<string> nextText = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements("Line").Select(line => line.Value);
                int indexFirst = 0;
                foreach (string line in nextText)
                {
                    displayTexts[indexFirst].text = line;

                    indexFirst++;

                }

                characterText.text = OnStandCharacter;
                MoveCameraToCharacter(OnStandCharacter);
                return;

            }

            (List<string>, string) test = ParagraphAndCharacterFromPressedInteractionInterator.Current;

            List<string> paragraph = test.Item1;
            string character = test.Item2;

            int indexSecond = 0;
            foreach (string line in paragraph)
            {
                displayTexts[indexSecond].text = line;
                indexSecond++;
            }

            characterText.text = character;
            MoveCameraToCharacter(character);

            // TODO: Write code here to use character
            return;
        }

        currentTestimonySeriesIndex++;

        List<XElement> targetTestimonySeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Skip(currentTestimonySeriesIndex).Take(1).ToList();

        if (targetTestimonySeries.Count <= 0)
        {
            // Last Testimony series perhaps. Handle transition to cross-examination mode

            inCrossExaminationMode = true;

            if (CrossExaminationContinueButton == null)
            {
                Debug.LogError("Reference to ContinueButton is null while trying to disable it!");
                return;
            }
            CrossExaminationContinueButton.SetActive(false);


            List<string> toSetText = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

            int indexFirst = 0;
            foreach (string line in toSetText)
            {
                displayTexts[indexFirst++].text = line;
            }

            characterText.text = OnStandCharacter;
            MoveCameraToCharacter(OnStandCharacter);

            currentTestimonySeriesIndex = 0;



            return;
        }



        List<string> text = GetDislpayTextFromTestimonyParagraph(targetTestimonySeries.First().Element(TESTIMONY_PARAGRAPH_XML_TAG));

        int index = 0;
        foreach (string line in text)
        {
            displayTexts[index++].text = line;
        }

        characterText.text = OnStandCharacter;
        MoveCameraToCharacter(OnStandCharacter);


    }

    private void StartInteratorForPressedInteraction(bool forPressed, string item = null)
    {

        ParagraphAndCharacterFromPressedInteractionInterator = GetParagraphAndCharacterFromPressedInteractionForCurrentStep(forPressed, item);

        ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();
        (List<string>, string) test2 = ParagraphAndCharacterFromPressedInteractionInterator.Current;

        int index = 0;

        foreach (string line in test2.Item1)
        {
            displayTexts[index++].text = line;
        }
        characterText.text = test2.Item2;
        MoveCameraToCharacter(test2.Item2);
    }

    private IEnumerator<(List<string>, string)> NotImportantMessageIterator()
    {

        IEnumerable<XElement> PressedInteractions = NotImportantPressedInteractions.Elements(PRESSED_INTERACTION_XML_TAG);

        foreach (XElement PressedInteraction in PressedInteractions)
        {
            List<string> paragraph = PressedInteraction.Element(PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(line => line.Value).ToList();

            string character = PressedInteraction.Element(CHARACTER_XML_TAG).Value;

            yield return (paragraph, character);

        }


    }

    private void DisplayNotImportantMessages()
    {
        progressingThroughNotImportantDialog = true;
        progressingThroughPressedInteraction = true;
        CrossExaminationContinueButton.SetActive(true);
        ParagraphAndCharacterFromPressedInteractionInterator = NotImportantMessageIterator();
        ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();
        (List<string>, string) test2 = ParagraphAndCharacterFromPressedInteractionInterator.Current;

        int index = 0;

        foreach (string line in test2.Item1)
        {
            displayTexts[index++].text = line;
        }
        characterText.text = test2.Item2;
        MoveCameraToCharacter(test2.Item2);
    }


    public void Press()
    {
        if (!inCrossExaminationMode)
        {
            return;
        }



        CrossExaminationContinueButton.SetActive(true);


        GameplayControllerScript.instance.HoldIt(1f);
        Debug.Log("Press!");

        // Set this variable to avoid weirdness with previous/next buttons
        progressingThroughPressedInteraction = true;

        isPress = true;

        StartInteratorForPressedInteraction(true);

    }

    // Open up the court record

    /* Instead of the 'on click' button sending us the item, 
     * we will reference the Court record to see which one is currently
     selected*/
    public void Present()
    {//should open the court record instead, before objection image appears
     //GameplayControllerScript.instance.Objection(2f);

        ScriptableObjectProfile selectedEvidence = CourtRecordManager.GetComponent<CourtRecordManager>().CurrentlySelectedEvidence;

        CourtRecordManager.SetActive(false);

        XElement testimonySeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex);

        string isConflicting = testimonySeries.Attribute("isConflicting").Value;

        //testimonySeries.Element
        if (isConflicting == "false")
        {
            // Handle default not important interaction
            DisplayNotImportantMessages();
            return;

        }
        if (!(isConflicting == "true"))
        {
            // Handle odd case
            Debug.LogError("IsConflicting is not a true or false value: " + isConflicting);
            return;

        }
        // Is conflicting true

        string itemName = selectedEvidence.WitnessName;

        string neededItem = testimonySeries.Elements(PRESSED_INTERACTIONS_XML_TAG).Where(pressedInteraction => pressedInteraction.Attribute(ITEM_XML_ATTRIBUTE) != null).First().Attribute(ITEM_XML_ATTRIBUTE).Value;

        isPress = false;

        if (neededItem.ToLower() != itemName.ToLower())
        {

            DisplayNotImportantMessages();
            return;

            // Handle default not important interaction

        }




        // Correct information
        CrossExaminationContinueButton.SetActive(true);
        GameplayControllerScript.instance.HoldIt(1f);
        progressingThroughPressedInteraction = true;
        StartInteratorForPressedInteraction(false, itemName);


    }

    void Start()
    {

        // Do the Not Important One
        string myDocumentsFolderPath = Application.dataPath + "/DocumentCases/";

        string xmlFileName = myDocumentsFolderPath + "Case1/NotImportant.xml";

        if (!File.Exists(xmlFileName))
        {
            Debug.Log("Not Important File Doesn't Exist!");
        }
        else
        {
            NotImportantPressedInteractions = XElement.Load(xmlFileName);
        }


        // Do the main one

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        crossExamination = firstFileXML.Element(CROSS_EXAMINATION_XML_TAG);

        OnStandCharacter = crossExamination.Attribute(ON_STAND_XML_ATTRIBUTE).Value;

        numberOfSeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Count();

    }


    private List<string> GetDislpayTextFromTestimonyParagraph(XElement testimonyParagraph)
    {

        List<string> lines = testimonyParagraph.Elements("Line").Select(x => x.Value).ToList();

        return lines;


    }




    private XElement GetXmlFromFile(int crossExaminationNumber)
    {

        string myDocumentsFolderPath = Application.dataPath + "/DocumentCases/";

        string xmlFileName = myDocumentsFolderPath + "Case1/CrossExamination" + crossExaminationNumber + ".xml";

        if (!File.Exists(xmlFileName))
        {
            Debug.Log("Target File Doesn't Exist!");
            return null;
        }

        return XElement.Load(xmlFileName);
    }
}
