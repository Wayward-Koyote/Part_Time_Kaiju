using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    [Header("Audio Files")]
    [SerializeField] AudioSource menuBGM;
    [SerializeField] AudioSource levelBGM;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMenuBGM()
    {

        menuBGM.Play();
    }

    public void StopMenuBGM()
    {
        menuBGM.Stop();
    }

    public void StartLevelBGM()
    {
        levelBGM.Play();
    }

    public void StopLevelBGM()
    {
        levelBGM.Stop();
    }

    public void PauseLevelBGM()
    {
        levelBGM.Pause();
    }

    public void ResumeLevelBGM()
    {
        levelBGM.UnPause();
    }
}
