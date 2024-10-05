using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruong : MonoBehaviour
{
    public Transform CheckPlayer; 
    public float CheckDistancePlayer; 
    public LayerMask WhatIsPlayer;

    public GameObject ruongMo;
    public virtual bool IsGroundDetectedPlayer()
    {

        if (Physics.Raycast(CheckPlayer.position, Vector3.back, CheckDistancePlayer, WhatIsPlayer))
        {

            if (gameObject != null)
            {
                gameObject.SetActive(false);
                Debug.Log("thay");
                ruongMo.gameObject.SetActive(true);
            }
            return true; 
        }

        return false; 
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawLine(CheckPlayer.position, new Vector3(CheckPlayer.position.x,
            CheckPlayer.position.y , CheckPlayer.position.z - CheckDistancePlayer));
    }

    private void Update()
    {
        IsGroundDetectedPlayer();
    }
}
