using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------------Audio Sources-----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [Header("----------------Audio Clips-----------------")]
    public AudioClip background;
    public AudioClip click;
    public AudioClip gameOver;
    public AudioClip hit;
    public AudioClip win;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;
    public AudioClip jetpack;
    public static AudioManager instance;
    void Awake()
    {
        // Kiểm tra nếu đã có đối tượng nhạc nền, nếu có thì xóa đi để tránh nhạc phát nhiều lần.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Giữ nhạc nền ngay cả khi chuyển scene
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;  // Đảm bảo nhạc phát liên tục
        audioSource.Play();      // Bắt đầu phát nhạc
    }

    void OnApplicationQuit()
    {
        // Dừng nhạc khi game tắt
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        //sfxSource.clip = clip;
        //sfxSource.Play();
        sfxSource.PlayOneShot(clip);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //musicSource.clip = background;
        //musicSource.loop = true;
        //musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
