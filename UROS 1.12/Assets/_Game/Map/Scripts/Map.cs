using UnityEngine;

// Cube type enum
public enum CUBETYPE { BORDER, ROCK, CRYSTAL, CHEST, EMPTY, EXIT }

// Map builer class
public class Map : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Grid width and height
    int gridWidth = 50;
    int gridHeight = 50;

    // Map generation Completed?
    bool mapGenerationComplete = false;

    [Header("Grid cubes and cave GameObjects")]
    public GameObject floor;
    public GameObject gridCube;
    public GameObject gridCubes;
    public GameObject caveShape1;
    public GameObject caveShape2;
    public GameObject caveShape3;
    private GameObject[,] gridCubeArray;

    [Header("Cave generation settings")]
    [Range(5, 30)]
    public int numberOfCaves;
    [Range(1, 10)]
    public float minCaveSize, maxCaveSize;
    [Range(1, 10)]
    public int minCaveDensity, maxCaveDensity;
    [Range(-10, 10)]
    public float minCaveDrift, maxCaveDrift;
    [Range(0, 1)]
    public float shape1Chance, shape2Chance, shape3Chance;
    [Range(0, 1)]
    public float crystalCubeChance;
    [Range(1, 2)]
    public float cubeScale;

    [Header("Grid cube materials")]
    public Material borderMaterial;
    public Material rockMaterial;
    public Material crystalMaterial;
    public Material edgeMaterial;
    public Material rockDamagedMaterial;
    public Material crystalDamagedMaterial;
    public Material crystalMoreDamagedMaterial;
    public Material crystalReallyDamagedMaterial;

    [Header("Grid cube particle systems")]
    public ParticleSystem rockDamagedSystem;
    public ParticleSystem rockDestroyedSystem;
    public ParticleSystem crystalDamagedSystem;
    public ParticleSystem crystalDestroyedSystem;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
    }

    // Initialise the map
    public void InitMap()
    {
        // Set the cube colours
        SetColours();

        // Clamp the min/max cave generation settings
        ClampMinMaxValues();

        // Generate the grid cubes
        GenerateGridCubes();

        // Generate the caves
        GenerateCaves();

        Debug.Log("MAP - INIT - COMPLETE");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Set the map generation as completed
        // Note: Called In /*Late*/Update as execution of OnTrigger events will have occured,
        // Grid Cube OnTrigger events with generated cave shapes will have occured and
        // updated the gridcubes accordingly
        if (!mapGenerationComplete)
        {
            // Search for and destory all the leftover caves
            if (GameObject.FindGameObjectsWithTag(Tags.caveTag) != null)
            {
                // Search for and destory all the leftover caves
                GameObject[] caves = GameObject.FindGameObjectsWithTag(Tags.caveTag);
                if (caves.Length > 0)
                    foreach (GameObject cave in caves) Destroy(cave);
            }

            // Count all the grid cubes (crystal and rock cubes)
            GameDataManager.instance.CountGridCubeTypes();

            // Map generation completed
            mapGenerationComplete = true;

            Debug.Log("MAP - MAP GEN - COMPLETE");
        }
    }

    // Clamp the min/max cave generation settings
    void ClampMinMaxValues()
    {
        minCaveSize = Mathf.Clamp(minCaveSize, 1, maxCaveSize);
        maxCaveSize = Mathf.Clamp(maxCaveSize, minCaveSize, 10);
        minCaveDensity = Mathf.Clamp(minCaveDensity, 1, maxCaveDensity);
        maxCaveDensity = Mathf.Clamp(maxCaveDensity, minCaveDensity, 10);
        minCaveDrift = Mathf.Clamp(minCaveDrift, -10, maxCaveDrift);
        maxCaveDrift = Mathf.Clamp(maxCaveDrift, minCaveDrift, 10);
        shape1Chance = Mathf.Clamp(shape1Chance, 0, shape2Chance);
        shape2Chance = Mathf.Clamp(shape2Chance, shape1Chance, shape3Chance);
        shape3Chance = 1.0f - (shape1Chance + shape2Chance);
    }

    // Generate tyhe grid cubes
    void GenerateGridCubes()
    {
        // Initialise the grid cube array
        gridCubeArray = new GameObject[gridWidth, gridHeight];

        // Build the map grid - loop through height of grid
        for (int z = 0; z < gridHeight; z++)
        {
            // Loop through width of grid
            for (int x = 0; x < gridWidth; x++)
            {
                // Build the grid - instantiate grid cube calculated position - assign the cube as a child of the gridCubes object
                GameObject cube = Instantiate(gridCube, new Vector3(x * 1.0f, 0.0f, z * 1.0f), Quaternion.identity, gridCubes.transform) as GameObject;
                cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);

                // Assign the grid cube to the array
                gridCubeArray[x, z] = cube;

                // Reference to the cube data
                CubeData cubeData = gridCubeArray[x, z].GetComponent<CubeData>();

                // Set the cubes coord in the grid and set all cubes to closed (initially)
                cubeData.SetCubeCoord(new Vector3(x, 0.0f, z));
                cubeData.SetCubeOpen(false);

                // Random between 0 and 1 - used to select rock type
                float crystalChance = Random.Range(0.0f, 1.0f);

                // Set the grid borders as border cubes - Set the gird cube indestructible and the cube type as border
                if (x == 0 || z == 0 || x == 1 || z == 1 || x == gridWidth - 1 || z == gridHeight - 1 || x == gridWidth - 2 || z == gridHeight - 2)
                    cubeData.SetAsBorderCube();

                // Set the cube as crystal cubes - Set the gird cube destructible and the cube type as rock
                else if (crystalChance < crystalCubeChance)
                    cubeData.SetAsCystalCube();

                // Set the rest of the cubes as rock cubes -- Set the gird cube destructible and the cube type as rock
                else
                    cubeData.SetAsRockCube();

                // If the cube is a rock/crystal or border cube
                if (cubeData.GetCubeType() == CUBETYPE.BORDER || cubeData.GetCubeType() == CUBETYPE.ROCK || cubeData.GetCubeType() == CUBETYPE.CRYSTAL)
                {
                    // Set the scale and rotation of the grid cubes
                    cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
                    cube.transform.rotation = Random.rotation;
                }
            }
        }

        Debug.Log("MAP - GEN GRID CUBES - COMPLETE");
    }

    // Generate the cave structures
    void GenerateCaves()
    {
        // Generate the caves
        for (int i = 0; i < numberOfCaves; i++)
        {
            // Get the initial position of the cave - random position from world center point
            Vector3 initialPosition = new Vector3(Random.Range(0.0f, gridWidth), 0.0f, Random.Range(0.0f, gridHeight));

            // Generate a random density of the cave between the min and max cave density
            for (int j = 0; j < Random.Range(minCaveDensity, maxCaveDensity); j++)
            {
                // Random between 0 and 1 - used to select cave shape
                float caveShapeChance = Random.Range(0.0f, 1.0f);

                // Chose a cave shape at random - shape 1
                if (caveShapeChance < shape1Chance)
                    InstantiateCaveShape(caveShape1, initialPosition);

                // Shape 2
                else if (caveShapeChance < shape2Chance && caveShapeChance >= shape1Chance)
                    InstantiateCaveShape(caveShape2, initialPosition);

                // Shape 3
                else
                    InstantiateCaveShape(caveShape3, initialPosition);
            }
        }

        Debug.Log("MAP - GEN CAVES - COMPLETE");
    }

    // Instantiate the cave shape
    void InstantiateCaveShape(GameObject caveShape, Vector3 initialPos)
    {
        // Generate a random size of a cave between the min and max size
        caveShape.transform.localScale = new Vector3(Random.Range(minCaveSize, maxCaveSize), 1.0f, Random.Range(minCaveSize, maxCaveSize));

        // Instaniate the cave (including cave shape variance) at the initial cave position in addition to random value between the min and max drift
        Instantiate(caveShape, initialPos + new Vector3(Random.Range(minCaveDrift, maxCaveDrift), 0.0f, Random.Range(minCaveDrift, maxCaveDrift)), Random.rotation);
    }

    // Position the game object in the map
    public bool PositionGameObjectInEmptyCube(GameObject go)
    {
        // Set the spawned bool as false
        bool spawned = false;

        // While the gameobject has not been spawned in
        while (!spawned)
        {
            // Counter for the open grid cubes
            int allCubesOpen = 0;

            // Choose a random grid x and y coord
            int randomX = Random.Range(2, gridWidth - 2);
            int randomZ = Random.Range(2, gridHeight - 2);

            // Check the cube is free
            if (!gridCubeArray[randomX, randomZ].GetComponent<CubeData>().GetCubeOpen())
                continue;

            // Loop through the adjacent grid cubes - z axis
            for (int z = -1; z < 2; z++)
            {
                // Loop through the adjacent grid cubes - x axis
                for (int x = -1; x < 2; x++)
                {
                    // If the grid cube at the random coord is open count the open space
                    if (gridCubeArray[randomX + x, randomZ + z].GetComponent<CubeData>().GetCubeOpen())
                        allCubesOpen++;
                }
            }

            // If all the adjacent grid cubes are open
            if (allCubesOpen == 9)
            {
                // Set the gamobjects position
                go.transform.position = new Vector3(randomX, go.transform.position.y, randomZ);
                gridCubeArray[randomX, randomZ].GetComponent<CubeData>().SetCubeOpen(false);

                // Break out of the loop and spawn the gameobject
                spawned = true;
            }
        }

        // Return true once completed
        return spawned;
    }

    // Position the player in the map
    public bool PositionPlayerInEmptyCube(GameObject go)
    {
        // Set the spawned bool as false
        bool spawned = false;

        // While the gameobject has not been spawned in
        while (!spawned)
        {
            // Counter for the open grid cubes
            int allCubesOpen = 0;

            // Choose a random grid x and y coord
            int randomX = Random.Range(2, gridWidth - 2);
            int randomZ = Random.Range(2, gridHeight - 2);

            // Check the cube is free
            if (!gridCubeArray[randomX, randomZ].GetComponent<CubeData>().GetCubeOpen())
                continue;

            // Loop through the adjacent grid cubes - z axis
            for (int z = -2; z <= 2; z++)
            {
                // Loop through the adjacent grid cubes - x axis
                for (int x = -2; x <= 2; x++)
                {
                    // If the grid cube at the random coord is open count the open space
                    if (gridCubeArray[randomX + x, randomZ + z].GetComponent<CubeData>().GetCubeOpen())
                        allCubesOpen++;
                }
            }

            // If all the adjacent grid cubes are open
            if (allCubesOpen == 25)
            {
                // Set the gamobjects position
                go.transform.position = new Vector3(randomX, go.transform.position.y, randomZ);
                gridCubeArray[randomX, randomZ].GetComponent<CubeData>().SetCubeOpen(false);

                // Break out of the loop and spawn the player
                spawned = true;
            }
        }

        Debug.Log("MAP - PLAYER POSITIONED - COMPLETE");

        // Return true once completed
        return spawned;
    }

    // Spawn the old miner
    public bool PositionOldMiner(GameObject go, Vector3 playerPos)
    {
        // Set the spawned bool as false
        bool spawned = false;

        // Counter for the open grid cubes
        int openX = 0;
        int openZ = 0;

        // Loop through the adjacent grid cubes - z axis
        for (int z = -1; z < 2; z++)
        {
            if (gridCubeArray[(int)playerPos.x - 1, (int)playerPos.z + z].GetComponent<CubeData>().GetCubeOpen())
            {
                openX = (int)playerPos.x - 1;
                openZ = (int)playerPos.z + z;
                spawned = true;
                break;
            }
            else if (gridCubeArray[(int)playerPos.x + 1, (int)playerPos.z + z].GetComponent<CubeData>().GetCubeOpen())
            {
                openX = (int)playerPos.x + 1;
                openZ = (int)playerPos.z + z;
                spawned = true;
                break;
            }
        }

        // If not already spawned
        if (!spawned)
        {
            // Loop through the adjacent grid cubes - x axis
            for (int x = -1; x < 2; x++)
            {
                if (gridCubeArray[(int)playerPos.x + x, (int)playerPos.z - 1].GetComponent<CubeData>().GetCubeOpen())
                {
                    openX = (int)playerPos.x + x;
                    openZ = (int)playerPos.z - 1;
                    spawned = true;
                    break;
                }
                else if (gridCubeArray[(int)playerPos.x + x, (int)playerPos.z + 1].GetComponent<CubeData>().GetCubeOpen())
                {
                    openX = (int)playerPos.x + x;
                    openZ = (int)playerPos.z + 1;
                    spawned = true;
                    break;
                }
            }
        }

        // Set the gamobjects position
        go.transform.position = new Vector3(openX, go.transform.position.y, openZ);
        gridCubeArray[openX, openZ].GetComponent<CubeData>().SetCubeOpen(false);

        Debug.Log("MAP - OLD MINER POSITIONED - COMPLETE");

        // Return true once completed
        return spawned;
    }

    // Set the colours of the grid cubes
    void SetColours()
    {
        // Set the particle system color over lifetime gradients
        Colours.ColoursPSGradient();

        // Set colours CVD
        if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.CVD)
        {
            // Red safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.RED)
            {
                // Cube colours
                borderMaterial.color = Colours.borderColourRedSafe;
                rockMaterial.color = Colours.rockColourRedSafe;
                edgeMaterial.color = Colours.edgeColourRedSafe;
                rockDamagedMaterial.color = Colours.rockDamagedColourRedSafe;
                crystalMaterial.color = Colours.crystalColourRedSafe;
                crystalDamagedMaterial.color = Colours.crystalColourRedSafe;
                crystalMoreDamagedMaterial.color = Colours.crystalColourRedSafe;
                crystalReallyDamagedMaterial.color = Colours.crystalColourRedSafe;

                // Floor colour
                floor.GetComponent<MeshRenderer>().material.color = Colours.floorColourRedSafe;

                // Particle system colours (damaged rock PS)
                var main = rockDamagedSystem.main;
                main.startColor = Colours.rockDamagedPSColourRedSafe;
                var col = rockDamagedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradRedSafe;

                // Particle system colours (destroyed rock PS)
                main = rockDestroyedSystem.main;
                main.startColor = Colours.rockDamagedPSColourRedSafe;
                col = rockDestroyedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradRedSafe;

                // Particle system colours (crystal damaged PS)
                main = crystalDamagedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourRedSafe;
                col = crystalDamagedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourRedSafe;

                // Particle system colours (crystal destroyed PS)
                main = crystalDestroyedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourRedSafe;
                col = crystalDestroyedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourRedSafe;
            }

            // Green safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.GREEN)
            {
                // Cube colours
                borderMaterial.color = Colours.borderColourGreenSafe;
                rockMaterial.color = Colours.rockColourGreenSafe;
                edgeMaterial.color = Colours.edgeColourGreenSafe;
                rockDamagedMaterial.color = Colours.rockDamagedColourGreenSafe;
                crystalMaterial.color = Colours.crystalColourGreenSafe;
                crystalDamagedMaterial.color = Colours.crystalColourGreenSafe;
                crystalMoreDamagedMaterial.color = Colours.crystalColourGreenSafe;
                crystalReallyDamagedMaterial.color = Colours.crystalColourGreenSafe;

                // Floor colour
                floor.GetComponent<MeshRenderer>().material.color = Colours.floorColourGreenSafe;

                // Particle system colours (damaged rock PS)
                var main = rockDamagedSystem.main;
                main.startColor = Colours.rockDamagedPSColourGreenSafe;
                var col = rockDamagedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradGreenSafe;

                // Particle system colours (destroyed rock PS)
                main = rockDestroyedSystem.main;
                main.startColor = Colours.rockDamagedPSColourGreenSafe;
                col = rockDestroyedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradGreenSafe;

                // Particle system colours (crystal damaged PS)
                main = crystalDamagedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourGreenSafe;
                col = crystalDamagedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourGreenSafe;

                // Particle system colours (crystal destroyed PS)
                main = crystalDestroyedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourGreenSafe;
                col = crystalDestroyedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourGreenSafe;
            }

            // Blue safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.BLUE)
            {
                // Cube colours
                borderMaterial.color = Colours.borderColourBlueSafe;
                rockMaterial.color = Colours.rockColourBlueSafe;
                edgeMaterial.color = Colours.edgeColourBlueSafe;
                rockDamagedMaterial.color = Colours.rockDamagedColourBlueSafe;
                crystalMaterial.color = Colours.crystalColourBlueSafe;
                crystalDamagedMaterial.color = Colours.crystalColourBlueSafe;
                crystalMoreDamagedMaterial.color = Colours.crystalColourBlueSafe;
                crystalReallyDamagedMaterial.color = Colours.crystalColourBlueSafe;

                // Floor colour
                floor.GetComponent<MeshRenderer>().material.color = Colours.floorColourBlueSafe;

                // Particle system colours (damaged rock PS)
                var main = rockDamagedSystem.main;
                main.startColor = Colours.rockDamagedPSColourBlueSafe;
                var col = rockDamagedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradBlueSafe;

                // Particle system colours (destroyed rock PS)
                main = rockDestroyedSystem.main;
                main.startColor = Colours.rockDamagedPSColourBlueSafe;
                col = rockDestroyedSystem.colorOverLifetime;
                col.color = Colours.rockDamagedColourGradBlueSafe;

                // Particle system colours (crystal damaged PS)
                main = crystalDamagedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourBlueSafe;
                col = crystalDamagedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourBlueSafe;

                // Particle system colours (crystal destroyed PS)
                main = crystalDestroyedSystem.main;
                main.startColor = Colours.crystalDamagedPSColourBlueSafe;
                col = crystalDestroyedSystem.colorOverLifetime;
                col.color = Colours.crystalDamagedPSColourBlueSafe;
            }
        }

        // Set colours Normal
        else if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.NORMAL)
        {
            // Cube colours
            borderMaterial.color = Colours.borderColour;
            rockMaterial.color = Colours.rockColour;
            edgeMaterial.color = Colours.edgeColour;
            rockDamagedMaterial.color = Colours.rockDamagedColour;
            crystalMaterial.color = Colours.crystalColour;
            crystalDamagedMaterial.color = Colours.crystalColour;
            crystalMoreDamagedMaterial.color = Colours.crystalColour;
            crystalReallyDamagedMaterial.color = Colours.crystalColour;

            // Floor colour
            floor.GetComponent<MeshRenderer>().material.color = Colours.floorColour;

            // Particle system colours (damaged rock PS)
            var main = rockDamagedSystem.main;
            main.startColor = Colours.rockDamagedPSColour;
            var col = rockDamagedSystem.colorOverLifetime;
            col.color = Colours.rockDamagedColourGrad;

            // Particle system colours (destroyed rock PS)
            main = rockDestroyedSystem.main;
            main.startColor = Colours.rockDamagedPSColour;
            col = rockDestroyedSystem.colorOverLifetime;
            col.color = Colours.rockDamagedColourGrad;

            // Particle system colours (crystal damaged PS)
            main = crystalDamagedSystem.main;
            main.startColor = Colours.crystalDamagedPSColour;
            col = crystalDamagedSystem.colorOverLifetime;
            col.color = Colours.crystalDamagedPSColour;

            // Particle system colours (crystal destroyed PS)
            main = crystalDestroyedSystem.main;
            main.startColor = Colours.crystalDamagedPSColour;
            col = crystalDestroyedSystem.colorOverLifetime;
            col.color = Colours.crystalDamagedPSColour;
        }

        Debug.Log("MAP - SET COLOURS - COMPLETE");
    }

    // Random cube position inside border
    public Vector3 RandomPosition()
    {
        // Choose a random grid x and y coord
        int randomX = Random.Range(3, gridWidth - 3);
        int randomZ = Random.Range(3, gridHeight - 3);

        // Return position
        return new Vector3(randomX, 0.0f, randomZ);
    }

    // Random cube position inside border
    public void RandomPosition(GameObject go)
    {
        // Choose a random grid x and y coord
        int randomX = Random.Range(3, gridWidth - 3);
        int randomZ = Random.Range(3, gridHeight - 3);

        // Return position
        go.transform.position = new Vector3(randomX, 0.0f, randomZ);
    }

    // Position a gameobject in place of a rock cube
    public void PositionChestInRock(GameObject go)
    {
        // Set eth gameobject as not spawned
        bool spawned = false;

        // While the gameobject has not been spawned in
        while (!spawned)
        {
            // Counter for the open grid cubes
            int allCubesClosed = 0;

            // Choose a random grid x and y coord
            Vector3 randomPos = RandomPosition();

            // Assign the cubedata
            CubeData cubeData = gridCubeArray[(int)randomPos.x, (int)randomPos.z].GetComponent<CubeData>();

            // If the cube type is rock 
            if (cubeData.GetCubeType() == CUBETYPE.ROCK)
            {
                // Loop through the adjacent grid cubes - z axis
                for (int z = -1; z < 2; z++)
                {
                    // Loop through the adjacent grid cubes - x axis
                    for (int x = -1; x < 2; x++)
                    {
                        // Assign the cubedata
                        CubeData cubeDataToCheck = gridCubeArray[(int)randomPos.x + x, (int)randomPos.z + z].GetComponent<CubeData>();

                        // If the grid cube at the random coord is not open
                        if (!cubeDataToCheck.GetCubeOpen())
                        {
                            // Count the adjacent closed cube (plus 1)
                            allCubesClosed++;
                        }
                    }
                }

                // If all the adjacent grid cubes are closed
                if (allCubesClosed == 9)
                {
                    // Open the grid cube and set as chest cube 
                    cubeData.OpenGridCube();
                    cubeData.SetAsChestCube();

                    // Rotate gameobject to face out from walls
                    if (randomPos.x <= gridWidth / 2 && randomPos.z <= gridHeight / 2)
                        go.transform.Rotate(Vector3.up, 90.0f, Space.Self);

                    if (randomPos.x <= gridWidth / 2 && randomPos.z > gridHeight / 2)
                        go.transform.Rotate(Vector3.up, 180.0f, Space.Self);

                    if (randomPos.x > gridWidth / 2 && randomPos.z > gridHeight / 2)
                        go.transform.Rotate(Vector3.up, -90.0f, Space.Self);

                    // Set the gamobjects position
                    go.transform.position = new Vector3(randomPos.x, go.transform.position.y, randomPos.z);

                    // Break out of the loop and spawn the gameobject
                    spawned = true;
                }
            }
        }
    }

    /////////////////////////Grid Cubes//////////////////////////////

    public bool MapGenerationCompleted() { return mapGenerationComplete; }

    public GameObject[,] GridCubeArray() { return gridCubeArray; }

    public int GridWidth() { return gridWidth; }

    public int GridHeight() { return gridHeight; }

    ///////////////////////End of Functions/////////////////////////
}