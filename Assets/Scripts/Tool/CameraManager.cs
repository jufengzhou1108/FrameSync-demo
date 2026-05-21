using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager :MonoBehaviour
{
    public static CameraManager Instance { get; set; }

    private Transform cameraTrans;
    private Transform playerTrans;

    private void Awake()
    {
        Instance = this;
        cameraTrans = Camera.main.transform;
    }

    public void AddPlayerTrans(Transform transform)
    {
        playerTrans = transform;
    }

    private void LateUpdate()
    {
        if (playerTrans == null)
        {
            return;
        }

        Vector3 aimPosition = new Vector3(playerTrans.position.x, cameraTrans.position.y, playerTrans.position.z);

        cameraTrans.position = aimPosition;
    }
}
