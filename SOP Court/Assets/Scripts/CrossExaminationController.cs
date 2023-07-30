using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

    private static readonly string CASE_XML_TAG = "Case";
    private static readonly string CROSS_EXAMINATION_XML_TAG = "CrossExamination";
    private static readonly string TESTIMONY_SERIES_XML_TAG = "TestimonySeries";
    private static readonly string TESTIMONY_PARAGRAPH_XML_TAG = "TestimonyParagraph";
    private static readonly string LINE_XML_TAG = "Line";
    private static readonly string PRESSED_INTERACTIONS_XML_TAG = "PressedInteractions";
    private static readonly string PRESSED_INTERACTION_XML_TAG = "PressedInteraction";
    private static readonly string CHARACTER_XML_TAG = "Character";
    private static readonly string PARAGRAPH_XML_TAG = "Paragraph";




    private int currentFileNumber = 1;

    private int currentTestimonySeriesIndex = 0;

    private bool inCrossExaminationMode = false;

    private int numberOfSeries = -1;




    private XElement crossExamination;

    // Debugging

    public Text displayText;

    private bool progressingThroughPressedInteraction = false;
    private IEnumerator<(string, string)> ParagraphAndCharacterFromPressedInteractionInterator;

    private IEnumerator<(string, string)> GetParagraphAndCharacterFromPressedInteractionForCurrentStep()
    {

        IEnumerable<XElement> series = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG);

        XElement targetSeries = series.ElementAt(currentTestimonySeriesIndex);

        XElement pressedInteractions = targetSeries.Element(PRESSED_INTERACTION_XML_TAG);

        IEnumerable<XElement> pressedInteraction = pressedInteractions.Elements(PRESSED_INTERACTION_XML_TAG);


        IEnumerable<XElement> pressedInteractions = pressedInteraction;

        foreach (XElement pressedInteraction in pressedInteractions)
        {

            List<string> paragraphStringList = pressedInteraction.Element(PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(x => x.Value).ToList();

            string combinedParagraph = string.Join("\n", paragraphStringList);

            string character = pressedInteraction.Element(CHARACTER_XML_TAG).Value;

            yield return (combinedParagraph, character);

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

        currentTestimonySeriesIndex--;

        IEnumerable<string> lines = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(line => line.Value);

        string combinedString = "";

        foreach (string line in lines)
        {
            combinedString += line + "\n";
        }

        displayText.text = combinedString;

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
        currentTestimonySeriesIndex++;

        IEnumerable<string> lines = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(TESTIMONY_PARAGRAPH_XML_TAG).Elements(LINE_XML_TAG).Select(line => line.Value);

        string combinedString = "";

        foreach (string line in lines)
        {
            combinedString += line + "\n";
        }

        displayText.text = combinedString;
    }

    public void Continue()
    {

        if (!progressingThroughPressedInteraction && inCrossExaminationMode)
        {
            // don't want to use it in this case
            return;
        }



        if (progressingThroughPressedInteraction)
        {

            ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();

            (string, string) test = ParagraphAndCharacterFromPressedInteractionInterator.Current;

            string paragraph = test.Item1;
            string character = test.Item2;

            displayText.text = paragraph;
            // TODO: Write code here to use character


            return;
        }


        currentTestimonySeriesIndex++;

        List<XElement> targetTestimonySeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Skip(currentTestimonySeriesIndex).Take(1).ToList();

        if (targetTestimonySeries.Count <= 0)
        {
            // Last Testimony series perhaps. Handle transition to cross-examination mode

            inCrossExaminationMode = true;


            string toSetText = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

            displayText.text = toSetText;

            currentTestimonySeriesIndex = 0;



            return;
        }



        string text = GetDislpayTextFromTestimonyParagraph(targetTestimonySeries.First().Element(TESTIMONY_PARAGRAPH_XML_TAG));

        displayText.text = text;


    }
    public void Press()
    {
        if (!inCrossExaminationMode)
        {
            return;
        }
        Debug.Log("Press!");

        // Set this variable to avoid weirdness with previous/next buttons
        progressingThroughPressedInteraction = true;



        IEnumerable<XElement> pressedInteractions = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).ElementAt(currentTestimonySeriesIndex).Element(PRESSED_INTERACTIONS_XML_TAG).Elements(PRESSED_INTERACTION_XML_TAG);

        List<XElement> test = pressedInteractions.ToList();

        ParagraphAndCharacterFromPressedInteractionInterator = GetParagraphAndCharacterFromPressedInteractionForCurrentStep();

        ParagraphAndCharacterFromPressedInteractionInterator.MoveNext();
        (string, string) test2 = ParagraphAndCharacterFromPressedInteractionInterator.Current;
        Debug.Log("Test");



    }

    void Start()
    {

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        crossExamination = firstFileXML.Element(CROSS_EXAMINATION_XML_TAG);

        numberOfSeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Count();

        string firstMessage = GetDislpayTextFromTestimonyParagraph(crossExamination.Element(TESTIMONY_SERIES_XML_TAG).Element(TESTIMONY_PARAGRAPH_XML_TAG));

        displayText.text = firstMessage;
    }


    private string GetDislpayTextFromTestimonyParagraph(XElement testimonyParagraph)
    {

        IEnumerable<string> lines = testimonyParagraph.Elements("Line").Select(x => x.Value);

        Debug.Log("Lines:");

        string combinedString = "";
        foreach (string line in lines)
        {
            combinedString += line + "\n";
            Debug.Log(line);
        }

        return combinedString;
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
