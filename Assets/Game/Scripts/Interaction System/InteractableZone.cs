using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using test11;
using PG;
using TMPro;
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
            CarDealerTrigger,
            CarTunerTrigger,
            OpenWorldTrigger,
            MoneyAdder,
        }

        [Header("Interaction Datas")]
        public InteractionText _interactTextData;
        public InteractionScene _interactSceneData;
        public TMP_Text interact3DText;
        string interactionText;
        string levelName;

        private void Start()
        {
            if (sceneType == SceneTypes.RaceTrigger)
            {
                interact3DText.text = _interactTextData.RaceTrigger3DText;
            }
            else if (sceneType == SceneTypes.GarageTrigger)
            {
                interact3DText.text = _interactTextData.GarageTrigger3DText;
            }
            else if (sceneType == SceneTypes.CarDealerTrigger)
            {
                interact3DText.text = _interactTextData.CarDealerTrigger3DText;
            }
            else if (sceneType == SceneTypes.CarTunerTrigger)
            {
                interact3DText.text = _interactTextData.CarTunerTrigger3DText;
            }
            else if (sceneType == SceneTypes.MoneyAdder)
            {
                interact3DText.text = "Add Money";
            }
            else
            {
                interact3DText.text = "";
            }
        }

        //INTERACT INTERFACE

        public void Interact()
        {
            if (sceneType == SceneTypes.RaceTrigger)
            {
                //
            }
            else if (sceneType == SceneTypes.GarageTrigger)
            {
                //
            }
            else if (sceneType == SceneTypes.CarDealerTrigger)
            {
                SceneManager.LoadScene(_interactSceneData.Dealer_1);
            }
            else if (sceneType == SceneTypes.CarTunerTrigger)
            {
                //
            }
            else if (sceneType == SceneTypes.OpenWorldTrigger)
            {
                SceneManager.LoadScene(_interactSceneData.OpenWorld);
            }
            else if (sceneType == SceneTypes.MoneyAdder)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 10000);
            }
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
            else if (sceneType == SceneTypes.CarDealerTrigger)
            {
                return interactionText = _interactTextData.CarDealerTriggerText;
            }
            else if (sceneType == SceneTypes.CarTunerTrigger)
            {
                return interactionText = _interactTextData.CarTunerTriggerText;
            }
            else if (sceneType == SceneTypes.OpenWorldTrigger)
            {
                return interactionText = _interactTextData.OpenWorldTrigger;
            }
            else if (sceneType == SceneTypes.MoneyAdder)
            {
                return interactionText = "Add 10.000$";
            }

            return interactionText = _interactTextData.defaultText;
        }

        //
    }
}
