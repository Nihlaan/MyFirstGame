using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public GameObject impactEffect;

    public void Seek(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        var duration = effectIns.GetComponent<ParticleSystem>().main.duration + 1f;
        Destroy(effectIns, duration);

        Destroy(target.gameObject);
        Destroy(gameObject);
    }
}
