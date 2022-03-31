using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scr_Menu : MonoBehaviour
{
    public GameObject Input;
    public GameObject Registration;
    public GameObject Menu_start;
    public GameObject About;

    public void Input_in()
    {
        Input.SetActive(true);
    }
    public void Input_out()
    {
        Input.SetActive(false);
    }

    public void Registration_in()
    {
        Registration.SetActive(true);
    }
    public void Registration_out()
    {
        Registration.SetActive(false);
    }

    public void Menu_start_in()
    {
        Menu_start.SetActive(true);
    }
    public void Menu_start_out()
    {
        Menu_start.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void About_in()
    {
        About.SetActive(true);
    }
    public void About_out()
    {
        About.SetActive(false);
    }







   

    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void Sound ()
    {
        AudioListener.pause = !AudioListener.pause;
    }
}
