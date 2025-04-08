using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public int maximumProgress;
    public int currentProgress;
    public Image mask;
    public Image fill;
    public static ProgressBar instance;
    //public Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();   
    }
    void GetCurrentFill()
    {
        float fillAmount = (float)currentProgress / (float)maximumProgress;
        mask.fillAmount = fillAmount;
        //fill.color = color;
    }
}
