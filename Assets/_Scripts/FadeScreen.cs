using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{

    public bool fadeOnStart = true; // Whether to start fading in immediately
    public float fadeDuration = 1f; // Duration of the fade effect
    public Color fadeColor;
    private Renderer rend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
       StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }
        
        Color newColor2 = fadeColor;
        newColor2.a =  alphaOut;
        rend.material.SetColor("_Color", newColor2);



    }
}
