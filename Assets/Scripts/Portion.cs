using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Portion : MonoBehaviour
{
    //This script will be attached to each portion on a wheel. It will keep up to date on whether or not it contains a segment.
    [SerializeField] public bool isOccupied;

    private void Update()
    {
        isOccupied = transform.GetComponentInChildren<Segment>();
    }
}
