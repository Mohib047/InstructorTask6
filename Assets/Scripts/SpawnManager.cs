using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    //Variable Declaration
    public GameObject enemyPrefab;
    public GameObject lifePrefab;
    public GameObject player;
    public Transform[] spawnPoints;
    public GameObject waveUI;
    public Text waveText;
    public GameObject endGameUI;
    private Vector3 offset = new Vector3(0, 1, 0);
    public int enemyCount;
    public int waveCount = 1;

    void Spawner(int wave) 
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < waveCount; i++)
        {
            if (randomPoint != i)
            {
                Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }
    void LifeSpawner() 
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        Instantiate(lifePrefab, spawnPoints[randomPoint].position +offset, spawnPoints[randomPoint].rotation);
    }

    IEnumerator Delay() 
    {
        yield return new WaitForSecondsRealtime(2);
        waveUI.SetActive(false);
    }
    void DelayAction() 
    {
        StartCoroutine(Delay());
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawner(waveCount);
        LifeSpawner();
        waveUI.SetActive(true);
        string WaveTextS = " Wave " + waveCount;
        waveText.text = WaveTextS;
        DelayAction();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveCount++;
            enemyPrefab.GetComponent<Enemy>().health += 5;
            Spawner(waveCount);
            LifeSpawner();
            waveUI.SetActive(true);
            string WaveTextS = " Wave " + waveCount;
            waveText.text = WaveTextS;
            DelayAction();
        }
        if (waveCount > 5) 
        {
            endGameUI.SetActive(true);
        }
    }
}
