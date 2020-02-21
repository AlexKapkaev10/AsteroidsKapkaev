using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //глобальная статическая переменная класса GameManager
    public static GameManager instance;

    //панель рестарта
    public GameObject restartPanel;

    //панель с текстом нажатия любой клавиши для старта
    public GameObject startPanel;

    //панель с победной надписью и кнопкой новой игры
    public GameObject winPanel;

    //количество всех противников включая астероиды и UFO
    public int allEnemyes;

    //массив точек на сцене для пути следования UFO
    public Transform[] waypointsUFO;

    //массив с изображением жизней в канвасе  UI
    public Image[] livesImg;

    //переменная текущего изображения в массиве livesImg[]
    private int currentLiveImg;

    //переменная очков достижения
    public float scores;

    //переменная текста UI
    public Text scoresTxt;

    //булевая переменная включается в методе StartGame()
    public bool isStart;

    //булевая переменная победы включается при уничтожении всех астероидов
    public bool isWin;

    //булевая переменная включается когда игрок уничтожен, игра закончена
    public bool gameOver;

    private void Awake()
    {
        instance = this;
        scoresTxt.text = scores.ToString();
        Time.timeScale = 0;
    }

    private void Update()
    {
        if(!isStart)
            StartGame();

        Win();

    }

    //метод реализует запуск игры, при нажатии кнопки Space
    public void StartGame()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isStart = true;
            startPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //при уничтожении всех астероидов игрок побеждает и может сыграть снова
    public void Win()
    {
        if (allEnemyes == 0 && isStart)
        {
            Time.timeScale = 0;
            winPanel.SetActive(true);
            isWin = true;
        }
    }

    //метод перезагружает игровую сцену, используется в кнопке Restart, в UI панеле рестарта
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //определение текущего изображения жизни и его выключение на игровой сцене в UI
    //реализуется в скрипте Player.cs, при коллизии
    public void ActiveLiveImg()
    {
        livesImg[currentLiveImg].gameObject.SetActive(false);
        currentLiveImg++;
    }

    //метод обновляет текст UI и переводит переменную float scores в текст 
    public void UpdateScores()
    {
        scoresTxt.text = scores.ToString();
    }
}
