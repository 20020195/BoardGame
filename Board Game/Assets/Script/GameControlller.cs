using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlller : MonoBehaviour
{
    public Button startButton;

    public void OnStartButtonClick(){
        SceneManager.LoadScene("GamePlay");
    }
}
