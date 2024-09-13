using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public UnityAction OnPlay = delegate{};
    public UnityAction OnSound = delegate{};
    
    [Header("UI")]
    public TextMeshProUGUI gameCoinText;
    public TextMeshProUGUI gameCoinTextStart;
    public TextMeshProUGUI gameCoinTextSuccess;
    public TextMeshProUGUI gameCoinTextGain;
    public TextMeshProUGUI startStickManLevelText;
    public Text startStickManCoinText;
    public TextMeshProUGUI extraCoinLevelText;
    public Text extraCoinLevelCoinText;
    public GameObject playUI,playSuccess,playFail,soundOffText,soundOnText,vibrateOnText, vibrateOffText;
    
    
    [Header("Audio")]
    public AudioSource clickAudio;
    public AudioSource successAudio;
    
    
    [HideInInspector] public int gameLevel;
    [HideInInspector] public bool soundOn;
    [HideInInspector] public bool vibrateOn;
    [HideInInspector] public int gameCoin;
    [HideInInspector] public int startStickManLevel;
    [HideInInspector] public int startStickManCoin;
    [HideInInspector] public int extraCoinLevel;
    [HideInInspector] public int extraCoinLevelCoin;
    [HideInInspector] public int extraCoinBonus;

    private Button[] _buttons;
    
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameLevel = PlayerPrefs.GetInt("gameLevel");
        
        vibrateOn = true;
        soundOn = true;

        if (!PlayerPrefs.HasKey("stickmanLevel"))
        {
            PlayerPrefs.SetInt("stickmanLevel", 1);
        }
        if (!PlayerPrefs.HasKey("stickmanCoin"))
        {
            PlayerPrefs.SetInt("stickmanCoin", 100);
        }
        if(!PlayerPrefs.HasKey("coinLevel"))
        {
            PlayerPrefs.SetInt("coinLevel", 1);
        }
        if (!PlayerPrefs.HasKey("extraCoin"))
        {
            PlayerPrefs.SetInt("extraCoin", 100);
        }
        if(!PlayerPrefs.HasKey("gameCoin"))
        {
            PlayerPrefs.SetInt("gameCoin", 100);
        }
        if(!PlayerPrefs.HasKey("vibrate"))
        {
            PlayerPrefs.SetInt("vibrate", 1);
        }
        if (!PlayerPrefs.HasKey("sound"))
        {
            PlayerPrefs.SetInt("sound", 1);
        }
        startStickManLevel = PlayerPrefs.GetInt("stickmanLevel");
        startStickManCoin = PlayerPrefs.GetInt("stickmanCoin");
        PlayerPrefs.SetInt("gameCoin", 9999);
        gameCoin = PlayerPrefs.GetInt("gameCoin");
        extraCoinLevel = PlayerPrefs.GetInt("coinLevel");
        extraCoinLevelCoin = PlayerPrefs.GetInt("extraCoin");
        extraCoinBonus = PlayerPrefs.GetInt("extraBonus");
        vibrateOn = PlayerPrefsGetBool("vibrate");
        soundOn = PlayerPrefsGetBool("sound");
        
        if(vibrateOn) 
        {
            vibrateOffText.SetActive(false);
            vibrateOnText.SetActive(true);
        }
        else
        {
            vibrateOffText.SetActive(true);
            vibrateOnText.SetActive(false);
        }

        if(soundOn)
        {
            soundOffText.SetActive(false);
            soundOnText.SetActive(true);
        }
        else
        {
            soundOffText.SetActive(true);
            soundOnText.SetActive(false);
        }
        
        gameCoinText.text = gameCoin.ToString();
        gameCoinTextStart.text = gameCoin.ToString();
        gameCoinTextSuccess.text = gameCoin.ToString();

        startStickManLevelText.text = "LEVEL\n" + PlayerPrefs.GetInt("stickmanLevel");
        startStickManCoinText.text = PlayerPrefs.GetInt("stickmanCoin").ToString();

        extraCoinLevelText.text = "LEVEL\n" + PlayerPrefs.GetInt("coinLevel");
        extraCoinLevelCoinText.text = PlayerPrefs.GetInt("extraCoin").ToString();
        
        _buttons = FindObjectsOfType<Button>();

        foreach (Button btn in _buttons)
        {
            btn.onClick.AddListener(UISoundandVibration);
        }
        
        OnSound?.Invoke();
    }

    private void UISoundandVibration()
    {
        if(vibrateOn)
        {
            Vibration.Vibrate(150);
        }
        clickAudio.Play();
    }


    public void StartGame()
    {
        OnPlay?.Invoke();
    }

    public void StartStickMan()
    {
        if(gameCoin >= startStickManCoin)
        {
            gameCoin -= startStickManCoin;
            PlayerPrefs.SetInt("gameCoin", gameCoin);
            gameCoinText.text = PlayerPrefs.GetInt("gameCoin").ToString();

            startStickManLevel++;
            PlayerPrefs.SetInt("stickmanLevel", startStickManLevel);
            startStickManLevelText.text = "LEVEL\n" + PlayerPrefs.GetInt("stickmanLevel");

            startStickManCoin = startStickManCoin + 60;
            PlayerPrefs.SetInt("stickmanCoin", startStickManCoin);
            startStickManCoinText.text = PlayerPrefs.GetInt("stickmanCoin").ToString();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            gameCoinText.text = gameCoin.ToString();
            gameCoinTextStart.text = gameCoin.ToString();
            gameCoinTextSuccess.text = gameCoin.ToString();
        }
    }


    public void ExtraCoin()
    {
        if(gameCoin >= extraCoinLevelCoin)
        {
            gameCoin -= extraCoinLevelCoin;
            PlayerPrefs.SetInt("gameCoin", gameCoin);
            gameCoinText.text = PlayerPrefs.GetInt("gameCoin").ToString();

            extraCoinBonus = extraCoinBonus + 20;
            PlayerPrefs.SetInt("extraBonus", extraCoinBonus);

            extraCoinLevel++;
            PlayerPrefs.SetInt("coinLevel", extraCoinLevel);
            extraCoinLevelText.text = "LEVEL\n" + PlayerPrefs.GetInt("coinLevel");

            extraCoinLevelCoin = extraCoinLevelCoin + 60;
            PlayerPrefs.SetInt("extraCoin", extraCoinLevelCoin);
            extraCoinLevelCoinText.text = PlayerPrefs.GetInt("extraCoin").ToString();
            gameCoinText.text = gameCoin.ToString();
            gameCoinTextStart.text = gameCoin.ToString();
            gameCoinTextSuccess.text = gameCoin.ToString();
        }
    }
    

    public void SoundOff()
    {
        soundOn = true;
        PlayerPrefsSetBool("sound", soundOn);
        OnSound?.Invoke();
    }
    public void SoundOn()
    {
        soundOn = false;
        PlayerPrefsSetBool("sound", soundOn);
        OnSound?.Invoke();
    }

    public void VibrateOff()
    {
        vibrateOn = true;
        PlayerPrefsSetBool("vibrate", vibrateOn);
    }

    public void VibrateOn()
    {
        vibrateOn = false;
        PlayerPrefsSetBool("vibrate", vibrateOn);
    }
    
    
    public void ButtonNo()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ButtonNoFail()
    {
        gameLevel++;
        PlayerPrefs.SetInt("gameLevel", gameLevel);
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TryAgain()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }
    
    void PlayerPrefsSetBool(string hash, bool value) 
    {
        int deger = (value == false) ? 0 : 1;
        PlayerPrefs.SetInt(hash, deger); 
    }

    bool PlayerPrefsGetBool(string hash) 
    {
        bool durum = (PlayerPrefs.GetInt(hash) == 0) ? false : true;
        return durum;
    }

    public IEnumerator BossDie(int stickManCount)
    {
        yield return new WaitForSeconds(1.5f);
        
        successAudio.Play();
        playSuccess.SetActive(true);
        playUI.SetActive(false);
        
        gameCoinTextGain.text = stickManCount.ToString();
        gameCoin += stickManCount;
        PlayerPrefs.SetInt("gameCoin", gameCoin);
        
        gameCoinTextSuccess.text = PlayerPrefs.GetInt("gameCoin").ToString();
        gameLevel++;
        PlayerPrefs.SetInt("gameLevel", gameLevel);
    }
}
