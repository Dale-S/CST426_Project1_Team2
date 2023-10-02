using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioSource coffeePour;
    public AudioSource milkPour;
    public AudioSource sugarPour;
    public AudioSource iceClink;

    private void Awake()
    {
        // Ensure only one instance of the SoundManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Don't destroy the SoundManager when changing scenes
        DontDestroyOnLoad(gameObject);

        // Initialize audio sources
        coffeePour = transform.GetChild(0).GetComponent<AudioSource>(); // Adjust index as needed
        milkPour = transform.GetChild(1).GetComponent<AudioSource>();
        sugarPour = transform.GetChild(2).GetComponent<AudioSource>();
        iceClink = transform.GetChild(3).GetComponent<AudioSource>();
    }

    // Play CoffeePour
    public void PlayCoffeePour()
    {
        coffeePour.Play();
    }

    // Play milkPour
    public void PlayMilkPour()
    {
        milkPour.Play();
    }
    
    //Play sugarPour
    public void PlaySugarPour()
    {
        sugarPour.Play();
    }
    
    //Play iceClink
    public void PlayIceClink()
    {
        iceClink.Play();
    }
    // Add more methods to play additional sounds as needed
}
