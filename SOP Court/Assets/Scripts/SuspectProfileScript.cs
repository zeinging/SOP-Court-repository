using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectProfileScript : MonoBehaviour
{

    public ScriptableObjectProfile[] Evidence;
    public ScriptableObjectProfile[] SuspectsInOrder;

    public ScriptableObjectProfile CurrentSuspectData;

    public List<ScriptableObjectProfile> SuspectsEncounteredOrder, RemainingSuspects;

    public bool ShuffleSuspects = true;

    public static SuspectProfileScript instance;

    // Start is called before the first frame update
    void Awake()
    {

        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
        }
           

        GetSuspects();
        //NextSuspect();
        
    }


    public void GetSuspects(){
        for(int i = 0; i < SuspectsInOrder.Length; i++){
            RemainingSuspects.Add(SuspectsInOrder[i]);
        }
    }

    public void GenerateListTest(){
        if(ShuffleSuspects){

            for(int i = 0; i < SuspectsInOrder.Length; i++){
                NextSuspect(Random.Range(0, RemainingSuspects.Count));
            }

        }else{
            
            for(int i = 0; i < SuspectsInOrder.Length; i++){
                NextSuspect(i);
            }
            RemainingSuspects.Clear();
        }
        CurrentSuspectData = SuspectsEncounteredOrder[0];
    }

    public void NextSuspect(int r){

        //r = Random.Range(0, RemainingSuspects.Count);
        //CurrentSuspect = Random.Range(0, RemainingSuspects.Count);
        CurrentSuspectData = RemainingSuspects[r];
        SuspectsEncounteredOrder.Add(CurrentSuspectData);

        if(ShuffleSuspects)
        RemainingSuspects.Remove(CurrentSuspectData);
                
    }


}
