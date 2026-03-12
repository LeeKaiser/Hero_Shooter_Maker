# HERO SHOOTER MAKER (HSM)

Replicates basic system seen in videogames based on Hero Shooter and MOBA Genres

Install Guide: 

Made on Unity editor version 6000.2.10f1

Made on MacOS (theoretically should work on any OS)


## Player Focused Feature
- Movement & camera controls 
  - Use the movement input to make the player move. Move the mouse to control the player camera.

- Player core (hitpoints, damage multiplier, etc.)
  - holds player stats in a scriptable object, and updates other scripts based on player's stats, and handle some player events
 
- Player Interaction
  - allow for interaction between players (such as dealing damage to each other)

- Ability array 
  - it listens for events then makes something happen. has internal cooldown that controls usage

- Inventory array 
  - holds items

- Status effect array 
  - handle temporary modifications of a player

- AI players
  - Primarily for making players who are not controlled by a client. Should also be applicable for making other AI controlled entity such as spawnables or npc monsters

## Match Focused Features
- Multiplayer
  - A match of a hero shooter game needs to be hosted from a server and accessed by clients, who control an individual character.

- Teams 
  - Team objects hold players that belong to the same team, and handle team wide functions

- Objective/win condition
  - Various template objectives, such as a "payload", "king of the hill", "generic scoreing system", etc.

- Game manager 
  - Manage game wide events, match timer, etc.
