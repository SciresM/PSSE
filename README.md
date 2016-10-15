# PSSE
*Pokemon Shuffle Save Editor* - Allows for the editing of most of the interesting parts of exported Pokemon Shuffle savegames.

![PSSE](http://i.imgur.com/HVpqjIb.png)

##Usage
1. Export a Pokemon Shuffle savefile onto your computer and open it in PSSE using the "*Open savedata.bin*" button.
2. Edit stuff using incredibly user-friendly GUI :+1:
3. Save using the "*Save savedata.bin*" button and inject that modified savefile back into your game.

##Tips
* **General**
  * You can use Tab key to quickly navigate between controls.
  * Double-click *Filepath field* to "eject" your save from PSSE and restore its windows to its original state (your edits won't be saved).

* **Owned Pokemon**
  * Left-click one of the *Team icons* to quickly access that pokemon.
  * Right-click a *Team icon*, then another, to switch the pokemons in these slots.
  * Ctrl+clic a *Team icon* to insert current active pokemon into that slot.
  * Click *Main pokemon icon* to switch between : Caught pokemon w/ everything maxxed out or Uncaught.
  * Click *Skill+ sprite* to switch between : Min or Max skill level for that pokemon.
  * Click *Lollipop sprite* to switch between : Min or Max lollipops and corresponding level for that pokemon.
  * Click *SpeedUps sprites* to switch between : Min or Max speedUps given to corresponding mega.
  
* **High-scores**
  * Left-click *Stages icons* to circle through ranks for that stage (S > A > B > C > S...).
  * Right-click *Stages icons* to switch between : Completed or Uncompleted stage.
  * Click *"Checked" sprite* to switch between : Safe or Unsafe mode.
  * In safe mode :
    * uncompleting a stage auto-uncompletes every stage that's after it (resets highscores & ranks),
    * completing a stage auto-completes every stage that's before it.
  * In unsafe mode, all of these are disabled.
  * When completing a stage or changing its rank, your highscore will be updated if it's below the minimum requirements for that rank.
  
* **Bulk Edits**
  * Hover your mose above the buttons for a more detailled description of what it does.
  * Most of the buttons can be Ctrl+clicked to apply a different value than the default one.
