## Table Of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

  - [Introduction](#introduction)
  - [Get Started](#Get-Started)
    - [How to install](#How-to-install)
    - [Using Scriptable Object](#Using-Scriptable-Object)
    - [How to use editable variables](#How-to-use-editable-variable)
  - [Features](#Features)
  - [Documentation](#Documentation)
  - [Misusing / Limitations](#Misusing-/-Limitations)
  - [License](#License)

## Introduction

This is a School Project about Procedural Generation on Unity. Unity doesn't manage pretty well asynchronous programmation so I need to use [UniTask](https://github.com/Cysharp/UniTask), "an open source library which provides an efficient async/await integration to Unity".

For Procedural Generation, I used Binary Space Partition for dungeon rooms and Perlin / OpenSimplex2S Noises for outside map. Noises created by [FastNoise](https://github.com/Auburn/FastNoiseLite), "an extremely portable open source noise generation library with a large selection of noise algorithms".

## Get Started 
## How to install
If you download this repo, you don't normally have to install anything. However, if you have any issue, follow these steps :
- [Download UnityHub](https://unity.com/download)
- Install one Unity version
- Create and open your project
- On the Unity Editor, go to Edit -> Project Settings -> Package Manager and like the screen below copy/paste these then apply :
-   Name : package.openupm.com
-   URL : https://package.openupm.com
-   Scope(s) : com.cysharp.unitask
  <img width="1891" height="927" alt="image" src="https://github.com/user-attachments/assets/ed60363e-7f59-48b0-a304-6d8213f1e436" />

- Go on Unity Editor package Manager -> My registries
- Install UniTask
  <img width="1881" height="942" alt="image" src="https://github.com/user-attachments/assets/90596d99-89f8-4239-bead-6ae489d08ad2" />

- Well done ! Now, you can use UniTask and start seeing what can you done with this tool project ! Create a new Level to make your own.
## Using Scriptable Object
## How to use editable variables

## Features
(Quick overview with gif)

## Documentation
(Grid, Cell and ProceduralGenerationMethod Architecture)
(Specify algorithm utility)



- Noise Script :
Allow to generate tiles proceduraly thanks to Noise (Perlin Noise, OpenSimplex2S and more) and seed. You can change the generation by seed, by noiseType, and few other parameters. It will generate different map except if you use same parameters and seed.
- Details :
- ApplyGeneration() : The whole logic about making the tile generation with noise. First Init the noise parameters then set each cells' tiles type and build the map.
<img width="798" height="521" alt="image" src="https://github.com/user-attachments/assets/4aa067ef-0943-4f2e-b780-bf1af4d7c1b0" />

- InitNoise() : 
Don't worry about this part, you can directly change the values in the Unity inspector. However, if some parameter is missing, add it here.
I recommand you to use this preview tools to make your own noise and then change values in Unity inspector
<img width="572" height="308" alt="image" src="https://github.com/user-attachments/assets/1308bb11-bfad-408d-ad28-27bde13c967b" />
<img width="522" height="801" alt="image" src="https://github.com/user-attachments/assets/9371d789-41b3-4627-aaba-a4a4b3653d86" />

- MakeMap() :
The function TryGetCellByCoordinates() is a Grid function which check if the cell exist at the specified coordinates. Then, if it's a valid place to create tile (contained by the Grid), the AddTileToCell create specific tile (cell.Value can be set GRASS_TILE_NAME for example)
<img width="730" height="143" alt="image" src="https://github.com/user-attachments/assets/75b8ec2e-ae08-4425-b501-88a6347a72d6" />

- CheckCell() :
This function is WIP !! You may change the formula because it works but not as I expected.
You can modify the grass and water quantity, using the Unity inspector values (cf. screenshot)
<img width="866" height="180" alt="image" src="https://github.com/user-attachments/assets/52c8d676-b9e0-4741-a990-1ca12ab2c60d" />
<img width="528" height="757" alt="image" src="https://github.com/user-attachments/assets/bf756adb-f1cc-48f5-90bc-94e7f3c2168a" />



Cellular Automaton Script :

Details :
- InitMap() :
It's a simple white noise on cells contained by Grid
<img width="850" height="423" alt="image" src="https://github.com/user-attachments/assets/b8c3c426-2030-44bb-b140-bbe8647518a6" />

- CheckCell() :
Check each cells' neighborhood. Depending on the type of neighbor cells, it'll change the type of the current cell.
<img width="1011" height="552" alt="image" src="https://github.com/user-attachments/assets/1eb617e5-e813-4af4-a3bc-cc7e8eb11bee" />




- ProceduralGenerationMethod Script : 
Allow to generate a map proceduraly thanks to few rules that you can set and seed. First, it generate a white noise, then, depending on the neighbor tiles and your rules, it generates the same or different tiles (for example, if you have more than 3 water tile and few grass in the cell neighborhood, the checked cell will be a sand_tile). You can change the proportion of the grass and water to customize your map. It will generate different map except if you use same parameters and seed. 

Details :
- Initialize() :
GetScriptableObject once for each different tile instead of getting these each step allows to increase the algorithm performance by approximatively 250% !
<img width="938" height="162" alt="image" src="https://github.com/user-attachments/assets/b1843e50-d6c5-4154-9ba9-d6385185a157" />

- CanPlaceRoom() :
This one is used for Binary Space Partition. Allows (or not) to split the room by sending boolean.
<img width="1262" height="403" alt="image" src="https://github.com/user-attachments/assets/8599bf20-8bcf-4635-957a-e86e9c61c9d4" />

- AddTileToCeil() :
This method doesn't replace an already existing cell with a same tile. If it's different, it does.
Here's a little DRY so it may be improved
<img width="612" height="522" alt="image" src="https://github.com/user-attachments/assets/bd0a4142-9592-4455-a613-2e645cf88aa3" />

- Grid Script : Allows to generate grid by parsing the sized land, which be used in procedural tile generation scripts.



- Cell Script : Contain tile type, using ScriptableObject by tile type


## How to add new algorithm
If you need to add new algorithm, you have to using VTools.Grid and the RandomService class by using VTools.RandomService and new RandomService

## Misusing / Limitations
The bad values / uses

## License
[Voir la licence](LICENSE)
