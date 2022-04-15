using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scr_Menu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void Sound()
    {
        AudioListener.pause = !AudioListener.pause;
    }
    public void HideObject(GameObject Object)
    {
        Object.SetActive(false);
    }
    public void ShowObject(GameObject Object)
    {
        Object.SetActive(true);
    }
    public void exit()
    {
        Application.Quit();
    }
}
