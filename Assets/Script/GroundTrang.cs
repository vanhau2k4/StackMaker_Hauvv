using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrang : MonoBehaviour
{
    public GameObject brike;
    private BoxCollider boxCollider;
    Player player;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            
            if (player.listBricks.Count > 0)
            {
                brike.SetActive(true);
                Invoke(nameof(Delay),0.1f);
            }
        }
    }

    private void Delay()
    {
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
        else
        {
            Debug.LogWarning("BoxCollider is already null!");
        }
    }
}
