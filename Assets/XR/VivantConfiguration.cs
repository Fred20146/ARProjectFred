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
    public Vector2 masseRandom = new Vector2(0.5f, 5f);
    public List<Material> materiaux;

    [Header("Mouvement")]
    public float rayonMouvement = 2f;
    public float acceleration = 5f;
    public float vitesseMax = 3f;
    public float distanceArret = 0.2f;

    [Header("Decision")]
    public Vector2 tempsDecision = new Vector2(1f, 3f);

    [Header("Saut")]
    public Vector2 tempsEntreSauts = new Vector2(2f, 6f);
    public Vector2 forceSaut = new Vector2(3f, 6f);
}
