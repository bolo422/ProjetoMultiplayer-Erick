using System.Collections;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;

public class Banner : MonoBehaviour
{
    [SerializeField]
    Material red, green, blue, magenta, yellow;

    [SerializeField]
    TagColors currentColor;

    [SerializeField]
    GameObject cube;

    Material currentMaterial;

    bool up = false;
    float originalY;

    public TagColors CurrentColor { get => currentColor; }

    void Start()
    {
        switch(currentColor)
        {
            case TagColors.red: currentMaterial = red; break;
            case TagColors.blue: currentMaterial = blue; break;
            case TagColors.green: currentMaterial = green; break;
            case TagColors.magenta: currentMaterial = magenta; break;
            case TagColors.yellow: currentMaterial = yellow; break;
        }

        foreach(MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material = currentMaterial;
        }

        originalY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (up)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), .6f * Time.deltaTime);
            
            if(transform.position.y > originalY + .5f)
            {
                up = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), .6f * Time.deltaTime);

            if (transform.position.y < originalY - .5f)
            {
                up = true;
            }
        }
    }

}
