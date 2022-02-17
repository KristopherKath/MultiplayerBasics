using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("Client connected to a server!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        //conn.identity is the NetworkIdentity component on the player object. This has info on the MyNetworkPlayer component.
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>(); 

        player.SetDisplayName($"Player {numPlayers}");

        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        player.SetPlayerColor(color);

    }


}
