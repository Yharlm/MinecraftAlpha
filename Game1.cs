using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MinecraftAlpha;



public class Game1 : Game
{
    public UserInterfaceManager _userInterfaceManager;
    public EntityManager _entityManager = new();
    public BlockManager _blockManager;
    public ActionManager _actionManager = new();
    public EntityAnimationService _entityAnimationService = new();
    public ParticleSystem _particleSystem = new();
    public RecipeManager _RecipeManager = new();
    public InputManager _inputManager = new();
    

    public Effect Shader;




    public Texture2D BreakTexture;
    public SpriteFont font;
    
    public Player Player;
    public bool DebugMode = false;

    public bool GameStarted = false;

    public float Daytime = 0f;
    public List<Entity> Entities;
    public List<Block> BlockTypes;
    public Items items = new();

    public bool creativeMode = true;
    //Chunks list

    public List<Chunk> Chunks = new List<Chunk>()
    {
        new Chunk(1,2),
        new Chunk(0,2),
        new Chunk(-1,2),


    };












    public Vector2 WorldMousePos = Vector2.Zero;
    public Vector2 MousePosition = Vector2.Zero;
    public bool InventoryOpen = false;
    public float BlockSize = 16f;


    static public int WorldSizeX = 300;
    static public int WorldSizeY = 300;

    public List<TileGrid[,]> Layers = new List<TileGrid[,]>()
    {
        new TileGrid[WorldSizeX, WorldSizeY], // Background3
        new TileGrid[WorldSizeX, WorldSizeY], // Background2
        new TileGrid[WorldSizeX, WorldSizeY], // Background
        new TileGrid[WorldSizeX, WorldSizeY], // Main World
        new TileGrid[WorldSizeX, WorldSizeY], // Foreground
    };
    public TileGrid[,] BackGround { get; set; } = new TileGrid[WorldSizeX, WorldSizeY];
    public TileGrid[,] Foreground { get; set; } = new TileGrid[WorldSizeX, WorldSizeY];
    public TileGrid[,] World { get; set; } = new TileGrid[WorldSizeX, WorldSizeY];


    public int windowWidth;
    public int windowHeight;


    private GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;

    public Game1()
    {




        _userInterfaceManager = new UserInterfaceManager(this);
        _blockManager = new BlockManager(this);

        _actionManager.Game = this;
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

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
        _entityManager.game = this;
        Player = new Player() { game = this};
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
        Entities = _entityManager.entities;
        BlockTypes = _blockManager.Blocks;



        {
            for (int i = 0; i < WorldSizeY; i++)
            {
                for (int j = 0; j < WorldSizeX; j++)
                {
                    World[j, i] = new TileGrid() { pos = new Vector2(j * 32, i * 32) };
                    BackGround[j, i] = new TileGrid();
                }
            }



            var Map = Generation.GenerateWhiteNoise(250, 5, 0, 0);
            Map = Generation.GeneratePerlinNoise(Map, 6, 1f);
            Generation.SumMaps(Map, Generation.GenerateFlat(250, 5, 0.2f), 0.7f);
            Map = Generation.GenerateSmoothNoise(Map, 4);
            //Map = Generation.GenerateFlat(250, 5, 0.2f);
            int Height = 17;
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                float Y = (Map[0, i]) * Height + 50;
                World[(int)Y, i].ID = 2;
                World[(int)Y + 1, i].ID = 1;
                World[(int)Y + 2, i].ID = 1;
                //place blocks Downwards from here
            }



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
    }




    protected override void LoadContent()
    {
        //BreakTexture = Content.Load<Texture2D>("break_animation");


        items.Atlas = Content.Load<Texture2D>("Items");


        // Items








        font = Content.Load<SpriteFont>("Font");
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var block in _blockManager.Blocks)
        {
            if(block.TexturePath == "_item") continue;
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }

        Shader = Content.Load<Effect>("Shaders/Shader");


        //_userInterfaceManager.ItemSlots = UserInterfaceManager.LoadItemSlots(_blockManager.Blocks);
        _entityManager.entities = EntityManager.LoadEntites(this);
        _entityManager.LoadSprites(Content);
        _entityManager.LoadJoints();




        _userInterfaceManager.LoadTextures(Content);

        //_entityAnimationService.LoadAnimations(_entityManager.entities);
        BreakTexture = Content.Load<Texture2D>("UIelements/destroy_stage_0-Sheet");


        _RecipeManager.Recipes = _RecipeManager.LoadRecipes(_blockManager);
        //Making player
        Player.Plr = Entity.CloneEntity(_entityManager.entities[0], new Vector2(0, 0));



        _entityManager.Workspace.Add(Player.Plr);






        //EFrame.Window = Content.Load<Texture2D>("UIelements/WindowFrame");

        _particleSystem.Load();
        _blockManager.LoadActions();
        _userInterfaceManager.selectedItem = _blockManager.Blocks[3];
        _userInterfaceManager.amount = 5;

        //var Grass = Content.Load<Texture2D>("dirt");
        //var wood = Content.Load<Texture2D>("oak_planks");

        //test = new Sprite3D(wood, Grass, wood, wood);


        // TODO: use this.Content to load your game content here

        _userInterfaceManager.LoadGUI();

        
        for(int i = 1; i < _blockManager.Blocks.Count; i++)
        {
            _userInterfaceManager.windows[0].ItemSlots[i].Item = _blockManager.Blocks[i];
            _userInterfaceManager.windows[0].ItemSlots[i].Count = 64;
        }

    }
    Sprite3D test;
    public void IluminateDiamond(int x, int y, float val1, TileGrid[,] grid)
    {
        int val = (int)val1;

        int size = (int)val1;
        //int mid = size / 2;
        //for (int i = 0; i <= size; i++)
        //{
        //    int diff = Math.Abs(mid - i);
        //    for (int j = diff; j <= size - diff; j++)
        //    {
        //        var Distancex = mid - j;
        //        var Distancey = mid - i;
        //        var DistanceToMid = 1 - Math.Sqrt(Distancex * Distancex + Distancey * Distancey) / mid;


        //        float dist = (new Vector2(j, i) - new Vector2(mid, mid)).Length();
        //        grid[i + y - val / 2, j + x - val / 2].brightness += 1 - (float)DistanceToMid;
        //    }

        //}

        //int size = 14;
        int mid = size / 2;
        for (int i = 0; i <= size; i++)
        {
            int diff = Math.Abs(mid - i);
            for (int j = diff; j <= size - diff; j++)
            {
                var Distancex = mid - j;
                var Distancey = mid - i;
                var DistanceToMid = Math.Sqrt(Distancex * Distancex + Distancey * Distancey) / mid;



                grid[i + y - val / 2, j + x - val / 2].brightness += 1 - (float)DistanceToMid;
            }

        }

    }

    public void Lighting(TileGrid[,] map, float layer)
    {
        var Grid = map;
        Vector2 pos = Player.Plr.position;
        for (float i = pos.Y - 20; i < pos.Y + 20; i++)
        {
            for (float j = pos.X - 20; j < pos.X + 20; j++)
            {
                var grid = Grid[(int)i, (int)j];

                grid.brightness = 0f;

            }
        }
        for (int i = (int)pos.X - 20; i < pos.X + 20; i += 1)
        {
            int localY = (int)pos.Y - 20;
            while (localY < (int)pos.Y + 20)
            {
                if (Grid[localY + 1, i].ID != 0)
                {
                    IluminateDiamond(i, localY, layer, Grid); break;
                }
                localY += 1;
            }
        }
    }
    public bool Clicked = false;
    
    protected override void Update(GameTime gameTime)
    {
        // block drop testing remove later
        //test.Update();

        //for (int i = 0; i < World.GetLength(0); i++)
        //{
        //    for (int j = 0; j < World.GetLength(1); j++)
        //    {
        //        var grid = World[i, j];
        //        Block block = BlockTypes[grid.ID];

        //        block.Update(grid);


        //    }
        //}


        
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

        if(Player.Plr.Health <= 0)
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



        Player.cam.position = -Player.Plr.position * BlockSize + new Vector2(windowWidth,windowHeight)/2;
        base.Update(gameTime);
        

        WorldMousePos = (MousePosition - Player.cam.position) / BlockSize;

        //
        _blockManager.BlockSize = BlockSize;
        foreach (var Animation in _entityAnimationService.entityAnimations)
        {
            var Entity = Animation.parent;

            var anim = Entity.Animations[Animation.id];
            anim.parent = Entity;
            anim.Update();
            //Entity.Joints[1].orientation += 10;



        }


        _entityAnimationService.entityAnimations.RemoveAll(x => x.parent.Animations[x.id].Paused);

        var ItemList = new List<Entity>();
        foreach (var entity in _entityManager.Workspace)
        {
            //entity.collisionBox.CheckCollision(entity,World);
            entity.Lifetime += 0.1f;
            if (entity.position.Y > 600)
            {
                entity.TakeDamage(null, 5,0);
            }


            entity.Grounded = true;
            if (entity.velocity.velocity.Y > 6f)
            {
                entity.Fall_damage = (int)(entity.velocity.velocity.Y)-7;
            }

            if (entity.collisionBox.Bottom && !entity.velocity.flying)
            {
                entity.TakeDamage(null, entity.Fall_damage,0);
                entity.Fall_damage = 0;
                entity.Grounded = false;
            }
            foreach (Entity entity1 in _entityManager.Workspace)
            {
                if (entity == entity1) continue;
                if (entity1.CheckCollisionEntity(entity))
                {
                    Entity.CollisionEventCollision(entity1, entity, this);
                }
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

            if (Running) _entityAnimationService.Play(1, entity);
            else
            {
                _entityAnimationService.Stop(1, entity);
            }
            

            




            if (entity.Health <= 0)
            {
                _entityManager.Die(entity, ItemList);
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
    }

    int HotbarIndex;

    

    public int MouseClick = 0;
    public void Input(GameTime time)
    {

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
                PLR.velocity.flying = !PLR.velocity.flying;

            }
            if (key == Keys.Y)
            {

                //var Chunk = Chunks.Last();


                Generation.GenerateChunk(WorldMousePos, Chunks);

            }
            
            if (key == Keys.LeftControl)
            {

                _entityManager.Attract(30,WorldMousePos);

            }
            if (key == Keys.Q)
            {
                Player.DropItem(_userInterfaceManager.selectedItem, (new Vector3(Player.Plr.position, Player.Plr.Layer)), WorldMousePos, 1);
                if(!creativeMode)
                {
                    _userInterfaceManager.amount -= 1;
                    if (_userInterfaceManager.amount <= 0)
                    {
                        _userInterfaceManager.selectedItem = null;
                    }
                }
            }
            if (key == Keys.R)
            {
                _actionManager.DeleteBlocksSphere(WorldMousePos, Player.Plr.Layer, 4);
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
                    size = 1+(float)random.NextDouble(),
                    Color = Color.Red,
                    Shift= new Vector4(-0.05f, 0.02f, 0.1f, 0f),
                    Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f)*2f,




                };



                _particleSystem.Particles.Add(part);
            }
        }
        
        if(_inputManager.IsKeyDown_Now(Keys.E))
        {
            _userInterfaceManager.windows[0].Visible = !_userInterfaceManager.windows[0].Visible;
            _userInterfaceManager.windows[2].Visible = !_userInterfaceManager.windows[2].Visible;
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

        _inputManager.UpdateKeyHistory(keyboardState);


        //_particleSystem.Particles[0].Position = PLR.position;




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
                _actionManager.BreakBlock(WorldMousePos,Player.Plr.Layer,0.5f); // When tools get added this will change
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
                _actionManager.PlaceBlock(WorldMousePos, _userInterfaceManager.selectedItem);
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

















        // Get the center of the screen in screen coordinates

        Vector2 screenCenter =Vector2.Zero;

        Player.Plr.WalkTo(plrVel,false); // Adjust speed as needed
        if (jump)
        {
            PLR.Jump();
        }
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
            _entityManager.Workspace.Add(Entity.CloneEntity(_entityManager.entities[0], WorldMousePos));
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
        if(keyboardState.IsKeyDown(Keys.F11))
        {
            
        }



        

        









    }




    protected override void Draw(GameTime gameTime)
    {

        // In your Draw() method, after setting up spriteBatch.Begin()
        
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //SunImage

        //_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        //_spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Daytime.ToString("0.00"), new Vector2(700, (float)Math.Sin(Daytime/12)), Color.Wheat);
        //_spriteBatch.End();
        //Shader.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.Milliseconds/10);

        //float scaleX = (float)GraphicsDevice.Viewport.Width / windowWidth;
        //float scaleY = (float)GraphicsDevice.Viewport.Height / windowHeight;
        //Matrix transformMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);



        if (GameStarted)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, effect: Shader);
            foreach (var Mob in Entities)
            {

                _spriteBatch.Draw(BlockTypes[2].Texture, BlockSize * Mob.position + Player.cam.position + (BlockSize) * Vector2.One / 2, null, Color.White, 0f, Vector2.Zero, BlockSize / BlockTypes[1].Texture.Width, SpriteEffects.None, 0f);


            }


            // 2 cycles to render both directions of the world
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f,(int)player.position.X - 30);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, player.position, BreakTexture);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);

            //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);
            //Player.cam.RenderChunk(_blockManager, _spriteBatch, Chunks[0], 0f, Player.Plr.position, BreakTexture);
            //Player.cam.RenderLayer(_blockManager, _spriteBatch, World, 0f, Player.Plr.position, BreakTexture);


            foreach (var chunk in Chunks)
            {
                int RenderDistance = 2;
                var Pos = BlockManager.GetChunkAtPos(Player.Plr.position);
                int x = Pos[0];
                int y = Pos[1];

                if (chunk.x <= x + RenderDistance && chunk.x > x - RenderDistance)
                {





                    for (var i = 0; i < chunk.Tiles.GetLength(1); i++)
                    {
                        for (var j = 0; j < chunk.Tiles.GetLength(2); j++)
                        {
                            float zindex = 0f;
                            float Z = 0;
                            var Tile = chunk.Tiles[0, i, j];
                            float TraZ = 0f;
                            TileGrid Transparent = null;

                            //TileGrid Opaque = null;

                            float plrZ = float.Floor(Player.Plr.Layer);
                            for (int z = 0; z < chunk.Tiles.GetLength(0) && z <= plrZ; z++)
                            {

                                if (_blockManager.Blocks[chunk.Tiles[z, i, j].ID].Transparent)
                                {
                                    Transparent = chunk.Tiles[z, i, j];
                                    TraZ = z;
                                    Z = z / 9f;
                                }
                                if (chunk.Tiles[z, i, j].ID != 0 && !_blockManager.Blocks[chunk.Tiles[z, i, j].ID].Transparent)
                                {



                                    zindex = z;
                                    Z = z / 9f;
                                    Tile = chunk.Tiles[z, i, j];

                                }

                            }


                            DrawBlock(Tile, chunk, i, j, Z, zindex / 10);
                            if (plrZ <= 8 && chunk.Tiles[(int)plrZ + 1, i, j] != null)
                            {
                                DrawBlock(chunk.Tiles[(int)plrZ + 1, i, j], chunk, i, j, Z, (TraZ + 9) / 10f, true);
                            }
                            if (Transparent != null)
                            {
                                DrawBlock(Transparent, chunk, i, j, Z, TraZ / 10);
                            }




                        }
                    }


                }
            }
            _entityManager.RenderAll(_spriteBatch, BlockSize, Player.cam.position);

            _spriteBatch.End();
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);



            //Player.cam.RenderLayer(_blockManager, _spriteBatch, Foreground, 0f, Player.Plr.position, BreakTexture);
            //Camera.RenderLayer(_blockManager, _spriteBatch, World, 2f);
            foreach (var P in _particleSystem.Particles)
            {

                P.DrawParticles(_spriteBatch, Player.cam.position, BlockSize, Content.Load<Texture2D>("ParticleSmokeEffect"));
            }
            base.Draw(gameTime);


            



            _spriteBatch.DrawString(font, ((int)Math.Ceiling((WorldMousePos.X / 32))).ToString(), Vector2.One * 40, Color.Chartreuse);
            _spriteBatch.DrawString(font, (Player.Plr.position).ToString(), Vector2.One, Color.Wheat);
            _spriteBatch.DrawString(font, Player.Plr.velocity.Gravity.ToString(), Vector2.One * 10, Color.Red);
            _spriteBatch.DrawString(font, Player.Plr.Health.ToString(), Vector2.One * 30, Color.Red);
            _spriteBatch.DrawString(font, ((int)(WorldMousePos.X % 32)).ToString(), Vector2.One * 60, Color.Red);
            _spriteBatch.DrawString(font, ((int)(WorldMousePos.Y % 32)).ToString(), Vector2.One * 60 + Vector2.UnitX * 30, Color.Red);
            _spriteBatch.DrawString(font, (HotbarIndex).ToString(), new Vector2(70, 20), Color.Red);



            _spriteBatch.DrawString(font, ((Player.Plr.velocity.velocity)).ToString(), new Vector2(500, 20), Color.WhiteSmoke);




            var Block = BlockManager.GetBlockAtPos(WorldMousePos, Chunks);
            if (Block != null)
            {
                _spriteBatch.DrawString(font, WorldMousePos.ToString(), Vector2.One * 40, Color.GhostWhite);

            }
            _spriteBatch.End();
        }

        _userInterfaceManager.DrawUI(_spriteBatch, Content);

        //test.Draw(_spriteBatch);

        _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
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
    void DrawBlock(TileGrid Tile, Chunk chunk, int i, int j, float Z, float layer)
    {
        DrawBlock(Tile, chunk, i, j, Z, layer, false);
    }
    void DrawBlock(TileGrid Tile,Chunk chunk,int i,int j, float Z,float layer,bool Opaque)
    {
        var block = _blockManager.Blocks[Tile.ID];
        
        float Light = Tile.brightness;
        float Light01 = Light - 0.2f;
        float a = Z /2 + 0.4f;
        //var color = Color.FromNonPremultiplied(new Vector4(Light01, Light01, Light01, 1)) ;
        var Layer = Color.FromNonPremultiplied(new Vector4(a, a, a, 1)) * block.Color;

        int healthPercent = (int)Tile.MinedHealth / 10;
        Rectangle sourceRectangle = new Rectangle(healthPercent * BreakTexture.Height, 0, BreakTexture.Height, BreakTexture.Height);
        if ((int)Tile.MinedHealth <= 0)
        {
            sourceRectangle = new Rectangle(0, 0, 0, 0);
        }
        Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);

        int State = block.DefaultState;
        if (Tile.state > 0)
        {
            BlockState = new Rectangle(State, 0, block.Texture.Width, block.Texture.Height);
        }

        if (Opaque)
        {
             Layer = Color.White * 0.2f;
        }
        Vector2 ChunkPos = (new Vector2(chunk.x, chunk.y) - Vector2.One) * chunk.Tiles.GetLength(1) * BlockSize;
        //Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);
        _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, BlockState, Layer, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, layer);
        _spriteBatch.Draw(BreakTexture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, sourceRectangle, Layer, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, layer+0.01f);
        
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
