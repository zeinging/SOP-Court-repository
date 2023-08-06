using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GetDocumentsScript : MonoBehaviour
{

    public UnityEngine.UI.Text URLTestText;

    private string myDocumentsFolderPath;

    public string[] FoldersInDocumentsFolder;

    public TextAsset BeginningTextFile;

    //public List<string>
    //FilesPathCase1Folder,
    //FilesPathCase2Folder,
    //FilesPathCase3Folder,
    //FilesPathCase4Folder,
    //FilesPathCase5Folder;

    public string[] DialogueFromWebFile = new string[1];

    //public string[] Dialogue;

    // Start is called before the first frame update
    void Awake()
    {
        //myDocumentsFolderPath = Application.dataPath + "/DocumentCases/";
        //GetCaseFolders();

        //GetFilesInFolder(FoldersInDocumentsFolder[1], FilesPathCase2Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[2], FilesPathCase3Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[3], FilesPathCase4Folder);
        //GetFilesInFolder(FoldersInDocumentsFolder[4], FilesPathCase5Folder);

        //StartCoroutine(GetTextFromWebFile());
    }

    public void GetCaseFolders()
    {
        //namesArray = File.ReadAllLines(myFilePathCase1);
        //filesInCaseFolder = Directory.GetFiles(myFilePathCase1);
        FoldersInDocumentsFolder = Directory.GetFiles(myDocumentsFolderPath);

        for (int s = 0; s < FoldersInDocumentsFolder.Length; s++)
        {
            int Sstart = FoldersInDocumentsFolder[s].IndexOf(".");
            FoldersInDocumentsFolder[s] = FoldersInDocumentsFolder[s].Remove(Sstart, 5);
            FoldersInDocumentsFolder[s] += "/";
        }

    }

    public void GetFilesInFolder(string casePath, List<string> Files)
    {

        string[] AllFiles = Directory.GetFiles(casePath);

        for (int i = 0; i < AllFiles.Length; i++)
        {
            if (!AllFiles[i].Contains(".meta"))
            {//don't add to list if it's a meta file
                //FilesInCase1Folder.Add(AllFiles[i]);
                Files.Add(AllFiles[i]);
            }
        }

    }


    public void StartGetTextFromWebFile()
    {
        _ = StartCoroutine(GetTextFromWebFile());
    }

    IEnumerator GetTextFromWebFile()
    {//probably run at start.
        Debug.Log("start getting files");

        //UnityWebRequest www = UnityWebRequest.Get("1.StartCase.txt");
        UnityWebRequest www = UnityWebRequest.Get("https://zeinging.itch.io/sop-court");

        yield return www.SendWebRequest();
        //yield return new WaitForSeconds(1f);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            URLTestText.text = www.error;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            URLTestText.text = results;
            //DialogueFromWebFile = www.downloadHandler.text;
        }

        www.Dispose();

    }

    public string[] GetTextFromFileTest()
    {
        //Debug.Log(FilesPathCase1Folder[0]);
        // ** To get the project built we'll just return the single file** //
        // ** Later we can implment a more flexible solution** //

        //if (s < FilesPathCase1Folder.Count)
        //{
        string t = BeginningTextFile.text;
        string[] test = t.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        string[] updatedList = new string[test.Length];
        int index = 0;
        foreach (string f in test)
        {
            updatedList[index] = f.Replace("\\'", "'");
            index++;
        }

        return updatedList;
        //URLTestText.text = Application.absoluteURL;
        //string[] Dialogue = [";
        //string[] Dialogue = File.ReadAllLines(Application.dataPath + "/1.StartCase.txt");
        //return Dialogue;
        //}
        //return null;
        //return Dialogue;
        //string[] Dialogue = new string[1];
        //Dialogue[0] = "testing this fly is really anoying!";
    }

    // Update is called once per frame
    void Update()
    {

    }
}