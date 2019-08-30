using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public float shootSpeed = 10f;
    public float cooldown = 1f;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public bool canShoot;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    public void FireProjectile()
    {
        if (!canShoot) return;

        GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Vector3 force = projectileSpawnPoint.transform.forward * shootSpeed;
        projectile.GetComponent<Rigidbody>().AddForce(force);

        canShoot = false;
        Invoke("CoolDown", cooldown);
    }

    void CoolDown()
    {
        canShoot = true;
    }
}
