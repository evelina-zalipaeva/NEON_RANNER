using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform player;
    public float tileLength = 20f;
    public int numberOfTiles = 5;

    private List<GameObject> tiles = new List<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Если игрок убежал вперед, перетаскиваем первую плитку в конец
        if (player.position.z > (tiles[0].transform.position.z + tileLength * 1.5f))
        {
            GameObject firstTile = tiles[0];
            tiles.RemoveAt(0);

            float newZ = tiles[tiles.Count - 1].transform.position.z + tileLength;
            firstTile.transform.position = new Vector3(0, 0, newZ);
            tiles.Add(firstTile);
        }
    }

    void SpawnTile()
    {
        GameObject newTile = Instantiate(tilePrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        tiles.Add(newTile);
        spawnZ += tileLength;
    }
}