using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Brick : MonoBehaviourPun
{
    public Action onDestroyed;
    bool isDestroyed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ball"))
        //{
        //    onDestroyed?.Invoke();
        //    StageManager.Instance.AddScore(10);
        //    SoundManager.Instance.PlaySFX("Hit1");
        //    Destroy(gameObject);
        //}
        //if (isDestroyed) return;
        //if (!collision.gameObject.CompareTag("Ball")) return;

        //isDestroyed = true;

        //PhotonView ballView = collision.gameObject.GetComponent<PhotonView>();
        //if (ballView == null) return;

        //int hitterActorNumber = ballView.Owner.ActorNumber;
        //int brickViewID = photonView.ViewID;

        //GameManager.Instance.photonView.RPC(
        //    "RPC_ReportHitMaster",
        //    RpcTarget.MasterClient,
        //    brickViewID,
        //    hitterActorNumber
        //);

        if (isDestroyed) return;
        if (!collision.gameObject.CompareTag("Ball")) return;

        isDestroyed = true;
        onDestroyed?.Invoke();
        PhotonView ballView = collision.gameObject.GetComponent<PhotonView>();

        if (ballView != null)
        {
            int hitterActorNumber = ballView.Owner.ActorNumber;
            int brickViewID = photonView.ViewID;

            GameManager.Instance.photonView.RPC(
                "RPC_ReportHitMaster",
                RpcTarget.MasterClient,
                brickViewID,
                hitterActorNumber
            );
        }
        SoundManager.Instance.PlaySFX("Hit1");

        //PhotonNetwork.Destroy(gameObject);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Destroy(gameObject);
        //}
    }
}
