using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlMover : MonoBehaviour
{
    public GameObject Cube;
    // Start is called before the first frame update
    void Start()
    {
        Cube.transform.DOMove(new Vector3(-20, 8, 0), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
