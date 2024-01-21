using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private string interactableTag = "Interactable";
    [SerializeField] private Image aimPoint;
    [SerializeField] private Sprite grabSprite, hoverSprite;
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionReference interactAction;

    private bool get, canGrab;
    private float startingDistance;
    private Vector3 startingScale, newPosition;
    private RaycastHit[] hits;
    private GameObject selectedItem;

    // Update is called once per frame
    private void Update() {
        CheckSprite();
    }

    void FixedUpdate()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 800, Color.red);

        hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 800f);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag(interactableTag))
                {
                    canGrab = true;
                    selectedItem = hit.transform.gameObject;
                    break;
                }
                else
                {
                    canGrab = false;
                    selectedItem = null;
                }
            }
        }

        if (canGrab)
        {
            if (selectedItem != null && selectedItem.CompareTag(interactableTag))
            {
                // Initialize grabbing
                if (!get)
                {
                    startingDistance = Vector3.Distance(cam.transform.position, selectedItem.transform.position);
                    startingScale = selectedItem.transform.localScale;
                    get = true;
                }
                if (interactAction.action.inProgress)
                {
                    selectedItem.transform.SetParent(cam.transform);
                    Rigidbody selectedRigidbody = selectedItem.GetComponent<Rigidbody>();
                    selectedRigidbody.isKinematic = true;

                    // Calculate the new position along the ray direction
                    float inverseItemScale = selectedItem.transform.localScale.x*-1; 
                    
                    if (!hits[0].transform.gameObject.Equals(selectedItem)) {
                        newPosition = hits[0].point + cam.transform.forward * inverseItemScale; // Move the item closer to the camera by its scale to prevent it from clipping through the walls.
                    } else {
                        newPosition = selectedItem.transform.position; // If the item is not colliding with anything, keep it at its current position.
                    }

                    // Smoothly move the selected item using interpolation (prevents "jittering" effect)
                    selectedItem.transform.position = Vector3.Lerp(selectedItem.transform.position, newPosition, Time.fixedDeltaTime * 1000f);

                    
                    if(get)
                    {
                        float distanceRatio = Vector3.Distance(cam.transform.position, selectedItem.transform.position) / startingDistance;
                        selectedItem.transform.localScale = startingScale * distanceRatio;
                    }
                }
                else
                {
                    get = false;
                    selectedItem.GetComponent<Rigidbody>().isKinematic = false;
                    selectedItem.transform.SetParent(null);
                }
            }
        }
    }

    private void CheckSprite() {
        if (canGrab) {
            aimPoint.sprite = !get ? hoverSprite : grabSprite;
        } else {
            aimPoint.sprite = null;
        }
    }
}
