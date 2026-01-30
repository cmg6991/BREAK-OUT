using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    readonly string version = "1.0";

    public TMP_InputField idInput;

    private void Awake()
    {
        //if (FindObjectsOfType<PhotonManager>().Length > 1)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
        //PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.GameVersion = version;
        //PhotonNetwork.NickName = userID;

        //Debug.Log(PhotonNetwork.SendRate);
        //PhotonNetwork.ConnectUsingSettings();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
    }

    public void Connect(string userId)
    {
        if (PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.NickName = userId;
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log($"Connecting.. Nickname = {userId}");
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master");
        //Debug.Log($"PhotonNetwork.lobby = {PhotonNetwork.InLobby}");

        //if (PhotonNetwork.IsConnectedAndReady)
        //    PhotonNetwork.JoinLobby();

        //else
        //    StartCoroutine(WaitAndJoinLobby());
        Debug.Log("Connected to Master");

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

    }
    //IEnumerator WaitAndJoinLobby()
    //{
    //    while(!PhotonNetwork.IsConnectedAndReady)
    //        yield return null;
    //    PhotonNetwork.JoinLobby();
    //}

    public override void OnJoinedLobby()
    {
        //Debug.Log($"PhotonNetwork.inlobby = {PhotonNetwork.InLobby}");
        //PhotonNetwork.JoinRandomRoom();

        Debug.Log("Joined Lobby");

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,          // 테스트용
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.JoinOrCreateRoom(
            "AUTO_MATCH_ROOM",       //고정된 방 이름
            options,
            TypedLobby.Default
        );
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"joinrandom filed {returnCode}:{message}");

    //    //RoomOptions ro = new RoomOptions();
    //    //ro.MaxPlayers = 20;
    //    //ro.IsOpen = true;
    //    //ro.IsVisible = true;

    //    //PhotonNetwork.CreateRoom("My Room",ro);
    //    RoomOptions ro = new RoomOptions
    //    {
    //        MaxPlayers = 20,
    //        IsOpen = true,
    //        IsVisible = true
    //    };

    //    PhotonNetwork.CreateRoom(null, ro);
    //}

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Created room");
    //    Debug.Log($"room name = {PhotonNetwork.CurrentRoom.Name}");
    //}

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room : {PhotonNetwork.CurrentRoom.Name}");

        //GameManager.Instance.SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"PhotonNetwork.inroom = {PhotonNetwork.InRoom}");
    //    Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

    //    foreach (var player in PhotonNetwork.CurrentRoom.Players)
    //    {
    //        Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
    //    }

    //    Transform[] points = GameObject.Find("SpawPointGroup").GetComponentsInChildren<Transform>();
    //    int idx = PhotonNetwork.CurrentRoom.PlayerCount;

    //    var paddle = PhotonNetwork.Instantiate("Paddle", points[idx].position, points[idx].rotation, 0);

    //    var move = paddle.GetComponent<PaddleMove>();

    //    if (paddle.GetComponent<PhotonView>().IsMine)
    //    {
    //        move.canMove = true;
    //        UIManager ui = FindObjectOfType<UIManager>();
    //        ui.paddle = move;
    //    }
    //}

    public override void OnLeftRoom()
    {
        //PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("StartScene");
    }

    public void OnLoginClick()
    {
        if (string.IsNullOrEmpty(idInput.text))
            return;
        Connect(idInput.text);
    }

}
