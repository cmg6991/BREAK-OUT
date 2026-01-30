using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Header("Spawn")]
    public Transform spawnParent; // SpawPointGroup

    Dictionary<int, int> scores = new Dictionary<int, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        } 
    }

    private IEnumerator Start()
    {
        // 방에 들어간 상태가 아니라면 실행하지 않음
        if (!PhotonNetwork.InRoom)
            yield return null;
        Debug.Log(
        $"LocalPlayer: {PhotonNetwork.LocalPlayer.NickName} " +
        $"(ActorNumber={PhotonNetwork.LocalPlayer.ActorNumber})");

        Debug.Log("GameManager Start - In Room");

        InitScore();
        SpawnPlayer();
        // 스테이지 로드는 마스터만
        if (PhotonNetwork.IsMasterClient)
        {
            StageManager.Instance.LoadStage(0);
        }

        // 각 클라이언트는 자기 자신을 스폰
    }

    // 플레이어가 룸에 들어왔을 때
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
    //    Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

    //    foreach (var player in PhotonNetwork.CurrentRoom.Players)
    //    {
    //        Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
    //    }

    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        StageManager.Instance.LoadStage(0);
    //    }

    //    SpawnPlayer();

    //}

    public void SpawnPlayer()
    {
        // 스폰 포인트 가져오기
        Transform[] points = spawnParent.GetComponentsInChildren<Transform>();
        List<Transform> spawnList = new List<Transform>();
        foreach (var t in points)
        {
            if (t != spawnParent.transform) spawnList.Add(t);
        }

        // 안정적인 스폰 인덱스
        int actorIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        int idx = actorIndex % Mathf.Max(1, spawnList.Count);
        Transform spawn = spawnList[idx];

        // 패들과 공 생성
        GameObject paddle = PhotonNetwork.Instantiate("Paddle", spawn.position, spawn.rotation, 0);
        GameObject ball = PhotonNetwork.Instantiate("Ball", spawn.position + Vector3.up * 0.5f, Quaternion.identity, 0);

        // 로컬 소유라면 조작 가능
        PhotonView pvPaddle = paddle.GetComponent<PhotonView>();
        PhotonView pvBall = ball.GetComponent<PhotonView>();

        if (pvPaddle != null && pvPaddle.IsMine)
        {
            PaddleMove pm = paddle.GetComponent<PaddleMove>();
            pm.canMove = true;

            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null) ui.paddle = pm;

            // Ball이 패들을 따라가도록 등록
            Ball b = ball.GetComponent<Ball>();
            if (b != null) b.paddle = pm.transform;
        }
    }

    // 벽돌 충돌 보고 (Ball에서 호출)
    [PunRPC]
    public void RPC_ReportHitMaster(int brickViewID, int hitterActorNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonView brickView = PhotonView.Find(brickViewID);
        if (brickView == null) return;

        // 점수 증가 RPC -> 각 클라이언트 StageManager AddScore 호출
        photonView.RPC("RPC_AddScore", RpcTarget.All, hitterActorNumber, 20);

        // 벽돌 제거
        PhotonNetwork.Destroy(brickView.gameObject);
    }

    [PunRPC]
    void RPC_AddScore(int actorNumber, int amount)
    {
        //// 로컬 플레이어만 StageManager에서 점수 증가
        //if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        //{
        //    UIManager ui = FindObjectOfType<UIManager>();
        //    ui.UpdateScore(amount);
        //}

        if (!scores.ContainsKey(actorNumber))
            scores[actorNumber] = 0;

        scores[actorNumber] += amount;

        // UI 갱신
        UIManager ui = FindObjectOfType<UIManager>();
        ui.UpdateScore(scores);

    }

    void InitScore()
    {
        scores.Clear();

        foreach (var p in PhotonNetwork.CurrentRoom.Players)
        {
            int actorNumber = p.Value.ActorNumber;
            scores[actorNumber] = 0;
        }
    }
}
