using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform player;
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform gunBarrel;

    ObjectPooling objectPooler;

    float timeSinceLastShot;

    private void Start()
    {
        objectPooler = ObjectPooling.instance;
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                //if (Physics.Raycast(player.position, player.forward, out RaycastHit hitInfo, gunData.maxDistance))
                //{
                //    IHeathSystem healthObject = hitInfo.transform.GetComponent<IHeathSystem>();
                //    healthObject?.TakeDamage(gunData.damage);
                //}

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(player.position, player.forward * gunData.maxDistance);
    }

    private void OnGunShot() { 

        objectPooler.spawnFromPool("Bullet", gunBarrel.position, gunBarrel.rotation);

        //GameObject bullet = Instantiate(projectilePrefab, gunBarrel.position, gunBarrel.rotation);

        //if (bullet != null)
        //{
        //    // Call its Fire() function
        //    projectile pro = bullet.GetComponent<projectile>();
        //    pro.Fire();
        //}
        //else
        //{
        //    Debug.LogError("Projectile prefab does not have ProjectileBullet component!");
        //}
    }
}