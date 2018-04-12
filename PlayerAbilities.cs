using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour {

    int coins = 0;
    public Text coinText;
    public GameObject shopPanel;
    public Image disabledCursor;

    public delegate void CoinDelegate(int _coins);
    public event CoinDelegate coinEvent;

    // For abilities
    

    // Use this for initialization
    void Start () {
        UpdateCoins();
        shopPanel.SetActive(false);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) Shop();
    }

    public void AddCoins(int _coins)
    {
        coins += _coins;

        UpdateCoins();
    }

    void UpdateCoins()
    {
        coinText.text = "Coins: " + coins;

        if(coinEvent != null)
            coinEvent(coins);
    }

    void Shop()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            shopPanel.SetActive(true);
            disabledCursor.enabled = false;
            Cursor.visible = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            shopPanel.SetActive(false);
            disabledCursor.enabled = true;
            Cursor.visible = false;
        }

    }

   public void AcquireAbility(Ability.Abilities ability, int cost)
    {
        AddCoins(-cost);
        print(ability);
        switch (ability)
        {
            case Ability.Abilities.FastInternet:
                ActivateFastInternet();
                break;
            case Ability.Abilities.WebBot:
                ActivateWebBot();
                break;
            case Ability.Abilities.SendDickPic:
                ActivateSendNudes();
                break;
            case Ability.Abilities.OffendWomen:
                ActivateOffendWomen();
                break;
            case Ability.Abilities.ThinkImGay:
                ActivateGayPost();
                break;
            case Ability.Abilities.ExtraverticDecoy:
                ActivateExtraverticDecoy();
                break;
        }
    }

    void ActivateSendNudes()
    {        
        FindObjectOfType<SendNudes>().Enable();
    }

    void ActivateOffendWomen()
    {
        FindObjectOfType<OffendWomen>().Enable();
    }

    void ActivateGayPost()
    {
        FindObjectOfType<GayPost>().Enable();
    }

    void ActivateExtraverticDecoy()
    {
        FindObjectOfType<ExtraverticDecoy>().Enable();
    }

    void ActivateFastInternet()
    {
        FindObjectOfType<FastInternet>().Enable();
    }

    void ActivateWebBot()
    {
        FindObjectOfType<WebBot>().Enable();
    }


}
