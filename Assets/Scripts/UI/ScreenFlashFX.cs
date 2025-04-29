using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlashFX : MonoBehaviour
{
    public static ScreenFlashFX instance;

    [SerializeField] Image flashImage;       // assign the ScreenFlash Image
    [SerializeField] float fadeTime = 0.35f; // time to fade back to clear

    void Awake()
    {
        instance = this;
        if (flashImage == null)
            flashImage = GetComponent<Image>();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // 1 ▸ show full-blue instantly
        Color c = flashImage.color;
        c.a = 1f;
        flashImage.color = c;

        // 2 ▸ fade OUT to transparent
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            flashImage.color = c;
            yield return null;
        }
        c.a = 0f;
        flashImage.color = c;
    }
}
