# Blackguard

Welcome to Blackguard :D

## Vision Statement 
Blackguard is a roguelike, 2D ASCII adventure game. Blackguard will be a fantasy environment with a mix of premade and randomly generated levels and biomes. The player can explore and complete quests assigned by NPCs, defeat bosses unique to each biome, and interact with in-game characters and shopkeepers. The setting will be fantasy but have friends and foes from fantasy worlds and exotic mythos. Each biome will have unique enemies that provide the player with a difficult challenge. The player will be expected to die repeatedly before challenging the final boss.

## Members
Landon Beert - Product Manager, [LandonLeo](https://github.com/LandonLeo), [Digital Portfolio](https://codermerlin.academy/users/landon-beert/Digital%20Portfolio/index.html)
Aidan Williams - Project Manager, [ppebb](https://github.com/ppebb), [Digital Portfolio](https://codermerlin.academy/users/aidan-williams/Digital%20Portfolio/index.html)
Dinesh Sayana - Release Manager, [Derate11](https://github.com/Derate11), [Digital Portfolio](https://codermerlin.academy/users/dinesh-sayana/Digital%20Portfolio/index.html)
Anuva Signh - QA Analyst, [anuvasingh](https://github.com/anuvasingh), [Digital Portfolio](https://codermerlin.academy/users/anuva-singh/Digital%20Portfolio/index.html)
Nathan Goldman - UI/UX Designer, [Nathan-Goldman](https://github.com/Nathan-Goldman), [Digital Portfolio](https://codermerlin.academy/users/nathan-goldman/Digital%20Portfolio/index.html)

### For instructions on how to set up the programming environment, look [here](https://github.com/ppebb/isp2/blob/main/docs/setup.md)

## Architecture Design Document
The architecture of this project centers around a single-threaded game loop synchronized at 60 ticks per second. There will be a separate thread to poll for input and a separate thread for drawing text to the screen.

The project will follow an OOP design structure, with base classes for things such as tiles, UI elements, entities, and the player, of which instances will be made and stored in a central list to be updated 60 times a second.

Entities and the player will each have an Update method that will handle movement and AI for each entity present on the screen and a Draw method that handles how it is rendered to the screen, along with methods to handle interactions with the players and dialogue.

Each screen will be implemented off of a base class that defines how it looks and the functionality of each element (buttons, text, etc.), where each screen will be swapped based on user input.

World generation will be implemented in a chunking system, with each chunk being saved and loaded to disk based on what region of the map is loaded.

Saves will be handled using a JSON format to allow players to pause and resume without leaving the game open.

## Coding standards and guidelines
Ensure that all code is tested before committing, and ensure you have a proper formatter run based on the configuration files in the repository.

## Setting
- The setting of Blackguard will be a medieval fantasy with technology similar to 16th-century technology. The world will have fantasy races like elves and have magic.
-  The magic will have an elemental base to it but will have other variations.
-  As stated before, no advanced technology will be present, including guns.

## Features
- Different-sized health and mana bars depending on chosen classes and race
- The different spells in the game will use mana in different amounts. Items and skills will be the only way to replenish the mana bar. This restriction will cause the player to be careful when they use their spells.
- The sprint bar will allow the player to run faster and flee from battle.
- The different biomes of the game will contain unique enemies, NPCs, items, shops, and bosses.
- The different enemies of the game will have unique abilities and stats. This uniqueness will allow each enemy to feel different and to have a new strategy for defeating them.
- The player and enemies can apply buffs and debuffs to themselves and each other. These status conditions will lower and boost the afflicted’s stats and drain health or mana.
- The overworld will be in real-time, meaning the enemies will move even if you don’t. The real-time overworld will keep the player on their toes and always expecting an attack.
- If the player dies at any point in their adventure, they will be forced back to the start of the game. Any death will also delete the savefile for no cheating.
- The player, during the playthrough can upgrade equipment to increase stats and defense. Upgrades can be acquired from enemies, bosses, shopkeepers, and quest completion.
- The combat of Blackguard will be turn-based allowing for strategy and complex thought.
- At the start of each Blackguard run the player will pick a starting race and class. These will each independently alter the stats of the player. These altering stats will create opportunities for replayability or changing of strategies.
- If the player is not willing to fight directly can shoot projectiles at enemies to deal damage. The enemies can also set up traps to shoot the player.
- The game will feature colored text to aid ASCII visuals.
- The game will also contain [Nerd Font](https://www.nerdfonts.com/) to help in ASCII art.
- Each biome will feature an Area boss. These bosses will have a unique strategy to attack the player and be stronger than the standard enemy. 
- The final boss will be the ultimate challenge of Blackguard, providing one last combat experience before the reward. It will incorporate the skills learned before in a final challenge.
- The experience system of Blackguard will be level-based. The experience gained will contribute to the level of the player. When the player levels up they will get an increase in stats and new skills depending on their race and class.
- The game will have a global scoreboard between players that is based of speed and perfection.


## Databases
The global database for the leaderboards will be interfaced with a simple JSON API that will return global scores as well as an endpoint to upload scores. The database will be a simple Postgresql table with columns for score, playtime, % completion, etc.
