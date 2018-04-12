using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour {

    Button button;
    Text text;
    public int cost;
    public int playerCoins;
    public enum Abilities
    {
        FastInternet,
        WebBot,
        SendDickPic,
        OffendWomen,
        ThinkImGay,
        ExtraverticDecoy
    }

    //int[] abilityCosts = { 500, 1500, 3000, 5000, 6000, 10000 };
    bool alreadyBought = false;
    public Abilities ability;
    PlayerAbilities playerAbilities;
    
    void SetAbility()
    {
        //cost = abilityCosts[(int)ability];                
    }

    // Use this for initialization
    void Start () {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        

        playerAbilities = FindObjectOfType<PlayerAbilities>();
        playerAbilities.coinEvent += UpdatePlayerCoins;
        button.interactable = false;
        button.onClick.AddListener(BuyAbility);

        SetAbility();
        text.text = text.text + ": " + cost;
    }
	
    void BuyAbility()
    {
        AcquireAbility(ability, cost);
    }

    void AcquireAbility(Abilities _ability, int _cost)
    {
        alreadyBought = true;
        button.interactable = false;
        playerAbilities.AcquireAbility(_ability, _cost);
    }

    void UpdatePlayerCoins(int _playerCoins)
    {
        playerCoins = _playerCoins;
        SetButton();
    }

    void SetButton()
    {
        if (!alreadyBought)
        {
            if (playerCoins >= cost) button.interactable = true;
            else button.interactable = false;
        }
    }
    
}
