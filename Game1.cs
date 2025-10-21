using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace MinecraftAlpha;



public class Game1 : Game
{
    public UserInterfaceManager _userInterfaceManager = new();
    public EntityManager _entityManager = new();
    public BlockManager _blockManager;
    public ActionManager _actionManager = new();
    public EntityAnimationService _entityAnimationService = new();
    public ParticleSystem _particleSystem = new();
    public RecipeManager _RecipeManager = new();


    public Texture2D BreakTexture;

    public Player Player = new Player();

    public float Daytime = 0f;
    public List<Entity> Entities;
    public List<Block> BlockTypes;


    //Structures







    


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


    



    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {

        _blockManager = new BlockManager(this);
            
        _actionManager.Game = this;
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    protected override void Initialize()
    {
        _particleSystem.Content = Content;
        Random random = new Random();
        // TODO: Add your initialization logic here
        Entities = _entityManager.entities;
        BlockTypes = _blockManager.Blocks;
        for (int i = 0; i < WorldSizeY; i++)
        {
            for (int j = 0; j < WorldSizeX; j++)
            {
                World[j, i] = new TileGrid();
                BackGround[j, i] = new TileGrid();
            }
        }


        // World generation
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


        base.Initialize();
    }

    public UIFrame EFrame = new UIFrame();


    protected override void LoadContent()
    {
        //BreakTexture = Content.Load<Texture2D>("break_animation");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var block in _blockManager.Blocks)
        {
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }




        //_userInterfaceManager.ItemSlots = UserInterfaceManager.LoadItemSlots(_blockManager.Blocks);
        _entityManager.LoadEntities(this);
        _entityManager.LoadSprites(Content);
        _entityManager.LoadJoints();
        
        _userInterfaceManager.LoadTextures(Content);
        _entityAnimationService.CreateAnimations(Entities);
        _entityAnimationService.LoadAnimations(_entityManager.entities);
        BreakTexture = Content.Load<Texture2D>("UIelements/destroy_stage_0-Sheet");
        

        _RecipeManager.Recipes = _RecipeManager.LoadRecipes(_blockManager);
        //Making player
        Player.Plr = _entityManager.entities[0];
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

        _userInterfaceManager.windows[0].ItemSlots[0].Item = _blockManager.Blocks[4];
        _userInterfaceManager.windows[0].ItemSlots[0].Count = 64;
        _userInterfaceManager.windows[0].ItemSlots[1].Item = _blockManager.Blocks[5];
        _userInterfaceManager.windows[0].ItemSlots[1].Count = 64;
        _userInterfaceManager.windows[0].ItemSlots[2].Item = _blockManager.Blocks[6];
        _userInterfaceManager.windows[0].ItemSlots[2].Count = 64;

    }
    Sprite3D test;
    public void IluminateDiamond(int x,int y,float val1, TileGrid[,] grid)
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

    public void Lighting(TileGrid[,] map,float layer)
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

    protected override void Update(GameTime gameTime)
    {
        // block drop testing remove later
        //test.Update();

        for(int i = 0;i < World.GetLength(0);i++)
        {
            for(int j = 0;j < World.GetLength(1);j++)
            {
                var grid = World[i, j];
                Block block = BlockTypes[grid.ID];

                block.Update(grid);
                
                
            }
        }

        foreach(var Window in _userInterfaceManager.windows)
        {
            Window.Update(this);
        }


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        //Lighting Setter

        Lighting(BackGround, 35);
        Lighting(World,7);

        Daytime += 0.01f;
        if (Daytime >= 24)
        {
            Daytime = 0f;
        }

        // TODO: Add your update logic here
        Input();



        Player.cam.position = -Player.Plr.position * BlockSize + new Vector2(400, 202);
        base.Update(gameTime);
        MousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

        WorldMousePos = (MousePosition - Player.cam.position) / BlockSize;
        _blockManager.BlockSize = BlockSize;
        foreach (var entity in _entityManager.Workspace)
        {

           
            foreach (Entity entity1 in _entityManager.Workspace)
            {
                if (entity == entity1) continue;
                if(entity1.CheckCollisionEntity(entity))
                {
                    Entity.CollisionEventCollision(entity1, entity,this);
                }
            }
            entity.collisionBox.UpdateCollision(entity, World);
            entity.velocity.apply_velocity(entity); // Apply gravity or any other force
            if (entity.ID == -1)
            {
                entity.Model3D.Update();
                continue;
            }
            entity.UpdateAnimation();


            

            if(entity.velocity.Gravity.Y > 0.3f)
            {
                entity.Fall_damage =(int)(entity.velocity.Gravity.Y*24);
            }

            if(entity.collisionBox.Bottom )
            {
                entity.Health-= entity.Fall_damage;
                entity.Fall_damage = 0;
            }
            
            // Example gravity, can be replaced with actual logic

        }
        _entityManager.Workspace.RemoveAll(x => x.Health <= 0);
        
        foreach (var entity in _particleSystem.Particles)
        {
            entity.Update();
        }
        _particleSystem.Particles.RemoveAll(x => x.lifeTime <= 0);

    }


    Random random = new Random();
    public bool RightClicked = false;
    public bool LeftClicked = false;
    public void Input()
    {
        var PLR = _entityManager.Workspace[0];

        

        

        PLR.Animations[2].Paused = true;

        



        //_particleSystem.Particles[0].Position = PLR.position;
        _userInterfaceManager.HoverAction(MousePosition, _actionManager);
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !LeftClicked)
        {

            LeftClicked = true;
            _userInterfaceManager.ClickAction(MousePosition, _actionManager, true);




        }
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (!_userInterfaceManager.Clicked)
            {
                if (PLR.Animations[2].Playing == false)
                {
                    PLR.Animations[2].Time = 0f;
                    PLR.Animations[2].Paused = false;
                }
                
                if (WorldMousePos.X > 0 && WorldMousePos.Y > 0)
                {
                    int BlockX = (int)(WorldMousePos.X);
                    int BlockY = (int)(WorldMousePos.Y);

                    _actionManager.BreakBlock(BlockX, BlockY);
                }
            }

        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released)
        {

            LeftClicked = false;
        }


        if (Mouse.GetState().RightButton == ButtonState.Pressed && !RightClicked)
        {
            RightClicked = true;

            _userInterfaceManager.ClickAction(MousePosition, _actionManager, false);

            _actionManager.Interact(WorldMousePos);




        }
        if (Mouse.GetState().RightButton == ButtonState.Pressed)
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
            if (!_userInterfaceManager.Clicked)
            {
                PLR.Animations[2].Time = 0f;
                PLR.Animations[2].Paused = false;
                int BlockX = (int)(WorldMousePos.X);
                int BlockY = (int)(WorldMousePos.Y);

                _actionManager.PlaceBlock(BlockX, BlockY);
            }
        }
        else if (Mouse.GetState().RightButton == ButtonState.Released)
        {
            _userInterfaceManager.Clicked = false;
            RightClicked = false;
        }




        if (TouchPanel.GetState().Count > 0)
        {
            var touch = TouchPanel.GetState()[0];
            if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
            {
                MousePosition = touch.Position.ToNumerics();
            }
        }

        Vector2 plrVel = Vector2.Zero;
        Player.Plr.velocity.velocity = new Vector2(0, 0);
        float zoomScale = 0.3f;
        var keyboardState = Keyboard.GetState();

        var keyboard = Keyboard.GetState().GetPressedKeys();


        PLR.Animations[1].Paused = true;

        foreach (var key in keyboard)
        {
            if (key == Keys.W)
            {
                plrVel += new Vector2(0, -12);
                Player.Jumping = true;
            }
            if (key == Keys.S)
            {

                plrVel += new Vector2(0, +1);
            }
            if (key == Keys.A)
            {
                PLR.Animations[1].Paused = false;
                PLR.Fliped = true;
                plrVel += new Vector2(-1, 0);
            }
            if (key == Keys.D)
            {
                PLR.Animations[1].Paused = false;
                PLR.Fliped = false;
                plrVel += new Vector2(+1, 0);
            }
        }
        if (Player.Jumping)
        {
            if (!PLR.collisionBox.Bottom)
            {
                plrVel += new Vector2(0, -12);
            }
            else
            {
                Player.Jumping = false;
            }
        }

        // Get the center of the screen in screen coordinates

        Vector2 screenCenter = Player.cam.size / 2f;
        _entityManager.Workspace[0].velocity.velocity += plrVel * 0.1f; // Adjust speed as needed
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
        //if (keyboardState.IsKeyDown(Keys.E))
        //{
        //    //InventoryOpen = !InventoryOpen;
        //    _userInterfaceManager.windows[0].Visible = !_userInterfaceManager.windows[0].Visible;
        //    _userInterfaceManager.windows[2].Visible = !_userInterfaceManager.windows[2].Visible;


        //}
        if (keyboardState.IsKeyDown(Keys.F))
        {
            //InventoryOpen = !InventoryOpen;
            Structure.LoadStructures()[0].GenerateStructure(World, WorldMousePos, true);



        }










    }

    
    

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        //SunImage

        //_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        //_spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Daytime.ToString("0.00"), new Vector2(700, (float)Math.Sin(Daytime/12)), Color.Wheat);
        //_spriteBatch.End();

        _spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        foreach (var Mob in Entities)
        {

            _spriteBatch.Draw(BlockTypes[2].Texture, BlockSize * Mob.position + Player.cam.position + (BlockSize) * Vector2.One / 2, null, Color.White, 0f, Vector2.Zero, BlockSize / BlockTypes[1].Texture.Width, SpriteEffects.None, 0f);


        }
        _spriteBatch.End();
        
        // 2 cycles to render both directions of the world
        //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f,(int)player.position.X - 30);
        //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, player.position, BreakTexture);
        Player.cam.RenderLayer(_blockManager, _spriteBatch, World, 0f, Player.Plr.position,BreakTexture);

        //Camera.RenderLayer(_blockManager, _spriteBatch, World, 2f);
        foreach (var P in _particleSystem.Particles)
        {

            P.DrawParticles(_spriteBatch, Player.cam.position, BlockSize, Content.Load<Texture2D>("ParticleSmokeEffect"));
        }
        base.Draw(gameTime);
        
        _entityManager.RenderAll(_spriteBatch, BlockSize, Player.cam.position);
        _userInterfaceManager.DrawUI(_spriteBatch, Content);

        _spriteBatch.Begin();
        
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Player.cam.position.ToString(), Vector2.One, Color.Wheat);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Player.Plr.velocity.Gravity.ToString(), Vector2.One * 10, Color.Red);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Player.Plr.Health.ToString(), Vector2.One*30, Color.Red);
        _spriteBatch.End();

        //test.Draw(_spriteBatch);

        

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
