using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public Image[] lifes;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI otherScoreText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    public PaddleMove paddle;

    public TextMeshProUGUI stageText;

    private void Start()
    {
        //Time.timeScale = 0f;
        if(paddle != null )
            paddle.canMove = true;

        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void UpdateHP(int hp)
    {
        for(int i = 0;i<lifes.Length;i++)
        {
            lifes[i].enabled = (i < hp);
        }
    }

    public void UpdateScore(Dictionary<int, int> playerScores)
    {
        int localActor = PhotonNetwork.LocalPlayer.ActorNumber;

        scoreText.text = "Score: " + playerScores[localActor];

        foreach (var kvp in playerScores)
        {
            if (kvp.Key != localActor)
                otherScoreText.text = "Score: " + kvp.Value;
        }
    }

    public void UpdateStageText(int Number)
    {
        stageText.text = "STAGE " + Number;
    }

    public void ShowGameOver(int currentScore)
    {
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        // 현재 저장된 최고 점수 불러오기
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // 현재 점수가 최고 점수보다 크면 갱신
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // UI 표시
        currentScoreText.text = "Score : " + currentScore;
        highScoreText.text = "High Score : " + highScore;

        if(paddle!=null)
            paddle.canMove = false;
    }
    public void ReStartGame()
    {
        //gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ReStartCoroutine());
    }

    IEnumerator ReStartCoroutine()
    {
        PhotonNetwork.LeaveRoom();

        while(PhotonNetwork.InRoom)
            yield return null;
        Photon.Pun.PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);

    }
}
