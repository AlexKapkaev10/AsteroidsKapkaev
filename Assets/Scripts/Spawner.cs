using UnityEngine;

public class Spawner : MonoBehaviour
{
    //ссылка на префаб
    public GameObject myPrefab;

    void Start()
    {
        //создание астероида в точке SpawnAsteroid на сцене
        GameObject enemy = Instantiate(myPrefab, transform.position, transform.rotation);
        //увеличение переменной общего количества врагов в игре
        GameManager.instance.allEnemyes++;
    }
}
