using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CrossExaminationController : MonoBehaviour
{

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
    public void Press()
    {
        Debug.Log("Press!");
    }

    void Awake()
    {

        Debug.Log("Start ran");

        displayText.text = "TEST TACO";




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
