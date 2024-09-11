using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] protected Material obstacleMaterial;
    [SerializeField] protected ParticleSystem destroyParticle;

    #endregion

    #region Private Variables

    private PlayerController _playerController;
    private bool _isTrigger;

    #endregion

    #endregion
    
    
    public virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Stickman"))
        {
            if (obstacleMaterial.color != other.GetComponent<Renderer>().sharedMaterial.color)
            {
                _isTrigger = true;
                destroyParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                _playerController = other.transform.parent.GetComponent<PlayerController>();
                _playerController.Stickmans.Remove(other.gameObject);
                StickmanObjectPooling.Instance.SetPool(other.gameObject);
                Instantiate(destroyParticle, other.transform.position, Quaternion.identity);
                
                if (UIManager.Instance.vibrateOn)
                {
                    Vibration.Vibrate(50);
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            if (_isTrigger)
            {
                _playerController.FormatStickman();
                _isTrigger = false;
            }
            
        }
    }
}
