using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damageText;
    public string template;

    public void SetDamage(int damage)
    {
        damageText.text = string.Format(template, damage);
    }
}
