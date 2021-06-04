using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameEvents.current.GetAssests();
    }
}
