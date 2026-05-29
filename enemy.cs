using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    [SerializeField] float rightPoint;
    [SerializeField] float leftPoint;
    bool movingRight = true;
    [SerializeField] float rotationSpeed = 90f; // degrees per second
    float currentAngle = 0f;



    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;

        // Apply rotation (around Z axis for 2D, Y axis for 3D)
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        if (movingRight)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

            if (transform.position.x >= rightPoint)
                movingRight = false;
        }
        else
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

            if (transform.position.x <= leftPoint)
                movingRight = true;
        }
    }
}
