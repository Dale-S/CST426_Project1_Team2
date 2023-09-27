using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimerManager : MonoBehaviour
{
    //Timer Variables
    private float loadBuffer = 5f;
    private float minutes = .2f;
    private float dayTime;
    private float timeLeft = 0f;
    
    //Day tracking
    private int dayCount = 1;
    private bool dayOver = false;
    private int maxDays = 3;
    
    //UI elements
    public TextMeshProUGUI day;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI total;
    public TextMeshProUGUI today;
    public TextMeshProUGUI pastDay;
    public GameObject nextDayButton;
    public GameObject gameplayUI;
    
    public MoneyHandler MH;
    public MinigameManager MM;
    

    public GameObject DayEndScreen;
    // Start is called before the first frame update
    void Start()
    {
        dayTime = (60 * minutes);
        timeLeft = dayTime + loadBuffer;
        day.text = $"{dayCount}";
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0.0)
        {
            timeLeft -= 1 * Time.deltaTime;
            if (Math.Floor(timeLeft % 60) < 10)
            {
                timer.text = $"{Math.Floor(timeLeft / 60)}:0{Math.Floor(timeLeft % 60)}";
            }
            else
            {
                timer.text = $"{Math.Floor(timeLeft / 60)}:{Math.Floor(timeLeft % 60)}";
            }
        }
        else
        {
            DayEndScreen.SetActive(true);
            gameplayUI.SetActive(false);
            if (!dayOver)
            {
                pastDay.text = $"End of Day: {dayCount}";
                total.text = $"Total Money Earned: ${MH.calcTotal()}";
                today.text = $"Money Earned Today: ${MH.returnFunds()}";
                MH.emptyCurrent();
                dayOver = true;
                //MM.disableAll();
            }
            if (dayCount >= maxDays)
            {
                nextDayButton.SetActive(false);
            }
        }
    }
    
    public void nextDay()
    {
        dayOver = false;
        DayEndScreen.SetActive(false);
        gameplayUI.SetActive(true);
        timeLeft = dayTime + loadBuffer;
        dayCount++;
        //MM.enableAll();
    }
}


