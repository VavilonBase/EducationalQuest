using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scr_Menu : MonoBehaviour
{
    public GameObject Main_menu;
    public GameObject Menu_Student;
    public GameObject Menu_Teacher;
    public GameObject Opt;
    public GameObject BackGround;
    public GameObject EditWindow;
    public GameObject Canvas;
    public GameObject School;

    public void opt () {
       Opt.SetActive(true);
    }
    public void student()
    {
        Main_menu.SetActive(false);
        Menu_Student.SetActive(true);
    }
    public void teacher()
    {
        Main_menu.SetActive(false);
        Menu_Teacher.SetActive(true);
    }
    public void close_opt()
    {
        Opt.SetActive(false);
    }
    public void exit()
    {
        Application.Quit();
    }
    public void exit_menu_st()
    {
        Menu_Student.SetActive(false);
        Main_menu.SetActive(true);
    }
    public void exit_menu_tech()
    {
        Menu_Teacher.SetActive(false);
        Main_menu.SetActive(true);
    }
    public void Game()
    {
        this.Canvas.SetActive(true);
        this.School.SetActive(true);
        BackGround.SetActive(false);
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

    public void ShowEditWindow()
    {
        this.EditWindow.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
