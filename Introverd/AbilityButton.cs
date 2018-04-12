using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

    protected Button button;
    protected Image image;
    protected Image disabledImage;
    protected Text text;

    protected float rechargeTime;
    protected IEnumerator currentCoroutine;

	// Use this for initialization
	public void Start () {
        button = GetComponent<Button>();
        Image[] images = GetComponentsInChildren<Image>();
        image = images[0];
        disabledImage = images[1];        
        text = GetComponentInChildren<Text>();

        button.interactable = false;
        image.enabled = false;
        disabledImage.enabled = false;
        text.enabled = false;

        button.onClick.AddListener(OnClick);        
	}
    

    public void Enable()
    {
        button.interactable = true;
        image.enabled = true;
        disabledImage.enabled = true;
        text.enabled = true;
    }

    public void OnClick()
    {
        Ability();
        Recharge();
    }

    public void SetRechargeTime(float _rechargeTime)
    {
        rechargeTime = _rechargeTime;
    }

    public void Recharge()
    {
        //rechargeTime = _rechargeTime;
        
        if (currentCoroutine == null)
        {
            currentCoroutine = RechargeCoroutine();
            StartCoroutine(RechargeCoroutine());
        }
    }

    public IEnumerator RechargeCoroutine()
    {
        //recharging = true;
        button.interactable = false;

        disabledImage.fillAmount = 1;
        while (disabledImage.fillAmount > 0)
        {
            disabledImage.fillAmount -= Time.deltaTime / rechargeTime;
            yield return null;
        }
        currentCoroutine = null;
        button.interactable = true;
        //recharging = false;
    }

    public virtual void Ability()
    {

    }

    
}
