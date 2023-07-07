using UnityEngine;
using UnityEngine.UI;

public class EnemyBossHpUI : MonoBehaviour
{
    Boss bossInfo;
    private GameObject hpBar;
    private Slider hpSlider;
    private Text bossName;
    private int languageIndex;

    private void Awake()
    {
        hpBar = GameObject.Find("MainCanvas").transform.GetChild(1).gameObject;
        bossInfo = GetComponent<BossInfo>().stat;
        hpSlider = hpBar.GetComponent<Slider>();
        bossName = hpBar.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        BossNameLanguageChange();
    }

    private void LateUpdate()
    {
        if(languageIndex != SettingManager.Instance.GetCurrentLanguageIndex())
        {
            BossNameLanguageChange();
        }

        if (bossInfo.GetIsTracking())
        {
            gameObject.SetActive(true);
        }
        hpSlider.value = (float)bossInfo.GetCurrentHp() / bossInfo.GetMaxHp();
    }

    private void BossNameLanguageChange()
    {
        languageIndex = SettingManager.Instance.GetCurrentLanguageIndex();

        if (languageIndex == 0)
        {
            if (bossInfo.GetBossType() == 0)
            {
                bossName.text = FindObjectOfType<EnemyPrefab>().enemyPrefabNames_KR[4];
            }
            else if (bossInfo.GetBossType() == 1)
            {
                bossName.text = FindObjectOfType<EnemyPrefab>().enemyPrefabNames_KR[5];
            }
        }
        else
        {
            if (bossInfo.GetBossType() == 0)
            {
                bossName.text = FindObjectOfType<EnemyPrefab>().enemyPrefabNames_EN[4];
            }
            else if (bossInfo.GetBossType() == 1)
            {
                bossName.text = FindObjectOfType<EnemyPrefab>().enemyPrefabNames_EN[5];
            }
        }
    }
}
