using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace test11
{
    public class moneyVisualizer : MonoBehaviour
    {

        [SerializeField] private TMP_Text moneyValueText;
        string moneyCountString;
        int moneyCount;

        void FixedUpdate()
        {
            moneyCount = PlayerPrefs.GetInt("Money");
            moneyCountString = moneyCount.ToString("N0");
            moneyValueText.text = moneyCountString + "$";
        }
    }
}
