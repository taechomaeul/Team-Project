using UnityEngine;

public class PlayerEffectAndSoundControll : MonoBehaviour
{
    [Header("재생할 이펙트 오브젝트")]
    [Tooltip("스킬")]
    [SerializeField] private GameObject effectSkill;

    [Tooltip("트레일 랜더러")]
    [SerializeField] private GameObject trailRenderer;

    public void SetTrail()
    {
        trailRenderer = transform.GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public GameObject GetTrail()
    {
        if (trailRenderer != null)
        {
            return trailRenderer;
        }
        else
        {
            return null;
        }
    }

    internal void TurnOnEffectAttack()
    {
        if (trailRenderer != null)
        {
            Debug.Log("트레일 끔");
            trailRenderer.GetComponent<TrailRenderer>().Clear();
            trailRenderer.SetActive(true);
        }
    }

    internal void TurnOffEffectAttack()
    {
        if (trailRenderer != null)
        {
            trailRenderer.SetActive(false);
        }
    }

    internal void TurnOnEffectSkill()
    {
        if (effectSkill != null)
        {
            effectSkill.SetActive(true);
        }
    }
}
