using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phao : MonoBehaviour
{
    public Transform CheckPlayer;
    public float CheckDistancePlayer;
    public LayerMask WhatIsPlayer;

    public GameObject phao1; 
    public GameObject phao2;

    private ParticleSystem phao1System;
    private ParticleSystem phao2System;

    private void Start()
    {
        phao1System = phao1.GetComponent<ParticleSystem>();
        phao2System = phao2.GetComponent<ParticleSystem>();
    }

    public virtual bool IsGroundDetectedPlayer()
    {
        Vector3 boxSize = new Vector3(0.5f, 0.5f, 0.5f); 
        Vector3 boxCenter = CheckPlayer.position + Vector3.back * (CheckDistancePlayer / 2);

        if (Physics.BoxCast(boxCenter, boxSize, Vector3.back, Quaternion.identity, CheckDistancePlayer, WhatIsPlayer))
        {
            if (phao1System != null)
            {
                phao1System.Play();
            }
            if (phao2System != null)
            {
                phao2System.Play();
            }
            return true;
        }

        return false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = CheckPlayer.position + Vector3.back * (CheckDistancePlayer / 2);
        Vector3 boxSize = new Vector3(0.5f, 0.5f, CheckDistancePlayer); 
        Gizmos.DrawWireCube(boxCenter, boxSize);
        /*
        Gizmos.DrawLine(CheckPlayer.position, new Vector3(CheckPlayer.position.x ,
            CheckPlayer.position.y , CheckPlayer.position.z - CheckDistancePlayer));*/

    }

    private void Update()
    {
        IsGroundDetectedPlayer();
        
    }
}
