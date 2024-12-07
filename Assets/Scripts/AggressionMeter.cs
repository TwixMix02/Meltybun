using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class AggressionMeter : MonoBehaviour
{
    [SerializeField] private GameObject hexagon; // Reference to the hexagon GameObject
    [SerializeField] private float maxScaleX = 0.95f;  // Maximum scale of the bar on X
    [SerializeField] private float minScaleX = 0.005f; // Minimum scale of the bar on X
    [SerializeField] private float startX = -0.4129f;  // Starting X position
    [SerializeField] private float targetX = 0f;       // Target X position
    [SerializeField] public float totalDuration = 300f;
    private float currentScaleX = 0.005f;
    private float currentX = -0.4129f;

    public void StartIncrease()
    {
        StopAllCoroutines();  // Ensure no other coroutines are running
        Debug.Log("INCREASING!");
        StartCoroutine(IncreaseBar());
    }

    public void StartDecrease()
    {
        StopAllCoroutines();  // Ensure no other coroutines are running
        Debug.Log("DECREASING!");
        StartCoroutine(DecreaseBar());
    }

    private IEnumerator IncreaseBar()
    {
        switch(DeathLoop.currLevel){
            case 1:
            totalDuration = 300f;
            break;
            case 2:
            totalDuration = 200f;
            break;
            case 3:
            totalDuration  = 60f;
            break;
            default:
            totalDuration  = 100f;
            break;
        } // 10 seconds to increase the bar (when timeRate is 30)
        float elapsedTime = 0f;

        // Adjust the duration to the actual time rate
        float adjustedTime = totalDuration / DeathLoop.timeRate; 

        while (elapsedTime < adjustedTime)
        {
            elapsedTime += Time.deltaTime; // Don't scale by DeathLoop.timeRate here directly
            float t = elapsedTime / adjustedTime;

            // Interpolate the bar's scale and position
            currentScaleX = Mathf.Lerp(minScaleX, maxScaleX, t);
            currentX = Mathf.Lerp(startX, targetX, t);

            // Apply the updated values to the hexagon GameObject
            hexagon.transform.localScale = new Vector3(currentScaleX, hexagon.transform.localScale.y, hexagon.transform.localScale.z);
            hexagon.transform.localPosition = new Vector3(currentX, hexagon.transform.localPosition.y, hexagon.transform.localPosition.z);

            yield return null;
        }

        // Ensure the bar reaches its final state
        hexagon.transform.localScale = new Vector3(maxScaleX, hexagon.transform.localScale.y, hexagon.transform.localScale.z);
        hexagon.transform.localPosition = new Vector3(targetX, hexagon.transform.localPosition.y, hexagon.transform.localPosition.z);
    }

    private IEnumerator DecreaseBar()
    {
        //yield return new WaitForSeconds(1);
        float totalDuration = 30f; // 1 second to decrease the bar (when timeRate is 30)
        float elapsedTime = 0f;

        // Adjust the duration to the actual time rate
        float adjustedTime = totalDuration / DeathLoop.timeRate;

        while (elapsedTime < adjustedTime)
        {
            elapsedTime += Time.deltaTime; // Don't scale by DeathLoop.timeRate here directly
            float t = elapsedTime / adjustedTime;

            // Interpolate the bar's scale and position
            currentScaleX = Mathf.Lerp(maxScaleX, minScaleX, t);
            currentX = Mathf.Lerp(targetX, startX, t);

            // Apply the updated values to the hexagon GameObject
            hexagon.transform.localScale = new Vector3(currentScaleX, hexagon.transform.localScale.y, hexagon.transform.localScale.z);
            hexagon.transform.localPosition = new Vector3(currentX, hexagon.transform.localPosition.y, hexagon.transform.localPosition.z);

            yield return null;
        }

        // Ensure the bar reaches its final state
        hexagon.transform.localScale = new Vector3(minScaleX, hexagon.transform.localScale.y, hexagon.transform.localScale.z);
        hexagon.transform.localPosition = new Vector3(startX, hexagon.transform.localPosition.y, hexagon.transform.localPosition.z);
    }
}
