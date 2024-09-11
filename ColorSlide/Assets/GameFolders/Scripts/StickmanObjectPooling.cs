using System;
using System.Collections.Generic;
using UnityEngine;

public class StickmanObjectPooling : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private GameObject stickmanPrefab;

    #endregion

    #region Private Variables
    
    private Queue<GameObject> _stickmans = new Queue<GameObject>();

    #endregion

    #region Public Variables

    public static StickmanObjectPooling Instance;

    #endregion

    #endregion
    
    
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < 650; i++)
        {
            var newStickman = Instantiate(stickmanPrefab, transform, true);
            newStickman.SetActive(false);
            _stickmans.Enqueue(newStickman);
        }
    }

    public void SetPool(GameObject stickmanObject)
    {
        stickmanObject.SetActive(false);
        stickmanObject.transform.parent = transform;
        _stickmans.Enqueue(stickmanObject);
    }
    
    public GameObject GetPool()
    {
        if (_stickmans.Count == 0)
        {
            InitializePool();
        }
        return _stickmans.Dequeue();
    }
}
