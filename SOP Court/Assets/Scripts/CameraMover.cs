using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public GameObject Cam;

    public Transform Witness, Judge, Defence, Prosector;

    public static CameraMover instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            SnapCamHere(Witness.position);
        }
        
        if(Input.GetKeyDown(KeyCode.W)){
            SnapCamHere(Judge.position);
        }

        if(Input.GetKeyDown(KeyCode.E)){
            SnapCamHere(Defence.position);
        }

        if(Input.GetKeyDown(KeyCode.R)){
            SnapCamHere(Prosector.position);
        }
    }

    public void SnapCamHere(Vector3 Pos){
        Cam.transform.position = new Vector3(Pos.x, Pos.y, Cam.transform.position.z);
    }
}
