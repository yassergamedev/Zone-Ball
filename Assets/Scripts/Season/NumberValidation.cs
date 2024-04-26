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

    private void Start()
    {
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
            // Allow 0 or clamp the input value to the desired range
            int clampedValue = Mathf.Clamp(inputValue, 0, maxValue);

            // Update the input field text if the value was clamped
            if (clampedValue != inputValue)
            {
                numberInput.text = clampedValue.ToString();
            }
            if(isDepthRelated)
            {
                Num.text = (int.Parse(Num.text) + oldestValue - inputValue).ToString();
            }
            // Update the associated number text
           

            // Update oldestValue
            oldestValue = int.Parse(numberInput.text);
        }
        else
        {
            // Reset the input field text to the minimum value if the input is not a valid integer
            numberInput.text = minValue.ToString();
            if(isDepthRelated)
            {
                Num.text = (int.Parse(Num.text) + oldestValue - minValue).ToString();
            }
            

            // Update oldestValue
            oldestValue = int.Parse(numberInput.text);
        }
    }
}
