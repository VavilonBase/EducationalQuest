using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scr_Menu : MonoBehaviour
{
    public GameObject Input;
    public GameObject Registration;
    public GameObject Menu_start;
    public GameObject Menu_Srudent;
    public GameObject Menu_Teacher;
    public GameObject Menu_Admin;
    public GameObject About;
    public GameObject Opt;
    public GameObject Results_Teacher;
    public GameObject Join_Student;
    public GameObject Results_Student;
    public GameObject All_users_Admin;
    public GameObject Group_list_Teacher;
    public GameObject Create_tasr_Teacher;
    public GameObject Group_Teacher;
    public GameObject Delete_Group_Teacher;
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void Sound()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void Delete_Group_Teacher_in()
    {
        Delete_Group_Teacher.SetActive(true);
    }
    public void Delete_Group_Teacher_out()
    {
        Delete_Group_Teacher.SetActive(false);
    }
    public void Group_Teacher_in()
    {
        Group_Teacher.SetActive(true);
    }
    public void Grou_Teacher_out()
    {
        Group_Teacher.SetActive(false);
    }
    public void Group_list_Teacher_in()
    {
        Group_list_Teacher.SetActive(true);
    }
    public void Group_list_Teacher_out()
    {
        Group_list_Teacher.SetActive(false);
    }
    public void Create_tasr_Teacher_in()
    {
        Create_tasr_Teacher.SetActive(true);
    }
    public void Create_tasr_Teacher_out()
    {
        Create_tasr_Teacher.SetActive(false);
    }
    public void Menu_Srudent_in()
    {
        Menu_Srudent.SetActive(true);
    }
    public void Menu_Srudent_out()
    {
        Menu_Srudent.SetActive(false);
    }
    public void Menu_Teacher_in()
    {
        Menu_Teacher.SetActive(true);
    }
    public void Menu_Teacher_out()
    {
        Menu_Teacher.SetActive(false);
    }
    public void Menu_Admin_in()
    {
        Menu_Admin.SetActive(true);
    }
    public void Menu_Admin_out()
    {
        Menu_Admin.SetActive(false);
    }
    public void Optt_in()
    {
        Opt.SetActive(true);
    }
    public void Opt_out()
    {
        Opt.SetActive(false);
    }

    public void Results_Teacher_in()
    {
        Results_Teacher.SetActive(true);
    }
    public void Results_Teacher_out()
    {
        Results_Teacher.SetActive(false);
    }

    public void Join_Student_in()
    {
        Join_Student.SetActive(true);
    }
    public void Join_Student_out()
    {
        Join_Student.SetActive(false);
    }

    public void Results_Student_in()
    {
        Results_Student.SetActive(true);
    }
    public void Results_Student_out()
    {
        Results_Student.SetActive(false);
    }

    public void All_users_Admin_in()
    {
        All_users_Admin.SetActive(true);
    }
    public void All_users_Admin_out()
    {
        All_users_Admin.SetActive(false);
    }

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
}
