using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test11
{
    [CreateAssetMenu(fileName = "New Interaction Text Data", menuName = "T11/Interaction")]
    public class InteractionText : ScriptableObject
    {
        [Header("Interaction Zone Texts")]
        public string RaceTriggerText;
        public string GarageTriggerText;
        public string GarageExitTriggerText;
        public string CarDealerTriggerrText;
        public string CarDealerExitTriggerText;
        public string CarTunerTriggerText;
        public string CarTunerExitTriggerText;

        [Header("Vehicle Interaction Texts")]
        public string InRaceAreaText;
        public string InOpenWorldText;
        public string InGarageText;
        public string InDealerText;
        public string InTunerText;

        [Header("Default Text")]
        public string defaultText;
    }
}
