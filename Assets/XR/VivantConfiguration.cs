using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "VivantConfiguration",
    menuName = "Vivant/Configuration"
)]
public class VivantConfiguration : ScriptableObject
{
    [Header("Apparence")]
    public Vector2 tailleRandom = new Vector2(0.3f, 1.2f);
    public Vector2 masseRandom = new Vector2(0.5f, 4f);
    public List<Material> materiaux;

    [Header("Mouvement")]
    public Vector2 rayonMouvement; // X = min, Y = max

    [Header("Croissance")]
    public float nourrirGrossissement = 0.1f;

    [Header("Sécurité")]
    public float limiteChuteY = -5f;



    public float acceleration = 12f;
    // public float vitesseMax = 5f;
    public float distanceArret = 0.1f;
    [Header("Stats aléatoires")]
    public Vector2 vitesseMaxRandom = new Vector2(2f, 5f);
    public Vector2 forceSautRandom = new Vector2(4f, 8f);


    [Header("Decision")]
    public Vector2 tempsDecision = new Vector2(0.3f, 1.2f);

    [Header("Saut")]
    public Vector2 tempsEntreSauts = new Vector2(0.8f, 2f);
    // public Vector2 forceSaut = new Vector2(5f, 9f);

    [Header("Nourriture")]
    public float rayonNourriture = 1f;
    public float distanceManger = 0.6f;
}
