using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameManager : MonoBehaviour
{
    public static UIGameManager Instance;

    public Transform uiInGame;

    public UIGameOver uIGameOver;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        ActiveUIGameOver(false);

        uiInGame.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        ActiveUIGameOver(true);

        uiInGame.gameObject.SetActive(false);
    }

    public void ActiveUIGameOver(bool val)
    {
        if (val)
        {
            uIGameOver.Setup();
        }

        uIGameOver.gameObject.SetActive(val);
    }
}
