using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System;

public class GetDocumentsScript : MonoBehaviour
{

    private string myDocumentsFolderPath;
    
    public string[] FoldersInDocumentsFolder;

    public List<string> 
    FilesPathCase1Folder,
    FilesPathCase2Folder,
    FilesPathCase3Folder,
    FilesPathCase4Folder,
    FilesPathCase5Folder;

    //public string[] Dialogue;

    // Start is called before the first frame update
    void Awake()
    {
        myDocumentsFolderPath = Application.dataPath + "/DocumentCases/";

        string xmlFileName = myDocumentsFolderPath + "Case1/CrossExamination.xml";

        if (!File.Exists(xmlFileName))
        {
            Debug.Log("Target File Doesn't Exist!");
            return;
        }

        var crossExaminationXML = XElement.Load(xmlFileName);
        var items =
            from statement in crossExaminationXML.Elements()
            select statement.Element("Text").Value + "";


       
        Debug.Log("Showing Results:");

        foreach ( var item in items ) {

            Debug.Log(item);
        }




        //GetCaseFolders();
        //GetFilesInFolder(FoldersInDocumentsFolder[0], FilesPathCase1Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[1], FilesPathCase2Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[2], FilesPathCase3Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[3], FilesPathCase4Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[4], FilesPathCase5Folder);
    }

    public void GetCaseFolders(){
        //namesArray = File.ReadAllLines(myFilePathCase1);
        //filesInCaseFolder = Directory.GetFiles(myFilePathCase1);
        FoldersInDocumentsFolder = Directory.GetFiles(myDocumentsFolderPath);

        for(int s = 0; s < FoldersInDocumentsFolder.Length; s++){
            int Sstart = FoldersInDocumentsFolder[s].IndexOf(".");
            FoldersInDocumentsFolder[s] = FoldersInDocumentsFolder[s].Remove(Sstart, 5);
            FoldersInDocumentsFolder[s] += "/";
        }
    }

    public void GetFilesInFolder(string casePath, List<string> Files){

        string [] AllFiles = Directory.GetFiles(casePath);

        for(int i = 0; i < AllFiles.Length; i++){
            if(!AllFiles[i].Contains(".meta")){//don't add to list if it's a meta file
                //FilesInCase1Folder.Add(AllFiles[i]);
                Files.Add(AllFiles[i]);
            }
        }

    }

    public string[] GetTextFromFileTest(){
        //Debug.Log(FilesPathCase1Folder[0]);
        string[] Dialogue = File.ReadAllLines(FilesPathCase1Folder[0]);
        return Dialogue;
        //string[] Dialogue = new string[1];
        //Dialogue[0] = "testing this fly is really anoying!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
