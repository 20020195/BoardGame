using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchLocation
{
    public int touchId;
    public Rigidbody rb;
    public bool isFire;
    public bool isPress;
    public touchLocation (int newtouchId, Rigidbody newRb){
        touchId = newtouchId;
        rb = newRb;
        
    }
}
