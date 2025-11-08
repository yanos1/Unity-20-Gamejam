using System;
using System.Collections;
using Obstacles.Laser;
using UnityEngine;

public class StaticLaser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float interval;
    [SerializeField] private Laser laser;

    void Start()
    {
        StartCoroutine(StartLaster());
    }

    private IEnumerator StartLaster()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            laser.gameObject.SetActive(!laser.gameObject.activeInHierarchy);
        }
    }
}