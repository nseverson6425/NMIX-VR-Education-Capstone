This file is intended as a smooth transition for any future groups that seek to 
finish Class Reality.
Unity V2021.3.8f1
XR Interaction Toolkit used (compatable with most VR headsets)


1. BOW AND ARROW
    * The Bow and Arrow game was designed as a starting point for future groups.
     It is roughly half as complete as the Bomb Game, so using the Bomb Game as 
     a guide is recommended
    * Some places to start: 
        - Change the hand assets to match the Main Menu and Bomb Game (easy)
        - Decide if movement is required for the game overall. (The previous 
         2 developers had disagreements on this, so the bow game has no movement
         while the bomb game does) If so, implement into Bow, if not, change Bomb 
         Game/Main Menu accordingly
        - Use the Bomb Game's scoreboard, menu navigation, and finger ray selection 
         aspects to match the play style of the two games. (think of it like a HUD or 
         pause menu that is persistent between both games)
        - Create a valid scoring system, +5 for correct target, -1 for incorrect and 
         implement that system to update on the scoreboard
        - Have the bow and targets change questions/answers after a correct answer is
         hit (this goes with DATABASE CONNECTION below as you will need to fetch new Q&A's)
    * Watch these youtube link for a description of how the Bow works: 
    https://www.youtube.com/watch?v=4U98MEFug6k
    https://www.youtube.com/watch?v=H0xTz4JtWiI&t=354s


2. BOMB GAME
   *  The main purpose of this game is to facilate memorization through a gamified multiple choice
      question and answer format. 
   *  Answering questions correctly adds to a player's ongoing score. Answering incorrectly does not
      directly hurt their score. Instead, it prompts an 'obstacle' which serves as a minigame to break
      up the sameness of the game.
         -  As of now, only one minigame has been implemented, a dodging game. Upon answering a question
            wrong a series of moving walls will be spawned that charge in the direction of the player.
            If dodged successfully, the game continues after the specified time.
            if the player fails, they recieve a time penalty which extends the length of their play session.
   * SCORING
      -  My current idea is that the final score should be calculated based on the number of correctly
         answered questions and the amount of time it took to answer those questions. 
      -  For example if a player answers 5 questions in 10 seconds they would have a higher score than someone
         who answered them in 15 seconds.
         -  The time delay from wrong answers has the likely effect of hurting a player's score, but they're
            still able to redeem themselves for answering more questions correclty. 
   * OTHER IDEAS
      -  Minigame idea, Memorization puzzle: In the play area for the bomb game there are 9 stones on the ground,
         the idea was to use them for a classic memory game. A symbol would appear on the question board and then
         various symbols would flash on the ground. The player would need to stand on the correct symbol to
         proceed.
      -  Minigame idea, Swarm: Upon answering incorrectly the player would be swarmed by many flying objects.
         Using their cannon they would have to shoot them all down to proceed. 
   * DOCUMENTATION
      -  Gameobject names in the scene should be fairly intuitive.
      -  Scripts also contain a decent amount of comments and method descriptions.
      -  If help is needed, contact Oscar or ask ChatGPT!
  

3. DATABASE CONNECTION/WEBSITE
   *  CONCEPT
      -  The main idea for having a website/database is to faciliate the entry of data for use in game and to
         eventually provide a hub for instructor classroom management. 
      -  Since the app mostly focuses on flashcard replacement and memorization aid, the premise of our games
         is to use 'Decks'. These decks contain questions and their corresponding answers. Ideally, an instructor
         or student would be able to use the website to create a deck from scratch. The website would link to 
         the database and then the game would be able to pull the deck from the database. 
      -  Once in-game, students can select decks to practice or compete with. 
      -  In terms of classroom managment, instructors should be able to launch multiplayer game sessions with
         specific decks that students can join.
         -  The players don't necessarily need to share the same virtual space, the multiplayer session would
            be geared towards allowing instructors to track progress and give students a chance to see how they
            rank against classmates through an ongoing leaderboard. 
   *  TECHNICAL
      -  A propper database connection is a must if there are any intentions to offically launch
         this game on a VR store
      -  A sample SQL database as well as a php based website is provided under the Capstone_Website
         folder. This is a sample which is intended to be improved upon. THIS IS NOT A LIVE DATABASE/Website
         -  For testing purposes, I use xampp and phpmyadmin to test database and website connection before 
            actually purchacing a live server. I recommend doing the same until you feel your database is
            secure and connection to Unity is solid
         -  Upon importing the current database/making a fresh one, connect it to Unity via C# script.
            Google is helpful with this step.
         -  After the website and database are implemented, use them to cycle through flashcards to use 
            in game. For example, after an arrow hits a correct answer, the target colors are reset, 
            the database is queried for the next question/answers, a new question is assigned to the bow,
            a new answer is assigned to each target, and the scorebaord is updated.
      -  Under the user category in the database, a section should be added for high scores to be used in 
         the scoreboard.
      -  Another helpful tip: add a "Study set" category to the website/database so all questions/answers
         are held under a set number, that way when you query a set, you can just cycle through all items
         in the set number
      -  Lastly, make the website look good. It is bare bones right now, but again, if you intend for the 
         project to be published, some stylistic changes would be nice.


4. DECKS AND SETS
   *  In terms of this game, a set is a pairing of a question and its corresponding answer(s). A deck, holds
      multiple sets. 
   *  There are scripts and classes that handle these.


5. CLOSING REMARKS
    * I know this seems like a lot to accomplish, but once some things fall into place (mainly the database),
     the project will begin to be easier. 
    * Unity developement takes patience and practice, but I have no doubt that by the end of the semester 
     your group will have one of the greatest projects to ever come out of Capstone if you see it through.
    * If after reading all of this you decide this project is out of your wheelhouse, talk to your Capstone
     instructors. Capstone should be a fun but challenging project you work on for months, not a foreign 
     coding exprience that makes you want to cry.
    * I would highly recommend 2-3 developers to be added to this team with at least one specializing in 
     database setup/management, one specializing in Unity, and a third that could maybe help with a bit of 
     both


6. CONTACTS
    * Feel free to reach out to the previous 2 developers for any question and/or updates (group chat). Personally, 
     I (Nick) would love to see any updates and have no problem lending a helping hand if you feel stuck.
    * This project WILL be cooler than your peers' so expect jealousy when presenting. And remember,
     HAVE FUN with it. IF you finish up most of those above steps, change whatever you want, add whatever
     you want, make a 3rd game, etc. It's as much your project as it was ours!

    Developers
     Nick Severson (949) 677-9006
     Oscar Parada (404) 426-1338
