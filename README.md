# <h1 align="center"> RunnerTestGame </h1>

## 📃Description
Runner game in hyper-casual genre, the goal is to avoid as many obstacles as possible with speed of the game increasing over time, game contain authorization module, 
leaderboard, UI, HUD, database to store player score, infinite level generation, infinite obstacle spawn, free to move player control, player animations, admob ads.
As a reference i take "subway surfers" game.

## 🚀Modules
|      Module Name                 |                                Description                                           |
|:---------------------------:|:------------------------------------------------------------------------------------:|
|Firebase realtime database   |  to store player score for leaderboard                                               
|Firebase authorization module|  registration and login features
|Admob ads                    |  player can watch an ad to revive ones per run after game over
|Obstacle pool                |  to optimize spawn and destroy logic for obstacles, factory pattern was used for this
|Player animator              |  player model using different kind of animations while moving
|Player movement              |  player can move freely in setted boundaries, state pattern was used for this
|UI                           |  game contains UI for main menu, leaderboard, authorization module and HUD
|Lunar Console                |  for debugging on a phone
|Cinemachine                  |  for smooth player camera movement


## <p id="technologies">⚙️Technologies</p>
* DOTween 
* Firebase
* Admob
* Lunar Console
* Cinemachine
