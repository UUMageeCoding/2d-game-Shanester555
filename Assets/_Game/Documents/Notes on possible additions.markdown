# MyMarkdownFile

1. Checkpoints
   1. Add a seperate script to set the y-value for each checkpoint's respawn position
   2. call that script in the player controller script to properly set y position instead of player respawning at whatever height they hit the checkpoint
2. add conveyor belts
   1. use the same code as the moving platform to make everything a child so I could later add boxes to move on the conveyors
3. add sprites
   1. player
      1. animate face
      2. update sprite when permanent upgrades are found
   2. collectables
      1. have the upgrades be parts from other bots in the factory
4. other robots?
   1. have them be fairly static in behaviour, just some flavour for the game
5. switches and doors
   1. have some switches control moving platforms or even obstacles such as conveyor belts or crushers
   2. have the main goal of the demo to be to get past a door which requires a certian amount of collectables
6. electric field trigger
   1. have an obstacle that uses a state machine to detect when a player gets close and activate a large electric field around it
7. turret
   1. have an obstacle which shoots projectiles at the player within a certian range
   2. pseudocode:
      1. detect player within range
      2. track player
      3. create object moving towards player
      4. set speed, zero gravity, kill player tag, destroyed on any collision
8. antenna upgrade
   1. add an antenna to the head which flashes when a collectable is nearby
9. Pause screen
   1.  have it access options and allow player to quit to screen


Changes:
1. try combining sprite animations to the player character to change how their eyes look
2. fix wall jump you fucking loser
3. change moving saws to slow down near peak, most likely by slowing the speed down as the distance between two points closes

Checklist (needs to be finished):
1. finish art for the main menu
   1. finish the options part of the menu
2. make the final two screens of the game
3. fix all the remaining level transitions
4. finish the second ending cutscene
   1. add sound to both cutscenes
5. Add text guide for interacting with final door and telling player if they have enough gears
   1. maybe add a visual cue on where they can interact, like a light source?