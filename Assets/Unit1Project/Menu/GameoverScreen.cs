using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameoverScreen : MonoBehaviour
{
    public TextMeshProUGUI headerTextMeshPro;
    public static string headerText;

    public static bool won;
    public static int blocksMined;
    public static int blocksPlaced;
    public static int itemsCrafted;
    public static int enemiesKilled;

    public static float timeSurvivedOrTaken;


    void Start()
    {
        var headerText = won ? "YOU WIN" : "YOU LOSE";
        var timeTakenVerb = won ? "Taken" : "Survived";
        headerTextMeshPro.text = $"{headerText}\n\nTime {timeTakenVerb}: {timeSurvivedOrTaken} seconds\nBlocks Mined: {blocksMined}\nBlocks Placed: {blocksPlaced}\nItems Crafted: {itemsCrafted}\nEnemies Killed: {enemiesKilled}";
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        print("exit");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
