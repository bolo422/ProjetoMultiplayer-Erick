using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoxes : MonoBehaviour
{
    [SerializeField]
    List<Material> materials;


    void Start()
    {

        Material mat = materials[Random.Range(0, materials.Count)];
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            if (m.gameObject.CompareTag("boxTag"))
                m.material = mat;
        }



    }


}
