

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    private Transform cam;
    private bool clicked;
    public GameObject cube;
    private float distance;
    private float newDistance;
    Vector3 startingScale;
    float startingDistance;
    RaycastHit cubeHit;
    bool get;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.Find("camera");
        startingDistance = 0;
        startingScale = new Vector3(0,0,0);
        get = false;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit[] hits;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 800, Color.red);
        hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 800.0f);

        if (hits.Length > 1)
        {
            if(hits[1].transform.name.Equals(cube.name)) {
                cubeHit = hits[1];
                if (!get)
                {
                    startingDistance = Vector3.Distance(transform.position, cube.transform.position);
                    startingScale = cubeHit.transform.localScale;
                    get = true;
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    cubeHit.transform.SetParent(GameObject.Find("camera").transform);
                    cubeHit.rigidbody.isKinematic = true;

                    cubeHit.transform.position = hits[0].point;

                    if (get)
                    {
                        float distanceRatio = Vector3.Distance(transform.position, cube.transform.position) / startingDistance;
                        
                        cubeHit.transform.localScale = startingScale * distanceRatio;
                        Debug.Log(cubeHit.transform.localScale);
                    }

                }
                else
                {
                    get = false;
                    cubeHit.rigidbody.isKinematic = false;
                    cubeHit.transform.SetParent(null);
                }
            }
        }
    }
}
