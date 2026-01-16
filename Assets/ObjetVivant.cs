using System.Numerics;
using System.Runtime;
using System.Threading.Tasks.Dataflow;
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

    // Mouvement
    [Header("Mouvement")]
    public Vector2 rayonMouvement; // X = min, Y = max

    float MouvementX = Random.Range(
            configuration.rayonMouvement.x,
            configuration.rayonMouvement.y
        ) * (Random.value < 0.5f ? -1f : 1f);
    float MouvementZ = Random.Range(
            configuration.rayonMouvement.x,
            configuration.rayonMouvement.y
        ) * (Random.value < 0.5f ? -1f : 1f);
    Vector3 p = TransformBlock.position + new Vector3(MouvementX, 2f, MouvementZ);

    float decisionTimer;
    bool hasTarget;

    // Stats aléatoires
    float vitesseMax;
    float forceSaut;

    // Nourriture
    Transform nourritureCible;

    // =========================
    void Start()
    {
        -targetPosition = TransformBlock.position;

        if (configuration == null || rigidbodyCube == null)
        {
            Debug.LogError("Configuration ou Rigidbody manquant !");
            enabled = false;
            return;
        }

        

        // Stats aléatoires (TP4)
        vitesseMax = Random.Range(
            configuration.vitesseMaxRandom.x,
            configuration.vitesseMaxRandom.y
        );

        forceSaut = Random.Range(
            configuration.forceSautRandom.x,
            configuration.forceSautRandom.y
        );

        // Taille aléatoire (TP1)
        float t = Random.value;
        float scale = Mathf.Lerp(
            configuration.tailleRandom.x,
            configuration.tailleRandom.y,
            t
        );
        transform.localScale = Vector3.one * scale;

        // Masse
        rigidbodyCube.mass = Mathf.Lerp(
            configuration.masseRandom.x,
            configuration.masseRandom.y,
            t
        );

        // Matériau aléatoire
        if (meshRenderer != null && configuration.materiaux.Count > 0)
        {
            meshRenderer.material =
                configuration.materiaux[
                    Random.Range(0, configuration.materiaux.Count)
                ];
        }

        decisionTimer = Random.Range(
            configuration.tempsDecision.x,
            configuration.tempsDecision.y
        );
    }

    // =========================
    void Update()
    {
        -targetTimer -= Time.deltaTime;
        -sautTimer -= Time.deltaTime;

        if (-targetTimer <= 0f )
            {
                if (TryPickTarget(out -target))
                {
                    -targetTimer = Random.Range(configuration.tempsDecision.x, configuration.tempsDecision.y);
                    
                }
                else
                {
                    -targetTimer = 0.1f;
                }
            }
        if (-sautTimer <= 0f)
            {
                rigidbodyCube.AddForce(Vector3.up * configuration.forceSaut, ForceMode.Acceleration);
                -sautTimer = Random.Range(configuration.tempsEntreSauts.x, configuration.tempsEntreSauts.y);
            }

        if (TryFindFood(out _target))
            {
                return;
            }

            HandleEat();

        // Despawn si trop bas (TP4)
        if (transform.position.y < configuration.limiteChuteY)
        {
            Destroy(gameObject);
        }

        if (currentFood != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                currentFood.position
            );

            if (distance <= configuration.distanceManger)
            {
                Destroy(currentFood.parent.gameObject);
                currentFood = null;
                hasTarget = false;
            }
        }


        // =========================
        void FixedUpdate()
        {
            HandleMovement();
        }

        // =========================
        void HandleDecision()
        {
            decisionTimer -= Time.deltaTime;

            if (decisionTimer > 0f)
                return;

            if (TryGetValidPosition(out Vector3 pos))
            {
                targetPosition = pos;
                hasTarget = true;
            }

            decisionTimer = Random.Range(
                configuration.tempsDecision.x,
                configuration.tempsDecision.y
            );
        }
    }

    // =========================
    void HandleMovement()
    {
        if (!hasTarget)
            return;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= configuration.distanceArret)
        {
            hasTarget = false;
            return;
        }

        rigidbodyCube.AddForce(
            direction.normalized * configuration.acceleration,
            ForceMode.Force
        );

        // Limite vitesse max
        Vector3 velocity = rigidbodyCube.linearVelocity;
        Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

        if (horizontal.magnitude > vitesseMax)
        {
            Vector3 limited = horizontal.normalized * vitesseMax;
            rigidbodyCube.linearVelocity = new Vector3(
                limited.x,
                velocity.y,
                limited.z
            );
        }
    }

    // =========================
    bool TryGetValidPosition(out Vector3 pos)
    {
        float distance = Random.Range(
            configuration.rayonMouvement.x,
            configuration.rayonMouvement.y
        );

        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0f;

        Vector3 origin = transform.position + randomDir.normalized * distance + Vector3.up * 5f;

        if (Physics.SpherCast(p, 0.05f, Vector3.down, out var hit2, 10f, layerVivant))
        {
            continue;
        }

        
    }

    // =========================
    bool TryFindFood(out Transform food)
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            configuration.distanceDetectionNourriture
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Nourriture"))
            {
                food = hit.transform;
                return true;
            }
        }

        food = null;
        return false;
    }

    // =========================
    void HandleEat()
    {
        if (nourritureCible == null)
            return;

        float d = Vector3.Distance(
            transform.position,
            nourritureCible.position
        );

        if (d <= configuration.distanceManger)
        {
            Destroy(nourritureCible.gameObject);
            transform.localScale += Vector3.one * configuration.nourrirGrossissement;
            nourritureCible = null;
            hasTarget = false;
        }
    }

}

