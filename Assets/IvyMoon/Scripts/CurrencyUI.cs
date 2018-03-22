using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IvyMoon;

public class CurrencyUI : MonoBehaviour {

    currency coins;//reference the currency script and give it a name in this script
    Score score;

    public bool useScore;
    public bool useGold;
    public bool showText;
    string scoretext = "Score: ";
    public Text scoreText;
    string goldtext = "Gold: ";
    public Text goldText;
    string silvertext = "Silver: ";
    public Text silverText;
    string coppertext = "Copper: ";
    public Text copperText;

    // Use this for initialization
    void Start()
    {
        coins = (currency)GameObject.FindObjectOfType(typeof(currency));
        score = (Score)GameObject.FindObjectOfType(typeof(Score));
    }

    // Update is called once per frame
    void Update()
    {

        if (useGold == true)
        {

            if (showText == true)
            {
                if (goldText != null)
                {
                    goldText.text = goldtext + coins.gold.ToString();
                }
                if (silverText != null)
                {
                    silverText.text = silvertext + coins.silver.ToString();
                }
                if (copperText != null)
                {
                    copperText.text = coppertext + coins.copper.ToString();
                }
            }
            else
            {
                if (goldText != null)
                {
                    goldText.text = coins.gold.ToString();
                }
                if (silverText != null)
                {
                    silverText.text = coins.silver.ToString();
                }
                if (copperText != null)
                {
                    copperText.text = coins.copper.ToString();
                }
            }
        }
        else
        {
            goldText.text = null;
            silverText.text = null;
            copperText.text = null;
        }
        if (useScore == true)
        {
            if (scoreText != null)
            {
                if (showText == true)
                {
                    scoreText.text = scoretext + score.score.ToString();
                }
                else
                {
                    scoreText.text = score.score.ToString();
                }
            }

        }
        else
        {
            scoreText.text = null;
        }
    }
}
