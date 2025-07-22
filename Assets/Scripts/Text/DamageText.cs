using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("config")]
    [SerializeField] private TextMeshProUGUI damageTMP;

    public void SetDamageText(float damage)
    {
        damageTMP.text = damage.ToString();
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
