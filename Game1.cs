using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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
        Random random = new Random();
        _particleSystem.Content = Content;

        foreach (Chunk c in Chunks)
        {
            
            for (int i = 0; i < c.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < c.Tiles.GetLength(1); j++)
                {
                    if(i == 0)
                    {
                        
                        c.Tiles[i, j] = new TileGrid()
                        { ID = 2 };
                        continue;
                    }
                    c.Tiles[i, j] = new TileGrid()
                    { ID = 1 };
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

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var block in _blockManager.Blocks)
        {
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }




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

        _userInterfaceManager.windows[0].ItemSlots[0].Item = _blockManager.Blocks[4];
        _userInterfaceManager.windows[0].ItemSlots[0].Count = 64;
        _userInterfaceManager.windows[0].ItemSlots[1].Item = _blockManager.Blocks[5];
        _userInterfaceManager.windows[0].ItemSlots[1].Count = 64;
        _userInterfaceManager.windows[0].ItemSlots[2].Item = _blockManager.Blocks[6];
        _userInterfaceManager.windows[0].ItemSlots[2].Count = 64;

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
        if (Keyboard.GetState().IsKeyDown(Keys.F11))
        {
            
            _graphics.ToggleFullScreen();
        }

        foreach (var Window in _userInterfaceManager.windows)
        {
            Window.Update(this);
        }


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        //Lighting Setter

        //Lighting(BackGround, 35);
        //Lighting(World, 7);
        float Time = 1f;
        if(Daytime <= 12)
        {
            Daytime += Time;

        }
        
        if (Daytime > 12)
        {
            Daytime -= Time;

        }
        

        // TODO: Add your update logic here
        Input(gameTime);



        Player.cam.position = -Player.Plr.position * BlockSize + new Vector2(400, 202);
        base.Update(gameTime);
        MousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

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
        foreach (var entity in _entityManager.Workspace)
        {
            //entity.collisionBox.CheckCollision(entity,World);

            foreach (Entity entity1 in _entityManager.Workspace)
            {
                if (entity == entity1) continue;
                if (entity1.CheckCollisionEntity(entity))
                {
                    Entity.CollisionEventCollision(entity1, entity, this);
                }
            }
            entity.collisionBox.UpdateCollision(entity, Chunks);
            entity.velocity.apply_velocity(entity); // Apply gravity or any other force
            if (entity.ID == -1)
            {
                entity.Model3D.Update();
                continue;
            }
            //entity.UpdateAnimation();

            bool Running = false;
            if (float.Abs(entity.velocity.velocity.X) > 0.2)
            {

                Running = true;
            }

            if (Running) _entityAnimationService.Play(1, entity);
            else
            {
                _entityAnimationService.Stop(1, entity);
            }

            


            var EntVal = entity.velocity.velocity;

            
            

            if (entity.Jumping)
            {
                if (!entity.collisionBox.Bottom)
                {
                    EntVal += new Vector2(0, -12);
                }
                else
                {
                    entity.Jumping = false;
                }
            }



            if (entity.velocity.Gravity.Y > 0.3f)
            {
                entity.Fall_damage = (int)(entity.velocity.Gravity.Y * 3);
            }

            if (entity.collisionBox.Bottom)
            {
                entity.Health -= entity.Fall_damage;
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
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
        }
    }

    int HotbarIndex;


    public bool RightClicked = false;
    public bool LeftClicked = false;
    public void Input(GameTime time)
    {
        var PLR = Player.Plr;

        HotbarIndex = (Mouse.GetState().ScrollWheelValue / 120) % 9 + 1;

        








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


                _entityAnimationService.Play(2, PLR);
                //add a attack part here instead

                
                    _actionManager.BreakBlock(WorldMousePos);
                
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

               

                _actionManager.PlaceBlock(WorldMousePos,_userInterfaceManager.selectedItem);
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


        //PLR.Animations[1].Paused = true;









        foreach (var key in keyboard)
        {
            if (key == Keys.W)
            {

                plrVel += new Vector2(0, -12);
                Player.Plr.Jumping = true;
            }
            if (key == Keys.S)
            {

                plrVel += new Vector2(0, +12);
            }
            if (key == Keys.A)
            {


                PLR.Fliped = true;
                plrVel += new Vector2(-1, 0);
            }
            if (key == Keys.D)
            {
                //PLR.Animations[1].Paused = false;
                PLR.Fliped = false;
                plrVel += new Vector2(+1, 0);
            }
            if (key == Keys.X)
            {
                PLR.velocity.flying = !PLR.velocity.flying;

            }
            if (key == Keys.Y)
            {
                
                //var Chunk = Chunks.Last();
                

                Generation.GenerateChunk(WorldMousePos,Chunks);

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
        if (keyboardState.IsKeyDown(Keys.E))
        {
            //InventoryOpen = !InventoryOpen;
            _userInterfaceManager.windows[0].Visible = !_userInterfaceManager.windows[0].Visible;
            _userInterfaceManager.windows[2].Visible = !_userInterfaceManager.windows[2].Visible;


        }
        if (keyboardState.IsKeyDown(Keys.F))
        {
            //InventoryOpen = !InventoryOpen;
            //Structure.LoadStructures()[0].GenerateStructure(Chunks, WorldMousePos, true);
            var list = Generation.CaveGenerate(WorldMousePos, 0, 10);
            foreach (var pos in list)
            {
                var tile = BlockManager.GetBlockAtPos(pos, Chunks);
                if (tile != null)
                {
                    tile.ID = 0;
                }
            }


        }
        if (keyboardState.IsKeyDown(Keys.T))
        {
            //InventoryOpen = !InventoryOpen;



            var ent = Entity.GetentityAtPosition(WorldMousePos, _entityManager.Workspace);
            if (ent != null)
            {
                ent.velocity.velocity = PLR.position - ent.position * 3;
                ent.Health -= 3;
            }
        }
        if (keyboardState.IsKeyDown(Keys.H))
        {
            _entityManager.Workspace.Add(Entity.CloneEntity(_entityManager.entities[0], WorldMousePos));

        }
        if (keyboardState.IsKeyDown(Keys.B))
        {
            //InventoryOpen = !InventoryOpen;
            //Structure.LoadStructures()[0].GenerateStructure(World, WorldMousePos, true);
            Chunks.Clear();
            //_entityManager.Workspace.Add(Entity.CloneEntity(_entityManager.entities[1], WorldMousePos));

        }









    }



    
    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.FromNonPremultiplied(new Vector4(0.5f, 0.5f, Daytime / 12, 1)));

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
        //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);

        //Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f, Player.Plr.position, BreakTexture);
        //Player.cam.RenderChunk(_blockManager, _spriteBatch, Chunks[0], 0f, Player.Plr.position, BreakTexture);
        //Player.cam.RenderLayer(_blockManager, _spriteBatch, World, 0f, Player.Plr.position, BreakTexture);

        _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
        foreach (var chunk in Chunks)
        {
            int RenderDistance = 2;
            var Pos = BlockManager.GetChunkAtPos(Player.Plr.position);
            int x = Pos[0];
            int y = Pos[1];

            if (chunk.x <= x + RenderDistance && chunk.x > x - RenderDistance)
            {


                for (var i = 0; i < chunk.Tiles.GetLength(0); i++)
                {
                    for (var j = 0; j < chunk.Tiles.GetLength(1); j++)
                    {
                        var block = _blockManager.Blocks[chunk.Tiles[i, j].ID];
                        var Map = chunk.Tiles;
                        float Light = Map[i, j].brightness;
                        float Light01 = Light - 0.2f;

                        var color = Color.FromNonPremultiplied(new Vector4(Light01, Light01, Light01, 1)) * block.Color;

                        int healthPercent = (int)Map[i, j].MinedHealth / 10;
                        Rectangle sourceRectangle = new Rectangle(healthPercent * BreakTexture.Height, 0, BreakTexture.Height, BreakTexture.Height);
                        if ((int)Map[i, j].MinedHealth <= 0)
                        {
                            sourceRectangle = new Rectangle(0, 0, 0, 0);
                        }
                        Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);

                        int State = block.DefaultState;
                        if (Map[i, j].state > 0)
                        {
                            BlockState = new Rectangle(State, 0, block.Texture.Width, block.Texture.Height);
                        }

                        Vector2 ChunkPos = (new Vector2(chunk.x, chunk.y) - Vector2.One) * chunk.Tiles.GetLength(1) * BlockSize;
                        //Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);
                        _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, BlockState, block.Color, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(BreakTexture, new Vector2(j * BlockSize, i * BlockSize) + Player.cam.position + ChunkPos, sourceRectangle, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);

                    }
                }
            }
        }
        _spriteBatch.End();





        //Player.cam.RenderLayer(_blockManager, _spriteBatch, Foreground, 0f, Player.Plr.position, BreakTexture);
        //Camera.RenderLayer(_blockManager, _spriteBatch, World, 2f);
        foreach (var P in _particleSystem.Particles)
        {

            P.DrawParticles(_spriteBatch, Player.cam.position, BlockSize, Content.Load<Texture2D>("ParticleSmokeEffect"));
        }
        base.Draw(gameTime);

        _entityManager.RenderAll(_spriteBatch, BlockSize, Player.cam.position);
        _userInterfaceManager.DrawUI(_spriteBatch, Content);

        _spriteBatch.Begin();

        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), ((int)Math.Ceiling((WorldMousePos.X / 32))).ToString(), Vector2.One * 40, Color.Chartreuse);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), (Player.Plr.position).ToString(), Vector2.One, Color.Wheat);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Player.Plr.velocity.Gravity.ToString(), Vector2.One * 10, Color.Red);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), Player.Plr.Health.ToString(), Vector2.One * 30, Color.Red);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), ((int)(WorldMousePos.X % 32)).ToString(), Vector2.One * 60, Color.Red);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), ((int)(WorldMousePos.Y % 32)).ToString(), Vector2.One * 60 + Vector2.UnitX * 30, Color.Red);
        _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), (HotbarIndex).ToString(), new Vector2(70,20), Color.Red);



        var Block = BlockManager.GetBlockAtPos(WorldMousePos, Chunks);
        if (Block != null)
        {
            _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), WorldMousePos.ToString(), Vector2.One * 40, Color.GhostWhite);

        }
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
