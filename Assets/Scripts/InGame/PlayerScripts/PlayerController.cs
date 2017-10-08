﻿using ClientServerSharedGameObjectMessages;
using System;
using UnityEngine;

/// <summary>
/// Controls a single player
/// </summary>
public class PlayerController
{
    private GameObject playerGmj;
    private UnityPlayerData generalPlayerData;
    private UnityPlayerData playerData;
    private Vector3 targetPos;
    int GMJGUID;
    int OwnerID;

    public PlayerController(GameObject playerGmj, Message_ServerCommand_CreateGameObject info, UnityPlayerData generalPlayerData)
    {
        this.playerGmj = playerGmj;
        this.generalPlayerData = generalPlayerData;
        GMJGUID = info.GmjGUID;
        OwnerID = info.OwnerGUID;
        playerGmj.transform.position = new Vector3(info.transform.xPos, info.transform.yPos, info.transform.zPos);
    }

    internal GameObject GetGmj()
    {
        return playerGmj;
    }

    internal void SetTargetPos(Vector3 vector3)
    {
        targetPos = vector3;
    }
    internal void SetCurrentPos(Vector3 vector3)
    {
        playerGmj.transform.position = vector3;
    }

    internal void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray mouseRay = InGameWrapper.instance.camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, 100f, generalPlayerData.groundMask))
            {
                Debug.DrawLine(playerGmj.transform.position, hit.point);
                targetPos = new Vector3(hit.point.x, playerGmj.transform.position.y, hit.point.z);
                var msg = new Message_Command_PlayerMovementUpdate()
                {
                    currentXPos = playerGmj.transform.position.x,
                    currentZPos = playerGmj.transform.position.z,
                    GMJGUID = GMJGUID,
                    moveTargetXPos = targetPos.x,
                    moveTargetZPos = targetPos.z
                };
                Match_DotNetAdapter.instance.Send(msg);
            }
        }
    }

    public void Update(float deltaTime)
    {
        var pos = playerGmj.transform.position;
        Vector3 direction = targetPos - playerGmj.transform.position;
        direction.Normalize();
        var proposedPosition = pos + direction * Time.deltaTime;
        var finalPos = proposedPosition;
        playerGmj.transform.position = finalPos;
    }
}