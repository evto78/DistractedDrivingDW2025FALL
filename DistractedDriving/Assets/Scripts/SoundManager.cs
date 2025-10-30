using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    List<AudioSource> sources;
    private void Start()
    {
        sources = new List<AudioSource>();
        for(int i = 0; i < transform.childCount; i++)
        {
            sources.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }
    }
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
    public void SetMotorVolAndPitch(float curSpeed, float maxSpeed)
    {
        sources[0].volume = Mathf.Lerp(0.05f, 0.25f, (curSpeed / maxSpeed));
        sources[0].pitch = Mathf.Lerp(0.9f, 1.5f, (curSpeed / maxSpeed));
    }
}
