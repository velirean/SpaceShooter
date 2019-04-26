using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;


    private float levelStartDelay = 1f;
    //private bool doingSetup = true; used when the player movement is blocked when the current level message appears
    private Text levelText;
    private GameObject levelImage;
    private GameObject gameManager;
    public GameObject meteorPrefab;
    public GameObject enemyPrefab;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        if (instance == null)
        {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(GameObject.Find("Player"));
        InitGame();
        StartCoroutine(GenerateMeteor());
        StartCoroutine(GenerateEnemy());
    }

    public void InitGame()
    {

        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
    }

    public void GameOver()
    {
        levelText.text = "Game Over";
        levelImage.SetActive(true);
        enabled = false;
    }

    IEnumerator GenerateMeteor()
    {
        yield return new WaitForSeconds(3);
        Instantiate(meteorPrefab, new Vector3(18, Random.Range(1f, 9f)), Quaternion.identity);
        StartCoroutine(GenerateMeteor());
    }

    IEnumerator GenerateEnemy()
    {
        yield return new WaitForSeconds(5);
        Instantiate(enemyPrefab, new Vector3(18, Random.Range(1f, 9f)), Quaternion.Euler(new Vector3(0, 0, -90f)));
        StartCoroutine(GenerateEnemy());
    }
}

