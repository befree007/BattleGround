using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDamageText : MonoBehaviour
{
    public Text damageText;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        DamageTaken();
    }

    public void DamageTaken()
    {
        Instantiate(damageText, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
    }
}
