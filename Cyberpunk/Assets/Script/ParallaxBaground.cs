using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBaground : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float parallaxEffect;
    private float lenght, starpos;

    private void Start()
    {
        starpos = transform.position.x;
        lenght = GetComponentInChildren<SpriteRenderer>().bounds.size.x ;
    }

    private void FixedUpdate()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float dist = (camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(starpos + dist, transform.position.y);

        if (temp > starpos + lenght) starpos += lenght;
        else if(temp < starpos - lenght) starpos -= lenght;
    }
}
