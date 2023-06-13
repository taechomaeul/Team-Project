using UnityEngine;

public class AttackTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBeAttacked>().BeAttacked(10);
        }
    }
}
