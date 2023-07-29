using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

    private static readonly string CaseXMLTag = "Case";
    private static readonly string CrossExaminationXMLTag = "CrossExamination";
    private static readonly string TestimonySeries = "TestimonySeries";
    private static readonly string TestimonyParagraph = "TestimonySeries";
    private static readonly string Line = "Line";
    private static readonly string PressedInteractions = "PressedInteractions";
    private static readonly string PressedInteraction = "PressedInteraction";



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

        IEnumerable<XElement> targetTestimonySeries = crossExamination.Elements("TestimonySeries").Skip(currentTestimonySeriesSlot - 1).Take(1);

        string displayText = GetDislpayTextFromTestimonyParagraph(targetTestimonySeries.Element("TestimonyParagraph")



    }
    public void Press()
    {
        Debug.Log("Press!");
    }

    void Start()
    {

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        crossExamination = firstFileXML.Element("CrossExamination");

        string firstMessage = GetDislpayTextFromTestimonyParagraph(crossExamination.Element("TestimonySeries").Element("TestimonyParagraph"));

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
