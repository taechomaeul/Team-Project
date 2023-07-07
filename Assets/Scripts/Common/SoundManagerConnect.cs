using UnityEngine;

public class SoundManagerConnect : MonoBehaviour
{
    private enum BGMType
    {
        Title,
        Field,
        MiniBoss,
        FinalBoss,
        Ending
    }

    [SerializeField] private BGMType bgmType;

    void Start()
    {
        SoundManager.Instance.BGMChangeWithFade((int)bgmType);
    }
}