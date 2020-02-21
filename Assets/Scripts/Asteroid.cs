using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //ссылка на компонент Rigidbody2D
    private Rigidbody2D rg;

    //количество очков получаемых за уничтожение австероида
    //в зависимости от размера астероида, назначается в инспекторе
    public float myScores;

    //сила движения астероида
    public float maxForce;

    //сила вращения астероида
    public float maxTorque;

    //максимальная верхняя точка на сцене для астероида
    public float screenTop;

    //минимальная нижняя точка на сцене для астероида
    public float screenBottom;

    //максимальная правая точка на сцене для астероида
    public float screenRight;

    //максимальная левая точка на сцене для астероида
    public float screenLeft;

    //время жизни эффекта взрыва
    public float liveTimeEffect;

    //префаб астороида среднего размера
    public GameObject asteroidStandart;

    //префаб астероида маленького размера
    public GameObject asteroidSmall;

    // эффект взрыва
    public GameObject explosionEffect;

    //перечисление возможных астероидов по размеру
    public enum Type
    {
        big,
        stadart,
        small
    }

    //публичная переменная перечилнения
    public Type myType; 

    void Start()
    {
        //увеличение переменной общего количества врагов в игре
        GameManager.instance.allEnemyes++;

        //инициализация компонента Rigidbody2D
        rg = GetComponent<Rigidbody2D>();

        //вычисление рандомного вектора направления и скорости вращения астероида
        Vector2 force = new Vector2(Random.Range(-maxForce, maxForce), Random.Range(-maxForce, maxForce));
        float torque = Random.Range(-maxTorque, maxTorque);

        //добавление скорости и вращение астероиду
        rg.AddForce(force);
        rg.AddTorque(torque);
        
    }

    void Update()
    {
        StayOnDisplay();
    }

    //отслеживание и изменение позиции, для постоянного нахождения астероида в зоне видимости камеры
    void StayOnDisplay()
    {
        Vector2 newPos = transform.position;

        if(transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom)
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

    //отслеживание коллизии попадания снаряда в астероид,
    //логика создания новых астероидов меньших размерв написана здесь
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            Destroy(collision.gameObject);

            //уменьшение общего количества врагов на сцене
            GameManager.instance.allEnemyes--;

            //большой астероид создает два астероида среднего размера
            if (myType == Type.big)
            {
                GameManager.instance.scores += myScores;
                GameManager.instance.UpdateScores();
                GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
                GameObject AteroidStandart = Instantiate(asteroidStandart, transform.position, transform.rotation);
                GameObject newAteroidStandart = Instantiate(asteroidStandart, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(effect, liveTimeEffect);
                
            }

            //средний астероид создает два астероида маленького размера
            if (myType == Type.stadart)
            {
                GameManager.instance.scores += myScores;
                GameManager.instance.UpdateScores();
                GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
                GameObject AteroidSmall = Instantiate(asteroidSmall, transform.position, transform.rotation);
                GameObject newAteroidSmall = Instantiate(asteroidSmall, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(effect, liveTimeEffect);
            }

            //маленький астероид
            if (myType == Type.small)
            {
                GameManager.instance.scores += myScores;
                GameManager.instance.UpdateScores();
                GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(effect, liveTimeEffect);
            }

        }
    }

    //маленький астероид уничтожается при столкновении с игроком
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(myType == Type.small && collision.gameObject.GetComponent<Player>() != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(effect, liveTimeEffect);
            GameManager.instance.allEnemyes--;
        }
    }
}
