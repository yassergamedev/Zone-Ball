using UnityEngine;

public class NumberManager : MonoBehaviour
{
    // Public array of sprites for each digit (0 to 9)
    public Sprite[] digitSprites;

    // Reference to the first and second digit GameObjects
    public GameObject firstDigitObject;
    public GameObject secondDigitObject;

    // Call this method to set the player's number
    public void SetPlayerNumber(int playerNumber)
    {
        // Get the tens and units digits of the player's number
        int tensDigit = playerNumber / 10; // Calculate tens digit
        int unitsDigit = playerNumber % 10; // Calculate units digit

        // Set the sprites for the first and second digits
        SetDigitSprite(firstDigitObject, tensDigit);
        SetDigitSprite(secondDigitObject, unitsDigit);
    }

    void SetDigitSprite(GameObject digitObject, int digit)
    {
        // Get the sprite renderer component of the digit object
        SpriteRenderer spriteRenderer = digitObject.GetComponent<SpriteRenderer>();

        // Set the sprite based on the digit value
        if (digit >= 0 && digit <= 9)
        {
            spriteRenderer.sprite = digitSprites[digit];
        }
        else
        {
            Debug.LogError("Invalid digit value: " + digit);
        }
    }
}
