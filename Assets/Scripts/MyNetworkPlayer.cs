using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar] [SerializeField] private string displayName = "Missing name";
    [SyncVar] [SerializeField] private Color playerColor = Color.black;

    [Server]
    public void SetDisplayName(string name) 
    {
        displayName = name;
    }

    [Server]
    public void SetPlayerColor(Color c)
    {
        playerColor = c;
    }

}
