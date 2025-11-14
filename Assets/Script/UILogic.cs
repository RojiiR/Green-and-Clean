using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    public GameObject PanelMainMenu;     // Panel utama menu
    public GameObject PanelAboutGame;    // Panel about
    public GameObject PanelHowToPlay;    // Panel how to play

    // === ABOUT ===
    public void NavigasiAboutGame()
    {
        PanelMainMenu.SetActive(false);
        PanelAboutGame.SetActive(true);
    }

    // === HOW TO PLAY ===
    public void NavigasiHowToPlay()
    {
        PanelMainMenu.SetActive(false);
        PanelHowToPlay.SetActive(true);
    }

    // === BACK BUTTON ===
    public void NavigasiBackToMainMenu()
    {
        PanelAboutGame.SetActive(false);
        PanelHowToPlay.SetActive(false);
        PanelMainMenu.SetActive(true);
    }

    // === START GAME ===
    public void NavigasiGamePlay()
    {
        SceneManager.LoadScene("MainScene");
    }

    // === QUIT GAME ===
    public void NavigasiQuit()
    {
        Application.Quit();

        // Agar tombol berfungsi saat testing di editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
