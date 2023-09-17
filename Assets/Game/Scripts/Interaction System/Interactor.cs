using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test11
{
    interface IInteractable
    {
        void Interact();
    }

    public class Interactor : MonoBehaviour
    {
        [SerializeField] private GameObject interactUIContainer;
        public float interactRange;
        bool UIAvailable;

        void Update()
        {

            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            bool anyInteractable = false;

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactObj))
                {
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
