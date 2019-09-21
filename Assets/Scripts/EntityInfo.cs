using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public float hp = 100;
    public AudioSource audioSource;
    public AudioClip[] damageSounds;
    public AudioClip[] dieSounds;

    private void Update()
    {
        if (hp <= 0) this.Die();
    }

    public void Damage(float damage, GameObject source)
    {
        hp -= damage;
        Debug.Log(gameObject.name + " takes damage:" + damage.ToString());
        audioSource.PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length - 1)]);
    }

    public void Die()
    {
        gameObject.AddComponent<Rigidbody>();
        audioSource.PlayOneShot(dieSounds[Random.Range(0, damageSounds.Length - 1)]);
        GameObject.Destroy(this);
    }
}
