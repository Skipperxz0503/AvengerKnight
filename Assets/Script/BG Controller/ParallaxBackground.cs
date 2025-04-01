using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;


        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float disctaceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3 (distanceToMove + xPosition, transform.position.y);

        if (disctaceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if (disctaceMoved < xPosition - length)
        { 
            xPosition = xPosition - length;
        }
    }
}
