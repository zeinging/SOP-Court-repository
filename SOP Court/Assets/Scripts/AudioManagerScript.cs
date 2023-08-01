using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public GameObject MusicA;

    public GameObject[] SFX;

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
        MusicA.GetComponent<AudioSource>().volume = volumeSlider.value;
        SFX[0].GetComponent<AudioSource>().volume = volumeSlider.value;
        SFX[1].GetComponent<AudioSource>().volume = volumeSlider.value;
        SFX[2].GetComponent<AudioSource>().volume = volumeSlider.value;
    }
}
