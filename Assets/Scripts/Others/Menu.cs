using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public AudioMixer audioMixer;
    public GameObject musicButton;
    public GameObject muteButton;
    public Slider slider;

    private float musicVolume;
    private float preVolume;

    void Update()
    {
        audioMixer.GetFloat("MainVolume", out musicVolume);
        if (slider != null)
        {
            slider.value = musicVolume;
        }

        if (musicButton != null || muteButton != null)
        {
            if (musicVolume <= -80)
            {
                musicButton.SetActive(false);
                muteButton.SetActive(true);
            }
            else
            {
                musicButton.SetActive(true);
                muteButton.SetActive(false);
            } 
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }

    public void MuteMusic()
    {
        audioMixer.GetFloat("MainVolume", out preVolume);
        audioMixer.SetFloat("MainVolume", -80f);
    }

    public void RecoverMusic()
    {
        audioMixer.SetFloat("MainVolume", preVolume);
    }
    
}
