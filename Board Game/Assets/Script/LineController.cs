using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineController : MonoBehaviour
{
    public LineRenderer line1;
    private bool isPress = false;
    private bool isFire = false;

    public IDictionary<Rigidbody, bool> dicRB = new Dictionary<Rigidbody, bool>();
    public List<touchLocation> touches = new List<touchLocation>();

    void Awake(){
        Rigidbody[] arrRB = UnityEngine.Object.FindObjectsOfType<Rigidbody>();
        for (int i = 0; i < arrRB.Length; i++) {
            dicRB.Add(arrRB[i],false);
        }
    }

    void Update(){
        int i = 0;

        while(i < Input.touchCount){
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began){
                if (Camera.main.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, Camera.main.nearClipPlane)).z > 1.38f) {
                    Ray ray = Camera.main.ScreenPointToRay(t.position);
                    RaycastHit hit;
                    bool a = true;

                    if (Physics.Raycast(ray, out hit)){
                        foreach(KeyValuePair<Rigidbody, bool> kvp in dicRB) {
                            if (hit.transform.name == kvp.Key.name){
                                touches.Add(new touchLocation(t.fingerId, kvp.Key));
                                a = false;
                                dicRB[kvp.Key] = true;
                                break;
                            }
                        }
                        if (a) {
                            touches.Add(new touchLocation(t.fingerId, null));
                        }
                    }
                } else touches.Add(new touchLocation(t.fingerId, null));   
            } else if (t.phase == TouchPhase.Ended){
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);

                if (thisTouch.rb != null){
                    dicRB[thisTouch.rb] = false;
                    if (isFire){
                        thisTouch.rb.AddForce(new Vector3(0f, 0f, -35f), ForceMode.Impulse);
                        isFire = false;
                    }
                }
                touches.RemoveAt(touches.IndexOf(thisTouch));    
            } 
            i++;
        }
    }

    void OnTriggerExit(Collider other) {
        Rigidbody otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody != null && dicRB.ContainsKey(otherRigidbody)){
                line1.SetPosition(1, line1.GetPosition(0));
                isFire = false;
        }
    }

    void OnTriggerStay(Collider other){ 
    Rigidbody otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody != null && dicRB.ContainsKey(otherRigidbody)){
            if (dicRB[otherRigidbody]) {
                line1.SetPosition(1, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z + 0.5f));
                isFire = true;
            }
        }
    }
}
