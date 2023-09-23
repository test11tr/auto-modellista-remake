using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test11
{
    [CreateAssetMenu(fileName = "New Interaction Text Data", menuName = "T11/InteractionTextData")]
    public class InteractionText : ScriptableObject
    {
        [Header("Interaction Zone 3D Texts")]
        public string RaceTrigger3DText;
        public string GarageTrigger3DText;
        public string CarDealerTrigger3DText;
        public string CarTunerTrigger3DText;

        [Header("Interaction Zone UI Texts")]
        public string RaceTriggerText;
        public string GarageTriggerText;
        public string CarDealerTriggerText;
        public string CarTunerTriggerText;
        public string OpenWorldTrigger;

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
