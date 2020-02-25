using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float delaySpawn;
    public float minDelay;
    public float maxDelay;

    private bool isSpawn;
    //ссылка на префаб
    public GameObject myPrefab;

    void Start()
    {
        delaySpawn = Random.Range(minDelay, maxDelay);
        //создание астероида в точке SpawnAsteroid на сцене
        //GameObject enemy = Instantiate(myPrefab, transform.position, transform.rotation);
    }

    private void Update()
    {
        if (!isSpawn)
            StartCoroutine(RepeatingSpawner());
    }

    IEnumerator RepeatingSpawner()
    {
        isSpawn = true;
        delaySpawn = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delaySpawn);
        GameObject enemy = Instantiate(myPrefab, transform.position, transform.rotation);
        isSpawn = false;
    }
}
