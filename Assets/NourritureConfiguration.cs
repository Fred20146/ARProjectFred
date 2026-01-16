using UnityEngine;

[CreateAssetMenu(
    fileName = "NourritureConfiguration",
    menuName = "AR/Nourriture Configuration"
)]
public class NourritureConfiguration : ScriptableObject
{
    public float chanceEmpoisonnee = 0.2f;

    [Header("Explosion")]
    public float rayonExplosion = 1.5f;
    public float forceExplosion = 400f;
    public float upwardModifier = 0.5f;
}
