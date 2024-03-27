using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float force;
    private Rigidbody rb;
    Cinemachine.CinemachineImpulseSource source;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.position;
    }

    public void Fire()
    {
        rb.AddForce(transform.forward * (50 * Random.Range(1.3f, 1.7f)) * Time.deltaTime, ForceMode.Impulse);
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();

        source.GenerateImpulse(Camera.main.transform.forward);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
        {
            rb.isKinematic = true;
            StartCoroutine(Countdown());
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }


}
