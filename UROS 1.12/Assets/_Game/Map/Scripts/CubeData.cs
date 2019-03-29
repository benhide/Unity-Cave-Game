using UnityEngine;

// Cube data class
public class CubeData : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Grid cube settings")]
    [SerializeField]
    bool cubeOpen = false;
    [SerializeField]
    bool cubeDestructible = true;
    [SerializeField]
    bool cubeDestroyed = false;

    [Header("Crystal light settings")]
    public GameObject crystalLight;
    public GameObject lightObject;
    public float lightDistanceY;

    [Header("Grid cube position and type settings")]
    [SerializeField]
    Vector3 cubeCoord;
    [SerializeField]
    CUBETYPE cubeType;

    [Header("Grid cube health")]
    [SerializeField]
    int rockHealth;
    [SerializeField]
    int crystalHealth;
    [SerializeField]
    int currentHealth;
    [SerializeField]
    int maxHealth;

    [Header("Grid cube SFXs")]
    public AudioSource soundFx;
    public AudioClip rockDamagedClip;
    public AudioClip rockDestroyedClip;
    public AudioClip crystalDamagedClip;
    public AudioClip crystalDestroyedClip;

    [Header("Script references")]
    public Map map;

    // Event callback
    public delegate void OnPlayerExitEvent(GameObject player);
    public static event OnPlayerExitEvent onPlayerExit;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the references
        map = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>();

        // Get the audio source
        soundFx = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for damage and if destroyed
        HasBeenDamaged();
        HasBeenDestroyed();

        // Set the crystal light
        CrystalLight();
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other game object is the player
        if (other.gameObject.tag == Tags.playerTag)
        {
            // If its the exit cube
            if (cubeType == CUBETYPE.EXIT)
            {
                // Callback
                if (onPlayerExit != null)
                    onPlayerExit.Invoke(other.gameObject);
            }
        }

        // If the other game object is a cave object
        if (other.gameObject.tag == Tags.caveTag)
        {
            // If the cube is destructible
            if (cubeDestructible)
            {
                // X axis edge detection
                for (int x = -1; x < 2; x++)
                    EdgeDetection(x, 0);

                // Z axis edge detection
                for (int z = -1; z < 2; z++)
                    EdgeDetection(0, z);

                // Set the cube as empty/open
                SetAsEmptyCube();
            }
        }
    }

    // Set as a border cube
    public void SetAsBorderCube()
    {
        cubeDestructible = false;
        cubeType = CUBETYPE.BORDER;
        GetComponent<MeshRenderer>().material = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>().borderMaterial;
        transform.rotation = Random.rotation;
    }

    // Set as a crystal cube
    public void SetAsCystalCube()
    {
        cubeDestructible = true;
        cubeType = CUBETYPE.CRYSTAL;
        GetComponent<MeshRenderer>().material = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>().crystalMaterial;
        currentHealth = maxHealth = crystalHealth;
        transform.rotation = Random.rotation;
    }

    // Set as a rock cube
    public void SetAsRockCube()
    {
        cubeDestructible = true;
        cubeType = CUBETYPE.ROCK;
        GetComponent<MeshRenderer>().material = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>().rockMaterial;
        currentHealth = maxHealth = rockHealth;
        transform.rotation = Random.rotation;
    }

    // Sete as an edge cube
    public void SetAsEdgeCube()
    {
        GetComponent<MeshRenderer>().material = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>().edgeMaterial;
    }

    // Set as a chest cube
    public void SetAsChestCube()
    {
        cubeDestructible = false;
        cubeType = CUBETYPE.CHEST;
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<MeshRenderer>());
        cubeDestroyed = true;
        cubeOpen = false;
        currentHealth = maxHealth = 0;
    }

    // Set as an empty cube
    public void SetAsEmptyCube()
    {
        cubeDestructible = false;
        cubeType = CUBETYPE.EMPTY;
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<MeshRenderer>());
        cubeDestroyed = true;
        cubeOpen = true;
        currentHealth = maxHealth = 0;
    }

    // Set as a full cube
    public void SetAsFullCube()
    {
        cubeOpen = false;
    }

    // Open the grid cube if blocked
    public void OpenGridCube()
    {
        // Remove the opened cube from the grid cube counts
        GameDataManager.instance.RemoveOpenedCubeFromCounts(cubeType);

        // Disable the cube components and reset the cube state
        SetAsEmptyCube();

        // Edge detection
        for (int x = -1; x < 2; x++)
            for (int z = -1; z < 2; z++)
                EdgeDetection(x, z, false);
    }

    // Edge decetion along one axis
    void EdgeDetection(int offsetX, int offsetZ, bool initialCheck = true)
    {
        // Assign references to the cube gameobject and cubedata script
        GameObject cube = map.GridCubeArray()[(int)cubeCoord.x + offsetX, (int)cubeCoord.z + offsetZ];
        CubeData cubeData = cube.GetComponent<CubeData>();

        // If the cube exsists on the x axis and is not of type metal e.g. (border)
        if (cube.gameObject != null && cubeData.cubeType != CUBETYPE.BORDER)
        {
            // If the edge detection is during map generation
            if (initialCheck)
            {
                // Set the cube as an edge cube - update material
                cubeData.SetAsEdgeCube();

                // If the edge cube is a cyrstal cube change it to a rock cube
                if (cubeData.cubeType == CUBETYPE.CRYSTAL)
                {
                    cubeData.SetAsRockCube();
                    cubeData.SetAsEdgeCube();
                }
            }

            // Else if the edge dectetion is in game
            else if (!initialCheck && cubeData.cubeType == CUBETYPE.ROCK && cube.GetComponent<MeshRenderer>() != null)
                cubeData.SetAsEdgeCube();
        }
    }

    // Check to see if grid cube has been damaged
    void HasBeenDamaged()
    {
        // If the grid cube has been damaged
        if (currentHealth < maxHealth && currentHealth != 0 && GetComponent<MeshRenderer>() != null)
        {
            // If the grid cube is rock - update to damaged rock material
            if (cubeType == CUBETYPE.ROCK && GetComponent<MeshRenderer>().material != map.rockDamagedMaterial)
                GetComponent<MeshRenderer>().material = map.rockDamagedMaterial;

            // If the grid cube is crystal - update to damaged or more damaged crystal material
            else if (cubeType == CUBETYPE.CRYSTAL)
            {
                // If the current health is greater than half max health set as damaged 
                if (currentHealth > maxHealth / 2 && GetComponent<MeshRenderer>().material != map.crystalDamagedMaterial)
                    GetComponent<MeshRenderer>().material = map.crystalDamagedMaterial;

                // If the current health is greater 2 set as more damaged 
                else if (currentHealth == 2 && GetComponent<MeshRenderer>().material != map.crystalMoreDamagedMaterial)
                    GetComponent<MeshRenderer>().material = map.crystalMoreDamagedMaterial;

                // Else set as really damaged material
                else if (GetComponent<MeshRenderer>().material != map.crystalReallyDamagedMaterial)
                    GetComponent<MeshRenderer>().material = map.crystalReallyDamagedMaterial;
            }
        }
    }

    // Check to see if grid cube has been destroyed
    void HasBeenDestroyed()
    {
        // If the cube is dead
        if (currentHealth <= 0 && !cubeDestroyed)
        {
            // X axis edge detection
            for (int x = -1; x < 2; x++)
                EdgeDetection(x, 0, false);

            // Z axis edge detection
            for (int z = -1; z < 2; z++)
                EdgeDetection(0, z, false);

            // Disable the cube components - Count the destroyed cube
            GameDataManager.instance.CountDestroyedGridCubeType(cubeType, transform.position);
            SetAsEmptyCube();
        }
    }

    // Called when the cube has taken damage
    public void CubeDamaged(int damage)
    {
        // Remove health from cube
        currentHealth -= damage;

        // If the cube damaged is a rock play particle effects
        if (cubeType == CUBETYPE.ROCK)
        {
            // If the cube has been damaged
            if (currentHealth > 0)
            {
                ParticleSystemController.InstaniateCubeParticleSystem(map.rockDamagedSystem, transform);
                soundFx.PlayOneShot(rockDamagedClip, 0.25f);
            }

            // Cube has been destroyed
            else
            {
                ParticleSystemController.InstaniateCubeParticleSystem(map.rockDestroyedSystem, transform);
                soundFx.PlayOneShot(rockDestroyedClip, 0.25f);
            }

            // Camera shake
            CameraShaker.Instance.Shake(CameraShakePresets.RockDamaged);
        }

        // If the cube damaged is a crystal play particle effects
        if (cubeType == CUBETYPE.CRYSTAL)
        {
            // If the cube has been damaged
            if (currentHealth > 0)
            {
                ParticleSystemController.InstaniateCubeParticleSystem(map.crystalDamagedSystem, transform);
                soundFx.PlayOneShot(crystalDamagedClip);
            }

            // Cube has been destroyed
            else
            {
                ParticleSystemController.InstaniateCubeParticleSystem(map.crystalDestroyedSystem, transform);
                soundFx.PlayOneShot(crystalDestroyedClip);
            }

            // Camera shake
            CameraShaker.Instance.Shake(CameraShakePresets.CrystalDamaged);
        }
    }

    // Add a light to the crystal cubes
    void CrystalLight()
    {
        if (cubeType == CUBETYPE.CRYSTAL && lightObject == null)
        {
            lightObject = Instantiate(crystalLight, transform.position, Quaternion.identity, transform) as GameObject;
            lightObject.transform.position = new Vector3(transform.position.x, lightDistanceY, transform.position.z);
        }
    }

    ///////////////////////////Cube Data////////////////////////////

    public int CurrentHealth()
    {
        return currentHealth;
    }

    public CUBETYPE GetCubeType()
    {
        return cubeType;
    }

    public void SetCubeType(CUBETYPE type)
    {
        cubeType = type;
    }

    public bool GetCubeOpen()
    {
        return cubeOpen;
    }

    public void SetCubeOpen(bool open)
    {
        cubeOpen = open;
    }

    public void SetCubeCoord(Vector3 coord)
    {
        cubeCoord = coord;
    }

    public Vector3 GetCubeCoord()
    {
        return cubeCoord;
    }

    public bool CubeDestructible()
    {
        return cubeDestructible;
    }

    public bool CubeDestroyed()
    {
        return cubeDestroyed;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    ///////////////////////End of Functions/////////////////////////
}