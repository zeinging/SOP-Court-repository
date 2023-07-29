using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class CrossExaminationController : MonoBehaviour
{

    // Debugging


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

    private void Start()
    {

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
