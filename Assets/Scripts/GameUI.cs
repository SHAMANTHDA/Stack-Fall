using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using mixpanel;

public class GameUI : MonoBehaviour
{
    public Image levelSlider, levelSliderFill;
    public Image currentLevelImg;
    public Image nextLevelImg;
    //public GameObject firstUI;
    public GameObject inGameUI, finishUI, gameOverUI;
    //public GameObject allButtons;
    //private bool _buttons;
    private Material _playerMaterial;
    public Text currentLevelText, nextLevelText, finishLevelText, gameOverScoreText, gameOverBestText;
    private Player _player;
    //public Button soundButton;
    //public Sprite soundOnImg, soundOffImg;

    void Awake()
    {

        IgnoreUI();
        var props = new Value();

        Mixpanel.Register("GAME_NAME", "Stack Fall");

        props["GAME_NAME"] = "Game Screen Loaded";
        Mixpanel.Track("Game Screen Loaded", props);


        Debug.Log("game screen loaded");

        _playerMaterial = FindObjectOfType<Player>().GetComponent<MeshRenderer>().material;
        _player = FindObjectOfType<Player>();
        levelSlider.color = _playerMaterial.color;
        levelSliderFill.color = _playerMaterial.color + Color.gray;
        nextLevelImg.color = _playerMaterial.color;
        currentLevelImg.color = _playerMaterial.color;
        //soundButton.onClick.AddListener(() => SoundManager.instance.SoundOnOff());



        
    }

    void Start()
    {

        currentLevelText.text = FindObjectOfType<LevelSpawner>()._level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>()._level + 1 + "";

    }

    void Update()
    {
       
        UIManagement();
    }

    private void UIManagement()
    {
        //if (_player.playerState == Player.PlayerState.Prepare)
        //{
        //    if (SoundManager.instance._soundPlay && soundButton.GetComponent<Image>().sprite != soundOnImg)
        //    {
        //        soundButton.GetComponent<Image>().sprite = soundOnImg;
        //    }

        //    else if (!SoundManager.instance._soundPlay && soundButton.GetComponent<Image>().sprite != soundOffImg)
        //    {
        //        soundButton.GetComponent<Image>().sprite = soundOffImg;
        //    }
        //}

        if (!IgnoreUI() && _player.playerState == Player.PlayerState.Prepare)
        {
            //var props = new Value();

            //props["GAME_NAME"] = "Game Started";
            //Mixpanel.Track("Game Started", props);
            Debug.Log("game started");



            _player.playerState = Player.PlayerState.Play;
            //firstUI.SetActive(true);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        if (_player.playerState == Player.PlayerState.Finish)
        {
            //firstUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<LevelSpawner>()._level;


        }

        if (_player.playerState == Player.PlayerState.Dead)
        {
            //firstUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreHandler.instance.score.ToString();
            //gameOverBestText.text = PlayerPrefs.GetInt("Highscore").ToString();
            gameOverBestText.text = GetInt("Highscore").ToString();




            if (Input.GetMouseButtonDown(0))
            {
                var props = new Value();

                
                ScoreHandler.instance.ResetScore();
                SceneManager.LoadScene(0);
                _player.ClickCheck();


                //var props = new Value();
                props["GAME_NAME"] = "Game Over";
                Mixpanel.Track("Game Over", props);
                Debug.Log("over");

                props["GAME_NAME"] = "Game Retry";
                Mixpanel.Track("Game Retry", props);
                Debug.Log("restart");
            }
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        //pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
                //Debug.Log("IgnoreUI");
            if (raycastResultList[i].gameObject.GetComponent<IgnoreUI>() != null)
            {
                //Debug.Log("title");
                
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    
    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    //public void Settings()
    //{
    //    _buttons = !_buttons;
    //    allButtons.SetActive(_buttons);
    //}

    //**********************************************************************************************************
    public static void SetInt(string key, int value)
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        SaveToLocalStorage(key: key, value: value.ToString());
#else
        PlayerPrefs.SetString(key: key, value: value.ToString());
#endif
    }

    public static int GetInt(string key, int defVal = 0)
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        int a = 0;
        if (HasKeyInLocalStorage(key) == 1) {
          a = int.Parse(LoadFromLocalStorage(key: key));
        }
        return a;
#else
        return (PlayerPrefs.GetInt(key, defVal));
#endif
    }

    public static void Save()
    {
#if !UNITY_WEBGL
        PlayerPrefs.Save();
#endif
    }

#if (UNITY_WEBGL && !UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern void SaveToLocalStorage(string key, string value);

        [DllImport("__Internal")]
        private static extern string LoadFromLocalStorage(string key);

        [DllImport("__Internal")]
        private static extern void RemoveFromLocalStorage(string key);

        [DllImport("__Internal")]
        private static extern int HasKeyInLocalStorage(string key);
#endif
}

