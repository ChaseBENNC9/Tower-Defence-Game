using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = gameObject.transform.localScale.x; //sets the initial scale of the health bar to 100%
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 tmpScale = gameObject.transform.localScale; //sets the intitial value of tmpscale to the current healthbar scale.
        tmpScale.x = currentHealth / maxHealth * originalScale; //scales tmpscale  on the x axis to the percentage of health left
        gameObject.transform.localScale = tmpScale; //scales the health bar to the value of tmpscale
    }
}
