using UnityEngine;

public class BossLineScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.BossLine();
        }
    }
}
