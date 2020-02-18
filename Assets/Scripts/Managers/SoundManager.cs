using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool musicEnabled = true;

    public bool fxEnabled = true;

    [Range(0, 1)]
    public float musicVolume = 1.0f;

    [Range(0, 1)]
    public float fxVolume = 1.0f;

    public AudioClip clearRowSound;

    public AudioClip moveSound;

    public AudioClip dropSound;

    public AudioClip gameOverSound;

    public AudioClip errorSound;

   // public AudioClip m_bgMusic;

    public AudioSource musicSource;

    public AudioClip[] musicClips;

    private AudioClip randomMusicClip;

    public AudioClip[] vocalClips;

    public AudioClip gameOverVocal;

    public AudioClip levelUpClip;

    public IconToggle musicIconToggle;

    public IconToggle fxIconToggle;

    private void Start()
    {
        randomMusicClip = GetRandomClip(musicClips);
        PlayBackgroundMusic(randomMusicClip);
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

   

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!musicEnabled || !musicClip || !musicSource)
        {
            return;
        }

        musicSource.Stop();

        musicSource.clip = musicClip;

        musicSource.volume = musicVolume;

        musicSource.loop = true;

        musicSource.Play();
    }

    void UpdateMusic()
    {
        if (musicSource.isPlaying != musicEnabled)
        {
            if (musicEnabled)
            {
                randomMusicClip = GetRandomClip(musicClips);
                PlayBackgroundMusic(randomMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateMusic();

        if (musicIconToggle)
        {
            musicIconToggle.ToggleIcon(musicEnabled);
        }
    }

    public void ToggleFX()
    {
        fxEnabled = !fxEnabled;

        if (fxIconToggle)
        {
            fxIconToggle.ToggleIcon(fxEnabled);
        }
    }

}
