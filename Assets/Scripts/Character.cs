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
    public AudioSource source;
    public AudioSource falseSource;

    public AudioClip falseDeath;

    public AudioClip trueDeath;
   // public AudioClip truesie;
    public Vector3 PlayerPosition => transform.position; // Assuming the character has a Transform component
    
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = GetComponent<Camera>();
        if (source == null)
        Debug.LogError("AudioSource 'source' is not assigned in the Inspector!");
        source = FindAnyObjectByType<AudioSource>();
        falseSource = FindAnyObjectByType<AudioSource>();

    if (falseSource == null)
        Debug.LogError("AudioSource 'falseSource' is not assigned in the Inspector!");
    }
    // Start is called before the first frame update
    void Start()
{
    source.clip = trueDeath;
    falseSource.clip = falseDeath;
    
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

     void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("NHE"))
    {
        if (source != null)
            StartCoroutine(TheBnnuyCapture(source));
        else
            Debug.LogWarning("AudioSource 'source' is not assigned!");

        DeathLoop.ExecuteTrueDeath();
    }
    else if (other.CompareTag("Enemy"))
    {
        if (falseSource != null)
            StartCoroutine(Ouchie(falseSource));
        else
            Debug.LogWarning("AudioSource 'falseSource' is not assigned!");

        DeathLoop.ExecuteFalseDeath();
    }
}

public IEnumerator TheBnnuyCapture(AudioSource source)
{
    //yield return new WaitForSeconds(3); // Optional delay
    if (source.clip != null)
    {
        source.Play();
        while (source.isPlaying)
        {
            yield return null; // Wait until the clip finishes playing
        }
    }
    else
    {
        Debug.LogWarning("No AudioClip assigned to 'source'!");
    }
}

public IEnumerator Ouchie(AudioSource falseSource)
{
    //yield return new WaitForSeconds(3); // Optional delay
    if (falseSource.clip != null)
    {
        falseSource.Play();
        while (falseSource.isPlaying)
        {
            yield return null; // Wait until the clip finishes playing
        }
    }
    else
    {
        Debug.LogWarning("No AudioClip assigned to 'falseSource'!");
    }
}
}
