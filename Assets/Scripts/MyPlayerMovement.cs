using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class MyPlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;

    private Camera mainCamera;


//Server run procedures
#region Server

    [Command]
    private void CmdMove(Vector3 pos)
    {
        //Validate Client request
        if (!NavMesh.SamplePosition(pos, out NavMeshHit hit, 1f, NavMesh.AllAreas)) 
            return;

        //Set client agent position
        agent.SetDestination(hit.position);

        RpcLogMovement(); //Have client log movement made
    }

#endregion

//Client run procedures
#region Client

    //A start methood for only objects this client has authority over
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }


    [ClientCallback] //This attribute makes it so only clients can run the code. Update runs on all machines by default
    private void Update()
    {
        //If the client doesnt have authority on this game object then stop them. Because all clients will run this on other clients game objects
        if (!hasAuthority) { return; }

        if (!Input.GetMouseButtonDown(1)) { return; } //If no click was made

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); //Get position cursor hits

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; } //Check if mouse hits something

        CmdMove(hit.point); //Have server move player after validating
    }

    [ClientRpc]
    private void RpcLogMovement()
    {
        Debug.Log("Moving Player");
    }

    #endregion
}
