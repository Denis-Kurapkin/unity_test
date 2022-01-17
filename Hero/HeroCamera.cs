using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;
    public float yawSpeed = 100f;

    public float currentZoom = 10f;
    public float currentYaw = 0f;
 

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; //приближение камеры при помощи колесика мыши
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom); //ограничение зума
         
        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime; //перемещение камеры по часовой стрелке или против

        transform.position = target.position - offset * currentZoom; /* позиционирование камеры
                                                                      */
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw); //вращение камеры
    }
}
