using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;


    [SyncVar(hook = nameof(HandleDisplayNameUpdated))] //SyncVar will mark variable to be updated on all clients when it is modified.
    [SerializeField] private string displayName = "Missing name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))] //hook will call the given method when the value changes on each client.
    [SerializeField] private Color playerColor = Color.black;

#region ServerLogic

    //Flow for CMD once CmdSetDisplayName is called: -- 2 --
    [Server] //Server attribute makes it so only server machines can run these methods. No client can run this.
    public void SetDisplayName(string name) 
    {
        //Validation can be done here. If done it will validate what is sent to each client and the server itself.

        displayName = name;
    }

    [Server]
    public void SetPlayerColor(Color c)
    {
        playerColor = c;
    }


    //Flow for CMD once CmdSetDisplayName is called: -- 1 --
    //This is a client called function, where it is run on the server
    [Command] //Command is an attribute that marks a method so that it can be called by a client. The server then will run its own logic
    private void CmdSetDisplayName(string newDisplayName)
    {
        //Do server validation here! If done here it validates what client says.
        if (newDisplayName.Length < 2 || newDisplayName.Length > 12)
            return;

        RpcLogNewName(newDisplayName); //This is a ClientRpc method. So each client will run the methods logic to log the name

        //This will set the display name and update the necesary fields on each client
        SetDisplayName(newDisplayName); //Run the server only method
    }

#endregion



#region ClientLogic

    //To update the color of a player
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    //Flow for CMD once CmdSetDisplayName is called: -- 3 --
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("SetMyName")]
    private void SetMyName()
    {
        CmdSetDisplayName("My new name wowow!");
    }

    [ClientRpc] //This attribute has the server send a request to each client to run the following method
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

#endregion



}
