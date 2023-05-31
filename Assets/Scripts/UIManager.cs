﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtBest;

    void Update()
    {
        txtBest.text = "Best: " + GameManager.Instance.best;
        txtScore.text = "Score: " + GameManager.Instance.score;
    }
}
