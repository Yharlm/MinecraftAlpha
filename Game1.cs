using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace MinecraftAlpha;



public class Game1 : Game
{

    //Game Rules
    public bool KeepInventory = false;
    public float Daytime = 0f;
    public bool DebugMode = false;
    public bool GameStarted = false;
    public float TimeSinceStart = 0f;
    public bool creativeMode = true;
    public bool InventoryOpen = false;

    //Program Settings
    public int windowWidth;
    public int windowHeight;
    private GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;



    public Items items = new();
    public Texture2D BreakTexture;
    public SpriteFont font;
    public Effect Shader;
    public Player Player; //Local player;
    public UserInterfaceManager _userInterfaceManager; //UI and GUI
    public EntityManager _entityManager = new(); //entities
    public BlockManager _blockManager; //Blocks and tiles
    public ActionManager _actionManager = new(); //Special actions in gameplay
    public EntityAnimationService _entityAnimationService = new(); //Animations
    public ParticleSystem _particleSystem = new(); //Particles / seperate form entity class.
    public RecipeManager _RecipeManager = new(); // Loads the difrent interactions in UI using items.
    public InputManager _inputManager = new(); //Reading and storing User Input.
    public CommandMannger _CommandManager = new(); //Debug and testing features
    public Generation generation = new Generation(0); //Procedural gen and structures.

    public List<Chunk> Chunks = new() // Preset 3 chunks
    {
        new Chunk(1,2),
        new Chunk(0,2),
        new Chunk(-1,2),
    };
    //Gameplay variables

    public List<Chunk> Loaded;

    public Vector2 WorldMousePos = Vector2.Zero;
    public Vector2 MousePosition = Vector2.Zero;
    public float BlockSize = 16f * 1;


    public Game1()
    {




        _userInterfaceManager = new UserInterfaceManager(this);
        _blockManager = new BlockManager(this);
        _entityManager.game = this;
        _actionManager.Game = this;
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _CommandManager.game = this;

        // In Game1 constructor, after 'graphics = new GraphicsDeviceManager(this);'
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; // Your desired width
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; // Your desired height
        _graphics.ApplyChanges(); // Crucial step to apply settings






        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    protected override void Initialize()
    {
        windowWidth = GraphicsDevice.Viewport.Width;
        windowHeight = GraphicsDevice.Viewport.Height;

        Player = new Player() { game = this };
        Random random = new Random();
        _particleSystem.Content = Content;





        foreach (Chunk c in Chunks)
        {
            for (int z = 0; z < c.Tiles.GetLength(0); z++)
            {
                for (int i = 0; i < c.Tiles.GetLength(1); i++)
                {
                    for (int j = 0; j < c.Tiles.GetLength(2); j++)
                    {
                        if (i == 0)
                        {

                            c.Tiles[z, i, j] = new TileGrid()
                            { ID = 2 };
                            continue;
                        }
                        c.Tiles[z, i, j] = new TileGrid()
                        { ID = 1 };
                    }
                }
            }
        }










        // TODO: Add your initialization logic here





        {




            //var Map = Generation.GenerateWhiteNoise(250, 5, 0, 0);
            //Map = Generation.GeneratePerlinNoise(Map, 6, 1f);
            //Generation.SumMaps(Map, Generation.GenerateFlat(250, 5, 0.2f), 0.7f);
            //Map = Generation.GenerateSmoothNoise(Map, 4);
            ////Map = Generation.GenerateFlat(250, 5, 0.2f);
            //int Height = 17;
            //for (int i = 0; i < Map.GetLength(1); i++)
            //{
            //    float Y = (Map[0, i]) * Height + 50;
            //    World[(int)Y, i].ID = 2;
            //    World[(int)Y + 1, i].ID = 1;
            //    World[(int)Y + 2, i].ID = 1;
            //    //place blocks Downwards from here
            //}



            // World generation



            /*
            int t = 100;
            int x = 0;
            while (x < World.GetLength(1))
            {



                int y = random.Next(-1, 2);
                t += y;
                int length = random.Next(1, 7);
                while (length > 0 && x < World.GetLength(1))
                {
                    int DirtLayer = random.Next(3, 5);
                    int StoneLayer = 30;


                    while (StoneLayer > 0)
                    {
                        World[t + StoneLayer, x].ID = 3; // Dirt
                        StoneLayer--;
                    }

                    World[t, x].ID = 2; // Dirt
                    while (DirtLayer > 0)
                    {
                        World[t + DirtLayer, x].ID = 1; // Dirt
                        DirtLayer--;
                    }
                    x++;
                    length--;
                }

            }


            // BackGround generation
            t = 95;
            x = 0;
            while (x < BackGround.GetLength(1))
            {



                int y = random.Next(-1, 2);
                t += y;
                int length = random.Next(1, 9);
                while (length > 0 && x < BackGround.GetLength(1))
                {
                    int DirtLayer = random.Next(3, 5);
                    int StoneLayer = 30;


                    while (StoneLayer > 0)
                    {
                        BackGround[t + StoneLayer, x].ID = 3; // Dirt
                        StoneLayer--;
                    }

                    BackGround[t, x].ID = 2; // Dirt
                    while (DirtLayer > 0)
                    {
                        BackGround[t + DirtLayer, x].ID = 1; // Dirt
                        DirtLayer--;
                    }
                    x++;
                    length--;
                }

            }

            */
        }
        base.Initialize();

        Lighting(); //Start lighting checks

    }




    protected override void LoadContent()
    {
        //BreakTexture = Content.Load<Texture2D>("break_animation");





        // Items








        font = Content.Load<SpriteFont>("Font");
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var block in _blockManager.Blocks)
        {
            if (block.TexturePath == "_item") continue;
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }

        Shader = Content.Load<Effect>("Shaders/BlockEffect");


        //_userInterfaceManager.ItemSlots = UserInterfaceManager.LoadItemSlots(_blockManager.Blocks);
        _entityManager.entities = EntityManager.LoadEntites(this);
        _entityManager.LoadSprites(Content);
        _entityManager.LoadJoints();




        _userInterfaceManager.LoadTextures(Content);

        //_entityAnimationService.LoadAnimations(_entityManager.entities);
        BreakTexture = Content.Load<Texture2D>("UIelements/destroy_stage_0-Sheet");


        _RecipeManager.Recipes = _RecipeManager.LoadRecipes(_blockManager);
        //Making player
        //Player.Respawn();

        Player.Plr = Entity.CloneEntity(_entityManager.entities[0], new Vector2(0, 0));



        _entityManager.Workspace.Add(Player.Plr);






        //EFrame.Window = Content.Load<Texture2D>("UIelements/WindowFrame");

        _particleSystem.Load();
        _blockManager.LoadActions();

        items.Atlas = Content.Load<Texture2D>("Items");
        //var Grass = Content.Load<Texture2D>("dirt");
        //var wood = Content.Load<Texture2D>("oak_planks");

        //test = new Sprite3D(wood, Grass, wood, wood);


        // TODO: use this.Content to load your game content here

        _userInterfaceManager.LoadGUI();


        //for (int i = 1; i < _blockManager.Blocks.Count; i++)
        //{
        //    _userInterfaceManager.windows[0].ItemSlots[i].Item = _blockManager.Blocks[i];
        //    _userInterfaceManager.windows[0].ItemSlots[i].Count = 64;
        //}

    }


    public void LightingOnChange()
    {
        var Changes = _actionManager.EventQue;

        foreach (var change in Changes)
        {
            var tile = change.tile;
            if (tile == null) continue;
            



        }
        
    }
    public void Lighting()
    {

        List<Vector4> Lights = new List<Vector4>();

        //Thread Day = new Thread(() =>
        //{

        //    while (true)
        //    {

        //        if (Loaded == null) continue;
        //        if (!GameStarted && Loaded.Count > 0) continue;
        //        int PlrPos = (int)float.Ceiling(Player.Plr.position.X / 32);
        //        Dictionary<int, Chunk> tallestByX = new Dictionary<int, Chunk>();
        //        bool playerInChunk = false;
        //        for (int c = 0; c < Chunks.Count; c++)
        //        {
        //            var L = Chunks[c];
        //            if (L == null) continue;
        //            if (!tallestByX.TryGetValue(L.x, out var current))
        //            {
        //                tallestByX[L.x] = L;
        //                if (PlrPos == L.x) playerInChunk = true;
        //            }
        //            else if (L.y < current.y)
        //            {
        //                tallestByX[L.x] = L;
        //            }
        //        }


        //        if (playerInChunk)
        //        {
        //            var tallest = tallestByX[PlrPos];

        //            for (int x = 0; x < tallest.Tiles.GetLength(2); x++)
        //            {
        //                for (int z = 0; z < tallest.Tiles.GetLength(0); z++)
        //                {

        //                    for (int y = 0; y < tallest.Tiles.GetLength(1); y++)
        //                    {
        //                        var Tile = tallest.Tiles[z, y, x];
        //                        if (Tile.ID != 0)
        //                        {
        //                            Lights.Add(new Vector4(Tile.pos.X, Tile.pos.Y, Tile.pos.Z, 2)); break;
        //                        }




        //                    }

        //                }
        //            }
        //        }


        //    }
        //}); Day.IsBackground = true;

    Thread Light = new Thread(() =>
    {
        //while (true)
        //{

        //    if (Loaded == null) continue;
        //    if (!GameStarted && Loaded.Count > 0) continue;
        //    int PlrPos = (int)float.Ceiling(Player.Plr.position.X / 32);
        //    Dictionary<int, Chunk> tallestByX = new Dictionary<int, Chunk>();
        //    bool playerInChunk = false;
        //    for (int c = 0; c < Chunks.Count; c++)
        //    {
        //        var L = Chunks[c];
        //        if (L == null) continue;
        //        if (!tallestByX.TryGetValue(L.x, out var current))
        //        {
        //            tallestByX[L.x] = L;
        //            if (PlrPos == L.x) playerInChunk = true;
        //        }
        //        else if (L.y < current.y)
        //        {
        //            tallestByX[L.x] = L;
        //        }
        //    }


        //    if (playerInChunk)
        //    {
        //        var tallest = tallestByX[PlrPos];

        //        for (int x = 0; x < tallest.Tiles.GetLength(2); x++)
        //        {
        //            for (int z = 0; z < tallest.Tiles.GetLength(0); z++)
        //            {

        //                for (int y = 0; y < tallest.Tiles.GetLength(1); y++)
        //                {
        //                    var Tile = tallest.Tiles[z, y, x];
        //                    if (Tile.ID != 0)
        //                    {
        //                        Lights.Add(new Vector4(Tile.pos.X, Tile.pos.Y, Tile.pos.Z, 2)); break;
        //                    }




        //                }

        //            }
        //        }
        //    }












        //    for (int c = 0; c < Chunks.Count; c++)
        //    {
        //        var L = Chunks[c];




        //        if (L != null)
        //        {


        //            for (int z = 0; z < L.Tiles.GetLength(0); z++)
        //            {

        //                for (int j = 0; j < L.Tiles.GetLength(2); j++)
        //                {

        //                    for (int i = 0; i < L.Tiles.GetLength(1); i++)
        //                    {

        //                        //float B = 0;
        //                        var tile = L.Tiles[z, i, j];

        //                        if (tile.ID == 0) continue;
        //                        var block = _blockManager.GetBlockAtTile(tile);
        //                        if (block == null) continue;
        //                        if (block.Light_Emission > 0)
        //                        {
        //                            Lights.Add(new Vector4(tile.pos.X, tile.pos.Y, tile.pos.Z, block.Light_Emission));
        //                        }


        //                    }

        //                }
        //            }
        //        }
        //    }
        //    for (int c = 0; c < Chunks.Count; c++)
        //    {
        //        var L = Chunks[c];
        //        if (L != null)
        //        {

        //            for (int z = 0; z < L.Tiles.GetLength(0); z++)
        //            {
        //                for (int i = 0; i < L.Tiles.GetLength(1); i++)
        //                {
        //                    for (int j = 0; j < L.Tiles.GetLength(2); j++)
        //                    {
        //                        var tile = L.Tiles[z, i, j];
        //                        float B = 0;

        //                        for (int e = 0; e < Lights.Count; e++)
        //                        {
        //                            var light = Lights[c];
        //                            float emision = float.Ceiling(light.W);
        //                            float dist = Vector3.Distance(new Vector3(light.X, light.Y, light.Z), tile.pos);
        //                            if (dist < emision)
        //                                B += 1 - dist / emision;
        //                        }
        //                        tile.brightness = B;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    Lights.Clear();


        //};


        











    }); Light.IsBackground = true;
        Light.Start();
        
    }

    public bool Clicked = false;

    protected override void Update(GameTime gameTime)
    {

        TimeSinceStart += 0.3f;

        MouseClick = 0;
        if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Released && Clicked)
        {

            Clicked = false;
        }
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            MouseClick = -1;
        }
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !Clicked)
        {
            MouseClick = 1;
            Clicked = true;
        }

        if (Mouse.GetState().RightButton == ButtonState.Pressed)
        {
            MouseClick = -2;
        }
        if (Mouse.GetState().RightButton == ButtonState.Pressed && !Clicked)
        {
            MouseClick = 2;
            Clicked = true;
        }






        MousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
        _userInterfaceManager.MouseAction(MousePosition, _actionManager, MouseClick);




        if (!GameStarted)
        {
            return;
        }
        _userInterfaceManager.windows[3].Visible = false;

        if (Player.Plr.Health <= 0)
        {



            Player.Respawn();
        }

        //if (Keyboard.GetState().IsKeyDown(Keys.F11))
        //{

        //    _graphics.ToggleFullScreen();
        //}

        foreach (var Window in _userInterfaceManager.windows)
        {
            Window.Update(this);
        }


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();




        float Time = 1f;
        if (Daytime <= 12)
        {
            Daytime += Time;

        }

        if (Daytime > 12)
        {
            Daytime -= Time;

        }


        // TODO: Add your update logic here
        Input(gameTime);



        Player.cam.position = -Player.Plr.position * BlockSize + new Vector2(windowWidth, windowHeight) / 2;
        base.Update(gameTime);


        WorldMousePos = (MousePosition - Player.cam.position) / BlockSize;

        //
        _blockManager.BlockSize = BlockSize;
        foreach (var Animation in _entityAnimationService.entityAnimations)
        {
            var Entity = Animation.parent;

            var anim = Entity.Animations[Animation.id];
            anim.parent = Entity;

            if (Entity.name == "Pig")
            {

            }

            anim.Update();
            //Entity.Joints[1].orientation += 10;



        }

        int Tick = (int)(TimeSinceStart);


        //TimeSinceStart = 0;
        int Render_Distance = 3;

        Loaded = new List<Chunk>();

        for (int i = -Render_Distance; i < Render_Distance; i++)
        {
            var pos = BlockManager.GetChunkAtPos(new Vector2(Player.Plr.position.X + i * 16, Player.Plr.position.Y));
            var chunk = BlockManager.GetChunk(pos[0], pos[1], Chunks);
            if (chunk == null) continue;
            Loaded.Add(chunk);
        }





        foreach (var L in Loaded)
        {

            for (int z = 0; z < L.Tiles.GetLength(0); z++)
            {
                for (int i = 0; i < L.Tiles.GetLength(1); i++)
                {
                    for (int j = 0; j < L.Tiles.GetLength(2); j++)
                    {
                        //float B = 0;
                        var tile = L.Tiles[z, i, j];

                        //foreach (var light in Lights)
                        //{

                        //    float dist = Vector3.Distance(light, tile.pos);
                        //    if (dist < 15)
                        //        B = 1-dist / 15;
                        //}
                        //tile.brightness = B;

                        var block = _blockManager.GetBlockAtTile(tile);

                        //if (!tile.updateLight)
                        //{

                        //    if (tile.brightness > 0)
                        //        tile.brightness -= 0.01f;
                        //}




                        if (block == null) continue;



                        if (block.IgnoreUpdate) continue;




                        //int TickUP = block.TickUpdate;
                        if (Tick % block.TickUpdate == 0)
                        {
                            if (block.Update == null) continue;
                            if ((tile.ID == 0 /*&& tile.brightness > 1*/) || tile.MarkedForUpdate) continue;
                            block.Update.Invoke(tile, tile.Data);

                        }
                        else
                        {
                            //tile.updateLight = false;
                            tile.MarkedForUpdate = false;
                        }


                    }
                }
            }
            ;

            //for (int z = 0; z < L.Tiles.GetLength(0); z++)
            //{
            //    for (int i = 0; i < L.Tiles.GetLength(1); i++)
            //    {
            //        for (int j = 0; j < L.Tiles.GetLength(2); j++)
            //        {

            //            var tile = L.Tiles[z, i, j];

            //            if (tile.ID == 0) continue;
            //            if (!tile.MarkedForUpdate) continue;
            //            tile.MarkedForUpdate = false;

            //        }
            //    }
            //}






        }


        _entityAnimationService.entityAnimations.RemoveAll(x => x.parent.Animations[x.id].Paused);

        var ItemList = new List<Entity>();
        foreach (var entity in _entityManager.Workspace)
        {
            if (entity.Health <= 0) continue;
            entity.Update.Invoke(entity);
            //entity.collisionBox.CheckCollision(entity,World);
            entity.Lifetime += 0.1f;
            if (entity.position.Y > 600)
            {
                entity.TakeDamage(null, 5, 0);
            }


            entity.Grounded = true;
            if (entity.velocity.velocity.Y > 12f)
            {
                entity.Fall_damage = (int)(entity.velocity.velocity.Y) - 7;
            }

            if (entity.collisionBox.Bottom && !entity.velocity.flying)
            {
                entity.TakeDamage(null, entity.Fall_damage, 0);
                entity.Fall_damage = 0;
                entity.Grounded = false;
            }
            foreach (Entity entity1 in _entityManager.Workspace)
            {
                if (entity1.Health <= 0) continue;
                if (entity == entity1) continue;
                if (entity1.CheckCollisionEntity(entity))
                {
                    Entity.CollisionEventCollision(entity1, entity, this);
                }

            }

            //Raycast for projectiles
            if (entity.ID < 0 && entity.velocity.velocity.Length() > 4)
            {
                Vector2 dist = entity.position - entity.previousPosition;
                var Hit = LogicsClass.RaycastDir(entity.position, dist, this, new List<Entity>() { entity });
                if (Hit != null)
                {
                    if (Hit.GetType() == typeof(Entity))
                    {
                        Entity.CollisionEventCollision(Hit, entity, this);
                    }
                }


                entity.previousPosition = entity.position;
            }




            entity.velocity.apply_velocity(entity); // Apply gravity or any other force
            entity.collisionBox.UpdateCollision(entity, Chunks, this);
            _entityManager.AI(entity);

            var EntVal = entity.velocity.velocity;
            entity.ResetIframes();
            if (entity.ID == -1)
            {
                entity.Model3D.Update();
                continue;
            }

            //entity.UpdateAnimation();

            bool Running = false;
            if (float.Abs(entity.velocity.velocity.X) > 0)
            {

                Running = true;
            }

            if (Running)
                _entityAnimationService.Play(1, entity);
            else
            {
                _entityAnimationService.Stop(1, entity);
            }







            if (entity.Health <= 0)
            {
                _entityManager.Die(entity, ItemList);
                //break;
            }


            // Example gravity, can be replaced with actual logic

        }
        _entityManager.Workspace.RemoveAll(x => x.Health <= 0);

        foreach (var entity in _particleSystem.Particles)
        {
            entity.Update();
        }
        _particleSystem.Particles.RemoveAll(x => x.timeElapsed >= x.lifeTime);
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
        }

        Player.Update();
        _inputManager.UpdateKeyHistory(Keyboard.GetState());

    }

    int HotbarIndex;



    public int MouseClick = 0;
    public void Input(GameTime time)
    {

        _CommandManager.Read();
        if (_CommandManager.active) { return; }


        bool jump = false;
        var PLR = Player.Plr;

        HotbarIndex = (Mouse.GetState().ScrollWheelValue / 120) % 9 + 1;


        Vector2 plrVel = Vector2.Zero;

        float zoomScale = 0.3f;
        var keyboardState = Keyboard.GetState();


        bool Front = false;
        bool Back = false;
        var keyboard = Keyboard.GetState().GetPressedKeys();
        foreach (var key in keyboard)
        {




            if (key == Keys.S)
            {
                Front = true;
                var tile = BlockManager.GetBlockAtPos(Player.Plr.position, (int)Player.Plr.Layer + 1, Chunks);
                if (tile != null)
                {
                    if (tile.ID == 0)
                    {
                        Player.Plr.Layer += 0.05f;
                    }

                }
            }
            if (key == Keys.W)
            {
                Back = true;
                var tile = BlockManager.GetBlockAtPos(Player.Plr.position, (int)Player.Plr.Layer - 1, Chunks);
                if (tile != null)
                {
                    if (tile.ID == 0)
                    {
                        Player.Plr.Layer -= 0.05f;
                    }

                }
            }
            if (key == Keys.LeftShift)
            {

                PLR.velocity.velocity += new Vector2(0, +2);
            }
            if (key == Keys.A)
            {


                PLR.Fliped = true;
                plrVel = new Vector2(-1, 0);
            }
            if (key == Keys.D)
            {
                //PLR.Animations[1].Paused = false;
                PLR.Fliped = false;
                plrVel = new Vector2(+1, 0);
            }

            if (key == Keys.Space)
            {
                //PLR.Jumping = true;
                if (PLR.velocity.flying)
                {
                    PLR.velocity.velocity += new Vector2(0, -2);
                }
                jump = true;
            }

            if (key == Keys.X)
            {

                TileGrid tile = BlockManager.GetLastBlockAtPos(WorldMousePos, Chunks);

                //_entityManager.GravityBlock(WorldMousePos, 8, true);
                _entityManager.GravityBlock(BlockManager.GetBlockAtPos(WorldMousePos, (int)Player.Plr.Layer, Chunks), true);














            }
            if (key == Keys.Y)
            {

                //var Chunk = Chunks.Last();


                generation.GenerateChunk(WorldMousePos, Chunks);

            }

            if (key == Keys.LeftControl)
            {

                _entityManager.Attract(30, WorldMousePos);

            }
            if (key == Keys.Q)
            {
                Player.DropItem(_userInterfaceManager.selectedItem, (new Vector3(Player.Plr.position, Player.Plr.Layer)), WorldMousePos, 1);
                if (!creativeMode)
                {
                    _userInterfaceManager.amount -= 1;
                    if (_userInterfaceManager.amount <= 0)
                    {
                        _userInterfaceManager.amount = 0;
                        _userInterfaceManager.selectedItem = null;
                    }
                }
            }
            if (_inputManager.IsKeyDown_Now(Keys.R))
            {
                //_actionManager.DeleteBlocksSphere(WorldMousePos, Player.Plr.Layer, 4);
                _actionManager.Explosion(new Vector3(WorldMousePos.X, WorldMousePos.Y, PLR.Layer), 5, true);
            }
            if (key == Keys.NumPad1)
            {
                Random random = new Random();
                var part = new Particle()
                {
                    Position = WorldMousePos,
                    TextureName = "BlockMineEffect",
                    Texture = _particleSystem.sprites[0],
                    lifeTime = 0.5f,
                    size = 1 + (float)random.NextDouble(),
                    Color = Color.Red,
                    Shift = new Vector4(-0.05f, 0.02f, 0.1f, 0f),
                    Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) * 2f,




                };



                _particleSystem.Particles.Add(part);
            }
        }

        if (_inputManager.IsKeyDown_Now(Keys.E))
        {
            //_userInterfaceManager.windows[0].Visible = !_userInterfaceManager.windows[0].Visible;
            //_userInterfaceManager.windows[2].Visible = !_userInterfaceManager.windows[2].Visible;

            _userInterfaceManager.GetWindow("Inventory").Visible = !_userInterfaceManager.GetWindow("Inventory").Visible;
            bool close = false;
            for (int i = 0; i < _userInterfaceManager.windows.Count; i++)
            {

                var u = _userInterfaceManager.windows[i];
                if (u.Tag == "cls")
                {
                    u.Visible = false;
                    close = true;
                }
            }
            if (close) { return; }

            _userInterfaceManager.GetWindow("Crafting2x2").Visible = !_userInterfaceManager.GetWindow("Crafting2x2").Visible;




        }
        if (_inputManager.IsKeyDown_Now(Keys.NumPad2))
        {
            Player.Plr.velocity.flying = !Player.Plr.velocity.flying;
        }

        if (_inputManager.IsKeyDown_Now(Keys.F))
        {
            var list = Generation.CaveGenerate(WorldMousePos, (int)WorldMousePos.X + 0, 10);
            foreach (var pos in list)
            {
                for (int x = -1; x <= 1; x += 1)
                {
                    for (int y = -1; y <= 1; y += 1)
                    {
                        var tile = BlockManager.GetLastBlockAtPos(pos + new Vector2(x, y), Chunks);
                        if (tile != null)
                        {
                            tile.ID = 0;
                        }
                    }

                }
            }

        }
        if (_inputManager.IsKeyDown_Now(Keys.F5))
        {

            DebugMode = !DebugMode;


        };
        if (_inputManager.IsKeyDown_Now(Keys.F6))
        {

            creativeMode = !creativeMode;


        };




        //_particleSystem.Particles[0].Position = PLR.position;



        if (!_userInterfaceManager.In_interface)
        {


            if (int.Abs(MouseClick) == 1)
            {

                var ent = Entity.GetentityAtPosition(WorldMousePos, _entityManager.Workspace);
                if (ent != null)
                {
                    PLR.Punch(ent, this);
                    return;
                }

                _entityAnimationService.Play(2, PLR);
                //add a attack part here instead

                float TempLayer = PLR.Layer;

                if (Front)
                {
                    PLR.Layer += 1;
                }
                if (Back)
                {
                    PLR.Layer -= 1;
                }

                float Damage = 0.3f;
                if (PLR.Item != null)
                {
                    if (PLR.Item.Item)
                    {
                        Damage = PLR.Item.Damage;
                    }
                }







                _actionManager.BreakBlock(WorldMousePos, Player.Plr.Layer, Damage); // When tools get added this will change
                PLR.Layer = TempLayer;


            }


            if (MouseClick == 2)
            {

                _actionManager.Interact(WorldMousePos);


            }
            if (int.Abs(MouseClick) == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    //var part = new Particle()
                    //{
                    //    Position = WorldMousePos,
                    //    TextureName = "ParticleSmokeEffect",
                    //    lifeTime = (float)random.NextDouble()*2,
                    //    Color = Microsoft.Xna.Framework.Color.FromNonPremultiplied(
                    //        new Vector4((float)random.NextDouble(),2f,0f, 1)
                    //        ),
                    //    Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f),

                    //};

                    //_particleSystem.Particles.Add(part);
                }


                float TempLayer = PLR.Layer;

                if (Front)
                {
                    PLR.Layer += 1;
                }
                if (Back)
                {
                    PLR.Layer -= 1;
                }
                _actionManager.PlaceBlock(WorldMousePos, PLR.Item);
                PLR.Layer = TempLayer;



            }





            if (TouchPanel.GetState().Count > 0)
            {
                var touch = TouchPanel.GetState()[0];
                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
                {
                    MousePosition = touch.Position.ToNumerics();
                }
            }



            //PLR.Animations[1].Paused = true;


        }














        // Get the center of the screen in screen coordinates

        Vector2 screenCenter = Vector2.Zero;

        Player.Plr.WalkTo(plrVel, false); // Adjust speed as needed
        if (jump)
        {
            PLR.Jump();
        }



        //Zooming


        if (keyboardState.IsKeyDown(Keys.OemPlus))
        {

            float oldBlockSize = BlockSize;


            Vector2 worldPos = (screenCenter - Player.cam.position) / oldBlockSize;
            BlockSize = oldBlockSize + zoomScale;

            Player.cam.position = screenCenter - worldPos * (oldBlockSize + zoomScale);
        }
        if (keyboardState.IsKeyDown(Keys.OemMinus))
        {
            // Zoom out
            float oldBlockSize = BlockSize;

            Vector2 worldPos = (screenCenter - Player.cam.position) / oldBlockSize;
            BlockSize = oldBlockSize - zoomScale;
            Player.cam.position = screenCenter - worldPos * (oldBlockSize - zoomScale);
        }






        if (keyboardState.IsKeyDown(Keys.T))
        {
            //InventoryOpen = !InventoryOpen;




        }
        if (keyboardState.IsKeyDown(Keys.H))
        {
            _entityManager.Workspace.Add(Entity.CloneEntity(_entityManager.entities[2], WorldMousePos));
            _entityManager.Workspace.Last().Speed = 0.6f;
        }
        if (keyboardState.IsKeyDown(Keys.B))
        {
            //InventoryOpen = !InventoryOpen;
            //Structure.LoadStructures()[0].GenerateStructure(World, WorldMousePos, true);
            Chunks.Clear();
            //_entityManager.Workspace.Add(Entity.CloneEntity(_entityManager.entities[1], WorldMousePos));

        }



        //New input
        if (keyboardState.IsKeyDown(Keys.F11))
        {

        }















    }

    TileGrid[] GetVisible(int x, int y, Chunk chunk) // Reduce lag by rendering only the 2-3 blocks at pos
    {
        var Render = new TileGrid[chunk.Tiles.GetLength(0)];
        TileGrid[,,] tileGrids = chunk.Tiles;

        for (int Z = tileGrids.GetLength(0) * 0 + (int)Player.Plr.Layer; Z >= 0; Z--)
        {

            var tile = tileGrids[Z, y, x];

            if (tile.ID == 0)
            {

                continue;

            }
            if (_blockManager.GetBlockAtTile(tile).Transparent && Render.Last() != tile)
            {
                Render[Z] = tile;

            }
            else
            {
                Render[Z] = tile;
                break;
            }

        }

        return Render;

    }








    protected override void Draw(GameTime gameTime)
    {

        // In your Draw() method, after setting up spriteBatch.Begin()

        Color Sky = Color.CornflowerBlue * float.Sin(TimeSinceStart / 24f);

        GraphicsDevice.Clear(Sky);

        //SunImage

        //_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        //_spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Daytime.ToString("0.00"), new Vector2(700, (float)Math.Sin(Daytime/12)), Color.Wheat);
        //_spriteBatch.End();
        //Shader.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.Milliseconds/10);

        //float scaleX = (float)GraphicsDevice.Viewport.Width / windowWidth;
        //float scaleY = (float)GraphicsDevice.Viewport.Height / windowHeight;
        //Matrix transformMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);


        //render layer, then entities 
















        if (GameStarted)
        {

            //foreach (var Mob in Entities)
            //{

            //    _spriteBatch.Draw(BlockTypes[2].Texture, BlockSize * Mob.position + Player.cam.position + (BlockSize) * Vector2.One / 2, null, Color.White, 0f, Vector2.Zero, BlockSize / BlockTypes[1].Texture.Width, SpriteEffects.None, 0f);


            //}


            // 2 cycles to render both directions of the world
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f,(int)player.position.X - 30);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, player.position, BreakTexture);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);

            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);
            //Player.cam.RenderChunk(_blockManager, _spriteBatch, Chunks[0], 0f, Player.Plr.position, BreakTexture);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, World, 0f, Player.Plr.position, BreakTexture);



            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var chunk in Chunks)
            {
                int maxLayer = chunk.Tiles.GetLength(0);

                for (int z = 0; z < maxLayer; z++)
                {
                    // -----------------------------
                    // DRAW TILES IN THIS LAYER
                    // -----------------------------
                    for (var i = 0; i < chunk.Tiles.GetLength(1); i++)
                    {
                        for (var j = 0; j < chunk.Tiles.GetLength(2); j++)
                        {
                            var renderList = GetVisible(j, i, chunk);
                            var tile = renderList[z];
                            if (tile != null)
                            {
                                var block = _blockManager.GetBlockAtTile(tile);
                                if (_blockManager.getBlock("Water").ID == tile.ID)
                                {
                                    //Shader.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
                                    //Shader.CurrentTechnique = Shader.Techniques["Water"];
                                    _spriteBatch.End();
                                    _spriteBatch.Begin(effect: Shader, samplerState: SamplerState.PointClamp);
                                    DrawBlock(tile, chunk, i, j, (float)z / (int)Player.Plr.Layer + 1, 1);
                                    _spriteBatch.End();
                                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                                    continue;

                                }

                                DrawBlock(tile, chunk, i, j, 1 - (float.Ceiling(Player.Plr.Layer - z)) / 9f, 1);
                            }


                        }
                    }

                    // -----------------------------
                    // DRAW ENTITIES IN THIS LAYER
                    // -----------------------------
                    foreach (Entity entity in _entityManager.Workspace)
                    {
                        if ((int)entity.Layer != z)
                            continue;

                        var pos = BlockManager.GetChunkAtPos(entity.position);
                        var entityChunk = BlockManager.GetChunk(pos[0], pos[1], Chunks);

                        if (entityChunk != chunk)
                            continue;

                        entity.DrawEntity(_spriteBatch, BlockSize, Player.cam.position, this);
                    }
                }
            }

            _spriteBatch.End();
















            //foreach (var chunk in Chunks)
            //{
            //    int RenderDistance = 2;
            //    var Pos = BlockManager.GetChunkAtPos(Player.Plr.position);
            //    int x = Pos[0];
            //    int y = Pos[1];

            //    if (chunk.x <= x + RenderDistance && chunk.x > x - RenderDistance)
            //    {





            //        for (var i = 0; i < chunk.Tiles.GetLength(1); i++)
            //        {
            //            for (var j = 0; j < chunk.Tiles.GetLength(2); j++)
            //            {
            //                float zindex = 0f;
            //                float Z = 0;
            //                var Tile = chunk.Tiles[0, i, j];
            //                float TraZ = 0f;
            //                TileGrid Transparent = null;

            //                //TileGrid Opaque = null;

            //                float plrZ = float.Floor(Player.Plr.Layer);
            //                for (int z = 0; z < chunk.Tiles.GetLength(0) && z <= plrZ; z++)
            //                {
            //                    if (chunk.Tiles[z, i, j].ID <= -1)
            //                    {
            //                        continue;
            //                    }
            //                    if (_blockManager.Blocks[chunk.Tiles[z, i, j].ID].Transparent)
            //                    {
            //                        Transparent = chunk.Tiles[z, i, j];
            //                        TraZ = z;
            //                        Z = z / 9f;
            //                    }
            //                    if (chunk.Tiles[z, i, j].ID != 0 && !_blockManager.Blocks[chunk.Tiles[z, i, j].ID].Transparent)
            //                    {



            //                        zindex = z;
            //                        Z = z / 9f;
            //                        Tile = chunk.Tiles[z, i, j];

            //                    }

            //                }
            //                if (Tile.ID == _blockManager.getBlock("Water").ID)
            //                {

            //                    Shader.Parameters["ID"].SetValue(1f);
            //                }

            //                DrawBlock(Tile, chunk, i, j, Z, zindex / 10);
            //                if (plrZ <= 8 && chunk.Tiles[(int)plrZ + 1, i, j] != null)
            //                {
            //                    DrawBlock(chunk.Tiles[(int)plrZ + 1, i, j], chunk, i, j, Z, (TraZ + 9) / 10f, true);
            //                }
            //                if (Transparent != null)
            //                {
            //                    DrawBlock(Transparent, chunk, i, j, Z, TraZ / 10);
            //                }




            //            }
            //        }


            //    }
            //}
            //_entityManager.RenderAll(_spriteBatch, BlockSize, Player.cam.position);

            //_spriteBatch.End();
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);



            //Player.cam.RenderLayer(_blockManager, _spriteBatch, Foreground, 0f, Player.Plr.position, BreakTexture);
            //Camera.RenderLayer(_blockManager, _spriteBatch, World, 2f);

            foreach (var P in _particleSystem.Particles)
            {

                P.DrawParticles(_spriteBatch, Player.cam.position, BlockSize, Content.Load<Texture2D>("ParticleSmokeEffect"));
            }

            base.Draw(gameTime);



            _spriteBatch.DrawString(font, TimeSinceStart.ToString(), new Vector2(0, 10), Color.White);

            if (DebugMode)
            {
                _spriteBatch.DrawString(font, ((int)Math.Ceiling((WorldMousePos.X / 32))).ToString(), Vector2.One * 40, Color.Chartreuse);
                _spriteBatch.DrawString(font, (Player.Plr.position).ToString(), Vector2.One, Color.Wheat);
                _spriteBatch.DrawString(font, Player.Plr.velocity.Gravity.ToString(), Vector2.One * 10, Color.Red);
                _spriteBatch.DrawString(font, Player.Plr.Health.ToString(), Vector2.One * 30, Color.Red);
                _spriteBatch.DrawString(font, ((int)(WorldMousePos.X % 32)).ToString(), Vector2.One * 60, Color.Red);
                _spriteBatch.DrawString(font, ((int)(WorldMousePos.Y % 32)).ToString(), Vector2.One * 60 + Vector2.UnitX * 30, Color.Red);
                _spriteBatch.DrawString(font, (HotbarIndex).ToString(), new Vector2(70, 20), Color.Red);
                _spriteBatch.DrawString(font, (BlockManager.GetPosAtBlock(BlockManager.GetBlockAtPos(WorldMousePos, Chunks))).ToString(), new Vector2(470, 40), Color.Beige);
                var b = BlockManager.GetBlockAtPos(WorldMousePos, (int)(Player.Plr.Layer), Chunks); //WHY NOT WORK!
                if (b != null)
                {
                    if (b.Data != "")
                    {
                        _spriteBatch.DrawString(font, b.Data, new Vector2(70, 70), Color.Blue);
                        _spriteBatch.DrawString(font, _blockManager.GetBlockAtTile(b).Name, new Vector2(70, 90), Color.Blue);
                    }
                }
                else _spriteBatch.DrawString(font, "Null", new Vector2(70, 70), Color.Blue);



            }
            if (_CommandManager.active)
            {
                int c = 0;

                foreach (var text in _CommandManager.Buffer)
                {
                    if (text[0] == '*')
                    {
                        _spriteBatch.DrawString(font, text, new Vector2(0, 20 * c), Color.IndianRed); c++; continue;

                    }
                    _spriteBatch.DrawString(font, text, new Vector2(0, 20 * c), Color.White);
                    c++;
                }
                _spriteBatch.DrawString(font, _CommandManager.Command + "_", new Vector2(0, 120 * c), Color.White);

            }



            _spriteBatch.DrawString(font, ((Player.Plr.velocity.velocity)).ToString(), new Vector2(500, 20), Color.WhiteSmoke);




            var Block = BlockManager.GetBlockAtPos(WorldMousePos, Chunks);
            if (Block != null)
            {
                _spriteBatch.DrawString(font, WorldMousePos.ToString(), Vector2.One * 40, Color.GhostWhite);

            }
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            for (int h = 0; h < Player.Plr.MaxHealth; h += 2)
            {

                Player.DrawStats(_spriteBatch, Content.Load<Texture2D>("UIelements/Stats"), "heart.0", new Vector2(h * 20 + windowWidth / 2, windowHeight * 0.8f));
            }

            for (int h = 0; h < Player.Plr.Health; h += 2)
            {
                string heartType = "heart";
                if (h + 1 == Player.Plr.Health)
                {
                    heartType = "heart.5";
                }

                Player.DrawStats(_spriteBatch, Content.Load<Texture2D>("UIelements/Stats"), heartType, new Vector2(h * 20 + windowWidth / 2, windowHeight * 0.8f));
            }
            _spriteBatch.End();
        }

        _userInterfaceManager.DrawUI(_spriteBatch, Content);

        //test.Draw(_spriteBatch);



    }
    void DrawBlock(TileGrid Tile, Chunk chunk, int i, int j, float Z, float layer)
    {
        DrawBlock(Tile, chunk, i, j, Z, layer, false);
    }
    void DrawBlock(TileGrid Tile, Chunk chunk, int i, int j, float Z, float layer, bool Opaque)
    {
        var block = _blockManager.Blocks[Tile.ID];


        float Light = Tile.brightness;

        float a = Z * Light;
        var Layer = Color.FromNonPremultiplied(new Vector4(a, a, a, 1)) * block.Color;
        int healthPercent = (int)Tile.MinedHealth / 10;
        Rectangle sourceRectangle = new Rectangle(healthPercent * BreakTexture.Height, 0, BreakTexture.Height, BreakTexture.Height);
        if ((int)Tile.MinedHealth <= 0)
        {
            sourceRectangle = new Rectangle(0, 0, 0, 0);
        }
        Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);
        if (block.Animated)
        {
            int Side = int.Min(block.Texture.Width, block.Texture.Height);
            int frames = int.Max(block.Texture.Width, block.Texture.Height);
            int framesCount = frames / Side;
            int currentFrame = (int)((TimeSinceStart) % framesCount);
            BlockState = new Rectangle(0, currentFrame * Side, Side, Side);


        }
        int State = block.DefaultState;
        if (Tile.state > 0)
        {
            BlockState = new Rectangle(State, 0, block.Texture.Width, block.Texture.Height);
        }

        if (Opaque)
        {
            Layer = Color.White * 0.3f;
        }
        Vector2 ChunkPos = (new Vector2(chunk.x, chunk.y) - Vector2.One) * chunk.Tiles.GetLength(1) * BlockSize;
        //Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);
        //_spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, BlockState, Layer, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, layer);
        DrawBlock(block, State, BlockSize / block.Texture.Width, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, layer, 0, Vector2.Zero, Layer, SpriteEffects.None);
        _spriteBatch.Draw(BreakTexture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, sourceRectangle, Layer, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 1);
        if (DebugMode)
        {
            if (Tile.Data != "")
                _spriteBatch.DrawString(font, Tile.Data, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, Color.WhiteSmoke);

        }

    }
    public void DrawBlock(Texture2D Texture, int state, Vector2 size, Vector2 position, float layer, float Orientation, Vector2 Origin, Color color, SpriteEffects spriteEffect)
    {
        Rectangle BlockState = new Rectangle(0, 0, Texture.Width, Texture.Height);

        int Side = int.Min(Texture.Width, Texture.Height);
        int frames = int.Max(Texture.Width, Texture.Height);
        int framesCount = frames / Side;
        int currentFrame = (int)((TimeSinceStart) * 2 % framesCount);
        BlockState = new Rectangle(0, currentFrame * Side, Side, Side);






        _spriteBatch.Draw(Texture, position, BlockState, color, Orientation, Origin, size, spriteEffect, layer);
    }

    public void DrawBlock(Block block, int state, float size, Vector2 position, float layer, float Orientation, Vector2 Origin, Color color, SpriteEffects spriteEffect) //Animated
    {
        Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);
        if (block.Animated)
        {
            int Side = int.Min(block.Texture.Width, block.Texture.Height);
            int frames = int.Max(block.Texture.Width, block.Texture.Height);
            int framesCount = frames / Side;
            int currentFrame = (int)((TimeSinceStart) * 2 % framesCount);
            BlockState = new Rectangle(0, currentFrame * Side, Side, Side);


        }



        _spriteBatch.Draw(block.Texture, position, BlockState, color, Orientation, Origin, size, spriteEffect, layer);
    }

    // UI:
    // - SingleClick get stack
    // - DoubleClick get Pull all from container
    // - right click take one individual
    // hold right click drag to sepearte item for each hovered cell
    // Q drop item
    // Q hold drop simulatinously
    // drag out of UI to drop stack or Shift Q



    // Fix The inventory UI or remove from main
    // Make steve Skin with rest of parts
}
