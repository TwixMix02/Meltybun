using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Status : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusText;
    private static Status instance;

    // Ensure only one instance exists
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        statusText.text = "";

        if(DeathLoop.isGO){
            statusText.text = "GAME OVER";
        }
        
        if (DeathLoop.isCom){
            statusText.text = "YOU WON!";
        }
    }


    // Access the current instance of Status
    public static Status getStatus()
    {
        return instance;
    }

    // Update the status text in a static context
    public static void setText(string text)
    {
        if (instance != null)
        {
            instance.statusText.text = text;
        }
        else
        {
            Debug.LogWarning("Status instance is not set. Make sure a Status object is in the scene.");
        }
    }

    void Update()
    {
        // TODO: Update the score that is incremented from the call on Carrot's OnTriggerEnter
    }
}
