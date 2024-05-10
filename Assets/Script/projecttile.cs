using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    float bulletDamage = 20f;
    private Rigidbody rb;
    //Cinemachine.CinemachineImpulseSource source;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.position;
    }

    private void Start()
    {
        Fire();
    }
    public void Fire()
    {
        rb.AddForce(transform.forward * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        //source = GetComponent<Cinemachine.CinemachineImpulseSource>();

        //source.GenerateImpulse(Camera.main.transform.forward);
    }

    void OnTriggerEnter(Collider collider)
    {
        IHeathSystem colliderThing = collider.GetComponent<IHeathSystem>();

        if (colliderThing != null)
        {
            colliderThing.takeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    //IEnumerator Countdown()
    //{
    //    yield return new WaitForSeconds(10);
    //    Destroy(gameObject);
    //}


}
