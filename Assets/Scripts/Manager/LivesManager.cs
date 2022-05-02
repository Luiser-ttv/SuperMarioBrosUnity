using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public int lives;
    private int defaultLives = 3;

    public Text livesText;
    public GameController GC;

    private void Awake()
    {
        lives = defaultLives;
        UpdateLivesText();
        GC = gameObject.GetComponent<GameController>();
    }

    public void AddLives()
    {
        lives++;
        Debug.Log(lives);
        SoundGuy.Instance.PlaySound("smb_1-up");
        UpdateLivesText();
    }

    public void LoseLives()
    {
        lives--;
        CheckLives();
        UpdateLivesText();
    }

    public bool CheckLives()
    {
        return lives > 0;
    }

    public int GetLives()
    {
        return PlayerPrefs.GetInt("Lives", defaultLives);
    }

    public void SetLives(int l)
    {
        PlayerPrefs.SetInt("Lives", l);
        PlayerPrefs.Save();
    }

    public void ResetLives()
    {
        SetLives(defaultLives);
    }

    public void LevelComplete()
    {
        SetLives(lives);
    }

    private void GameOver()
    {
        SetLives(defaultLives);
        GC.GameOver();
        
    }

    public void UpdateLivesText()
    {

        if (livesText)
        {
            livesText.text = lives.ToString();
        }
    }
}
