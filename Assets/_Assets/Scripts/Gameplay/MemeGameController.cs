using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static AYellowpaper.SerializedCollections.SerializedDictionarySample;

public class MemeGameController : MonoBehaviour
{
    public static MemeGameController Instance { get; private set; }

    public float time = 301f;

    public TextMeshProUGUI timeText;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highScoreText;

    //left and right hand raycast
    public GameObject lefthand;
    public GameObject righthand;

    [SerializeField] GameObject BackgroundMusic;
    [SerializeField] GameObject pausedButton;
    [SerializeField] GameObject pausedText;
    [SerializeField] GameObject isPlayText;

    [SerializeField] bool isPlay = false;
    public bool IsPlay { get { return isPlay; } }

    [SerializeField] GameObject shotgunVideo;
    [SerializeField] GameObject rifleVideo;


    public int score { get; private set; }

    [SerializedDictionary("meme name", "kill count")]
    public SerializedDictionary<string, int> KilledDict = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // if game envent is not starting yet
        isPlay = false;
        BackgroundMusic.SetActive(false);

        // turn on raycast on hand visual
        lefthand.gameObject.SetActive(false);
        righthand.gameObject.SetActive(false);

        CreateKilledCountDict();

        AddScore(0);

        highScoreText.text = "Top: " + GetScore().ToString();

        UIGameManager.Instance.Setup();
        MemeSpawner.Instance.DisableMemeSpawner();

        // condition with canvas if there is an video in there
        if (shotgunVideo != null && rifleVideo != null)
        {// if there is a video 
            shotgunVideo.SetActive(true);
            rifleVideo.SetActive(true);
        }

    }

    private void Update()
    {
        if (isPlay)
        {
            TimeCount();
            BackgroundMusic.SetActive(true);
        }
    }

    public void AddScore(int val)
    {
        score += val;

        scoreText.text = "Score: " + score.ToString();
    }

    void CreateKilledCountDict()
    {
        KilledDict.Clear();
        foreach (Sprite sprite in MemeSpawner.Instance.memeSprites)
        {
            KilledDict.Add(sprite.name, 0);
        }
    }

    public void AddKilled(string spriteName)
    {
        if (KilledDict.TryGetValue(spriteName, out int value))
        {
            KilledDict[spriteName] += 1;
        }
        else
        {
            KilledDict.Add(spriteName, 1);
        }
    }

    public int GetScore()
    {
        return PlayerPrefs.GetInt("score", 0);
    }

    public void SetScore(int val)
    {
        PlayerPrefs.SetInt("score", val);
    }

    public void UpdateHighScore()
    {
        if (score > GetScore())
        {
            SetScore(score);
        }
    }

    void TimeCount()
    {
        if (isPlay)
        {

            if (time > 0f)
            {
                time -= Time.deltaTime;

                if (time <= 0f)
                {
                    if (time < 0f)
                    {
                        time = 0f;
                    }

                    GameOver();
                }

                int minute = (int)(time / 60f);

                int second = (int)(time - (minute * 60f));

                timeText.text = minute.ToString("00") + ":" + second.ToString("00");
            }
        }
    }

    void GameOver()
    {

        UIGameManager.Instance.GameOver();

        MemeSpawner.Instance.DisableMemeSpawner();

        lefthand.gameObject.SetActive(true);
        righthand.gameObject.SetActive(true);


    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleIsPlay()
    {
        if (isPlay)
        {
            isPlay = false;
            pausedText.SetActive(true);
            isPlayText.SetActive(false);
            MemeSpawner.Instance.DisableMemeSpawner();
            
        }
        else
        {

            shotgunVideo.SetActive(false);
            rifleVideo.SetActive(false);


            isPlay = true;
            pausedText.SetActive(false);
            isPlayText.SetActive(true);
            MemeSpawner.Instance.ReactivateMemeSpawner();

        }



    }
}
