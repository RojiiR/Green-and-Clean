using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic1 : MonoBehaviour
{
    public GameObject panelMainMenu;      // Panel utama (Start, How To Play, About, Quit)
    public GameObject panelHowToPlay;     // Panel How To Play
    public GameObject panelAbout;         // Panel About

    // ==== MAIN MENU ====
    public void NavigasiStartGame()
    {
        SceneManager.LoadScene("BasicMaze");
    }

    public void NavigasiHowToPlay()
    {
        panelMainMenu.SetActive(false);
        panelHowToPlay.SetActive(true);
    }

    public void NavigasiAbout()
    {
        panelMainMenu.SetActive(false);
        panelAbout.SetActive(true);
    }

    public void NavigasiQuit()
    {
        Application.Quit();
    }

    // ==== BACK BUTTON ====
    public void NavigasiBackToMainMenu()
    {
        panelHowToPlay.SetActive(false);
        panelAbout.SetActive(false);
        panelMainMenu.SetActive(true);
    }
}
