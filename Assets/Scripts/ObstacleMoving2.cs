using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoving2 : MonoBehaviour
{
    public float startTime;
    public float minX, maxX;

    [Range(1, 100)]
    public float moveSpeed;
    private int sign = -1;

    void Update()
    {
        if (Time.time >= startTime)
        {
            transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime * sign);
            if (transform.position.z <= minX || transform.position.z >= maxX)
            {
                sign *= -1;
            }
        }
    }
}
