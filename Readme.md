![Unity 2021.3.5f1 badge](https://img.shields.io/badge/Unity-2021.3.5f1-blue)
![Elympics 0.7.0 badge](https://img.shields.io/badge/Elympics-0.7.0-white)
![ruby 2.7.2 badge](https://img.shields.io/badge/ruby-2.7.2-red)

![Elympics](https://static.elympics.cc/assets/logo/elympics-light.png#gh-dark-mode-only)
![Elympics](https://static.elympics.cc/assets/logo/elympics-dark.png#gh-light-mode-only)

# Open-source FPS template by Elympics

This repository is a *free* FPS template for Unity game developers that want to build their first multiplayer shooter. We designed it as a sample browser game to give you a real feel of its possibilities, but you can also access its source code here.
You can use the template as a basis for your own shooter or any other multiplayer game for 2-4 players: the mechanics it features are universal and may be re-used for other genres. You can find their complete list in the next section.

This template is meant to be a learning resource for the new users of Elympics, our standard industry framework for blockchain-integrated multiplayer games. It’ll help you understand how it works and show you the good practices related to it.


> __IMPORTANT__: 
> This project uses Git Large Files Support (LFS). Downloading this repository in a zip file **will not work**. You need to clone this repo using `git clone` with LFS.
> You can download Git LFS here: https://git-lfs.github.com/.

## How to use the template?

### Trying it out
- You can see the game in action in your browser (other than *Mozilla Firefox*) [here](https://template-fps.elympics.cc/)
- Use *Play Solo* button to try it alone or *Play Duel* if you have someone to play with. Note that you could even play with someone connecting from the Unity editor!
- Use *WASD* or *arrow keys* to move, *space* to jump, *mouse* to target and *left click* to shoot. With *1* and *2* keys you can change your weapon. Pressing *Tab* shows you game stats.

### Development
- Launch this project in Unity (version 2021.3.5f1 is recommended)
- To see it in action inside the editor, run the game starting from the *MainMenu* scene, just like in the browser.
- If you want to make changes and be able to upload your own builds, [register](https://docs.elympics.cc/getting-started/add-elympics/) your own game in the [Elympics panel](https://panel.elympics.cc/login) and change *GameConfig* in *Tools/Elympics/ManageGamesInElympics* in Unity.
- Note that after providing your own *GameConfig* menu will try to connect to your own server build, which have to be [uploaded](https://docs.elympics.cc/getting-started/upload-builds/) first. To be able to play solo, you also have to configure proper queue in the *Elympics panel* mentioned before.
- To start building on the template, use [half remote](https://docs.elympics.cc/getting-started/run-locally/#half-remote-mode) game mode and start from *GameplayScene* on both original Unity instance and a clone.
- After major steps it is worth to test the game online - by uploading server build and playing from the menu or using *Debug Online Player* game mode.

## Features

- Character movement 🚶
- Jumping 🤸‍♀️
- Projectile weapon 🔫
- Raycast weapon 🔦
- Weapon switching ♼
- Dealing damage 💥
- Player HP system ❤️‍🩹
- Player respawn 💆
- Match phase synchronization (start and finish based on predefined criteria) 🔂
- Basic HUD 🖥
- Kill cam 🎥
- Character animation synchronized 🏃
- Menu 📋
- Matchmaking 🔀
