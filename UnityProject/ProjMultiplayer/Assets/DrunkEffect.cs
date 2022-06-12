using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkEffect : MonoBehaviour
{
    private float maxFov;
    private float minFov;
    private float normalFov;

    private Camera cam;

    private bool plus = true;


    private void Start()
    {
        cam = GetComponent<Camera>();
        normalFov = cam.fieldOfView;

        maxFov = normalFov + 20;
        minFov = normalFov - 20;
    }
    private void FixedUpdate()
    {
        if(plus)
        {
            cam.fieldOfView += 0.2f;
            if(cam.fieldOfView >= maxFov)
                plus = false;
        }
        else
        {
            cam.fieldOfView -= 0.2f;
            if (cam.fieldOfView <= minFov)
                plus = true;
        }
    }







    //private int Z_rotateVector;
    //private bool Z_rotateToZero = false;

    //private int X_rotateVector;
    //private bool X_rotateToZero = false;

    //public float pitch = 0;



    //void Start()
    //{
    //    isDrunk = true;
    //    Z_rotateVector = 1;
    //    X_rotateVector = 1;
    //    StartCoroutine(Z_ChangeRotation());
    //    StartCoroutine(Z_StartRotatoToZero());
    //    StartCoroutine(X_ChangeRotation());
    //    StartCoroutine(X_StartRotatoToZero());
    //}

    //void Update()
    //{
    //    //transform.localEulerAngles = new Vector3(pitch, 0f, 0f);

    //    if (!isDrunk) return;

    //    DrunkZ();
    //    DrunkX();


    //}

    //private void DrunkZ()
    //{
    //    float strenght = Random.Range(0.01f, 0.04f);

    //    if (Z_rotateToZero)
    //    {
    //        if (transform.rotation.z < 0)
    //            transform.Rotate(new Vector3(0, 0, 1), strenght);
    //        if (transform.rotation.z > 0)
    //            transform.Rotate(new Vector3(0, 0, -1), strenght);

    //        if (transform.rotation.z >= -0.04f && transform.rotation.z <= 0.04f)
    //        {
    //            Debug.Log("Stop at: " + transform.rotation);
    //            Z_rotateToZero = false;
    //            StartCoroutine(Z_StartRotatoToZero());
    //        }
    //    }
    //    else
    //    {
    //        transform.Rotate(new Vector3(0, 0, 1 * Z_rotateVector), strenght);
    //    }
    //}


    //IEnumerator Z_ChangeRotation()
    //{
    //    Z_rotateVector *= -1;
    //    float timeToChange = Random.Range(0.5f, 1f);
    //    yield return new WaitForSeconds(timeToChange);
    //    StartCoroutine(Z_ChangeRotation());

    //}

    //IEnumerator Z_StartRotatoToZero()
    //{
    //    yield return new WaitForSeconds(5.0f);
    //    Z_rotateToZero = true;
    //}


    //private void DrunkX()
    //{
    //    float strenght = Random.Range(0.005f, 0.01f);

    //    if (X_rotateToZero)
    //    {
    //        if (transform.rotation.x < 0)
    //            transform.Rotate(new Vector3(0, 1, 0), strenght);
    //        if (transform.rotation.x > 0)
    //            transform.Rotate(new Vector3(0, -1, 0), strenght);

    //        if (transform.rotation.x >= -0.04f && transform.rotation.x <= 0.04f)
    //        {
    //            Debug.Log("Stop at: " + transform.rotation);
    //            X_rotateToZero = false;
    //            StartCoroutine(X_StartRotatoToZero());
    //        }
    //    }
    //    else
    //    {
    //        transform.Rotate(new Vector3(0, 1 * X_rotateVector, 0), strenght);
    //    }
    //}

    //IEnumerator X_ChangeRotation()
    //{
    //    X_rotateVector *= -1;
    //    float timeToChange = Random.Range(0.2f, 0.6f);
    //    yield return new WaitForSeconds(timeToChange);
    //    StartCoroutine(X_ChangeRotation());

    //}

    //IEnumerator X_StartRotatoToZero()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //    X_rotateToZero = true;
    //}


}
