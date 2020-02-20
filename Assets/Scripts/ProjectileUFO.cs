using UnityEngine;

public class ProjectileUFO : MonoBehaviour
{
    //ссылка на компонет Rigidbody2D
    private Rigidbody2D rg;

    //сила скорости снаряда
    public float forceSpeed;

    //урон наносимый снарядом
    public float damage = 10;

    void Start()
    {
        //инициализация компонента Rigidbody2D
        rg = GetComponent<Rigidbody2D>();

        //добавление скорости снаряду по вектору направления Vector2.up
        rg.AddRelativeForce(Vector2.up * forceSpeed, ForceMode2D.Impulse); 
    }

    void Update()
    {
        //уничтожение объекта через две секунды после создания, если не в кого не попал
        Destroy(gameObject, 2);
    }
}
