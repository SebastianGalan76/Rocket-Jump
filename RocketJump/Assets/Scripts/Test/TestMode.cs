using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMode : MonoBehaviour
{
    public static bool testMode = true;

    private void Start() {
        Debug.Log("Test Mode: " + testMode);
    }
}
