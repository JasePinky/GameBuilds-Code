using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [System.Serializable]
    public struct PlayerStats
    {
        public CarThings car;
        public int currentLap;
        public int currentCheckPoint;
        public int score;
    }

    public int highScore;

    public PlayerStats player1;
    public PlayerStats player2;

    public Text currentLapP1;
    public Text currentLapP2;
    public Text scoreTextP1;
    public Text scoreTextP2;
    public Text counting;
    public Text finishTime;

    public bool finishP1;
    public bool finishP2;
	public bool starting;

    public GameObject winScreenP1;
    public GameObject loseScreenP1;
    public GameObject winScreenP2;
    public GameObject loseScreenP2;
    public GameObject pauseMenu;

    public Checkpoint[] checkPoints;

    void Start ()
    {
        SetLapP1Text ();
        SetLapP2Text ();
        SetScoreP1Text();
        SetScoreP2Text();

        highScore = PlayerPrefs.GetInt("score");

        pauseMenu.SetActive(false);

        StartCoroutine("countDown");

        foreach (Checkpoint checkpoint in checkPoints)
        {
            checkpoint.gameController = this;
        }
	}

    void Update ()
    {
        SetLapP1Text();
        SetLapP2Text();
        SetScoreP1Text();
        SetScoreP2Text();

        player1.currentLap = Mathf.Clamp(player1.currentLap, 0, 3);

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;              
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

		if (player1.currentLap == 3 && player1.currentCheckPoint == 11 && !finishP2)
        {
            winScreenP1.SetActive(true);
            loseScreenP2.SetActive(true);
            finishP1 = true;

            if (player1.score > highScore)
            {
                PlayerPrefs.SetInt("score", player1.score);
                PlayerPrefs.Save();
            }
            Debug.Log("win?");
        }
		if (player1.currentLap == 3 && player1.currentCheckPoint == 11 && finishP2) 
		{
			finishP1 = true;
			if (player1.score > highScore)
			{
				PlayerPrefs.SetInt("score", player1.score);
				PlayerPrefs.Save();
			}
		}


		if (player2.currentLap == 3 && player2.currentCheckPoint == 11 && !finishP1)
        {
            winScreenP2.SetActive(true);
            loseScreenP1.SetActive(true);
			finishP2 = true;

            if (player2.score > highScore)
            {
                PlayerPrefs.SetInt("score", player2.score);
                PlayerPrefs.Save();
            }
            Debug.Log ("win?");
        }
		if (player2.currentLap == 3 && player2.currentCheckPoint == 11 && finishP1) 
		{
			finishP2 = true;
			if (player2.score > highScore)
			{
				PlayerPrefs.SetInt("score", player2.score);
				PlayerPrefs.Save();
			}
		}

        if (finishP1)
            StartCoroutine("FinishTimer");
        if (finishP2)
            StartCoroutine("FinishTimer");
        if (finishP1 && finishP2)
            StartCoroutine("Finished");

        PlayerPrefs.SetInt("", player1.score);
        PlayerPrefs.GetInt("");

        PlayerPrefs.SetInt("", player2.score);
        PlayerPrefs.GetInt("");
    }
	
	public void UpdatePlayerCheckpoint (string tag, int checkpointIndex, bool finalCheckPoint)
    {
        if (tag == "Player1" && player1.currentCheckPoint == checkpointIndex)
        {
            player1.currentCheckPoint++;
            if (finalCheckPoint && player1.currentLap != 3)
            {
                player1.currentCheckPoint = 0;
                player1.currentLap++;
            }
        }
        if (tag == "Player2" && player2.currentCheckPoint == checkpointIndex)
        {
            player2.currentCheckPoint++;
            if (finalCheckPoint && player2.currentLap != 3)
            {
                player2.currentCheckPoint = 0;
                player2.currentLap++;
            }
        }
    }

    void SetLapP1Text()
    {
        currentLapP1.text = "Lap : " + player1.currentLap + "/3";
    }

    void SetLapP2Text()
    {
        currentLapP2.text = "Lap : " + player2.currentLap + "/3";
    }

    void SetScoreP1Text()
    {
        scoreTextP1.text = "Score : " + player1.score;
    }

    void SetScoreP2Text()
    {
        scoreTextP2.text = "Score : " + player2.score;
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("score", player1.score);
        PlayerPrefs.Save();
    }

    IEnumerator countDown()
    {
        counting.text = "3";
        yield return new WaitForSeconds(1);
        counting.text = "2";
        yield return new WaitForSeconds(1);
        counting.text = "1";
        yield return new WaitForSeconds(1);
        counting.text = "Go!";
        starting = true;
		yield return new WaitForSeconds(1);
		counting.text = "";
    }

    IEnumerator Finished()
    {
        yield return new WaitForSeconds(5);
        Save();
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FinishTimer()
    {
        finishTime.text = "20";
        yield return new WaitForSeconds(1);
        finishTime.text = "19";
        yield return new WaitForSeconds(1);
        finishTime.text = "18";
        yield return new WaitForSeconds(1);
        finishTime.text = "17";
        yield return new WaitForSeconds(1);
        finishTime.text = "16";
        yield return new WaitForSeconds(1);
        finishTime.text = "15";
        yield return new WaitForSeconds(1);
        finishTime.text = "14";
        yield return new WaitForSeconds(1);
        finishTime.text = "13";
        yield return new WaitForSeconds(1);
        finishTime.text = "12";
        yield return new WaitForSeconds(1);
        finishTime.text = "11";
        yield return new WaitForSeconds(1);
        finishTime.text = "10";
        yield return new WaitForSeconds(1);
        finishTime.text = "9";
        yield return new WaitForSeconds(1);
        finishTime.text = "8";
        yield return new WaitForSeconds(1);
        finishTime.text = "7";
        yield return new WaitForSeconds(1);
        finishTime.text = "6";
        yield return new WaitForSeconds(1);
        finishTime.text = "5";
        yield return new WaitForSeconds(1);
        finishTime.text = "4";
        yield return new WaitForSeconds(1);
        finishTime.text = "3";
        yield return new WaitForSeconds(1);
        finishTime.text = "2";
        yield return new WaitForSeconds(1);
        finishTime.text = "1";
        yield return new WaitForSeconds(1);
        finishTime.text = "0";
        yield return new WaitForSeconds(1);
        finishTime.text = "You DNFed";
        yield return new WaitForSeconds(4);
        Save();
        int indexSC = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("MainMenu");        
    }
}
