using UnityEngine;

public class Spawner : MonoBehaviour
{
    //ссылка на префаб
    public GameObject myAsterod;

    void Start()
    {
        //создание астероида в точке SpawnAsteroid на сцене
        GameObject Asteroid = Instantiate(myAsterod, transform.position, transform.rotation);
        //увеличение переменной общего количества астероидов в игре
        GameManager.instance.allEnemyes++;
    }
}
