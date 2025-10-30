using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> sources;
    public void PlaySoundByKey(int key)
    {
        sources[key].Play();
    }
    public void ToggleLoopingSoundByKey(int key)
    {
        if (sources[key].isPlaying)
        {
            sources[key].Stop();
        }
        else
        {
            sources[key].Play();
        }
    }
}
