using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBrick : MonoBehaviour
{
    public StageData stageData;
    public GameObject bricks;

    int remainingBricks;
    public void Init(StageData data)
    {
        stageData = data;
        remainingBricks = 0;

        if (!PhotonNetwork.IsMasterClient)
            return;

        Vector2Int size = stageData.size;
        Vector2 offset = stageData.offset;
        Gradient gradient = stageData.gradient;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (!stageData.GetBrick(i, j)) continue;

                Vector3 pos = transform.position +
                    new Vector3((size.x - 1) * 0.5f - i * offset.x, j * offset.y, 0);

                GameObject newBrick = PhotonNetwork.InstantiateRoomObject(
                    "Brick",
                    pos,
                    Quaternion.identity
                );

                newBrick.GetComponent<SpriteRenderer>().color =
                    gradient.Evaluate((float)j / (size.y - 1));

                Brick brickScript = newBrick.GetComponent<Brick>();
                brickScript.onDestroyed += OnBrickDestroyed;

                remainingBricks++;

                //if (!stageData.GetBrick(i, j)) continue;

                //GameObject newBrick = Instantiate(bricks, transform);
                //newBrick.transform.position = transform.position +
                //    new Vector3((size.x - 1) * 0.5f - i * offset.x, j * offset.y, 0);
                //newBrick.GetComponent<SpriteRenderer>().color = gradient.Evaluate((float)j / (size.y - 1));
                //Brick brickScript = newBrick.AddComponent<Brick>();
                //brickScript.onDestroyed += OnBrickDestroyed;

                //remainingBricks++;
            }
        }
    }
    private void OnBrickDestroyed()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        remainingBricks--;

        if (remainingBricks <= 0)
        {
            Debug.Log($"Stage {stageData.stageName} Å¬¸®¾î!");
            if(Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                StageManager.Instance.NextStage();
            }
        }
    }
}
