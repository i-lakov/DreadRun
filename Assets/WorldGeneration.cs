using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour
{
    #region Members
    private int[,] map;
    private int width = 1500;
    private int height = 25;

    private Tilemap tilemap;
    [SerializeField] private TileBase tileGrass;
    [SerializeField] private TileBase tileDirt;
    [SerializeField] private int minSectionWidth;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject spikesPrefab;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject flowerPrefabA;
    [SerializeField] private GameObject flowerPrefabB;
    [SerializeField] private GameObject flowerPrefabC;
    private float seed;
    #endregion

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        seed = Time.time;

        CreateEmptyArray();
        GenerateLevel(minSectionWidth);
        RenderMap();
    }

    public void CreateEmptyArray()
    {
        map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x, y] = 0;
            }
        }
    }

    public void RenderMap()
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileDirt);
                }
            }
        }

        // Set top block to grass
        bool grassSet = false;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            grassSet = false;
            for (int y = map.GetUpperBound(1); y > 0 ; y--)
            {
                if (map[x, y] == 1 && !grassSet)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileGrass);
                    grassSet = true;
                }
            }
        }
    }

    public void GenerateLevel(int minSectionWidth)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int lastHeight = 7; // the initial height shall be 7 to match with the starting platform
        int nextMove; // flipcoin, to decide whether to alter terrain upwards or downwards
        int heightJump; // amount of alteration in heigth
        int sectionWidth = 0;

        int lastTreeX = 5;
        int treeDistance = 15;

        int lastFlowerX = 5;
        int flowerDistance = 3;

        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            if(x > lastTreeX + treeDistance)
            {
                if(rand.Next(2) == 0) // 1/2 chance to spawn a tree
                {
                    GameObject tree = Instantiate(treePrefab, new Vector3(x, lastHeight + 10, 0), Quaternion.Euler(Vector3.zero));
                    tree.transform.localScale += new Vector3(Random.Range(0, 0.3f), 0, 0);
                    lastTreeX = x;
                }
            }

            if (x > lastFlowerX + flowerDistance)
            {
                if (rand.Next(2) == 0) // 1/2 chance to spawn a tree
                {
                    switch (rand.Next(3)) // pick a flower sprite
                    {
                        case 0: Instantiate(flowerPrefabA, new Vector3(x, lastHeight, 0), Quaternion.Euler(Vector3.zero)); break;
                        case 1: Instantiate(flowerPrefabB, new Vector3(x, lastHeight, 0), Quaternion.Euler(Vector3.zero)); break;
                        case 2: Instantiate(flowerPrefabC, new Vector3(x, lastHeight, 0), Quaternion.Euler(Vector3.zero)); break;
                    }
                    
                    lastFlowerX = x;
                }
            }

            nextMove = rand.Next(2);

            // Change the height if we have used this height more than the minimum required width
            if (nextMove == 0 && lastHeight > 3 && sectionWidth > minSectionWidth)
            {
                if(rand.Next(3) == 0) // 1/3 chance to keep on going
                {
                    heightJump = rand.Next(3);
                    heightJump++;

                    lastHeight -= heightJump;
                    sectionWidth = 0;

                    if (rand.Next(4) == 0) // 1/4 chance to spawn spikes at this level
                    {
                        Instantiate(spikesPrefab, new Vector3(x, lastHeight, 0), Quaternion.Euler(Vector3.zero));
                    }
                }
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1)-3 && sectionWidth > minSectionWidth)
            {
                if (rand.Next(3) == 0) // 1/3 chance to keep on going
                {
                    heightJump = rand.Next(3);
                    heightJump++;

                    lastHeight += heightJump;
                    sectionWidth = 0;

                    if (rand.Next(3) == 0) // 1/3 chance to spawn an enemy at this level
                    {
                        Instantiate(enemyPrefab, new Vector3(x, lastHeight + 10, 0), Quaternion.Euler(Vector3.zero));
                    }
                }
            }
            sectionWidth++;

            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
    }
}
