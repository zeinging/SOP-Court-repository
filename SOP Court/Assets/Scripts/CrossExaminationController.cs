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

    private int currentTestimonySeriesSlot = 1;




    private XElement crossExamination;

    // Debugging

    public Text displayText;


    public void Previous()
    {
        Debug.Log("Previous!");
    }
    public void Next()
    {
        Debug.Log("Next!");
    }

    public void Continue()
    {

        currentTestimonySeriesSlot++;

        XElement targetTestimonySeries = crossExamination.Elements(TESTIMONY_SERIES_XML_TAG).Skip(currentTestimonySeriesSlot - 1).Take(1).ToList().First();

        string text = GetDislpayTextFromTestimonyParagraph(targetTestimonySeries.Element(TESTIMONY_PARAGRAPH_XML_TAG));

        displayText.text = text;


    }
    public void Press()
    {
        Debug.Log("Press!");
    }

    void Start()
    {

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        crossExamination = firstFileXML.Element(CROSS_EXAMINATION_XML_TAG);

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
