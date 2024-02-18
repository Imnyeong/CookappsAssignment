using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource[] effectSources;

    [Header("BGM")]
    [SerializeField] private AudioClip lobbyBGM;
    [SerializeField] private AudioClip stageBGM;

    [Header("Effect")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip sword;
    [SerializeField] private AudioClip axe;
    [SerializeField] private AudioClip staff;
    [SerializeField] private AudioClip enemy;
    [SerializeField] private AudioClip skill;

    private void Start()
    {
        playLobbyBGM();
    }

    public void playLobbyBGM()
    {
        if (bgmSource.clip != null)
            bgmSource.clip = null;

        bgmSource.clip = lobbyBGM;
        bgmSource.volume = 0.5f;
        bgmSource.Play();
    }

    public void playStageBGM()
    {
        if (bgmSource.clip != null)
            bgmSource.clip = null;

        bgmSource.clip = stageBGM;
        bgmSource.volume = 1.0f;
        bgmSource.Play();
    }

    public void playEffect(string _name)
    {
        switch(_name)
        {
            case "Knight":
            case "Goblin":
            {
                swordEffectSound();
                break;
            }
            case "Viking":
            case "Orc":
            {
                axeEffectSound();
                break;
            }
            case "WhiteWizard":
            case "GoblinArcher":
            {
                staffEffectSound();
                break;
            }
            default:
            {
                enemyEffectSound();
                break;
            }
        }
            
    }
    public void clickEffectSound()
    {
        if (effectSources[0].isPlaying)
            effectSources[0].Stop();
        if (effectSources[0].clip != null)
        {
            effectSources[0].clip = null;
        }
        effectSources[0].clip = click;
        effectSources[0].Play();
    }
    public void swordEffectSound()
    {
        if (effectSources[1].isPlaying)
            effectSources[1].Stop();
        if (effectSources[1].clip != null)
        {
            effectSources[1].clip = null;
        }
        effectSources[1].clip = sword;
        effectSources[1].Play();
    }
    public void axeEffectSound()
    {
        if (effectSources[2].isPlaying)
            effectSources[2].Stop();
        if (effectSources[2].clip != null)
        {
            effectSources[2].clip = null;
        }
        effectSources[2].clip = axe;
        effectSources[2].Play();
    }
    public void staffEffectSound()
    {
        if (effectSources[3].isPlaying)
            effectSources[3].Stop();
        if (effectSources[3].clip != null)
        {
            effectSources[3].clip = null;
        }
        effectSources[3].clip = staff;
        effectSources[3].Play();
    }
    public void enemyEffectSound()
    {
        if (effectSources[4].isPlaying)
            effectSources[4].Stop();
        if (effectSources[4].clip != null)
        {
            effectSources[4].clip = null;
        }
        effectSources[4].clip = enemy;
        effectSources[4].Play();
    }
    public void SkillEffectSound()
    {
        if (effectSources[5].isPlaying)
            effectSources[5].Stop();
        if (effectSources[5].clip != null)
        {
            effectSources[5].clip = null;
        }
        effectSources[5].clip = skill;
        effectSources[5].Play();
    }
}
