
using System.Collections;
using UnityEngine;

class DragAndDrop : MonoBehaviour
{
    private Color mouseOverColor = Color.blue;
    private Color originalColor = Color.yellow;
    private bool dragging = false;
    private float distance;
    [SerializeField]
    Camera camera;


    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, camera.transform.position);
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }
}

