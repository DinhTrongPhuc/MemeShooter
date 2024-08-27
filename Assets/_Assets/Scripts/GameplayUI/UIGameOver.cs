using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public Button restartBtn;

    public Transform[] killedItems;

    // Start is called before the first frame update
    void Start()
    {
        restartBtn.onClick.AddListener(MemeGameController.Instance.Restart);
    }

    public void Setup()
    {
        MemeGameController.Instance.UpdateHighScore();

        scoreText.text = MemeGameController.Instance.score.ToString();

        for (int i = 0; i < killedItems.Length; i++)
        {
            killedItems[i].GetChild(0).GetComponent<Image>().sprite = MemeSpawner.Instance.GetMemeSpriteByName(MemeGameController.Instance.KilledDict.ElementAt(i).Key);
            killedItems[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + MemeGameController.Instance.KilledDict.ElementAt(i).Value.ToString();
        }
    }
}
