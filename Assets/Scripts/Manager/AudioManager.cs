using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    private bool _titleMusicPlaying = false;
    private bool _gameMusicPlaying = false;

    [Header("UI")]

    [SerializeField] private AudioSource _OpenSource;
    [SerializeField] private AudioSource _CloseSource;

    [Header("Gameplay")]
    [SerializeField] private AudioSource _PlantSource;

    public static AudioManager GetInstance() { return me; }
    public static AudioManager me;

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayPanelEffect(bool status)
    {
        _OpenSource.time = 0.3f;
        _CloseSource.time = 0.3f;

        if (status) _OpenSource.Play();
        else _CloseSource.Play();
    }

    public void PlayPlantSound() => _PlantSource.Play();
}
