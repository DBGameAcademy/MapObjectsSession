using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindowUI : MonoBehaviour
{
    public Text HealthText;
    public Image HealthBar;

    public void UpdateHUD(Player _player)
    {
        HealthText.text = _player.CurrentHealth.ToString() + " / " + _player.MaxHealth.ToString();
        float normalisedHealth = (float)_player.CurrentHealth / (float)_player.MaxHealth;
        HealthBar.fillAmount = normalisedHealth;
    }
}
