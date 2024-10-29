using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    [SerializeField]Camera cam;
    [SerializeField]public float currSpeed;
    [SerializeField]public float topSpeed = 10f;
    [SerializeField]public float gravity = 9.8f;
    [SerializeField]public bool isMoving = false;
    public static bool isBall = false;
    float timer;
    public Vector3 PlayerPosition => transform.position; // Assuming the character has a Transform component
    
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       sr = GetComponent<SpriteRenderer>(); 
       cam = GetComponent<Camera>();
    }

    public float speedUpdate(float currSpeed){
        if((currSpeed < topSpeed) && ((topSpeed - currSpeed) > 1.0f)){
            currSpeed += 1f;
        }
        else if((currSpeed < topSpeed) && !((topSpeed - currSpeed) > 1.0f)){
            currSpeed += (topSpeed - currSpeed)/2.0f;
        }
        else{
            currSpeed = topSpeed;
        }
        return currSpeed;
    }

    public void deAccelerate(){
        // Apply gravity only when grounded or in the context you want (for vertical only)
        if (!isMoving)
        {
            currSpeed = Mathf.Max(0, currSpeed + (-gravity * Time.fixedDeltaTime)); // Prevents negative speed
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void jump(){
        isMoving = true;
        CameraBev.isJumping = true;
        //isBall = true; - Will use post-update
        UnityEngine.Vector3 charVector = new Vector3(0,1,0);
        currSpeed = speedUpdate(currSpeed);
        transform.localPosition += charVector * currSpeed * 0.015f;
        transform.localPosition -= charVector * currSpeed * 0.0015f;
        //isBall = false; - Will use post-update

    }

    void FixedUpdate(){
        timer += Time.fixedDeltaTime;
        deAccelerate();
    }
}
