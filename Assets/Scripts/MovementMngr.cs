using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMngr : MonoBehaviour
{
    [SerializeField] Character snoc;
    Rigidbody2D rb;
        // Start is called before the first frame update

    void Start()
    {
        snoc = GetComponent<Character>();
        snoc.currSpeed = 0;
        snoc.topSpeed = 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UnityEngine.Vector3 charVector = Vector3.zero;
        //TODO: Vertical-based movement. Referencing lectures 5-6 for those.
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            //print("Bnnuy wants up! :<");
            charVector += new UnityEngine.Vector3(-1,0,0);
            snoc.isMoving = true;
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            //print("Bnnuy wants down! :<");
            charVector += new UnityEngine.Vector3(1,0,0);
            snoc.isMoving = true;
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
            snoc.jump();
        }
        // Only update speed if the character is moving
        if (snoc.isMoving)
        {
            snoc.currSpeed = snoc.speedUpdate(snoc.currSpeed);
            transform.localPosition += charVector * snoc.currSpeed * Time.fixedDeltaTime;
        }
        if(!Input.GetKeyDown(KeyCode.None)){
        snoc.isMoving = false;
        }
    }

}
