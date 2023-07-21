using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject panel;
    public Button ResumeButton;
    public Button ReplayButton;
    public Button ExitButton;
    public Slider slider;
    private float value;

    void Start (){
        slider.onValueChanged.AddListener((value) => {
            if (value == 1) {
                panel.SetActive(true);
                slider.interactable = false;
            }
        });
    }

    public void OnResumeButtonCLick(){
        panel.SetActive(false);
        slider.interactable = true;
        slider.value = 0;
    }

    public void OnReplayButtonClick(){
        SceneManager.LoadScene("GamePlay");
    }

    public void OnExitButtonClick(){
        SceneManager.LoadScene("Menu");
    }
}
