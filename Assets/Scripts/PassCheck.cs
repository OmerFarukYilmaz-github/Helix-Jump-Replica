using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour 
{

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddScore(2);

        FindObjectOfType<BallController>().perfectPass++;
    }
}
