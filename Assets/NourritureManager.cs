using UnityEngine;

public class NourritureManager : MonoBehaviour
{
    public GameObject donutPrefab;
    public int maxNourriture = 10;
    public Vector2 zoneSpawn = new Vector2(4f, 8f);
    public LayerMask layerSol;

    void Start()
    {
        for (int i = 0; i < maxNourriture; i++)
        {
            Vector3 pos = GetRandomPositionOnGround();
            Instantiate(donutPrefab, pos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-zoneSpawn.y, zoneSpawn.y);
            float z = Random.Range(-zoneSpawn.y, zoneSpawn.y);
            Vector3 origin = new Vector3(x, 5f, z);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f, layerSol))
            {
                return hit.point + Vector3.up * 0.1f;
            }
        }

        return Vector3.zero;
    }
}
