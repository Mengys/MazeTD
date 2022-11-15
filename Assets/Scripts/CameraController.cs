using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _zoomScale = 10f;
    private Camera _camera;

    private void Start() 
    {
        _camera = Camera.main;
    }

    private void Update() 
    {
        float mouseScroleInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroleInput != 0)
        {
            MoveCamera(mouseScroleInput);
        }
    }

    private void MoveCamera(float mouseScroleInput)
    {
        float newCameraSize = _camera.orthographicSize - mouseScroleInput * _zoomScale;
        if (newCameraSize >= 5 && newCameraSize <= 20)
        {
            Vector3 mousePosition1 = Input.mousePosition;
            Vector3 p1 = _camera.ScreenToWorldPoint(new Vector3(mousePosition1.x, mousePosition1.y, 0));
            _camera.orthographicSize = newCameraSize;
            Vector3 mousePosition2 = Input.mousePosition;
            Vector3 p2 = _camera.ScreenToWorldPoint(new Vector3(mousePosition2.x, mousePosition2.y, 0));
            Vector3 tramslateVector = p1 - p2;
            tramslateVector.z = 0;
            transform.Translate(tramslateVector);
        }
    }
}
