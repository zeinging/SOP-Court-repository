using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

    private int currentFileNumber = 1;

    private int currentTestimonySeriesIndex = 1;




    private XElement currentTestimonySeries;

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
        Debug.Log("Continue");
    }
    public void Press()
    {
        Debug.Log("Press!");
    }

    void Start()
    {

        XElement firstFileXML = GetXmlFromFile(currentFileNumber);

        currentTestimonySeries = firstFileXML.Element("CrossExamination").Element("TestimonySeries");

        string firstMessage = GetDislpayTextFromTestimonyParagraph(currentTestimonySeries.Element("TestimonyParagraph"));

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
