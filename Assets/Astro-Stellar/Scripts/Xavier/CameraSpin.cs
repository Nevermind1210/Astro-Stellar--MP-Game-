using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraSpin : MonoBehaviour
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float zSpeed;

    private void Start()
    {
        xSpeed = Random.Range(5, 10);
        ySpeed = Random.Range(5, 10);
        zSpeed = Random.Range(5, 10);
    }

    void Update()
    {
        transform.Rotate(xSpeed * Time.deltaTime,ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
    }
}
