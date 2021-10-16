using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    #region Score
    [Header("Score")]
    [Tooltip("UI-объект для текста Score")]
    public Text ScoreCounter;
    int _score = 0;
    #endregion
    #region Coins
    [Header("Coins")]
    [Tooltip("UI-объект для текста Coins")]
    public Text CoinCounter;
    int _coins = 0;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ScoreTrigger":
                Debug.Log("Score inc");
                _score++;
                collision.enabled = false;
                ScoreCounter.text = Convert.ToString(_score);
                break;
            case "Floor":
                Lose();
                break;
            case "Coin":
                Debug.Log("Coin collected");
                _coins++;
                collision.gameObject.SetActive(false);
                CoinCounter.text = Convert.ToString(_coins);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                Debug.Log("Obstacle");
                Lose();
                break;
        }
    }
    private void Lose()
    {
        Debug.Log("LOSE");
        LeanTouch.OnFingerTap += Reload;
        Time.timeScale = 0;
        ScoreCounter.color = Color.red;
    }

    private void Reload(LeanFinger finger)
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
        LeanTouch.OnFingerTap -= Reload;
    }
}
