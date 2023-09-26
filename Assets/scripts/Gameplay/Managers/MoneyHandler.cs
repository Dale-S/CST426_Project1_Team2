using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    private int currMoney = 0;
    private int total = 0;
    public TextMeshProUGUI moneyText;
    // Start is called before the first frame update
    void Start()
    {
        currMoney = 0;
        total = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + currMoney;
    }

    public void addFunds(int amount)
    {
        currMoney += amount;
    }

    public void removeFunds(int amount)
    {
        currMoney -= amount;
    }

    public int returnFunds()
    {
        return currMoney;
    }

    public int calcTotal()
    {
        total += currMoney;
        return total;
    }

    public void emptyCurrent()
    {
        currMoney = 0;
    }
}
