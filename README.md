# Cave Adventure Arcade Game
Procedurally generated cave adventure game created for the UROS project at University of Lincoln.
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

___
*Click on image to see video*
<a href="https://www.youtube.com/watch?v=xgxYK691VSQ&t=50s" target="_blank"><img src="https://img.youtube.com/vi/xgxYK691VSQ/maxresdefault.jpg" 
alt="PhysX Rugby Kicking Game" width="920" height="517" border="0" /></a>

<br />

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
