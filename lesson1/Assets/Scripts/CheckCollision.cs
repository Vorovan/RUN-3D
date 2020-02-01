using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    float time;

    void Update()
    {
        if (time < 1)
            time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.name == "LOSE")
        {
            Destroy(gameObject);
        }
        else if (trigger.name == "Bonus" || trigger.name == "SpeedBonus")
        {
            if (time < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
