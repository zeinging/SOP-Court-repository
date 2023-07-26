using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFaderScript : MonoBehaviour
{

    public Image BlackFader;

    public float TimerSpeed;

    private static SceneFaderScript _i;

    public static SceneFaderScript i {
        get {
            if (_i == null) _i = (Instantiate(Resources.Load("SceneFader Canvas")) as GameObject).GetComponent<SceneFaderScript>();
            return _i;
        }
    }

    public void StartFade() {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {

        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOut());
    
    }

    private IEnumerator FadeIn()
    {
        BlackFader.gameObject.SetActive(true);
        Color temp = BlackFader.color;
        temp.a = 0;
        BlackFader.color = temp;
        while (BlackFader.color.a < 1) {
            //yield return new WaitForSeconds(timer);
            temp.a += Time.deltaTime * TimerSpeed;
            BlackFader.color = temp;
            yield return null;
        }

    }

    private IEnumerator FadeOut() {
        Color temp = BlackFader.color;
        temp.a = 1;
        BlackFader.color = temp;
        while (BlackFader.color.a > 0)
        {
            //yield return new WaitForSeconds(timer);
            temp.a -= Time.deltaTime * TimerSpeed;
            BlackFader.color = temp;
            yield return null;
        }
        BlackFader.gameObject.SetActive(false);
    }

}
