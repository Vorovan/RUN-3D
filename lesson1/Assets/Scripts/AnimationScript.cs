using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    int i = 1;
    void Start()
    {

    }
    void OnCollisionEnter(Collision other)
    {
        gameObject.GetComponent<Animator>().SetBool("1", false);
    }
    void OnCollisionExit(Collision other)
    {
        gameObject.GetComponent<Animator>().SetBool("1", true);
    }
}
