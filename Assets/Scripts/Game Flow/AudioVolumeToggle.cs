using UnityEngine;

public class AudioVolumeToggle : MonoBehaviour
{
    public GameObject targetGameObject;
    public GameObject Graphic;
    private bool isMuted = false;
    private float[] originalVolumes;

    public void ToggleAudio()
    {
        if (targetGameObject == null)
        {
            Debug.LogError("Target GameObject is not assigned!");
            return;
        }

        AudioSource[] audioSources = targetGameObject.GetComponentsInChildren<AudioSource>();

        if (audioSources.Length == 0)
        {
            Debug.LogWarning("No AudioSource components found on target GameObject or its children.");
            return;
        }

        if (originalVolumes == null)
        {
            originalVolumes = new float[audioSources.Length];
            for (int i = 0; i < audioSources.Length; i++)
            {
                originalVolumes[i] = audioSources[i].volume;
            }
        }

        isMuted = !isMuted;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = isMuted ? 0 : originalVolumes[i];
        }

        if (isMuted)
        {
            Graphic.SetActive(true);

        }
        else
        {
            Graphic.SetActive(false);
        }

        Debug.Log("Audio " + (isMuted ? "muted" : "unmuted"));
    }
}
