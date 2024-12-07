using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSign : MonoBehaviour
{
    public AudioSource aas;
    public DeathLoop dl;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Awake(){
        Debug.Log("BEFORE AWAKE: "+ FileManager.saveFile.ToString());
        FileManager.doesExist = FileManager.FileCheck();
        Debug.Log("AFTER AWAKE: "+ FileManager.saveFile.ToString());
    }

    void OnTriggerEnter2D(Collider2D other){
        
        if(other.CompareTag("Snocc")){
            StartCoroutine(YouWon(aas));
            
        }
    }

         public IEnumerator YouWon(AudioSource aas){
        aas.Play();
        yield return new WaitForSeconds(5);
        dl.ExecuteLevelCompletion(DeathLoop.currLevel);
        
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
