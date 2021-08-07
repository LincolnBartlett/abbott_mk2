using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator;

public class ItemRaycastEmitter : MonoBehaviour
{

    
    public Vector3 collision = Vector3.zero;
    public LayerMask layer;
    public GameObject currentRayTargetHit;
    public GameObject previousRayTargetHit;
    public GameObject rayOrigin;
    public GameObject bruce;
    public GameObject itemUI;
    public GameObject focusUI;

    private string inputSource;
    private Ray ray;
    public float lookSensitivity = .01f;
    private Quaternion lookRotation;


    // Start is called before the first frame update
    void Start()
    {
        lookRotation = Quaternion.LookRotation(bruce.transform.forward, bruce.transform.up);
        inputSource = "xbox360";
        previousRayTargetHit = null;
        GameCreator.Variables.VariablesManager.SetGlobal("selectedItemName", "");

    }

    void xbox360Input() {
        Vector3 InputAxis = new Vector3(Input.GetAxis("RightHorizontal"), 0, (Input.GetAxis("RightVertical") * -1));
        if (InputAxis != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(InputAxis, bruce.transform.up);
            rayOrigin.transform.rotation = Quaternion.Slerp(rayOrigin.transform.rotation, lookRotation, lookSensitivity);
        }
        else
        {
            rayOrigin.transform.rotation = Quaternion.Slerp(rayOrigin.transform.rotation, bruce.transform.rotation, lookSensitivity *  5);
        }
    }


    void mouseInput() {
        /*
         * Did not write this
         * so will need work
         */
        var groundPlane = new Plane(Vector3.up, -rayOrigin.transform.position.y);
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance;

        if (groundPlane.Raycast(mouseRay, out hitDistance))
        {
            var lookAtPosition = mouseRay.GetPoint(hitDistance);
            var targetRotation = Quaternion.LookRotation(lookAtPosition - rayOrigin.transform.position, Vector3.up);
            var rotation = Quaternion.Lerp(rayOrigin.transform.rotation, targetRotation, lookSensitivity);
            rayOrigin.transform.rotation = rotation;
        }
    }

    void LookController() {

        //Fast and dirty quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (inputSource) {
            case "xbox360":
                xbox360Input();
                break;
            case "mouse":
                mouseInput();
                break;
            default:
                break;
        }      
    }
     
    void RayConroller() {
        ray = new Ray(rayOrigin.transform.position, rayOrigin.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, .2f, out hit, 100f))
        {
            currentRayTargetHit = hit.transform.gameObject;

            if (previousRayTargetHit) 
            {
                if (previousRayTargetHit != currentRayTargetHit) {
                    //Remove Highlight from previous
                    if (previousRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>()) {
                        previousRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;
                    }
                    //Remove HeadTrack from previous
                    if (previousRayTargetHit.GetComponentInChildren<GameCreator.Core.HPHeadTrack>())
                    {
                        SphereCollider sphere = previousRayTargetHit.GetComponentInChildren<GameCreator.Core.HPHeadTrack>().GetComponents<SphereCollider>()[0];
                        sphere.radius = 0;
                    }
                    GameCreator.Variables.VariablesManager.SetGlobal("selectedItemName", "");
                }
            }

            if (currentRayTargetHit)
            {
                //Check if Focus is active
                if (focusUI.activeInHierarchy == true)
                {
                    //Add Highlight to current
                    if (currentRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>())
                    {
                        currentRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;

                    }
                }
                else {
                    //Remove Highlight from current
                    if (currentRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>())
                    {
                        currentRayTargetHit.GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;

                    }
                }


                //Add HeadTrack to current
                if (currentRayTargetHit.GetComponentInChildren<GameCreator.Core.HPHeadTrack>()) {

                    Component headTrack = currentRayTargetHit.GetComponentInChildren<GameCreator.Core.HPHeadTrack>();
                    SphereCollider sphere = headTrack.GetComponentInChildren<SphereCollider>();
                    sphere.radius = 5;     
                }


                //Get current variable info and push name to UI
                if (currentRayTargetHit.GetComponent<GameCreator.Variables.LocalVariables>())
                {
                    var itemName = GameCreator.Variables.VariablesManager.GetLocal(currentRayTargetHit, "name");
                    GameCreator.Variables.VariablesManager.SetGlobal("selectedItemName", itemName);
                    GameCreator.Variables.VariablesManager.SetGlobal("selectedItem", currentRayTargetHit);
                    //itemUI.SetActive(true);

                }
                else
                {
                    //itemUI.SetActive(false);
                    GameCreator.Variables.VariablesManager.SetGlobal("selectedItemName", "");
                    GameCreator.Variables.VariablesManager.SetGlobal("selectedItem", null);


                }

                previousRayTargetHit = currentRayTargetHit;
            }

            collision = hit.point;

 
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookController();
        RayConroller();
    }

    private void OnDrawGizmos()
    {
        
     Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collision, .2f);
        Gizmos.DrawRay(ray);
 
        
    }
}
