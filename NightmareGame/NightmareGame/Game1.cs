using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; //add songs
using System.Collections.Generic;
using System;
using System.IO;

//im trying

//Everyone has worked on this- Lauren w/ game states, player movement, screenwrap, etc
//Kevin with level editor and file IO
//Marc & Robley with their classes being implemented how they need
//Lucas with adding art aspects such as menu interfaces, position coordinates, initializations, etc

namespace NightmareGame
{
    //create enum to track game states
    public enum GameState
    {
        MainMenu, // main menu goes to game, credits, controls, and exit
        Game, // game goes to game over and pause
        GameOver, // gameover only goes to main menu
        Credits, //credits goes to main menu 
        Controls, //controls goes to main menu
        PauseGame, //pause game goes to game and main menu
        Win //The win screen. 
    }

    public class Game1 : Game
    {
        //added to just change the clone
        // base fields
        private Random rand = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Rectangle baseScreenSize;

        // content loaded fields
        private SpriteFont arial12;
        private Song mainMenuSong;
        private Song gameSong;
        private Song gameOverSong;
        private Song winSong;
        //private List<SoundEffect> soundEffects;

        //Menus and Screens
        private Texture2D mainMenu;
        private Texture2D pauseMenu;
        private Texture2D controls;
        private Texture2D credits;
        private Texture2D endScreen;
        private Texture2D winScreen;

        //Temp Background
        private Texture2D tempBackground;
        private Rectangle backgroundBox;
        private Texture2D tree;
        private Texture2D treeInsane;

        //Insanity Bar
        private InsanityBar insanity;
        private Texture2D insanityBar0;
        private Texture2D insanityBar1;
        private Texture2D insanityBar2;
        private Texture2D insanityBar3;
        private Texture2D insanityBar4;
        private Texture2D insanityBar5;
        private Texture2D[] insanityBarImages;

        //Character and Enemies
        private Texture2D[] bears;
        private Texture2D teddy0;
        private Texture2D teddy1;
        private Texture2D character;
        private Texture2D characterHitbox;
        private Texture2D[] characterImages;
        private Texture2D[] treeImages;
        private Texture2D masterSpriteSheet;

        private GameState gameState;
        private KeyboardState kbState;
        private KeyboardState prevKBState;
        private MouseState mState;
        private MouseState prevMState;

        private Player player;
        private EntityManager manager;

        // Initializes the required components for creating the map
        private StreamReader loadMap;

        private int mapHeight;
        private int mapWidth;
        private int tileDimensions = 100;

        private List<GameObject> treeTiles;
        private List<Enemy> enemyTiles;
        private List<BorderTree> boundaryTiles;

        private int playerColor = -5383962;
        private int treeColor = -7278960;
        private int enemyColor = -65536;
        private int objectiveColor = -16777077;
        private int boundaryColor = -16751616;
        private int rugColor = -16728065;
        private int objToPlace;

        Point origin;


        //Object named plate that will be used for the tea party objective
        private Texture2D[] texturesPlate;
        private ObjectiveObject plate;
        private Texture2D platePicture;

        //object name tea kettle that is used for the tea party objective
        private Texture2D[] texturesTeaKettle;
        private ObjectiveObject teaKettle;
        private Texture2D teaKetlePictures;

        //object name tea cups that is used for the teap party objective
        private Texture2D[] texturesCups;
        private ObjectiveObject cups;
        private Texture2D cupsPictures;

        //will be an invisible object that will allow, for some reason, work.
        private Texture2D[] textureInvisible;
        private ObjectiveObject invisibleObject;
        private Texture2D invisible;
        
        //objective tea party will be used for the objectives in the game.
        private TeaParty tea;

        //Lauren coded this on Robley's computer- fields for rug
        private Texture2D[] rugTextures;
        private ObjectiveObject rug;
        private Texture2D rugPicture;

        //New game states for music
        private GameState prevGameState;
        private GameState currentGameState;

        //will be fields that will hold the 
        //text and position of the objective text when starting the game
        private string objectiveText;
        private Vector2 objectiveTextVector;

        //will show the text for the objective in the pause menu
        private string pauseMenuObjectiveText;

        //will hold the time until the objective text on the screen disapears
        private int clock;

        //holds the texture2D and rectangle of the 
        //black background behind the text to make it pop
        private Texture2D blackBackground;
        private Rectangle blackBackgroundRectangle; 


        /// <summary>
        /// PROPERTIES ARE NEXT UP. MOST ARE USED FOR THE ENTITY MANAGER
        /// </summary>

        public Texture2D[] Bears
        {
            get { return bears; }
        }

        public Texture2D[] CharacterImages
        {
            get { return characterImages; }
        }

        public List<GameObject> TreeTiles
        {
            get { return treeTiles; }
        }

        public List<Enemy> EnemyTiles
        {
            get { return enemyTiles; }
        }

        public Player Player
        {
            get { return player; }
        }

        public List<BorderTree> BoundaryTiles
        {
            get { return boundaryTiles; }
        }

        public Texture2D HitboxImage
        {
            get { return characterHitbox; }
        }

        public InsanityBar Insanity
        {
            get { return insanity; }
        }

        public Point Origin
        {
            get { return origin; }
        }

        public GameObject Rug
        {
            get { return rug; }
        }

        /// <summary>
        /// FIELDS ARE NOW DONE
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //soundEffects = new List<SoundEffect>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            baseScreenSize = new Rectangle(-10, -12, 840, 630);
            //backgroundBox = new Rectangle(-1500, -1500, 10000, 10000);

            Point origin = new Point(0, 0);

            //instantiating the insanity bar array
            insanityBarImages = new Texture2D[6];


            this.IsMouseVisible = true;

            //start game at the menu
            gameState = GameState.MainMenu;

            //holds the text for the objective description and creates the start time
            //for the clock 
            clock = 0;
            objectiveText = "Collect all the objects " +
                            "\non the ground and place them together " +
                            "\non the rug three times to win the game.";

            pauseMenuObjectiveText = "Collect all the" +
                            "\nobjects on the " +
                            "\nground and place " +
                            "\nthem together " +
                            "\non the rug" +
                            "\nthree times to win " +
                            "\nthe game";


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            characterHitbox = Content.Load<Texture2D>("unnamed");

            //Menus and Screens
            mainMenu = Content.Load<Texture2D>("MainMenu");
            pauseMenu = Content.Load<Texture2D>("PauseMenu");
            credits = Content.Load<Texture2D>("CreditsScreen");
            controls = Content.Load<Texture2D>("ControlsScreen");
            endScreen = Content.Load<Texture2D>("EndScreen");
            winScreen = Content.Load<Texture2D>("YouWin");

            tempBackground = Content.Load<Texture2D>("BackgroundPic");

            //Temp Background
            //tempBackground = Content.Load<Texture2D>("BackgroundPic");
            tree = Content.Load<Texture2D>("Tree");
            treeInsane = Content.Load<Texture2D>("TreeInsane");
            treeImages = new Texture2D[2];
            treeImages[0] = treeInsane;
            treeImages[1] = characterHitbox;

            //Insanity Bar
            insanityBar0 = Content.Load<Texture2D>("NightmareBar0");
            insanityBar1 = Content.Load<Texture2D>("NightmareBar1");
            insanityBar2 = Content.Load<Texture2D>("NightmareBar2");
            insanityBar3 = Content.Load<Texture2D>("NightmareBar3");
            insanityBar4 = Content.Load<Texture2D>("NightmareBar4");
            insanityBar5 = Content.Load<Texture2D>("NightmareBar5");

            insanityBarImages[0] = insanityBar0;
            insanityBarImages[1] = insanityBar1;
            insanityBarImages[2] = insanityBar2;
            insanityBarImages[3] = insanityBar3;
            insanityBarImages[4] = insanityBar4;
            insanityBarImages[5] = insanityBar5;
            insanity = new InsanityBar(insanityBar0, insanityBarImages);

            //loading the objective object plate into the game
            platePicture = Content.Load<Texture2D>("Plate");
            texturesPlate = new Texture2D[1];
            texturesPlate[0] = platePicture;
            plate = new ObjectiveObject(300, 300, texturesPlate);

            //loading in the objective object tea kettle into the game
            teaKetlePictures = Content.Load<Texture2D>("TeaKettle");
            texturesTeaKettle = new Texture2D[1];
            texturesTeaKettle[0] = teaKetlePictures;
            teaKettle = new ObjectiveObject(1500, 1000, texturesTeaKettle);

            //loading in the objective object tea cup into the game
            cupsPictures = Content.Load<Texture2D>("TeaCup");
            texturesCups = new Texture2D[1];
            texturesCups[0] = cupsPictures;
            cups = new ObjectiveObject(1700, 400, texturesCups);

            invisible = Content.Load<Texture2D>("blank");
            textureInvisible = new Texture2D[1];
            textureInvisible[0] = invisible;

            //Character and Enemies
            teddy0 = Content.Load<Texture2D>("masterSpriteSheet");
            teddy1 = Content.Load<Texture2D>("TeddyDeath");
            character = Content.Load<Texture2D>("Character");
            masterSpriteSheet = Content.Load<Texture2D>("MasterSpriteSheet");

            characterImages = new Texture2D[12];
            characterImages[0] = character;
            characterImages[1] = characterHitbox;

            arial12 = Content.Load<SpriteFont>("arial12");

            //sets the black background to a blank picture
            blackBackground = Content.Load<Texture2D>("unnamed");


            //Array of bears
            bears = new Texture2D[3];
            bears[0] = masterSpriteSheet;
            bears[1] = teddy1;
            bears[2] = characterHitbox;

            //Songs
            mainMenuSong = Content.Load<Song>("MainMenuMusic");

            //start the game by playing main menu song
            MediaPlayer.Play(mainMenuSong);

            gameSong = Content.Load<Song>("maingame");
            gameOverSong = Content.Load<Song>("GameOverMusic");
            winSong = Content.Load<Song>("WinGame Music");

            rugPicture = Content.Load<Texture2D>("rug");
            rugTextures = new Texture2D[1];
            rugTextures[0] = rugPicture;

            // TODO: use this.Content to load your game content here
        }

        //hook up sound to event
        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume = .2f;
            //MediaPlayer.Play(gameSong);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //set fields to get the previous and past states
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            //get current and previous game state for each different song
            prevGameState = currentGameState;
            currentGameState = gameState;

            //if current game state doesnt equal last game state, switch songs
            if(currentGameState != prevGameState)
            {
                switch(gameState)
                {
                    //game state
                    case GameState.Game:

                        MediaPlayer.Play(gameSong);
                        MediaPlayer.IsRepeating = true;
                        break;

                    case GameState.GameOver:

                        
                        MediaPlayer.Play(gameOverSong);
                        MediaPlayer.IsRepeating = true;
                        
                        break;

                    case GameState.Win:

                        MediaPlayer.Play(winSong);
                        MediaPlayer.IsRepeating = true;
                        break;

                    case GameState.MainMenu:
                        MediaPlayer.Stop();
                        MediaPlayer.Play(mainMenuSong);
                        MediaPlayer.IsRepeating = true;
                        break;

                }
            }


            switch (gameState)
            {
                //main menu state
                case GameState.MainMenu:

                    //start game
                    if(mState.LeftButton == ButtonState.Pressed && 
                        mState.X > 50 && mState.X < 345 && mState.Y > 238 && mState.Y < 323 &&
                        mState != prevMState) 
                    {
                        LoadMap("..\\..\\..\\..\\..\\..\\gameMap.level");

                        //will set the clock count back to 0 every time 
                        //the main menu is hit.
                        clock = 0;
                    }

                    // Loads the user's local test map
                    if (Keyboard.GetState().IsKeyDown(Keys.F8))
                    {
                        LoadMap("testMap.level");
                    }

                    //open credits
                    if(mState.LeftButton == ButtonState.Pressed &&
                        mState.X > 470 && mState.X < 763 && mState.Y > 380 && mState.Y < 463 &&
                        mState != prevMState) 
                    {
                        gameState = GameState.Credits;
                    }

                    //open controls
                    if(mState.LeftButton == ButtonState.Pressed &&
                        mState.X > 463 && mState.X < 757 && mState.Y > 243 && mState.Y < 325 &&
                        mState != prevMState) 
                    {
                        gameState = GameState.Controls;
                    }

                    //exit game
                    if(mState.LeftButton == ButtonState.Pressed &&
                        mState.X > 54 && mState.X < 347 && mState.Y > 379 && mState.Y < 464 &&
                        mState != prevMState) 
                    {
                        Exit();
                    }
                    break;

                    //when click back, go back to main menu
                case GameState.Controls:

                    if(mState.LeftButton == ButtonState.Pressed &&
                        mState.X > 254 && mState.X < 548 && mState.Y > 406 && mState.Y < 476 &&
                        mState != prevMState) 
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;

                    //when click back, go back to main menu
                case GameState.Credits:

                    if (mState.LeftButton == ButtonState.Pressed &&
                        mState.X > 427 && mState.X < 721 && mState.Y > 342 && mState.Y < 411 &&
                        mState != prevMState) 
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;

                case GameState.Game:
                    
                    player.UpdateAnimation(gameTime);
                    player.DirectionCheck();

                    //will set the objective text to nothing if the clock has gone
                    //above 500 and whenever the game resets set the objective text back
                    //to what it was and will also make the black background disapear
                    if (clock > 500)
                    {
                        objectiveText = "";

                        blackBackgroundRectangle.Width = 0;
                        blackBackgroundRectangle.Height = 0;
                    }
                    else if (clock == 0)
                    {
                        objectiveText = "Collect all the objects " +
                            "\non the ground and place them together " +
                            "\nthree times to win the game";

                        blackBackgroundRectangle.Width = 300;
                        blackBackgroundRectangle.Height = 75;
                    }

                    //add one to the clock everytime a frame is hit
                    clock++;

                    // THE NEXT SEVERAL LINES DEAL WITH MOVEMENT
                    // tests for tree collision
                    bool hasCollided = false;
                    GameObject collidedObject = null;

                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        if (player.CheckCollision(treeTiles[i]) && !hasCollided)
                        {
                            collidedObject = treeTiles[i];
                            hasCollided = true;
                        }
                    }

                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        if (player.CheckCollision(boundaryTiles[i]) && !hasCollided)
                        {
                            collidedObject = boundaryTiles[i];
                            hasCollided = true;
                        }
                    }

                    if (hasCollided)
                    {
                        CollidedPlayerMovement(collidedObject);
                    }
                    else
                    {
                        BasicPlayerMovement();
                    }
                    // THIS IS THE END OF THE MOVEMENT

                    if(kbState.IsKeyDown(Keys.P))
                    {
                        gameState = GameState.PauseGame;
                    }

                    if (insanity.InsanityNum >= 100)
                    {
                        insanity.ResetInsanityBar();
                        gameState = GameState.GameOver;
                    }

                    //Will check if the player is holding an object.
                    //If not then if the player is intersecting an object they can press e to
                    //pick them up. IF the player is in fact holding something, they then cannot 
                    //pick up anything and instead they can press e to drop the object.
                    if (player.ObjectPickedUp == false)
                    {
                        if (player.Position.Intersects(teaKettle.Position))
                        {
                            player.InteractObject(teaKettle);
                        }
                        else if (player.Position.Intersects(plate.Position))
                        {
                            player.InteractObject(plate);
                        }
                        else if (player.Position.Intersects(cups.Position))
                        {
                            player.InteractObject(cups);
                        }
                        else if (player.Position.Intersects(invisibleObject.Position))
                        {
                            player.InteractObject(invisibleObject);
                        }
                    }
                    else if (player.ObjectPickedUp)
                    {
                        if (teaKettle.PickedUp)
                        {
                            player.InteractObject(teaKettle);
                        }
                        else if (plate.PickedUp)
                        {
                            player.InteractObject(plate);
                        }
                        else if (cups.PickedUp)
                        {
                            player.InteractObject(cups);
                        }
                    }

                    manager.Update(gameTime);

                    if (tea.Solution())
                    {
                        gameState = GameState.Win;
                    }

                    break;

                    
                case GameState.PauseGame:

                    //when you pause the game, go back to the game
                    if(mState.LeftButton == ButtonState.Pressed && mState.X > 129 && mState.X < 433 && mState.Y > 141 && mState.Y < 244) 
                    {
                        gameState = GameState.Game;
                    }

                    //when you pause the game, go back to the main menu
                    if(mState.LeftButton == ButtonState.Pressed && mState.X > 118 && mState.X < 437 && mState.Y > 287 && mState.Y < 388) 
                    {
                        gameState = GameState.MainMenu;
                    }

                    break;

                case GameState.GameOver:

                    //go back to the main menu after you die
                    if(mState.LeftButton == ButtonState.Pressed && mState.X > 228 && mState.X < 555 && mState.Y > 362 && mState.Y < 457) //&& coordinates of main menu button
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;

                    //win game state menu which lets you back to the menu game state
                case GameState.Win:
                    //go back to the main menu after you die
                    if (mState.LeftButton == ButtonState.Pressed && mState.X > 228 && mState.X < 555 && mState.Y > 362 && mState.Y < 457) //&& coordinates of main menu button
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;

                

            }

            prevKBState = kbState;
            prevMState = mState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //change background color later
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            spriteBatch.Begin();


            switch(gameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(mainMenu, baseScreenSize, Color.White);
                    break;

                case GameState.Controls:
                    spriteBatch.Draw(controls, new Rectangle(-10,-12,840,510), Color.White);
                    break;

                case GameState.Credits:
                    spriteBatch.Draw(credits, new Rectangle(-10, -12, 840, 510), Color.White);
                    break;

                case GameState.Game:
                    invisibleObject.Draw(spriteBatch);
                    spriteBatch.Draw(tempBackground, backgroundBox, Color.White);

                    rug.Draw(spriteBatch);
                    plate.Draw(spriteBatch);
                    cups.Draw(spriteBatch);
                    teaKettle.Draw(spriteBatch);

                    manager.Draw(spriteBatch);

                    insanity.Draw(spriteBatch);
                    spriteBatch.DrawString(arial12, "Insanity Bar", new Vector2(370, 15), Color.White);
                    spriteBatch.Draw(blackBackground, blackBackgroundRectangle, Color.Black);
                    spriteBatch.DrawString(arial12, objectiveText, objectiveTextVector, Color.White);

                    break;

                case GameState.PauseGame:
                    spriteBatch.Draw(pauseMenu, new Rectangle(-10, 0, 800, 510), Color.White);
                    spriteBatch.DrawString(arial12, pauseMenuObjectiveText, new Vector2(650, 100), Color.Black);
                    break;

                case GameState.GameOver:
                    spriteBatch.Draw(endScreen, new Rectangle(-18, -20, 850, 512), Color.White);
                    break;

                    //you win game state will show after beating the game 
                case GameState.Win:
                    spriteBatch.Draw(winScreen, new Rectangle(-18, -20, 850, 512), Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Makes the background move to simulate that the player is moving in a certain directon
        /// AUTHOR: Lucas
        /// </summary>
        public void BasicPlayerMovement()
        {
            // FOR MOVING TO THE LEFT 
            if (kbState.IsKeyDown(Keys.A))
            {
                // moves regularly with the player
                backgroundBox.X += player.Speed;
                for (int i = 0; i < treeTiles.Count; i++)
                {
                    treeTiles[i].X += player.Speed;
                    treeTiles[i].HitBoxX += player.Speed;
                }
                for (int i = 0; i < enemyTiles.Count; i++)
                {
                    enemyTiles[i].X += player.Speed;
                    enemyTiles[i].HitBoxX += player.Speed;
                }
                for (int i = 0; i < boundaryTiles.Count; i++)
                {
                    boundaryTiles[i].X += player.Speed;
                    boundaryTiles[i].HitBoxX += player.Speed;
                }

                //will move all the on screen objects and text
                //against where the player is moving so 
                //the objects stay in place at all times
                plate.HitBoxX += player.Speed;
                plate.X += player.Speed;

                cups.HitBoxX += player.Speed;
                cups.X += player.Speed;

                teaKettle.HitBoxX += player.Speed;
                teaKettle.X += player.Speed;

                objectiveTextVector.X += player.Speed;

                blackBackgroundRectangle.X += player.Speed;


                rug.HitBoxX += player.Speed;
                rug.X += player.Speed;

                // Moves the origin point
                origin.X += Player.Speed;
            }

            // FOR MOVING TO THE RIGHT
            if (kbState.IsKeyDown(Keys.D))
            {
                // makes everything move to the left
                backgroundBox.X -= player.Speed;
                for (int i = 0; i < treeTiles.Count; i++)
                {
                    treeTiles[i].X -= player.Speed;
                    treeTiles[i].HitBoxX -= player.Speed;
                }
                for (int i = 0; i < enemyTiles.Count; i++)
                {
                    enemyTiles[i].X -= player.Speed;
                    enemyTiles[i].HitBoxX -= player.Speed;
                }
                for (int i = 0; i < boundaryTiles.Count; i++)
                {
                    boundaryTiles[i].X -= player.Speed;
                    boundaryTiles[i].HitBoxX -= player.Speed;
                }

                //will move all the on screen objects and text
                //against where the player is moving so 
                //the objects stay in place at all times
                plate.HitBoxX -= player.Speed;
                plate.X -= player.Speed;

                cups.HitBoxX -= player.Speed;
                cups.X -= player.Speed;

                teaKettle.HitBoxX -= player.Speed;
                teaKettle.X -= player.Speed;

                objectiveTextVector.X -= player.Speed;

                blackBackgroundRectangle.X -= player.Speed;


                rug.HitBoxX -= player.Speed;
                rug.X -= player.Speed;

                // Moves the origin point
                origin.X -= Player.Speed;

                
            }

            // FOR MOVING "UP" THE SCREEN
            if (kbState.IsKeyDown(Keys.W))
            {
                // moves regularly with the player because there is no collisions
                backgroundBox.Y += player.Speed;
                for (int i = 0; i < treeTiles.Count; i++)
                {
                    treeTiles[i].Y += player.Speed;
                    treeTiles[i].HitBoxY += player.Speed;
                }
                for (int i = 0; i < enemyTiles.Count; i++)
                {
                    enemyTiles[i].Y += player.Speed;
                    enemyTiles[i].HitBoxY += player.Speed;
                }
                for (int i = 0; i < boundaryTiles.Count; i++)
                {
                    boundaryTiles[i].Y += player.Speed;
                    boundaryTiles[i].HitBoxY += player.Speed;
                }

                //will move all the on screen objects and text
                //against where the player is moving so 
                //the objects stay in place at all times
                plate.HitBoxY += player.Speed;
                plate.Y += player.Speed;

                cups.HitBoxY += player.Speed;
                cups.Y += player.Speed;

                teaKettle.HitBoxY += player.Speed;
                teaKettle.Y += player.Speed;

                objectiveTextVector.Y += player.Speed;

                blackBackgroundRectangle.Y += player.Speed;


                rug.HitBoxY += player.Speed;
                rug.Y += player.Speed;

                // Moves the origin point
                origin.Y += Player.Speed;

                
            }

            // FOR MOVING "DOWN" THE SCREEN
            if (kbState.IsKeyDown(Keys.S))
            {
                // the character moves regularly down, background and objects move up
                backgroundBox.Y -= player.Speed;
                for (int i = 0; i < treeTiles.Count; i++)
                {
                    treeTiles[i].Y -= player.Speed;
                    treeTiles[i].HitBoxY -= player.Speed;
                }
                for (int i = 0; i < enemyTiles.Count; i++)
                {
                    enemyTiles[i].Y -= player.Speed;
                    enemyTiles[i].HitBoxY -= player.Speed;
                }
                for (int i = 0; i < boundaryTiles.Count; i++)
                {
                    boundaryTiles[i].Y -= player.Speed;
                    boundaryTiles[i].HitBoxY -= player.Speed;
                }

                //will move all the on screen objects and text
                //against where the player is moving so 
                //the objects stay in place at all times
                plate.HitBoxY -= player.Speed;
                plate.Y -= player.Speed;

                cups.HitBoxY -= player.Speed;
                cups.Y -= player.Speed;

                teaKettle.HitBoxY -= player.Speed;
                teaKettle.Y -= player.Speed;

                objectiveTextVector.Y -= player.Speed;

                blackBackgroundRectangle.Y -= player.Speed;
                

                rug.HitBoxY -= player.Speed;
                rug.Y -= player.Speed;

                // Moves the origin point
                origin.Y -= Player.Speed;

            }
        }

        /// <summary>
        /// Uses the base movement method but this one is if the player collides with a tree.
        /// I have gone through so many pains to make this work. I am hoping it does now ~_~
        /// AUTHOR: Robley, BASE MOVEMENT: Lucas
        /// </summary>
        public void CollidedPlayerMovement(GameObject collidedObject)
        {
            // FOR MOVING LEFT INTO A TREE
            if (kbState.IsKeyDown(Keys.A))
            {
                if (player.HitBoxX > collidedObject.HitBoxX + (collidedObject.HitBox.Width / 2))
                {
                    // when player is touch the tree from the right, cant move left
                    backgroundBox.X -= 0;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].X -= 0;
                        treeTiles[i].HitBoxX -= 0;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].X -= 0;
                        enemyTiles[i].HitBoxX -= 0;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].X -= 0;
                        boundaryTiles[i].HitBoxX -= 0;
                    }

                }
                else
                {
                    // moves regularly with the player
                    backgroundBox.X += player.Speed;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].X += player.Speed;
                        treeTiles[i].HitBoxX += player.Speed;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].X += player.Speed;
                        enemyTiles[i].HitBoxX += player.Speed;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].X += player.Speed;
                        boundaryTiles[i].HitBoxX += player.Speed;
                    }

                    //will move all the on screen objects and text
                    //against where the player is moving so 
                    //the objects stay in place at all times
                    plate.HitBoxX += player.Speed;
                    plate.X += player.Speed;

                    cups.HitBoxX += player.Speed;
                    cups.X += player.Speed;

                    teaKettle.HitBoxX += player.Speed;
                    teaKettle.X += player.Speed;

                    objectiveTextVector.X += player.Speed;

                    blackBackgroundRectangle.X += player.Speed;


                    rug.HitBoxX += player.Speed;
                    rug.X += player.Speed;

                    // Moves the origin point
                    origin.X += Player.Speed;
                }
            }

            // FOR MOVING RIGHT INTO A TREE
            if (kbState.IsKeyDown(Keys.D))
            {
                // when player is touching tree from the left, cant move right
                if (player.HitBoxX < collidedObject.HitBoxX + (collidedObject.HitBox.Width / 2))
                {
                    backgroundBox.X -= 0;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].X -= 0;
                        treeTiles[i].HitBoxX -= 0;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].X -= 0;
                        enemyTiles[i].HitBoxX -= 0;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].X -= 0;
                        boundaryTiles[i].HitBoxX -= 0;
                    }

                }
                else
                {
                    // makes everything move to the left
                    backgroundBox.X -= player.Speed;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].X -= player.Speed;
                        treeTiles[i].HitBoxX -= player.Speed;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].X -= player.Speed;
                        enemyTiles[i].HitBoxX -= player.Speed;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].X -= player.Speed;
                        boundaryTiles[i].HitBoxX -= player.Speed;
                    }

                    //will move all the on screen objects and text
                    //against where the player is moving so 
                    //the objects stay in place at all times
                    plate.HitBoxX -= player.Speed;
                    plate.X -= player.Speed;

                    cups.HitBoxX -= player.Speed;
                    cups.X -= player.Speed;

                    teaKettle.HitBoxX -= player.Speed;
                    teaKettle.X -= player.Speed;

                    objectiveTextVector.X -= player.Speed;

                    blackBackgroundRectangle.X -= player.Speed;


                    rug.HitBoxX -= player.Speed;
                    rug.X -= player.Speed;

                    // Moves the origin point
                    origin.X -= Player.Speed;
                }
                
            }

            // FOR MOVING "UP" INTO A TREE
            if (kbState.IsKeyDown(Keys.W))
            {
                if (player.HitBoxY > collidedObject.HitBoxY + (collidedObject.HitBox.Height / 2))
                {
                    backgroundBox.Y += 0;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].Y += 0;
                        treeTiles[i].HitBoxY += 0;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].Y += 0;
                        enemyTiles[i].HitBoxY += 0;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].Y += 0;
                        boundaryTiles[i].HitBoxY += 0;
                    }
                }
                else
                {
                    // moves regularly with the player because there is no collisions
                    backgroundBox.Y += player.Speed;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].Y += player.Speed;
                        treeTiles[i].HitBoxY += player.Speed;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].Y += player.Speed;
                        enemyTiles[i].HitBoxY += player.Speed;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].Y += player.Speed;
                        boundaryTiles[i].HitBoxY += player.Speed;
                    }

                    //will move all the on screen objects and text
                    //against where the player is moving so 
                    //the objects stay in place at all times
                    plate.HitBoxY += player.Speed;
                    plate.Y += player.Speed;

                    cups.HitBoxY += player.Speed;
                    cups.Y += player.Speed;

                    teaKettle.HitBoxY += player.Speed;
                    teaKettle.Y += player.Speed;

                    objectiveTextVector.Y += player.Speed;

                    blackBackgroundRectangle.Y += player.Speed;


                    rug.HitBoxY += player.Speed;
                    rug.Y += player.Speed;

                    // Moves the origin point
                    origin.Y += Player.Speed;
                }
            }

            // FOR MOVING "DOWN" INTO A TREE
            if (kbState.IsKeyDown(Keys.S))
            {
                if (player.HitBoxY < collidedObject.HitBoxY + (collidedObject.HitBox.Height / 2))
                {
                    backgroundBox.Y += 0;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].Y += 0;
                        treeTiles[i].HitBoxY += 0;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].Y += 0;
                        enemyTiles[i].HitBoxY += 0;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].Y += 0;
                        boundaryTiles[i].HitBoxY += 0;
                    }
                }
                else
                {
                    // the character moves regularly down, background and objects move up
                    backgroundBox.Y -= player.Speed;
                    for (int i = 0; i < treeTiles.Count; i++)
                    {
                        treeTiles[i].Y -= player.Speed;
                        treeTiles[i].HitBoxY -= player.Speed;
                    }
                    for (int i = 0; i < enemyTiles.Count; i++)
                    {
                        enemyTiles[i].Y -= player.Speed;
                        enemyTiles[i].HitBoxY -= player.Speed;
                    }
                    for (int i = 0; i < boundaryTiles.Count; i++)
                    {
                        boundaryTiles[i].Y -= player.Speed;
                        boundaryTiles[i].HitBoxY -= player.Speed;
                    }

                    //will move all the on screen objects and text
                    //against where the player is moving so 
                    //the objects stay in place at all times
                    plate.HitBoxY -= player.Speed;
                    plate.Y -= player.Speed;

                    cups.HitBoxY -= player.Speed;
                    cups.Y -= player.Speed;

                    teaKettle.HitBoxY -= player.Speed;
                    teaKettle.Y -= player.Speed;

                    objectiveTextVector.Y -= player.Speed;

                    blackBackgroundRectangle.Y -= player.Speed;


                    rug.HitBoxY -= player.Speed;
                    rug.Y -= player.Speed;

                    // Moves the origin point
                    origin.Y -= Player.Speed;
                }
                
            }
        }

        /// <summary>
        /// Load Map Method - Kevin
        ///     Accepts a string to be turned into a file path. The method then reads
        ///     the corresponding file and builds a map from it.
        /// </summary>
        /// <param name="file"></param>
        public void LoadMap(string file)
        {
            insanity.ResetInsanityBar();

            // Reads information from the map file
            loadMap = new StreamReader(file);

            // Skips the first line. It is only needed for the level editor
            loadMap.ReadLine();

            // Reads the map height and width
            mapHeight = int.Parse(loadMap.ReadLine());
            mapWidth = int.Parse(loadMap.ReadLine());

            // Creates a list of trees and enemies to help with drawing
            treeTiles = new List<GameObject>();
            enemyTiles = new List<Enemy>();
            boundaryTiles = new List<BorderTree>();

            // Creates a list of points and a random object for spawing
            List<Point> initialObjSpawn = new List<Point>();
            List<Point> objSpawn;
            Random rand = new Random();
            int spawnIndex;

            // Resets the origin
            origin = new Point(0, 0);

            //rug = new ObjectiveObject(1700, 500, rugTextures);
            // Creating Game Objects from the file
            //Loops through each line in the file
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    // Parses the line into an int
                    objToPlace = int.Parse(loadMap.ReadLine());

                    if (objToPlace == playerColor)
                    {
                        // Creates a new player object
                        player = new Player(masterSpriteSheet, 400,
                            240, characterImages, invisible);
                        backgroundBox = new Rectangle(j * tileDimensions * -1, i * tileDimensions * -1, 6000, 6000);
                    }

                    if (objToPlace == treeColor)
                    {
                        // Creates a new tree MapObject and adds it to the
                        //      list of trees
                        MapObject t = new MapObject(treeImages,
                            j * tileDimensions, i * tileDimensions);
                        treeTiles.Add(t);
                    }

                    if (objToPlace == enemyColor)
                    {
                        // Creates a new Enemy and adds it to the list of enemies
                        Enemy e = new Enemy(bears, j * tileDimensions,
                            i * tileDimensions, player, this);
                        enemyTiles.Add(e);
                    }

                    if (objToPlace == boundaryColor)
                    {
                        // Creates a new BoarderTree object and adds it to the list
                        //      of boarder trees
                        BorderTree b = new BorderTree(treeImages, j * tileDimensions,
                            i * tileDimensions);
                        boundaryTiles.Add(b);
                    }

                    if (objToPlace == objectiveColor)
                    {
                        // Creates a new spawn location for objectives and adds it to the list
                        //      of spawn points
                        Point p = new Point(j * tileDimensions, i * tileDimensions);
                        initialObjSpawn.Add(p);
                    }

                    if (objToPlace == rugColor)
                    {
                        // Creates a new rug
                        rug = new ObjectiveObject(j * tileDimensions, i * tileDimensions, rugTextures);
                        rug.Position = new Rectangle(j * tileDimensions, i * tileDimensions, 172, 82);
                    }
                }
            }

            // Copys the initial list to pass to the tea party
            objSpawn = initialObjSpawn;

            // Spawns the tea kettle at a random point from the list and removes the point from the list
            spawnIndex = rand.Next(initialObjSpawn.Count);
            teaKettle = new ObjectiveObject(initialObjSpawn[spawnIndex].X, initialObjSpawn[spawnIndex].Y, texturesTeaKettle);
            initialObjSpawn.RemoveAt(spawnIndex);
            
            // Spawns the plate at a random point from the list and removes the point from the list
            spawnIndex = rand.Next(initialObjSpawn.Count);
            plate = new ObjectiveObject(initialObjSpawn[spawnIndex].X, initialObjSpawn[spawnIndex].Y, texturesPlate);
            initialObjSpawn.RemoveAt(spawnIndex);

            // Spawns the cup at a random point from the list and removes the point from the list
            spawnIndex = rand.Next(initialObjSpawn.Count);
            cups = new ObjectiveObject(initialObjSpawn[spawnIndex].X, initialObjSpawn[spawnIndex].Y, texturesCups);
            initialObjSpawn.RemoveAt(spawnIndex);
            
            // Creates a TeaParty object and passes in the objectives, spawn location list, and the game
            tea = new TeaParty(teaKettle, cups, plate, initialObjSpawn, this, rug, insanity);

            // Closes the StreamReader
            loadMap.Close();

            // Creates a new entity manager
            manager = new EntityManager(this, insanity);

            //sets the invisible object to the same position of the player
            invisibleObject = new ObjectiveObject(player.HitBoxX, player.HitBoxY, textureInvisible);

            //sets the objective text's vector equal to the players position
            objectiveTextVector = new Vector2(player.Position.X, player.Position.Y - 80);

            //sets the rectangle of the black background of the objective text to the position of the player
            blackBackgroundRectangle = new Rectangle(player.Position.X - 10, player.Position.Y - 90, 300, 75);

            gameState = GameState.Game;
            
        }
    }
}
