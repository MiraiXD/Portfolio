using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour {

    [SerializeField]
    public int maxNumberOfFriends = 5;
    int friends = 0;
    
    
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
        StartCoroutine(WaitAndQuit(5));
    }

    IEnumerator WaitAndQuit(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

}
