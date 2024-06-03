using UnityEngine;
using UnityEngine.UI;

public class NumberValidation : MonoBehaviour
{
    public InputField numberInput;
    public Text Num;
    public bool isOff;
    public bool isDepthRelated = false;
    public int minValue = 4;
    public int maxValue = 8;
    private int oldestValue = 0;
    CurrentGame currGame;
    private void Start()
    {
        FileDataHandler<CurrentGame> gameDataHandler = new(Application.persistentDataPath, "Current Game");
        currGame = gameDataHandler.Load();
        minValue = currGame.minPlays;
        maxValue = currGame.maxPlays;
        // Subscribe to the onEndEdit event
        numberInput.onEndEdit.AddListener(OnEndEditHandler);
        if(isDepthRelated)
        {
            if (isOff)
            {
                Num = GameObject.FindGameObjectWithTag("Opp").GetComponent<Text>();
            }
            else
            {
                Num = GameObject.FindGameObjectWithTag("Ball").GetComponent<Text>();
            }

            // Initialize oldestValue to the default input value
            oldestValue = int.Parse(numberInput.text);
            Num.text = (int.Parse(Num.text) - int.Parse(numberInput.text)).ToString();
        }
      
    }

    private void OnDestroy()
    {
        // Unsubscribe from the onEndEdit event to prevent memory leaks
        numberInput.onEndEdit.RemoveListener(OnEndEditHandler);
    }

    private void OnEndEditHandler(string value)
    {
        // Parse the input value to an integer
        int inputValue;
        if (int.TryParse(value, out inputValue))
        {
            int clampedValue;

            // Check the input value and clamp or keep it accordingly
            if (inputValue == 0)
            {
                clampedValue = 0; // Allow 0
            }
            else if (inputValue < minValue && inputValue > 0)
            {
                clampedValue = minValue; // Clamp to minValue if it's lower than minValue but greater than 0
            }
            else
            {
                clampedValue = Mathf.Clamp(inputValue, minValue, maxValue); // Clamp within range minValue to maxValue
            }

            // Update the input field text if the value was clamped
            if (clampedValue != inputValue)
            {
                numberInput.text = clampedValue.ToString();
            }

            if (isDepthRelated)
            {
                Num.text = (int.Parse(Num.text) + oldestValue - clampedValue).ToString();
            }

            // Update oldestValue
            oldestValue = clampedValue;
        }
        else
        {
            // Reset the input field text to the minimum value if the input is not a valid integer
            numberInput.text = minValue.ToString();
            if (isDepthRelated)
            {
                Num.text = (int.Parse(Num.text) + oldestValue - minValue).ToString();
            }

            // Update oldestValue
            oldestValue = minValue;
        }
    }

}
