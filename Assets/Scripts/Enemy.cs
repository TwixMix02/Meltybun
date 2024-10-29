using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public enum enemyState {Static, Aerial, Kinetic};



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Snocc")){
            if(Character.isBall){
                //TODO: Add destroy function (Post-Prototype)
            }
            else{
                DeathLoop.ExecuteFalseDeath();
            }
        }
        else{
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
