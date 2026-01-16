using UnityEngine;

public class ObjetVivant : MonoBehaviour
{
    [Header("Configuration")]
    public VivantConfiguration configuration;

    [Header("Références")]
    public MeshRenderer meshRenderer;
    public Rigidbody rigidbodyCube;

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
        int indexMaterial = Random.Range(
            0,
            configuration.materiaux.Count
        );
        meshRenderer.sharedMaterial =
            configuration.materiaux[indexMaterial];
    }
}
