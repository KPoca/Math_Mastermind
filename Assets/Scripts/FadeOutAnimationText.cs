using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutAnimationText : MonoBehaviour
{
    public Text text;  // Reference to the UI Text component
    private bool isFading = true;  // Track fading state
    private bool isWin = false;  // Checked when Win is called

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(ShowLevelText());
    }
    // Update is called once per frame
    void Update()
    {
        if (isWin)
        {
            StopCoroutine(ShowText());
            Color color = text.color;
            color.a = 0f;
            text.color = color;
            isFading = false;
        }
    }
    // Coroutine to display the level text with fade effect
    public IEnumerator ShowText()
    {
        // Set initial alpha to 0 (invisible)
        Color startColor = text.color;
        startColor.a = 1f;
        text.color = startColor;

        //// Fade in over 3 seconds
        //float fadeInDuration = 3f;
        //float time = 0;
        //while (time < fadeInDuration)
        //{
        //    startColor.a = Mathf.Lerp(0f, 1f, time / fadeInDuration);
        //    text.color = startColor;
        //    time += Time.deltaTime;
        //    yield return null;
        //}
        //startColor.a = 1f;
        //text.color = startColor;

        //float fadeInDuration = 0f;
        text.color = new Color(startColor.r, startColor.g, startColor.b, 1f);  // alpha = 1 for immediately display

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out over 2 seconds
        float fadeOutDuration = 2f;
        float time = 0;
        while (time < fadeOutDuration && isFading)
        {
            startColor.a = Mathf.Lerp(1f, 0f, time / fadeOutDuration);
            text.color = startColor;
            time += Time.deltaTime;
            yield return null;
        }

        startColor.a = 0f;
        text.color = startColor;
    }
    public void EndShowText()
    {
        isWin = true;
        //Debug.Log("I'm in EndShowText");
        //StopCoroutine(ShowText());
    }
}
