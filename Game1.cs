using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Vector2 = System.Numerics.Vector2;

namespace MinecraftAlpha;

public class Cammera
{
    public Vector2 position = new Vector2(30, -500);
    public Vector2 size { get; set; } = new Vector2(800, 600);

}

public class Game1 : Game
{
    public Sprite steve;
    public List<Button> ButtonList = new List<Button>();
    public List<Entity> Entities = new List<Entity>();
    public Vector2 WorldMousePos = Vector2.Zero;
    public Vector2 MousePosition = Vector2.Zero;
    public bool InventoryOpen = false;
    public float BlockSize = 16f;

    
    static public int WorldSizeX = 300;
    static public int WorldSizeY = 300;
    public int[,] BackGround { get; set; } = new int[WorldSizeX, WorldSizeY];
    public int[,] Foreground { get; set; } = new int[WorldSizeX, WorldSizeY];
    public int[,] World      { get; set; } = new int[WorldSizeX, WorldSizeY];
    public List<Block> Blocks { get; set; } = new List<Block>();
    public Cammera Camera = new Cammera();

    public List<Block> BlockTypes { get; set; } = new List<Block>
    {
        new Block { Name = "Air", TexturePath = "air" },
        new Block { Name = "Dirt", TexturePath = "dirt" },
        new Block { Name = "Grass", TexturePath = "grass_block_side" },
        new Block { Name = "Stone", TexturePath = "stone" },
        new Block { Name = "Wood", TexturePath = "oak_planks" },
    };
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        ButtonList = Button.LoadButtons()
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    
    protected override void Initialize()
    {

        Random random = new Random();
        // TODO: Add your initialization logic here
       
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
        steve = new Sprite();

        base.Initialize();
    }
    

    Entity player = new Entity
    {
        name = "Player",
        position = new Vector2(10, 10),
        velocity = new Velocity(),
        collisionBox = new CollisionBox()
    };
    protected override void LoadContent()
    {
        
        Entities.Add(player);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        foreach (var block in BlockTypes)
        {
            block.Texture = Content.Load<Texture2D>(block.TexturePath);
        }
        steve.texture = (Content.Load<Texture2D>("Steve"));

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Input();
        Camera.position = -player.position * BlockSize + new Vector2(400,202);
        base.Update(gameTime);
        MousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
        WorldMousePos = (MousePosition - Camera.position) / BlockSize;

        foreach (var entity in Entities)
        {
            entity.velocity.apply_velocity(entity); // Apply gravity or any other force

            

            if (entity.position.X < 0 || entity.position.X >= World.GetLength(1) || entity.position.Y < 0 || entity.position.Y >= World.GetLength(0))
            {
                continue; // Skip if the entity is out of bounds
            }
            entity.collisionBox = new CollisionBox(); // Reset collision box for each update
            //World[(int)(entity.position.Y), (int)(entity.position.X)] = 1;
            if (World[(int)(entity.position.Y + 1.1f), (int)(entity.position.X+ 0.5f) ] != 0)
            {
                entity.collisionBox.Bottom = true;
            }
            if (World[(int)(entity.position.Y -0.1f), (int)(entity.position.X + 0.5f)] != 0)
            {
                entity.collisionBox.Top = true;
                 
            }
            if (World[(int)(entity.position.Y+0.5f), (int)(entity.position.X + 1.1f)] != 0)
            {
                entity.collisionBox.Right = true;
            }
            if (World[(int)(entity.position.Y + 0.5f), (int)(entity.position.X - 0.1f)] != 0)
            {
                entity.collisionBox.Left = true;
            }
            //World[(int)(entity.position.Y), (int)(entity.position.X) - 1] = 1;
            //World[(int)(entity.position.Y), (int)(entity.position.X) + 1] = 1; // Clear previous position

            //World[(int)(entity.position.Y) - 1, (int)(entity.position.X)] = 1; // Clear previous position


            // Example gravity, can be replaced with actual logic
            
            // Example movement, can be replaced with actual logic
        }
    }

    public void Input()
    {
        
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
                plrVel = new Vector2(0, +1);
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
        
        Vector2 screenCenter = Camera.size / 2f;
        player.velocity.velocity = plrVel * 0.1f; // Adjust speed as needed
        if (keyboardState.IsKeyDown(Keys.OemPlus))
        {

            float oldBlockSize = BlockSize;


            Vector2 worldPos = (screenCenter - Camera.position) / oldBlockSize;
            BlockSize = oldBlockSize + zoomScale;

            Camera.position = screenCenter - worldPos * (oldBlockSize + zoomScale);
        }
        if (keyboardState.IsKeyDown(Keys.OemMinus))
        {
            // Zoom out
            float oldBlockSize = BlockSize;

            Vector2 worldPos = (screenCenter - Camera.position) / oldBlockSize;
            BlockSize = oldBlockSize - zoomScale;
            Camera.position = screenCenter - worldPos * (oldBlockSize - zoomScale);
        }
        if (keyboardState.IsKeyDown(Keys.E))
        {
            InventoryOpen = !InventoryOpen;
        }
        

        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            foreach(var b in ButtonsList)
            {
                if (b.CheckPos(GetMousePos))
                {
                    TriggerEvent(b.Event);
                    break;
                    
                }
            
            }
            int BlockX = (int)(WorldMousePos.X);
            int BlockY = (int)(WorldMousePos.Y);

            World[BlockY, BlockX] = 0; // Set to air
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

        // 2 cycles to render both directions of the world

        

        for (int i = 0; i < BackGround.GetLength(0); i++)
        {
            for (int j = 0; j < BackGround.GetLength(1); j++)
            {
                if (BackGround[i, j] != 0)
                {
                    var block = BlockTypes[BackGround[i, j]];
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, null, Color.LightGray, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                    _spriteBatch.End();
                }
            }
        }

        for (int i = 0; i < World.GetLength(0); i++)
        {
            for (int j = 0; j < World.GetLength(1); j++)
            {
                if (World[i, j] != 0)
                {
                    var block = BlockTypes[World[i, j]];
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, null, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                    _spriteBatch.End();
                }
            }
        }
        foreach (var Mob in Entities)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(BlockTypes[2].Texture, BlockSize * Mob.position + Camera.position, null, Color.White, 0f, Vector2.Zero, BlockSize / BlockTypes[1].Texture.Width, SpriteEffects.None, 0f);

            _spriteBatch.End();

            steve.DrawSprite(1, _spriteBatch, BlockSize * Mob.position + Camera.position + new Vector2(), BlockSize / BlockTypes[1].Texture.Width);
        }
        for (int i = 0; i < Foreground.GetLength(0); i++)
        {
            for (int j = 0; j < Foreground.GetLength(1); j++)
            {
                if (Foreground[i, j] != 0)
                {
                    var block = BlockTypes[Foreground[i, j]];
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, null, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                    _spriteBatch.End();
                }
            }
        }

        if (InventoryOpen)
        {
            var InventoryUI = Content.Load<Texture2D>("Sprite-0001");
            _spriteBatch.Begin();
            _spriteBatch.Draw(InventoryUI, new Vector2(250, 200), null, Color.White, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
            _spriteBatch.End();
        }
        base.Draw(gameTime);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(Content.Load<SpriteFont>("File"), $"World Mouse Position: {WorldMousePos}", new Vector2(10, 10), Color.White);
        _spriteBatch.End();
        
        

    }


    // UI:
    // - SingleClick get stack
    // - DoubleClick get Pull all from container
    // - right click take one individual
    // hold right click drag to sepearte item for each hovered cell
    // Q drop item
    // Q hold drop simulatinously
    // drag out of UI to drop stack or Shift Q
}
