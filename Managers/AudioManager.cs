using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour, IGameManager {
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;
    [SerializeField] private AudioSource music2Source;

    private AudioSource activeMusic;
    private AudioSource inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool crossFading;

    public ManagerStatus status { get; private set; }

    public float SoundVolume {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    public bool SoundMute {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    private float musicVolume;
    public float MusicVolume {
        get { return musicVolume; }
        set {
            musicVolume = value;

            if(music1Source != null) {
                music1Source.volume = musicVolume;
                music2Source.volume = musicVolume;
            }
        }
    }

    public bool MusicMute {
        get {
            if(music1Source != null) {
                return music1Source.mute;
            }

            return false;
        }

        set {
            if(music1Source != null) {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

    public void Startup() {
        Debug.Log("Audio manager starting...");

        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        SoundVolume = 1f;
        MusicVolume = 1f;

        activeMusic = music1Source;
        inactiveMusic = music2Source;

        status = ManagerStatus.Started;
    }

    public void PlaySound(AudioClip clip) {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic() {
        PlayMusic(Resources.Load($"Music/{introBGMusic}") as AudioClip);
    }

    public void PlayLevelMusic() {
        PlayMusic(Resources.Load($"Music/{levelBGMusic}") as AudioClip);
    }

    private void PlayMusic(AudioClip clip) {
        if(crossFading) {
            return;
        }
        StartCoroutine(CrossFadeMusic(clip));
    }

    private IEnumerator CrossFadeMusic(AudioClip clip) {
        crossFading = true;

        inactiveMusic.clip = clip;
        inactiveMusic.volume = 0;
        inactiveMusic.Play();

        var scaledRate = crossFadeRate * musicVolume;

        while(activeMusic.volume > 0) {
            activeMusic.volume -= scaledRate * Time.deltaTime;
            inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null;
        }

        var temp = activeMusic;

        activeMusic = inactiveMusic;
        activeMusic.volume = musicVolume;

        inactiveMusic = temp;
        inactiveMusic.Stop();

        crossFading = false;
    }

    public void StopMusic() {
        activeMusic.Stop();
        inactiveMusic.Stop();
    }
}