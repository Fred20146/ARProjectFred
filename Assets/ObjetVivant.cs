using UnityEngine;

public class ObjetVivant : MonoBehaviour
{
    [Header("Configuration")]
    public VivantConfiguration configuration;

    [Header("Références")]
    public MeshRenderer meshRenderer;
    public Rigidbody rigidbodyCube;

    [Header("Layers")]
    public LayerMask layerSol;
    public LayerMask layerObstacle;

    // Décision & mouvement
    Vector3 targetPosition;
    float decisionTimer;
    bool hasTarget;

    // Saut
    float jumpTimer;

    void Start()
    {
        // Valeur aléatoire commune
        float randomValue = Random.value;

        // Taille aléatoire
        float taille = Mathf.Lerp(
            configuration.tailleRandom.x,
            configuration.tailleRandom.y,
            randomValue
        );
        transform.localScale = Vector3.one * taille;

        // Masse aléatoire
        float masse = Mathf.Lerp(
            configuration.masseRandom.x,
            configuration.masseRandom.y,
            randomValue
        );
        rigidbodyCube.mass = masse;

        // Matériau aléatoire
        if (configuration.materiaux.Count > 0)
        {
            int index = Random.Range(0, configuration.materiaux.Count);
            meshRenderer.sharedMaterial = configuration.materiaux[index];
        }

        // Timers initiaux
        decisionTimer = Random.Range(
            configuration.tempsDecision.x,
            configuration.tempsDecision.y
        );

        jumpTimer = Random.Range(
            configuration.tempsEntreSauts.x,
            configuration.tempsEntreSauts.y
        );
    }

    void Update()
    {
        HandleDecision();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // =========================
    // DECISION
    // =========================
    void HandleDecision()
    {
        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            if (TryGetValidPosition(out Vector3 pos))
            {
                targetPosition = pos;
                hasTarget = true;

                decisionTimer = Random.Range(
                    configuration.tempsDecision.x,
                    configuration.tempsDecision.y
                );
            }
            else
            {
                // Retente rapidement
                decisionTimer = 0.2f;
            }
        }
    }

    // =========================
    // MOUVEMENT
    // =========================
    void HandleMovement()
    {
        if (!hasTarget) return;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude < configuration.distanceArret)
        {
            hasTarget = false;
            return;
        }

        Vector3 force = direction.normalized * configuration.acceleration;
        rigidbodyCube.AddForce(force, ForceMode.Force);

        // Limiter la vitesse max
        Vector3 velocity = rigidbodyCube.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        if (horizontalVelocity.magnitude > configuration.vitesseMax)
        {
            horizontalVelocity = horizontalVelocity.normalized * configuration.vitesseMax;
            rigidbodyCube.linearVelocity = new Vector3(
                horizontalVelocity.x,
                velocity.y,
                horizontalVelocity.z
            );
        }
    }

    // =========================
    // SAUT
    // =========================
    void HandleJump()
    {
        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0f)
        {
            float force = Random.Range(
                configuration.forceSaut.x,
                configuration.forceSaut.y
            );

            rigidbodyCube.AddForce(
                Vector3.up * force,
                ForceMode.Impulse
            );

            jumpTimer = Random.Range(
                configuration.tempsEntreSauts.x,
                configuration.tempsEntreSauts.y
            );
        }
    }

    // =========================
    // POSITION VALIDE
    // =========================
    bool TryGetValidPosition(out Vector3 position)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-configuration.rayonMouvement, configuration.rayonMouvement),
                2f,
                Random.Range(-configuration.rayonMouvement, configuration.rayonMouvement)
            );

            Vector3 rayOrigin = transform.position + randomOffset;

            if (!Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                5f,
                layerSol))
            {
                continue;
            }

            if (Physics.CheckSphere(
                hit.point,
                0.5f,
                layerObstacle))
            {
                continue;
            }

            position = hit.point;
            return true;
        }

        position = transform.position;
        return false;
    }
}
