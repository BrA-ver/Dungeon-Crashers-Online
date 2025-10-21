using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sound[] musicSounds, sfxSounds;

    public AudioSource musicSource, sfxSource;

    
    public void PlayMusic(string name, bool looping = true)
    {
        StopMusic();

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            musicSource.loop = looping;
        }
    }

    // This method just needs the name of the sound it will play
    public void PlaySFX(string name)
    {
        // Get the sound of the song name from the music array. You can use this method `using System`;
        Sound s = Array.Find(sfxSounds, x => x.name == name);


        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    // This method is made just in case you want to stop the music for e.g. cutscenes
    public void StopMusic()
    {
        // Tell the music audio source to stop if it is playing
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}

// Make the class serializable so we can edit it's values 
[System.Serializable]
public class Sound
{
    // We need to give a name to the sound clip we will be playing and have that clip avaialable
    public string name;
    public AudioClip clip;
}