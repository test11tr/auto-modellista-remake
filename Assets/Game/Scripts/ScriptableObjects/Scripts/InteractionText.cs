using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test11
{
    [CreateAssetMenu(fileName = "New Interaction Text Data", menuName = "T11/Interaction")]
    public class InteractionText : ScriptableObject
    {
        [Header("Interaction Texts")]
        public string RaceTriggerText;
        public string GarageTriggerText;
        public string GarageExitTriggerText;
        public string CarDealerTriggerrText;
        public string CarDealerExitTriggerText;
        public string CarTunerTriggerText;
        public string CarTunerExitTriggerText;
        public string defaultText;
    }
}
