using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour {
    public Texture2D cursorTXT;

    private void Start() {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.SetCursor(cursorTXT, new Vector3(32, 32), CursorMode.Auto);
    }
}
