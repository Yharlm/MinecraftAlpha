using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MinecraftAlpha;



public class Game1 : Game
{
    UserInterfaceManager _userInterfaceManager = new UserInterfaceManager();
    EntityManager _entityManager = new EntityManager();
    BlockManager _blockManager = new BlockManager();
    ActionManager _actionManager = new ActionManager();
    EntityAnimationService _entityAnimationService = new EntityAnimationService();

    


    Player Player = new Player();


    public List<Entity> Entities;
    public List<Block> BlockTypes;


    Entity player;


    public Vector2 WorldMousePos = Vector2.Zero;
    public Vector2 MousePosition = Vector2.Zero;
    public bool InventoryOpen = false;
    public float BlockSize = 16f;


    static public int WorldSizeX = 300;
    static public int WorldSizeY = 300;
    public int[,] BackGround { get; set; } = new int[WorldSizeX, WorldSizeY];
    public int[,] Foreground { get; set; } = new int[WorldSizeX, WorldSizeY];
    public int[,] World { get; set; } = new int[WorldSizeX, WorldSizeY];


    public List<Block> Blocks;



    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {

        _entityManager.LoadEntities();

        _actionManager.Game = this;
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    protected override void Initialize()
    {

        Random random = new Random();
        // TODO: Add your initialization logic here
        Entities = _entityManager.entities;
        BlockTypes = _blockManager.Blocks;
        player = Entities[0];



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
                    World[t + StoneLayer, x] = 3; // Dirt
                    StoneLayer--;
                }

                World[t, x] = 2; // Dirt
                while (DirtLayer > 0)
                {
                    World[t + DirtLayer, x] = 1; // Dirt
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
                    BackGround[t + StoneLayer, x] = 3; // Dirt
                    StoneLayer--;
                }

                BackGround[t, x] = 2; // Dirt
                while (DirtLayer > 0)
                {
                    BackGround[t + DirtLayer, x] = 1; // Dirt
                    DirtLayer--;
                }
                x++;
                length--;
            }

        }


        base.Initialize();
    }



    protected override void LoadContent()
    {


        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var block in _blockManager.Blocks)
        {
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }
        _entityManager.LoadSprites(Content);
        _entityManager.LoadJoints();
        _userInterfaceManager.LoadTextures(Content);
        _entityAnimationService.LoadAnimations(_entityManager.entities);
        _entityManager.Workspace.Add(player);

        

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Input();
        var player = _entityManager.Workspace[0];


        Player.cam.position = -player.position * BlockSize + new Vector2(400, 202);
        base.Update(gameTime);
        MousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

        WorldMousePos = (MousePosition - Player.cam.position) / BlockSize;
        _blockManager.BlockSize = BlockSize;
        foreach (var entity in _entityManager.Workspace)
        {
            entity.velocity.apply_velocity(entity); // Apply gravity or any other force
            entity.UpdateAnimation();


            entity.collisionBox.UpdateCollision(entity, World);



            // Example gravity, can be replaced with actual logic
            if (!entity.collisionBox.Bottom)
            {
                entity.velocity.velocity += new Vector2(0, 0.1f);
            }
            else
            {
                entity.velocity.velocity *= new Vector2(1, 0);
            }
        }


    }

    public void Input()
    {
        _userInterfaceManager.HoverAction(MousePosition, _actionManager);
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {

            _userInterfaceManager.ClickAction(MousePosition, _actionManager);
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
        player.velocity.velocity = new Vector2(0, 0);
        float zoomScale = 0.3f;
        var keyboardState = Keyboard.GetState();

        var keyboard = Keyboard.GetState().GetPressedKeys();

        foreach (var key in keyboard)
        {
            if (key == Keys.W)
            {
                plrVel += new Vector2(0, -1);
            }
            if (key == Keys.S)
            {
                plrVel += new Vector2(0, +1);
            }
            if (key == Keys.A)
            {
                plrVel += new Vector2(-1, 0);
            }
            if (key == Keys.D)
            {
                plrVel += new Vector2(+1, 0);
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


            //ent.Sprites[1].Orientation += 0.1f;
            Entities[0].Sprites[0].Orientation += 1f;
            Entities[0].Sprites[3].Orientation += 1.2f;
            Entities[0].Sprites[1].Orientation -= 2f;
            Entities[0].Sprites[5].Orientation += 1.2f;
            Entities[0].Sprites[4].Orientation -= 2f;

        }


        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (WorldMousePos.X > 0 && WorldMousePos.Y > 0)
            {



                int BlockX = (int)(WorldMousePos.X);
                int BlockY = (int)(WorldMousePos.Y);

                World[BlockY, BlockX] = 0; // Set to air
            }
        }

        if (Mouse.GetState().RightButton == ButtonState.Pressed)
        {
            int BlockX = (int)(WorldMousePos.X);
            int BlockY = (int)(WorldMousePos.Y);

            World[BlockY, BlockX] = 2; // Set to air
        }

    }



    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        foreach (var Mob in Entities)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(BlockTypes[2].Texture, BlockSize * Mob.position + Player.cam.position + (BlockSize) * Vector2.One / 2, null, Color.White, 0f, Vector2.Zero, BlockSize / BlockTypes[1].Texture.Width, SpriteEffects.None, 0f);

            _spriteBatch.End();
        }
        // 2 cycles to render both directions of the world
        Player.cam.RenderLayer(_blockManager, _spriteBatch, BackGround, 0f);
        Player.cam.RenderLayer(_blockManager, _spriteBatch, World, 1f);
        //Camera.RenderLayer(_blockManager, _spriteBatch, World, 2f);
        if (InventoryOpen)
        {
            var InventoryUI = Content.Load<Texture2D>("Sprite-0001");
            _spriteBatch.Begin();
            _spriteBatch.Draw(InventoryUI, new Vector2(250, 200), null, Color.White, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
            _spriteBatch.End();
        }
        base.Draw(gameTime);
        _entityManager.RenderAll(_spriteBatch, BlockSize, Player.cam.position);
        _userInterfaceManager.DrawUI(_spriteBatch);



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
