using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractLvlItem : MonoBehaviour
{
    [System.NonSerialized]
    public string objectID;
    private void Awake()
    {
        objectID = gameObject.scene.name + ", x=" + gameObject.transform.position.x + ", y=" + gameObject.transform.position.y;
        if (PlayerPrefs.HasKey(objectID)) gameObject.SetActive(false);
    }
}
