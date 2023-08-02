using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public GameObject[] MusicObjects;

    public GameObject[] SFX;

    public GameObject currentMusic;

    //public float volume;

    public UnityEngine.UI.Slider volumeSlider;

    public static AudioManagerScript instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        for(int s = 0; s < MusicObjects.Length; s++){
            if(MusicObjects[s].activeInHierarchy){
                currentMusic = MusicObjects[s];
            }
        }

    }

    public void PlayMusic(int i){

        if(currentMusic != null){
            currentMusic.SetActive(false);
        }

        currentMusic = MusicObjects[i];
        MusicObjects[i].SetActive(true);
    }

    public void StopMusic(){
        currentMusic.SetActive(false);
    }

    public void PlaySound(int index){
        StartCoroutine(soundTimer(index));
    }

    IEnumerator soundTimer(int i){
        SFX[i].SetActive(true);
        yield return new WaitForSeconds(SFX[i].GetComponent<AudioSource>().clip.length);
        SFX[i].SetActive(false);

    }

    public void changeVolume(){

        for(int i = 0; i < MusicObjects.Length; i++){
            MusicObjects[i].GetComponent<AudioSource>().volume = volumeSlider.value;
        }
        SFX[0].GetComponent<AudioSource>().volume = volumeSlider.value;
        SFX[1].GetComponent<AudioSource>().volume = volumeSlider.value;
        SFX[2].GetComponent<AudioSource>().volume = volumeSlider.value;
    }
}
