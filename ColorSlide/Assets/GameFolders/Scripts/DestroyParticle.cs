using System.Collections;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Destroy(gameObject);
    }
}
