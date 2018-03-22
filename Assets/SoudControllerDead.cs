using UnityEngine;
using System.Collections;

public class SoundControllerDead : MonoBehaviour
{

    // Use this for initialization

    public AudioClip Slider, clickSound, CarCrashSound, counting_Sound, new_Best_Score;
    public AudioClip coinHitSound;
    public AudioClip pickUpSound;

    public static SoundController Static;
    public AudioSource[] audioSources;
    public AudioSource boostAudioControl, car_brake;
    public GameObject BgSoundsObj;
    void Start()
    {
       // Static = this;
        //toStop bg music on mainMenu and Splash Screen
        if (Application.loadedLevel < 2)
        {

            BgSoundsObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayCarCrashSound()
    {
        swithAudioSources(CarCrashSound);
    }
    public void PlayClickSound1()
    {

        swithAudioSources(counting_Sound);
    }
    public void PlayClickSound()
    {

        swithAudioSources(clickSound);
    }

    public void playCoinHit()
    {

        GetComponent<AudioSource>().PlayOneShot(coinHitSound);
    }

    public void PlayPowerPickUp()
    {

        swithAudioSources(pickUpSound);
    }
    public void PlaySlider()
    {

        swithAudioSources(Slider);

    }
    public void playNewBestScoreSound()
    {

        swithAudioSources(new_Best_Score);

    }


    void swithAudioSources(AudioClip clip)
    {
        if (audioSources[0].isPlaying)
        {
            audioSources[1].PlayOneShot(clip);
        }
        else audioSources[0].PlayOneShot(clip);

    }
}
