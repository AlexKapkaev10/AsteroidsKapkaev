using System.Collections;
using UnityEngine;

public class UFO : MonoBehaviour
{
    //скорость движения тарелки
    public float speed;

    public float myScores = 200;

    //булевая переменная для корутины Shoot()
    private bool isShooting;

    //булевая переменная для корутины MoveActive()
    public bool canMove;

    //время ожидания для корутины Shoot()
    public float reloadTime;

    //время ожидания для корутины MoveActive()
    public float waitTime;

    //минимальное время ожидания для корутины MoveActive()
    public float minWaitTime;

    //максимальное время ожидания для корутины MoveActive()
    public float maxWaitTime;

    //массив точек следования тарелки
    public Transform[] waypoints;

    //текущая точка в массиве следования
    private int currentWaypoint = 0;

    //массив позиций стрельбы тарелки
    public Transform[] firePositions;

    //текущая позиция стрельбы
    private int currentFirePos;

    //снаряд тарелки
    public GameObject projectile;

    //эффект взрыва
    public GameObject explosionEffect;

    //ссылка на компонент
    private AudioSource audioSource;

    //булевая переменная тревоги для проигрывание сирены
    private bool alarm;

    private void Start()
    {
        //увеличение переменной общего количества врагов в игре
        GameManager.instance.allEnemyes++;

        //присвоение рандомного времени ожидани от момента старта игры,
        //до момнета движения тарелки, используется в корутине MoveActive()
        waitTime = Random.Range(minWaitTime, maxWaitTime);

        //инициализация компонента
        audioSource = GetComponent<AudioSource>();

        waypoints = GameManager.instance.waypointsUFO;
    }

    private void Update()
    {
        //все настройки движения тарелки
        Setting();
    }

    void Setting()
    {
        //старт корутины для переключения булевой переменной canMove в состояние true
        if (!canMove)
            StartCoroutine(MoveActive());

        //проигрывание сирены
        if(canMove && !alarm && !GameManager.instance.gameOver)
        {
            audioSource.Play();
            alarm = true;
        }
        if (GameManager.instance.gameOver)
            audioSource.Stop();

        //движение тарелки по точкам следования и релизация стрельбы, когда текущая точка следования - это игрок
        if (currentWaypoint < waypoints.Length && canMove && !GameManager.instance.gameOver)
        {

            //движение тарелки по точкам следования
            if (!GameManager.instance.gameOver)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, Time.deltaTime * speed);
            }
                

            //пеключение текущей точки следования в массиве waypoints[]
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.5f)
            {
                currentWaypoint++; 

            }

            //когда текущая точка увеличена до = 1, текущей точкой становится игрок,
            //стартует корутина реализующая стрельбу Shoot()
            if (currentWaypoint > 0)
            {
                if (!isShooting && !GameManager.instance.gameOver)
                {
                    currentFirePos = Random.Range(0, 2); //рандомный выбор текущей позиции стрельбы
                                                         //реализуется в корутине Shoot()
                    StartCoroutine(Shoot());
                }
            }
        }
    }

    //коллизия триггера
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //попадание снаряда
        if (collision.gameObject.GetComponent<Projectile>() != null && canMove)
        {
            GameManager.instance.scores += myScores;
            GameManager.instance.UpdateScores();
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(effect, 1);
            //уменьшение общего количества врагов в игре
            GameManager.instance.allEnemyes--;
        }
        //косание игрока
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(effect, 1);
            //уменьшение общего количества врагов в игре
            GameManager.instance.allEnemyes--;
        }
    }

    //корутина реализующая стрельбу тарелки
    IEnumerator Shoot()
    {
        isShooting = true;
        GameObject shot = Instantiate(projectile, firePositions[currentFirePos]);
        yield return new WaitForSeconds(reloadTime);
        isShooting = false;
    }

    //корутина включающая булевую переменную canMove в положение true,
    //при котором начинается движение тарелки
    IEnumerator MoveActive()
    {
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }
}
