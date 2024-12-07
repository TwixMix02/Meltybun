using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathClips : MonoBehaviour
{

    public AudioSource fakeDeath;
    public AudioSource trueDeath;
    // Start is called before the first frame update
    void Start()
    {
    }

     public void deathSound(bool state){
        if (!state) {
            fakeDeath.Play();
        }else{
            trueDeath.Play();
        }
    }
}
