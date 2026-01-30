using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameStartButton : MonoBehaviour
{
    public GameObject startButton;

    public void StartGame()
    {
        startButton.SetActive(false);

        SceneManager.LoadScene("SampleScene");
    }
}
