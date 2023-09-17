using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PG;

namespace test11
{
    interface IInteractable
    {
        string GetInteractionText();
        void Interact();
    }

    public class Interactor : MonoBehaviour
    {
        [SerializeField] private GameObject interactUIContainer;
        public float interactRange;
        public TMP_Text _interactionText;

        void Update()
        {

            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            bool anyInteractable = false;

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactObj))
                {
                    _interactionText.text = interactObj.GetInteractionText();
                    anyInteractable = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                    }
                }
            }

            handleUI(anyInteractable);
        }

        private void handleUI(bool interactable)
        {
            if (interactable)
            {
                interactUIContainer.SetActive(true);
            }
            else
            {
                interactUIContainer.SetActive(false);
            }
        }
    }
}
