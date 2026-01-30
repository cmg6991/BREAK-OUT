using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public StageData[] stages;
    public LevelBrick levelBrickPrefab;

    private int currentStage = 0;
    private LevelBrick currentLevel;

    public int hp = 5;
    public int score = 0;
    //Dictionary<int, int> playerScore = new Dictionary<int, int>();
    public UIManager uiManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        score = 0;
        hp = 5;
        //if (Photon.Pun.PhotonNetwork.IsMasterClient)
        //    LoadStage(currentStage);
        uiManager.UpdateHP(hp);
    }

    public void LoadStage(int stageIndex)
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            return;

        if (currentLevel != null)
            Destroy(currentLevel.gameObject);

        if (stageIndex >= stages.Length)
        {
            Debug.Log("모든 스테이지 완료!");
            return;
        }

        // StageData 패턴 초기화
        stages[stageIndex].InitBrickPattern();

        // LevelBrick을 RoomObject로 생성
        GameObject brickGO = PhotonNetwork.InstantiateRoomObject(
            "Level",
            Vector3.zero,
            Quaternion.identity
        );

        currentLevel = brickGO.GetComponent<LevelBrick>();
        currentLevel.Init(stages[stageIndex]);

        // LevelBrick 생성
        //currentLevel = Instantiate(levelBrickPrefab);
        //currentLevel.Init(stages[stageIndex]);

        uiManager.UpdateStageText(stageIndex+1);
    }

    // 스테이지 클리어 시 호출
    public void NextStage()
    {
        currentStage++;
        LoadStage(currentStage);

        ResetBallAndPaddle();
    }

    public void hpCount()
    {
        hp--;
        uiManager.UpdateHP(hp);
        if (hp <= 0)
        {
            Debug.Log("게임 오버!");
            uiManager.ShowGameOver(score);
        }
        else
        {
            Debug.Log("남은 생명: " + hp);
            //ResetBallAndPaddle();
        }
    }
    //public void AddScore(int amount)
    //{
    //    score += amount;
    //    uiManager.UpdateScore(score);
    //}

    void ResetBallAndPaddle()
    {
        PaddleMove paddle = FindObjectOfType<PaddleMove>();
        if (paddle != null && paddle.GetComponent<Photon.Pun.PhotonView>().IsMine)
            paddle.ResetPaddle();

        // 공 찾기
        Ball ball = FindObjectOfType<Ball>();
        if (ball != null && ball.GetComponent<Photon.Pun.PhotonView>().IsMine)
            ball.ResetBall();   // ← 공을 패들 위에 붙게 하는 부분
    }
}
