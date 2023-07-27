using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        GetCaseFolders();
        GetFilesInFolder(FoldersInDocumentsFolder[0], FilesPathCase1Folder);
        GetFilesInFolder(FoldersInDocumentsFolder[1], FilesPathCase2Folder);
        GetFilesInFolder(FoldersInDocumentsFolder[2], FilesPathCase3Folder);
        GetFilesInFolder(FoldersInDocumentsFolder[3], FilesPathCase4Folder);
        GetFilesInFolder(FoldersInDocumentsFolder[4], FilesPathCase5Folder);
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

    public string[] GetTextFromFileTest(int s){
        //Debug.Log(FilesPathCase1Folder[0]);
        if( s < FilesPathCase1Folder.Count){
        string[] Dialogue = File.ReadAllLines(FilesPathCase1Folder[s]);
        return Dialogue;
        }
        return null;
        //return Dialogue;
        //string[] Dialogue = new string[1];
        //Dialogue[0] = "testing this fly is really anoying!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
