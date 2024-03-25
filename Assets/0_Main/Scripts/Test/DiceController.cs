using System.Collections;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private bool isRolling = false;
    private int result = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            StartCoroutine("RollDice");
        }
    }

    IEnumerator RollDice()
    {
        isRolling = true;
        int randomSide = Random.Range(1, 7); // Generate a random number between 1 and 6
        float duration = 0.5f; // Duration of the rolling animation
        float timer = 0f;

        while (timer < duration)
        {
            // Rotate the dice
            transform.Rotate(Vector3.up, 360 * Time.deltaTime / duration);

            timer += Time.deltaTime;
            yield return null;
        }

        // Set the result to the randomly chosen side
        result = randomSide;
        Debug.Log("Dice rolled: " + result);

        // Rotate the dice to display the result
        transform.rotation = Quaternion.Euler(0f, 0f, (result - 1) * 90f);

        isRolling = false;
    }
}
