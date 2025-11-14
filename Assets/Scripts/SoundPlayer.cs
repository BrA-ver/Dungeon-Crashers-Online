using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;

    public void PlayRandomSound()
    {
        // 1. Pick a sound at random to play
        int chosenSound = Random.Range(0, sounds.Length);
        AudioClip sound = sounds[chosenSound];

        audioSource.pitch = Random.Range(0.95f, 1.5f);

        // 2. Assign the sound to the audio source
        audioSource.PlayOneShot(sound);
    }
}
