using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test11
{
    [CreateAssetMenu(fileName = "New Interaction Scene Data", menuName = "T11/InteractionSceneData")]
    public class InteractionScene : ScriptableObject
    {
        [Header("OpenWorld Scene")]
        public string OpenWorld;

        [Header("Garage Scenes")]
        public string Garage_1;
        public string Garage_2;
        public string Garage_3;

        [Header("Dealer Scene")]
        public string Dealer_1;
        public string Dealer_2;
        public string Dealer_3;

        [Header("Tuner Scene")]
        public string Tuner_1;
        public string Tuner_2;
        public string Tuner_3;
    }
}
