## Table Of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details

  - [Introduction](#introduction)
  - [Get Started](#Get-Started)
    - [How to install](#How-to-install)
    - [Using Scriptable Object](#Using-Scriptable-Object)
    - [How to use editable variables](#How-to-use-editable-variable)
  - [Features](#Features)
  - [Documentation](#Documentation)
  - [Misusing / Limitations](#Misusing-/-Limitations)
  - [License](#License)
</summary>
## Introduction
<img width="387" height="291" alt="image" src="https://github.com/user-attachments/assets/98b02619-a580-4fc3-9fc4-1c87eba28281" />

Squalaland is a RPG game with zelda CDI references, made for a School Project about Procedural Generation on Unity. Unity doesn't manage pretty well asynchronous programmation so I need to use [UniTask](https://github.com/Cysharp/UniTask), "an open source library which provides an efficient async/await integration to Unity".

For Procedural Generation, I used Binary Space Partition for dungeon rooms and Perlin / OpenSimplex2S Noises for outside map. Noises created by [FastNoise](https://github.com/Auburn/FastNoiseLite), "an extremely portable open source noise generation library with a large selection of noise algorithms".
I plan to add procedural Names generator thanks to [Markov Name Generator](https://github.com/Tw1ddle/MarkovNameGenerator?tab=readme-ov-file) and personally cooked procedural Spells Generator. So in this video game, player won't have the same spells for each sessions ! That'll make it harder. 

We have one constraints : the theme of the game : "below the surface" so we made a dungeon under the ground.

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
(How to add new algorithm)

## Misusing / Limitations
The bad values / uses

## License
[Voir la licence](LICENSE)
