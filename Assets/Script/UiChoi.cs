﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiChoi : MonoBehaviour
{
    public Button playButton; 
    public Button setingButton; 
    public GameObject background; 
    public GameObject layGame; 

    public Button play2Button;
    public Button buttonDiem;
    public TextMeshProUGUI diem;

    public List<GameObject> mapPrefabs;
    private int currentMapIndex = 0;
    private GameObject currentMapInstance;
    Player player;
    public Transform mapSpawnPoint;

    public GameObject meNu;
    public Button Play1;
    public Button Play2;
    public Button Play3;
    public Button Play4;
    public Button Play5;


    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        play2Button.onClick.AddListener(ChuyenMan2);
        player = FindObjectOfType<Player>();

        Play1.onClick.AddListener(() => SelectMap(0));
        Play2.onClick.AddListener(() => SelectMap(1));
        Play3.onClick.AddListener(() => SelectMap(2));
        Play4.onClick.AddListener(() => SelectMap(3));
        Play5.onClick.AddListener(() => SelectMap(4));

        PlayGame();
    }

    void OnPlayButtonClicked()
    {
        meNu.SetActive(true);
        background.SetActive(false);
        playButton.gameObject.SetActive(false);
        setingButton.gameObject.SetActive(false);
    }
    void PlayGame()
    {
        layGame.SetActive(true);
    }
    public void Find()
    {
        play2Button.gameObject.SetActive(true);
        buttonDiem.gameObject.SetActive(true);
    }
    public void FindHuy()
    {
        play2Button.gameObject.SetActive(false);
        buttonDiem.gameObject.SetActive(false);
    }
    private void ChuyenMan2()
    {
        FindHuy();
        NextMap();
        player.transform.position = new Vector3(3.505f, 2.55f, -5.47f);
    }
    public void NextMap()
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        currentMapIndex = (currentMapIndex + 1) % mapPrefabs.Count;

        SpawnMap(currentMapIndex);
    }

    void SpawnMap(int index)
    {
        currentMapInstance = Instantiate(mapPrefabs[index], mapSpawnPoint.position, Quaternion.identity);
    }
    void SelectMap(int index)
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        currentMapIndex = index;
        SpawnMap(currentMapIndex);
        player.transform.position = new Vector3(3.505f, 2.55f, -5.47f);
        meNu.SetActive(false);
    }
}