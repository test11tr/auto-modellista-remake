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
        [Header("Interaction Zone")]
        [Tooltip("Etkilesime gecilebilecek sahne secenekleri.")]
        public SceneTypes sceneType;
        public enum SceneTypes
        {
            RaceTrigger,
            GarageTrigger,
            GarageExitTrigger,
            CarDealerTrigger,
            CarDealerExitTrigger,
            CarTunerTrigger,
            CarTunerExitTrigger
        }

        [Header("Interaction Text Data")]
        public InteractionText _interactTextData;
        string interactionText;


        //INTERACT INTERFACE

        public void Interact()
        {
            //SceneManager.LoadScene();
        }

        public string GetInteractionText()
        {
            if (sceneType == SceneTypes.RaceTrigger)
            {
                return interactionText = _interactTextData.RaceTriggerText;
            }
            else if (sceneType == SceneTypes.GarageTrigger)
            {
                return interactionText = _interactTextData.GarageTriggerText;
            }
            else if (sceneType == SceneTypes.GarageExitTrigger)
            {
                return interactionText = _interactTextData.GarageExitTriggerText;
            }
            else if (sceneType == SceneTypes.CarDealerTrigger)
            {
                return interactionText = _interactTextData.CarDealerTriggerrText;
            }
            else if (sceneType == SceneTypes.CarDealerExitTrigger)
            {
                return interactionText = _interactTextData.CarDealerExitTriggerText;
            }
            else if (sceneType == SceneTypes.CarTunerTrigger)
            {
                return interactionText = _interactTextData.CarTunerTriggerText;
            }
            else if (sceneType == SceneTypes.CarTunerExitTrigger)
            {
                return interactionText = _interactTextData.CarTunerExitTriggerText;
            }

            return interactionText = _interactTextData.defaultText;
        }

        //
    }
}
