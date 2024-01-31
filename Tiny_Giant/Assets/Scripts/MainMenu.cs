using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject volumeSettings;
    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptions()
    {
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        volumeSettings.SetActive(true);
    }

    public void OnBack()
    {
        startButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        volumeSettings.SetActive(false);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
