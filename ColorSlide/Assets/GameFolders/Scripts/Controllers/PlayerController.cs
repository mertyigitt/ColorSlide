using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    #region Self Variables

    #region Serialize Variables
    
    [SerializeField] private LevelSO currentLevelSo;
    [SerializeField] private TextMeshPro counterTxt;
    [SerializeField] private AudioSource newStickmanSound;
    [SerializeField] private GameObject playerStickman;
    [SerializeField] private Slider levelSlider;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private List<GameObject> stickmans;

    #endregion

    #region Private Variables

    private bool _isStarted;
    private int _materialNumber;
    private float _moveSpeed;

    #endregion

    #region Public Variables

    public LevelSO CurrentLevelSo => currentLevelSo;
    public List<GameObject> Stickmans => stickmans;
    public int MaterialNumber => _materialNumber;

    #endregion

    #endregion
    
    
    private void Start()
    {
        MakeStickman(UIManager.Instance.startStickManLevel);
    }

    private void OnEnable()
    {
        UIManager.Instance.OnPlay += OnMoveStart;
        UIManager.Instance.OnSound += OnSoundCheck;
    }

    private void OnMoveStart()
    {
        _isStarted = true;
        _moveSpeed = 20f;
        foreach (GameObject stickman in stickmans)
        {
            stickman.GetComponent<Animator>().SetBool("Run", true);
        }
    }
    
    private void OnSoundCheck()
    {
        if(UIManager.Instance.soundOn == false)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.OnPlay -= OnMoveStart;
        UIManager.Instance.OnSound -= OnSoundCheck;
    }

    private void Update()
    {
        if (stickmans.Count <= 0)
        {
            _moveSpeed = 0;
            UIManager.Instance.playFail.SetActive(true);
        }
        
        ChangeMaterial();
        counterTxt.text = stickmans.Count.ToString();
        transform.Translate(0,0,_moveSpeed * Time.deltaTime);
    }
    
    public void FormatStickman()
    {
        for (int i = 1; i < stickmans.Count; i++)
        {
            var x = .5f * Mathf.Sqrt(i) * Mathf.Cos(i * 1);
            var z = .5f * Mathf.Sqrt(i) * Mathf.Sin(i * 1);
            
            var randomOffsetX = Random.Range(-0.5f, 0.5f);
            var randomOffsetZ = Random.Range(-0.5f, 0.5f);
            
            var newPos = new Vector3(x + randomOffsetX, -0.28f, z + randomOffsetZ);
            transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }
    }
    
    public void MakeStickman(int number)
    {
        for (int i = 0; i < number; i++)
        {
            var stickman = StickmanObjectPooling.Instance.GetPool();
            stickman.transform.position = transform.position;
            stickman.transform.parent = transform;
            stickmans.Add(stickman);
        }

        foreach (var stickman in stickmans)
        {
            stickman.SetActive(true);
            if (_isStarted)
                stickman.GetComponent<Animator>().SetBool("Run", true);
        }
        counterTxt.text = stickmans.Count.ToString();
        
        newStickmanSound.Play();

        FormatStickman();
    }
    
    private void ChangeMaterial()
    {
        var sliderAverage = levelSlider.maxValue / currentLevelSo.SliderParts;
        for (int i = 0; i < currentLevelSo.SliderParts; i++)
        {
            if (levelSlider.value > sliderAverage * i && levelSlider.value <= sliderAverage * (i + 1))
            {
                _materialNumber = i;
                foreach (GameObject stickman in stickmans)
                {
                    stickman.GetComponent<Renderer>().material = currentLevelSo.Materials[MaterialNumber];
                }
            }
        }
    }
    
    public void BossLine()
    {
        _moveSpeed = 0.35f;
    }

    public void FinishLine()
    {
        _moveSpeed = 5f;
        levelSlider.gameObject.SetActive(false);
        foreach (GameObject stickman in stickmans)
        {
            stickman.GetComponent<Animator>().SetBool("Run", false);
            stickman.GetComponent<Animator>().SetBool("Walk", true);
        }
        StartCoroutine(CameraFOV());
    }

    IEnumerator CameraFOV()
    {
        float time = 0;
        float startFOV = playerCamera.m_Lens.FieldOfView;
        while (time < 1)
        { 
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV,80 ,time/ 1f);
            time += Time.deltaTime;
            yield return null;
        }
        playerCamera.m_Lens.FieldOfView = 80;
    }
}
