using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine fadeCoroutine;
    public float fadeDuration = 1f;
    public float targetVolume = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            StartFade(targetVolume);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(0f);
        }
    }

    void StartFade(float newVolume)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAudio(newVolume));
    }

    System.Collections.IEnumerator FadeAudio(float newVolume)
    {
        float currentTime = 0f;
        float startVolume = audioSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, newVolume, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.volume = newVolume;

        if (newVolume == 0f)
            audioSource.Stop();
    }
}
