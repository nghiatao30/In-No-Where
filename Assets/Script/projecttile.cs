using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour,IPooledObject
{
    [SerializeField] float bulletSpeed;
    float bulletDamage = 20f;
    private Rigidbody rb;
    //Cinemachine.CinemachineImpulseSource source;

    public void Action()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.position;
        Vector3 tarDir = (GameObject.Find("w").transform.position - transform.position).normalized;
        rb.AddForce(tarDir * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        StartCoroutine(Countdown());
        //source = GetComponent<Cinemachine.CinemachineImpulseSource>();

        //source.GenerateImpulse(Camera.main.transform.forward);
    }

    void OnTriggerEnter(Collider collider)
    {
        IHeathSystem colliderThing = collider.GetComponent<IHeathSystem>();

        if (colliderThing != null)
        {
            colliderThing.takeDamage(bulletDamage);

        }
        gameObject.SetActive(false);
        
    }

    

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }


}
