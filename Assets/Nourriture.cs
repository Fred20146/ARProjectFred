using UnityEngine;

public class Nourriture : MonoBehaviour
{
    void Start()
    {
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        r.material = new Material(r.material);
        r.material.color = Random.ColorHSV();
    }

    public NourritureConfiguration configuration;

    void OnDestroy()
    {
        if (configuration == null)
            return;

        if (Random.value > configuration.chanceEmpoisonnee)
            return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            configuration.rayonExplosion
        );

        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(
                configuration.forceExplosion,
                transform.position,
                configuration.rayonExplosion,
                configuration.upwardModifier,
                ForceMode.Impulse
            );
        }
    }
    


}
