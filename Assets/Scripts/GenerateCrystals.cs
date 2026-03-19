using UnityEngine;

public class GenerateCrystals : MonoBehaviour
{
    public GameObject crystalPrefab;
    public int numberOfCrystals = 100;

    public Vector3 spawnSpread = new(300, 0, 300);

    public Vector3 mapCenter = new(1000, 1505, 0);
    void Start()
    {
        for (int i = 0; i < numberOfCrystals; i++)
        {
            float xOffset = Random.Range(-spawnSpread.x, spawnSpread.x);
            float zOffset = Random.Range(-spawnSpread.z, spawnSpread.z);

            Vector3 spawnPos = new Vector3(1000 + xOffset, 1505, 0 + zOffset);

            Instantiate(crystalPrefab, spawnPos, Quaternion.identity);
        }
    }
}