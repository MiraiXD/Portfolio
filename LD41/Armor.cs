using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class describes all items, that can be put on and worn - armor. Wearing armor heals the player and increases his attack speed
public class Armor : Interactable {
    // Enum type to select a part of armor
    public enum ArmorType { Shirt, Shoes, Helmet, PlateArmor, PlateLegs, Pants};
    // Set in the inspector which part of armor should this script represent
    public ArmorType armorType;
    // Amount we're healed for, when put on
    public int healthBoost;
    // Player transform
    Transform player;

    // Invoke Interactable's Start() to find the needed Text object
    new void Start()
    {
        base.Start();
    }

    // Overridden function invoked, when player decides to interact with the dropped armor
    public override void Interact()
    {        
        base.Interact();
        PutOn();
        Destroy(gameObject);

    }

    // When picking up armor, decides which part of armor on the player to set active, increases attack speed and heals the player
    void PutOn()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        player.GetComponent<Controller>().IncreaseAttackSpeed();
        player.GetComponent<Health>().HealthBoost(healthBoost);
        switch (armorType)
        {
            case ArmorType.Shirt:
                player.Find("Shirt").gameObject.SetActive(true);
                break;
            case ArmorType.Shoes:
                player.Find("Shoes").gameObject.SetActive(true);
                break;
            case ArmorType.Helmet:
                player.Find("Hair").gameObject.SetActive(false);
                player.Find("Helmet").gameObject.SetActive(true);
                break;
            case ArmorType.PlateArmor:
                player.Find("Shirt").gameObject.SetActive(false);
                player.Find("PlateArmor").gameObject.SetActive(true);
                break;
            case ArmorType.PlateLegs:
                player.Find("Pants").gameObject.SetActive(false);
                player.Find("PlateLegs").gameObject.SetActive(true);
                break;
            case ArmorType.Pants:                
                player.Find("PlateLegs").gameObject.SetActive(false);
                player.Find("Pants").gameObject.SetActive(true);
                break;


        }
    }
    
}
