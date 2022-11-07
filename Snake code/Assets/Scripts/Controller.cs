using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

    public float moveSpeed;
    public float steerSpeed;
    public int Gap;
    public int Health;
    public Text HealthText;

    public GameObject bodyPrefab;

    List<GameObject> BodyParts = new List<GameObject>();
    List<Vector3> PositionHistory = new List<Vector3>();

    public int Score;
    public Text ScoreText;

    public GameObject GameOverText;
    public GameObject ResButton;

    public GameObject CompliteText;
    public GameObject NextButton;

    public GameObject StartButton;

    void Start()
    {

        GameOverText.SetActive(false);
        ResButton.SetActive(false);

        CompliteText.SetActive(false);
        NextButton.SetActive(false);

        StartButton.SetActive(true);

        Time.timeScale = 0;
    }

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        PositionHistory.Insert(0, transform.position);

        int index = 0;
        
        foreach(var body in BodyParts)
        {
            Vector3 point = PositionHistory[Mathf.Clamp(index*Gap, 0, PositionHistory.Count-1)];

            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * moveSpeed * Time.deltaTime;

            body.transform.LookAt(point);

            index++;
        }

        ScoreText.text = Score.ToString();
        HealthText.text = Health.ToString();

        if(Score >= 3) {
            Time.timeScale = 0;
            CompliteText.SetActive(true);
            NextButton.SetActive(true);
        }

        if(Health <= 0) {
            Time.timeScale = 0;
            GameOverText.SetActive(true);
            ResButton.SetActive(true);
        }
    }

    void GrowSnake() 
    {
        GameObject body = Instantiate(bodyPrefab);
        BodyParts.Add(body);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "food")
        {
            GrowSnake();
            Destroy(other.gameObject);
            Score++;
        }

        if(other.gameObject.tag == "Wall")
        {
            Time.timeScale = 0;
            GameOverText.SetActive(true);
            ResButton.SetActive(true);
        }

        if(other.gameObject.tag == "Enemy")
        {
            Health--;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Enemy2")
        {
            Health -= 2;
            Destroy(other.gameObject);
        }
    }

    public void RestartButton(int _sceneNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneNumber);
    }

    public void NexButton(int _sceneNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneNumber);
    }

    public void StarButton()
    {
        Time.timeScale = 1;
        StartButton.SetActive(false);
    }

}
