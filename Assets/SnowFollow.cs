using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

sealed class SnowFollow : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    void Update()
    {
        gameObject.transform.position = new Vector3(followObject.transform.position.x, 7f , followObject.transform.position.z);
    }
}
