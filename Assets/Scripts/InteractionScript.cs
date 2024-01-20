using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private string interactableTag = "Interactable";
    [SerializeField] private Image aimPoint;
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionReference interactAction;

    private bool get, canGrab;
    private float startingDistance;
    private Vector3 startingScale, newPosition;
    private RaycastHit[] hits;
    private GameObject selectedItem;

    // Update is called once per frame
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
                    aimPoint.color = Color.green;
                    canGrab = true;
                    selectedItem = hit.transform.gameObject;
                    break;
                }
                else
                {
                    aimPoint.color = Color.white;
                    canGrab = false;
                    selectedItem = null;
                }
            }
        }

        if (canGrab)
        {
            if (selectedItem != null && selectedItem.CompareTag(interactableTag))
            {
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
                    //float distanceAlongRay = startingDistance + Vector3.Distance(selectedItem.transform.position, hits[0].point);
                    Vector3 distanceAlongRay = selectedItem.transform.localScale * 0.5f;
                    if (!hits[0].transform.gameObject.Equals(selectedItem)) {
                        newPosition = hits[0].point + cam.transform.forward * -distanceAlongRay.z;
                    } else {
                        newPosition = selectedItem.transform.position;
                    }

                    // Smoothly move the selected item using interpolation
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
                    Rigidbody selectedRigidbody = selectedItem.GetComponent<Rigidbody>();
                    selectedRigidbody.isKinematic = false;
                    selectedItem.transform.SetParent(null);
                }
            }
        }
    }
}
