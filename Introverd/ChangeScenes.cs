using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScenes : MonoBehaviour {

    public Button newGameButton;
    public Button quitButton;

	// Use this for initialization
	void Start () {
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
	}
	
    void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("MenuScene");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
