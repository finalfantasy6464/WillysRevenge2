using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInMusic : MonoBehaviour
{
    GameObject SoundManager;
    MusicManagement music;

    private string musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager = GameObject.Find("SoundManager");
        music = SoundManager.GetComponent<MusicManagement>();
        musicVolume = MusicManagement.MUSIC_VOLUME;

        FadeIn();
    }

    // Update is called once per frame
    void FadeIn()
    {
        if (GameControl.control.faded)
        {
            StartCoroutine(FadeAudioGroup.EndFade(music.audioMixer, musicVolume, 0.5f));
            GameControl.control.faded = false;
        }
    }
}
