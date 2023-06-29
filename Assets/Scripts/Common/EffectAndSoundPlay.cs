using System.Collections;
using UnityEngine;

public class EffectAndSoundPlay : MonoBehaviour
{
    [SerializeField] GameObject[] effects;

    [SerializeField] AudioClip[] sounds;

    [SerializeField] float[] delays;

    [SerializeField] AudioSource audioSource;

    private void OnEnable()
    {
        StopAllCoroutines();
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
        StartCoroutine(TurnOn());
    }

    IEnumerator TurnOn()
    {
        for (int i = 0; i < delays.Length; i++)
        {
            yield return new WaitForSeconds(delays[i]);
            effects[i].SetActive(true);
            audioSource.PlayOneShot(sounds[i]);
            if (i == delays.Length - 1)
            {
                yield return new WaitForSeconds(effects[i].GetComponent<ParticleSystem>().main.duration);
                gameObject.SetActive(false);
            }
        }
    }
}
