using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private EnemyInfo enemyInfo;
    public GameObject Cam;
    Slider hpbar;

    private void Awake()
    {
        enemyInfo = transform.parent.parent.GetComponent<EnemyInfo>();
    }

    private void OnEnable()
    {
        Cam = Camera.main.gameObject;
        hpbar = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        if (enemyInfo.stat.GetIsDead())
        {
            transform.parent.gameObject.SetActive(false);
        }
        transform.LookAt(Cam.transform);

        hpbar.value = (float)enemyInfo.stat.GetCurrentHp() / enemyInfo.stat.GetMaxHp();
    }
}