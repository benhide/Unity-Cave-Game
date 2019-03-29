# CaveGame
Procedurally generate cave adventure game created for the UROS project at University of Lincoln.
<br />
***(Web build may need rebuilding as one lib file was too large to commit to GitHub)***

## Game features:
-	**Procedurally generated map / caves** <br />
1) Rock cubes – less juicy / fewer hits to destroy <br />
2) Crystal cubes – More juicy more hits to destroy <br />
<br />
  
-	**Quest system feat. Four quests** <br />
1) Collect pickups (gold)<br />
2) Search for keys to open chests to collect artefacts<br />
3) Kill monsters<br />
4) Exit the caves (complete game)<br />
5) Save/kill civilian either male/female/cat (hidden quest)<br />
<br />  
  
- **Selectable font sizes for in game text**<br />
<br />

- **Selectable colour settings for game**<br />
1) Normal mode<br />
2) Red colour blind mode<br />
3) Green colour blind mode<br />
4) Blue colour blind mode<br />
  <br />
  
-	**Selectable character**<br />
1) Male<br />
2) Female<br />
<br />

-	**Two monster types**<br />
1) Basic monster (less juicy damaged effects and shrinks on death / no death particles)<br />
2) Juicy monsters (increased juiciness when damaging / killing monster)<br />
  <br />
  
-	**Game and settings data collection via firebase. Collected data detailed below in firebase data section.**<br />
<br />

-	**Playable character has two actions**<br />
1) Digging through rock / crystal cubes – attacking monsters using pickaxe<br />
2) Dropping TNT to destroy rock / crystal cubes – killing monsters<br />

<br />

# Firebase Data Collected:

## Global game data manager enums:
1) **GENDER** { MALE = 0, FEMALE = 1 }
2) **CVDCOLOURSCHEME** { RED = 0, GREEN = 1, BLUE = 2, NONE = 3 }
3) **FONTSIZE** { SMALL = 0, MEDIUM = 1, LARGE = 2 }
4) **CIVILLIANTYPE** { MALE = 0, FEMALE = 1, CAT = 2 }
5) **MONSTERTYPE** { BASIC = 0, JUICY = 1 }


## Game stat classes

- **class ChestStat**
{<br />
    float spawnTime;<br />
    float openedTime;<br />
    bool opened;<br />
    Vector3 position;<br />
}<br />

- **class KeyStat**
{<br />
    float spawnTime;<br />
    float collectedTime;<br />
    bool collected;<br />
    Vector3 position;<br />
}<br />

- **class PickupStat**
{<br />
    float spawnTime;<br />
    float collectedTime;<br />
    bool collected;<br />
    Vector3 position;<br />
}<br /><br />

- **class QuestStat**
{<br />
    float startTime;<br />
    float endTime;<br />
    string name;<br />
    bool complete;<br />
}<br />

- **class MonsterStat**
{<br />
    float spawnTime;<br />
    float killedTime;<br />
    Vector3 killedPos;<br />
    Vector3 spawnedPos;<br />
    MONSTERTYPE type;<br />
    bool killed;<br />
    bool killedInQuest;<br />
}<br />

- **class CubeStat**
{<br />
    Vector3 position;<br />
    CUBETYPE type;<br />
}<br /><br />

- **class CubeDestroyedStat**
{<br />
    Vector3 position;<br />
    CUBETYPE type;<br />
    bool destroyed;<br />
    float destroyedTime;<br />
}<br />

- **class CivillianStat**
{<br />
    Vector3 position;<br />
    CIVILLIANTYPE type;<br />
    bool saved;<br />
    bool killed;<br />
     float savedKilledTime;<br />
}<br />

- **class TNTStat**
{<br />
    Vector3 position;<br />
    float usedTime;<br />
}<br />

- **class HealthStat**
{<br />
    float collectedTime;<br />
    float spawnTime;<br />
    bool collected;<br />
    Vector3 position;<br />
}<br />

## Game statistics collected

- **Player settings / statistics**
    - playerName;<br />
    - playerHealth;<br />
    - playerScore; (end of game)<br />
    - genderInitial;<br />
    - genderSelected;<br />
    - List<Vector3> playerPositionsList;<br />
    - List<TNTStat> playerTNTStatsList;<br />
    - List<HealthStat> playerHealthPickupStatsList;<br /><br />

- **Accessability settings**
    - selectedFontSize;<br />
    - selectedColourScheme;<br />
    - selectedCVDColourScheme;<br /><br />

- **Grid cube counts**
    - numberOfTotalCubes;<br />
    - numberOfRockCubes;<br />
    - numberOfCrystalCubes;<br />
    - numberOfCubesDestroyed;<br />
    - numberOfCubesDestroyedRock;<br />
    - numberOfCubesDestroyedCrystal;<br /><br />

- **Pickup statistics**
    - pickupsTotal;<br />
    - pickupsCollected;<br /><br />

- **Chests statistics**
    - chestsTotal;<br />
    - chestsOpened;<br />
    - keysTotal;<br />
    - keysCollected;<br /><br />

- **Monster statistics**
    - monstersTotal;<br />
    - monstersBasic;<br />
    - monstersJuicy;<br />
    - monstersKilledTotal;<br />
    - monstersKilledBasic;<br />
    - monstersKilledJuicy;<br /><br />

- **Game statistics**
    - string[] questNames;<br />
    - questsNumber;<br />
    - List<QuestStat> questStatsList;<br />
    - questOldMinerPosition;<br /><br />

- **Civillian statistics**
    - civillianType;<br />
    - civillianStats;<br />
    - civillianPosition;<br /><br />

- **Cube positional statistics**
    - List<CubeDestroyedStat> cubePositionsDestroyed;<br />
    - List<CubeStat> cubePositionsStart;<br />
    - List<CubeStat> cubePositionsEnd;<br /><br />

 - **Monster positional statistics**
    - List<MonsterStat> monsterKilledStatsBasicList;<br />
    - List<MonsterStat> monsterKilledStatsJuicyList;<br /><br />

- **Objective positional statistics**
    - List<PickupStat> listPickupStats;<br />
    - List<ChestStat> listChestStats;<br />
    - List<KeyStat> listKeyStats;<br /><br />


# Colour and Font Size settings
To modify colour settings the values sorted in the Colours C# file need to be modified.<br />

The Colours C# file is located in:<br />
**CaveGame-master\UROS 1.12\Assets\\_Utilities\Scripts folder (called "Colours.cs")**
<br />

To modify font sizes settings the values sorted in the FontSizes C# file need to be modified.<br />

The FontSize C# file is located in:<br />
**CaveGame-master\UROS 1.12\Assets\\_Utilities\Scripts folder (called "FontSizes.cs")**
<br />

Colours used in the game can be set for all colour blind settings and normal colour settings. Colours defined in this file will automatically be used during the game. The Unity colours used in this file are clearly named and should be easy to modify with any RGB values.<br />
