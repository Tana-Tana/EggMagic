using System;
using Assets.Scripts.Common;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    [Header("AUDIO SOURCE", order = 0)]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    // public
    public AudioClip audioClipMusic;
    public AudioClip audioClipSound;
    public float saveTime = 0;

    private void Start()
    {
        audioClipMusic = Resources.Load<AudioClip>(GameConfig.AUDIO_PATH + GameConfig.BACKGROUND_AUDIO);
        musicSource.clip = audioClipMusic;
        musicSource.Play();
        LoadAudioSource();
    }

    private void LoadAudioSource()
    {
        if (GamePrefs.GetMusic() == 1)
        {
            musicSource.mute = false;
        }
        else
        {
            musicSource.mute = true;
        }

        if (GamePrefs.GetSound() == 1)
        {
            soundSource.mute = false;
        }
        else
        {
            soundSource.mute = true;
        }
    }

    public void ChangeStatusMusic()
    {
        if (GamePrefs.GetMusic() == 0)
        {
            musicSource.mute = true;
            saveTime = musicSource.time;
        }
        else
        {
            musicSource.time = saveTime;
            musicSource.mute = false;
        }
    }

    public void ChangeStatusSound()
    {
        if (GamePrefs.GetSound() == 0)
        {
            soundSource.mute = true;
        }
        else
        {
            soundSource.mute = false;
        }
    }

    public void PlayOneShotAudio(string nameAudio)
    {
        audioClipSound = Resources.Load<AudioClip>(GameConfig.AUDIO_PATH + nameAudio);
        soundSource.PlayOneShot(audioClipSound);
    }

    public float GetLengthOfClip(string nameAudio)
    {
        audioClipSound = Resources.Load<AudioClip>(GameConfig.AUDIO_PATH + nameAudio);
        return audioClipSound.length;
    }
}
