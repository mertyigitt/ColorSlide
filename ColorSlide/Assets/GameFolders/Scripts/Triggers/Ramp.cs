using DG.Tweening;
using UnityEngine;

public class Ramp : Obstacles
{
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (obstacleMaterial.color == other.GetComponent<Renderer>().material.color)
        {
            other.transform.DOJump(
                    new Vector3(other.transform.position.x, other.transform.position.y,other.transform.position.z + 20), 5f, 1, 1f).SetEase(Ease.Flash); 
        }
    }
}
