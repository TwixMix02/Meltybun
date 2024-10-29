using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Snocc")){
            DeathLoop.ExecuteVictory();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
