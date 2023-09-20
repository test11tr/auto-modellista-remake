using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using test11;
using PG;
using UnityEngine.SceneManagement;

namespace test11
{
    public class InteractableZone : MonoBehaviour, IInteractable
    {
        public string interactionText = "Interact";
        [Header("Load Scene")]
        [Tooltip("Etkilesime gecilebilecek sahne secenekleri.")]
        public SceneTypes sceneType;
        public enum SceneTypes
        {
            ToRace,
            ToGarage,
            ExitGarage,
            ToVehicleDealer,
            ExitVehicleDealer,
            ToVehicleTuner,
            ExitVehicleTuner
        }

        void Awake()
        {
        }

        //INTERACT INTERFACE

        public void Interact()
        {
            //SceneManager.LoadScene();
        }

        public string GetInteractionText()
        {
            if (sceneType == SceneTypes.ToRace)
            {
                return interactionText = "Race!";
            }
            else if (sceneType == SceneTypes.ToGarage)
            {
                return interactionText = "Enter Garage";
            }
            else if (sceneType == SceneTypes.ExitGarage)
            {
                return interactionText = "Exit Garage";
            }
            else if (sceneType == SceneTypes.ToVehicleDealer)
            {
                return interactionText = "Enter Vehicle Dealer";

            }
            else if (sceneType == SceneTypes.ExitVehicleDealer)
            {
                return interactionText = "Exit Vehicle Dealer";
            }
            else if (sceneType == SceneTypes.ToVehicleTuner)
            {
                return interactionText = "Enter Vehicle Tuner";
            }
            else if (sceneType == SceneTypes.ExitVehicleTuner)
            {
                return interactionText = "Exit Vehicle Tuner";
            }

            return interactionText = "Interact";
        }

        ///
    }
}
