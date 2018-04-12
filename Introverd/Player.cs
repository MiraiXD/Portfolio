// This script manages scenes and gameover

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour {

    // You lose the game when you reach maxNumberOfFriends
    [SerializeField]
    public int maxNumberOfFriends = 5;
    // Current number of friends
    int friends = 0;
    //Time to wait after the game has been lost
    int timeToQuit = 5;
    // Invoked when an enemy has gone through your defences
    public void NewFriend()
    {
        friends++;
        if (friends >= maxNumberOfFriends) EndGame();
    }

    void EndGame()
    {
        GameObject gameOver = GameObject.FindWithTag("GameOver");
        gameOver.GetComponent<Image>().enabled = true;
        gameOver.GetComponentInChildren<Text>().enabled = true;
        StartCoroutine(WaitAndQuit(timeToQuit));
    }

    IEnumerator WaitAndQuit(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

    private void Update()
    {
        // Load the menu scene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

}
