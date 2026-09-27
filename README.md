# HERO SHOOTER MAKER (HSM)

Replicates basic system seen in videogames based on Hero Shooter and MOBA Genres

### Install Guide: 

Made on Unity editor version 6000.4.2f1

Made on MacOS (theoretically should work on Windows as well)

The primary content and demo is placed in assets/HeroShooterMaker
The package file can be accessed in assets/HSMExport.

## Player Focused Feature
- Movement & camera controls 
  - Use the movement input to make the player move. Move the mouse to control the player camera.

- Player core (hitpoints, damage multiplier, etc.)
  - holds player stats in a scriptable object, and updates other scripts based on player's stats, and handle some player events
 
- Player Interaction
  - allow for interaction between players (such as dealing damage to each other)

- Ability 
  - it listens for events then makes something happen. has internal cooldown that controls usage

- Status effect 
  - handle temporary modifications of a player
  
- Projectiles
  - Robust projectile system for implementing wide variety of attacks

- AI players
  - Primarily for making players who are not controlled by a client. also applicable for making other AI controlled entity such as spawnables or npc monsters

## Match Focused Features
- Teams 
  - Team objects hold players that belong to the same team, and handle team wide functions

- Objective/win condition
  - Parent class for creating objective/win condition. Demo includes an objective that functions like king of the hill.

- Game manager 
  - Manage game wide events, match timer, etc.
 
## Not Included
- Multiplayer: This package does not include any sort of networking solution, but it should be compatible or can be modified to be compatible with most options. This package was not designed with a specific networking solution in mind. 
