using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour,IPooledObject
{
    [SerializeField] float bulletSpeed;
    float bulletDamage = 20f;
    private Rigidbody rb;
    Vector3 tarDir;
    //Cinemachine.CinemachineImpulseSource source;
    
    void Awake() { rb = GetComponent<Rigidbody>();}

    void Update()
    {
        
    }
    public void Action()
    {   
        rb.velocity = Vector3.zero;
        tarDir = (GameObject.Find("w").transform.position - transform.position).normalized;
        rb.centerOfMass = transform.position;
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
