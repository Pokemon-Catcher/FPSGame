using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Collider collider;
    public float damage;
    public GameObject owner;
    public AudioSource audioSource;
    public AudioClip[] ricochetSounds;
    public Rigidbody rigidbody;
    public float decayInterval=1.5f;
    private float decayTimer;
    // Start is called before the first frame update
    void Start()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }
    private void Update()
    {
        if(decayTimer>0) decayTimer += Time.deltaTime;
        if (decayTimer > decayInterval) Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!Object.Equals(collision.collider.gameObject, owner)) { 
        EntityInfo entity=collision.collider.gameObject.GetComponent<EntityInfo>();
        if(!(entity is null)) {
            entity.Damage(damage, owner);
            }
            else
            {
                audioSource.PlayOneShot(ricochetSounds[Random.Range(0, ricochetSounds.Length - 1)], Mathf.Min(Vector3.Distance(rigidbody.velocity,Vector3.zero)*0.001f,1));
                decayTimer += 0.01f;
            }
        }
    }
}
