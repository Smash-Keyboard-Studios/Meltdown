using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private static float playerMaxHealth = 100f;
    public float playerCurrentHealth = 100f;
    public Image playerHealthBar;
    // Update is called once per frame
    void Update()
    {
        playerHealthBar.fillAmount = playerCurrentHealth / playerMaxHealth;
    }
}
