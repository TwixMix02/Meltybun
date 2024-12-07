using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Public fields to assign songs via the Inspector
    [Header("Assign Songs in the Inspector")]
    public AudioClip song1;
    public AudioClip song2;

    // Dictionary to map song names to AudioClips
    private Dictionary<string, AudioClip> songLibrary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        // Ensure the AudioSource is attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Populate the song library
        if (song1 != null) songLibrary.Add("Song1", song1);
        if (song2 != null) songLibrary.Add("Song2", song2);
    }

    // Function to play a specific song by name
    public void PlaySong(string songName)
    {
        if (songLibrary.ContainsKey(songName))
        {
            // Stop the current song if it's playing
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Assign the new clip and play it
            audioSource.clip = songLibrary[songName];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Song not found in the library: " + songName);
        }
    }

    // Function to stop the current song
    public void StopSong()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
