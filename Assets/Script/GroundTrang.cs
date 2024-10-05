using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrang : MonoBehaviour
{
    public GameObject brike;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            brike.SetActive(true);
            Invoke(nameof(Delay),0.1f);  
        }
    }

    private void Delay()
    {
        if (boxCollider != null)
        {
            Destroy(boxCollider); 
        }
    }
}
