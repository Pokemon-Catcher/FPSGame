using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletType;
    public float force = 100f;
    public float shotInterval = 0.1f;
    public float reloadInterval = 10f;
    public float ammoCount = 10;
    public AudioSource audioSource;
    public AudioClip[] fireSounds;
    public AudioClip[] reloadSounds;
    private float shotTimer = 0f;
    private float reloadTimer = 0f;
    private float currentAmmo = 10;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = ammoCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotTimer > 0)
            shotTimer += Time.deltaTime;
        if (reloadTimer > 0)
            reloadTimer += Time.deltaTime;
        if (Input.GetAxis("Fire1") !=0f && shotTimer==0f && currentAmmo>0)
        {
            audioSource.PlayOneShot(fireSounds[Random.Range(0, fireSounds.Length - 1)]);
            GameObject bullet = Object.Instantiate(bulletType, transform.position+transform.forward * 1.0f, Quaternion.LookRotation(transform.TransformDirection(Vector3.down),transform.forward));
            shotTimer += 0.01f;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * force);
            bullet.GetComponent<BulletBehaviour>().owner = gameObject.GetComponentInParent<EntityInfo>().gameObject;
            currentAmmo--;
            if (currentAmmo <= 0)
            {
                reloadTimer += 0.01f;
                audioSource.PlayOneShot(reloadSounds[0]);
            }
        }
        if (reloadTimer >= reloadInterval)
        {
            reloadTimer = 0;
            currentAmmo = ammoCount;
            audioSource.PlayOneShot(reloadSounds[1]);
        }
        if (shotTimer > shotInterval)
        {
            shotTimer = 0;
        }
    }
}
