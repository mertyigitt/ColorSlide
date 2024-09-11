using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] float lookRadius = 27f;
    [SerializeField] float stopRadius = 6f;
    [SerializeField] private ParticleSystem destroyStickmanParticle, destroyBossParticle;
    [SerializeField] private int health;
    [SerializeField] private TextMeshPro bossHealthText;
    [SerializeField] Transform target;

    #endregion

    #region Private Variables

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private bool _isStop;
    private bool _isDead;

    #endregion

    #endregion
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        if (health <= 0)
        {
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _isDead = true;
            if (_isDead)
            {
                StartCoroutine(WaitTime(target.gameObject.GetComponent<PlayerController>()));
            }
        }
        else
        {
            bossHealthText.text = health.ToString();
            var distance = Vector3.Distance(target.position , gameObject.transform.position);
            var targetPos = new Vector3(target.position.x, target.position.y, target.position.z + 5);
            if(distance < lookRadius && _isStop == false)
            {
                _navMeshAgent.SetDestination(targetPos);
                _animator.SetBool("walk", true);
            }
            if(distance <= stopRadius)
            {
                _isStop = true;
                _animator.SetBool("walk", false);
                _animator.SetBool("punch", true);
            }
            else
            {
                _isStop = false;
            }
        }
    }

    public void BossColliderActive()
    {
        if (UIManager.Instance.vibrateOn)
        {
            Vibration.Vibrate(200);
        }
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    
    public void BossColliderDeactive()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stickman"))
        {
            destroyStickmanParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            other.transform.parent.GetComponent<PlayerController>().Stickmans.Remove(other.gameObject);
            StickmanObjectPooling.Instance.SetPool(other.gameObject);
            Instantiate(destroyStickmanParticle, other.transform.position, Quaternion.identity);
            health--;
        }
    }
    
    IEnumerator WaitTime(PlayerController playerController)
    {
        health = 1;
        _isDead = false;
        Instantiate(destroyBossParticle, transform.position, Quaternion.identity);
        UIManager.Instance.StartCoroutine(UIManager.Instance.BossDie(playerController.Stickmans.Count * 2 + UIManager.Instance.extraCoinBonus));
        Destroy(gameObject);
        yield return null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}
