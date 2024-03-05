using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionTextManager : MonoBehaviour
{
    public Transform playerHead; // Reference to the player's head position
    public GameObject actionTextObject; // Reference to the action text GameObject

    // Function to display action text above the player's head
    public void ShowActionText(string action)
    {
        // Set the position of the action text above the player's head
        actionTextObject.transform.position = playerHead.position + Vector3.up * 2f; // Adjust the height as needed

        // Set the text content
        Text actionTextComponent = actionTextObject.GetComponent<Text>();
        if (actionTextComponent != null)
        {
            actionTextComponent.text = action;
        }

        // Set the action text object active
        actionTextObject.SetActive(true);

        // Start coroutine to hide the action text after a delay
        StartCoroutine(HideActionText());
    }

    // Coroutine to hide the action text after a delay
    IEnumerator HideActionText()
    {
        // Wait for a few seconds before hiding the action text
        yield return new WaitForSeconds(2f); // Adjust the duration as needed

        // Set the action text object inactive
        actionTextObject.SetActive(false);
    }
}
