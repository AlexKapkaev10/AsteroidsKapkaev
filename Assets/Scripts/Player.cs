using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //ссылка на компонент Rigidbody2D
    private Rigidbody2D rg;

    //скорость движения 
    public float forcePower;

    //сила поворота влево
    public float rotateSpeedLeft;

    //сила поворота вправо
    public float rotateSpeedRight;

    //максимальная верхняя точка на сцене для игрока
    public float screenTop;

    //минимальная нижняя точка на сцене для игрока
    public float screenBottom;

    //максимальная правая точка на сцене для игрока
    public float screenRight;

    //максимальная левая точка на сцене для игрока
    public float screenLeft;

    //префаб снаряда
    public GameObject projectile;

    //префаб эффекта взрыва
    public GameObject explosionEffect;

    //позиция создания снаряда
    public Transform firePoint;

    //здоровье игрока
    [SerializeField] private float HP = 3;

    //ссылка на компонент AudioSource, для воспроизведения звука
    private AudioSource audioSource;

    //время ожидания для корутины Recovery()
    public float recoveryTime;

    //булевая переменная столкновения либо с астероидом либо с тарелкой
    private bool isHit;

    private void Start()
    {
        //инициализация компонентов
        rg = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Shoot();
        StayOnDisplay();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //движение и повороты игрока, при нажатии кнопок на клавиатуре
    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rg.AddRelativeForce(Vector2.up * forcePower * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rg.AddRelativeForce(Vector2.down * forcePower * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rg.AddTorque(rotateSpeedLeft * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rg.AddTorque(rotateSpeedRight * Time.deltaTime);
        }
    }

    //стрельба, при нажатии левой кнопки мышки, проверки через GameManager, для коректной работы со звуком снаряда
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.isWin && GameManager.instance.isStart)
        {
            GameObject shot = Instantiate(projectile, firePoint);
        }
    }

    //отслеживание и изменение позиции, для постоянного нахождения игрока в зоне видимости камеры
    void StayOnDisplay()
    {
        Vector2 newPos = transform.position;

        if (transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if (transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }
        transform.position = newPos;
    }

    //коллизия столкновения с астероидом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Asteroid>() != null  && !isHit)
        {
            if(HP > 0)
                audioSource.Play();
            isHit = true;
            HP--;
            if (HP == 0)
            {
                Death();
            }
            GameManager.instance.ActiveLiveImg();
            StartCoroutine(Recovery());
        }
    }

    //коллизия триггера снаряда торелки
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ProjectileUFO>() || collision.gameObject.GetComponent<UFO>() != null && !isHit)
        {
            if (HP > 0)
                audioSource.Play();
            isHit = true;
            Destroy(collision.gameObject);
            HP--;
            if (HP == 0)
            {
                Death();
            }
            GameManager.instance.ActiveLiveImg();
            StartCoroutine(Recovery());
        }
    }

    //метод вызывается при нулевом здоровье
    void Death()
    {
        GameManager.instance.gameOver = true;
        GameManager.instance.restartPanel.SetActive(true);
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(effect, 2);
    }

    //корутина неуязвимости после столкнвения или попадания снаряда торелки
    IEnumerator Recovery()
    {
        yield return new WaitForSeconds(recoveryTime);
        isHit = false;
    }
}
