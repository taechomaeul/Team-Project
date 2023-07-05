using System.Collections;
using UnityEngine;

public class EffectAndSoundPlay : MonoBehaviour
{
    [Tooltip("재생될 이펙트들")]
    [SerializeField] GameObject[] effects;

    [Tooltip("재생될 사운드들")]
    [SerializeField] AudioClip[] sounds;

    [Tooltip("재생 선딜레이")]
    [SerializeField] float[] delays;

    // 사운드 재생을 위한 오디오 소스
    //private AudioSource audioSource;



    //private void Awake()
    //{
    //    // SFX 오디오 소스 초기화
    //    audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
    //}

    private void OnEnable()
    {
        // 이전 코루틴 중지
        StopAllCoroutines();
        // 모든 이펙트 off
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
        // 이펙트 재생
        StartCoroutine(TurnOn());
    }

    /// <summary>
    /// 설정한 이펙트, 사운드 순차적으로 재생
    /// </summary>
    IEnumerator TurnOn()
    {
        for (int i = 0; i < delays.Length; i++)
        {
            // 선딜레이만큼 기다림
            yield return new WaitForSeconds(delays[i]);
            // 이펙트 재생
            effects[i].SetActive(true);
            // 사운드 재생
            //audioSource.PlayOneShot(sounds[i]);
            SoundManager.Instance.GetSFX().PlayOneShot(sounds[i]);

            // 마지막 이펙트라면
            if (i == delays.Length - 1)
            {
                // 이펙트 재생 길이 만큼 기다림
                yield return new WaitForSeconds(effects[i].GetComponent<ParticleSystem>().main.duration);
                // 이펙트 종료
                gameObject.SetActive(false);
            }
        }
    }
}
