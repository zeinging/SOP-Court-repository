using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class GetDocumentsScript : MonoBehaviour
{



    //public string[] Dialogue;

    // Start is called before the first frame update
    void Awake()
    {
    }


    public XElement GetXmlFromFile(int crossExaminationNumber)
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
