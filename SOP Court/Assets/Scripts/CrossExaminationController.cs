using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

    private static readonly string CROSS_EXAMINATION_XML_TAG = "CrossExamination";
    private static readonly string TESTIMONY_SERIES_XML_TAG = "TestimonySeries";
    private static readonly string TESTIMONY_PARAGRAPH_XML_TAG = "TestimonyParagraph";
    private static readonly string LINE_XML_TAG = "Line";
    private static readonly string PRESSED_INTERACTIONS_XML_TAG = "PressedInteractions";
    private static readonly string PRESSED_INTERACTION_XML_TAG = "PressedInteraction";
    private static readonly string CHARACTER_XML_TAG = "Character";
    private static readonly string PARAGRAPH_XML_TAG = "Paragraph";


    private static readonly string ON_STAND_XML_ATTRIBUTE = "onStand";




    private int currentFileNumber = 1;

    private int currentTestimonySeriesIndex = 0;

    private bool inCrossExaminationMode = false;

    private int numberOfSeries = -1;

    public GameObject ContinueButton;




    private XElement crossExamination;

    private string OnStandCharacter;

    // Debugging

    public Text[] displayTexts;
    public Text characterText;

    private bool progressingThroughPressedInteraction = false;
    private IEnumerator<(List<string>, string)> ParagraphAndCharacterFromPressedInteractionInterator;

    private void ResetDisplayTexts()
    {
        displayTexts[0].text = "";
        displayTexts[1].text = "";
        displayTexts[2].text = "";
    }

    private IEnumerator<(List<string>, string)> GetParagraphAndCharacterFromPressedInteractionForCurrentStep()
    {

        IEnumerable<XElement> series = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG);

        XElement targetSeries = series.ElementAt(currentTestimonySeriesIndex);

        XElement pressedInteractionsTag = targetSeries.Element(PRESSED_INTERACTIONS_XML_TAG);

        IEnumerable<XElement> pressedInteractionList = pressedInteractionsTag.Elements(PRESSED_INTERACTION_XML_TAG);

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

    }
    public void Next()
    {
        if (!inCrossExaminationMode || progressingThroughPressedInteraction)
        {
            return;
        }


        Debug.Log("Next!");
        if (currentTestimonySeriesIndex > numberOfSeries - 1)
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
                // last pressed interation. Go to next block
                currentTestimonySeriesIndex++;

                progressingThroughPressedInteraction = false;

                IEnumerable<string> nextText = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements("Line").Select(line => line.Value);
                int indexFirst = 0;
                foreach (string line in nextText)
                {
                    displayTexts[indexFirst].text = line;

                    indexFirst++;

                }

                characterText.text = OnStandCharacter;
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
            // TODO: Write code here to use character
            return;
        }

        currentTestimonySeriesIndex++;

        List<XElement> targetTestimonySeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Skip(currentTestimonySeriesIndex).Take(1).ToList();

        if (targetTestimonySeries.Count <= 0)
        {
            // Last Testimony series perhaps. Handle transition to cross-examination mode

            inCrossExaminationMode = true;

            if (ContinueButton == null)
            {
                Debug.LogError("Reference to ContinueButton is null while trying to disable it!");
                return;
            }
            ContinueButton.SetActive(false);


            List<string> toSetText = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

            int indexFirst = 0;
            foreach (string line in toSetText)
            {
                displayTexts[indexFirst++].text = line;
            }

            characterText.text = OnStandCharacter;

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


    }
    public void Press()
    {
        if (!inCrossExaminationMode)
        {
            return;
        }


        GameplayControllerScript.instance.HoldIt(1f);
        Debug.Log("Press!");

        // Set this variable to avoid weirdness with previous/next buttons
        progressingThroughPressedInteraction = true;


        IEnumerable<XElement> pressedInteractions = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(PRESSED_INTERACTIONS_XML_TAG).Elements(PRESSED_INTERACTION_XML_TAG);

        List<XElement> test = pressedInteractions.ToList();

        ParagraphAndCharacterFromPressedInteractionInterator = GetParagraphAndCharacterFromPressedInteractionForCurrentStep();

        ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();
        (List<string>, string) test2 = ParagraphAndCharacterFromPressedInteractionInterator.Current;

        int index = 0;

        foreach (string line in test2.Item1)
        {
            displayTexts[index++].text = line;
        }
        characterText.text = test2.Item2;
    }

    // Open up the court record
    public void Present()
    {//should open the court record instead, before objection image appears
        GameplayControllerScript.instance.Objection(2f);
    }

    void Start()
    {

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        crossExamination = firstFileXML.Element(CROSS_EXAMINATION_XML_TAG);

        OnStandCharacter = crossExamination.Attribute(ON_STAND_XML_ATTRIBUTE).Value;

        numberOfSeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Count();

        List<string> firstMessage = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

        int index = 0;

        foreach (string line in firstMessage)
        {
            displayTexts[index++].text = line;
        }

        characterText.text = OnStandCharacter;
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
