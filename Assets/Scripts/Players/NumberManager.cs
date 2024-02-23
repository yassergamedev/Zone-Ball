using UnityEngine;

public class NumberManager : MonoBehaviour
{
    // Public array of sprites for each digit (0 to 9)
    public Sprite[] digitSprites;

    // Reference to the first and second digit GameObjects
    public GameObject firstDigitObject;
    public GameObject secondDigitObject;

    void Start()
    {
        // Set the sprites for the first and second digits based on the player's index
        SetDigitSprites();
    }

    void SetDigitSprites()
    {
        // Get the index of the player in the team
        int playerIndex = transform.GetSiblingIndex(); // Assuming players are ordered in the hierarchy

        // Calculate the tens and units digits of the player's number
        int tensDigit = playerIndex / 10; // Calculate tens digit
        int unitsDigit = playerIndex % 10; // Calculate units digit

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
