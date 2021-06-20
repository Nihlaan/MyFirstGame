using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f;

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
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        var duration = effectIns.GetComponent<ParticleSystem>().main.duration + 1f;
        Destroy(effectIns, duration);

        if (explosionRadius > 0f)
		{
            Explode();
		}
        else
		{
            Damage(target);
		}

        Destroy(gameObject);
    }

    private void Explode()
	{
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var collider in colliders)
		{
            if (collider.tag == "Enemy")
			{
                Damage(collider.transform);
			}
		}
	}

    private void Damage(Transform enemy)
	{
        Destroy(enemy.gameObject);
	}

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
