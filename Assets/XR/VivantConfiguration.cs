using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "VivantConfiguration",
    menuName = "ScriptableObjects/VivantConfiguration"
)]
public class VivantConfiguration : ScriptableObject
{
    public Vector2 tailleRandom;
    public Vector2 masseRandom;
    public List<Material> materiaux;
}
