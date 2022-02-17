using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;


    [SyncVar(hook = nameof(HandleDisplayNameUpdated))] //SyncVar will mark variable to be updated on all clients when it is modified
    [SerializeField] private string displayName = "Missing name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))] //hook will call the given method when the value changes
    [SerializeField] private Color playerColor = Color.black;

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

    //To update the color of a player
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

}
