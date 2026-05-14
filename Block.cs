using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MinecraftAlpha
{


    public class Block
    {
        public Texture2D[] Variants;

        public int ID = 0;
        public float Health = 30;
        public string Tag = "";
        public bool Animated = false;
        public string Name { get; set; }
        public string Description { get; set; }
        public Block ItemDrop = null;
        public bool Transparent = false;
        public int DefaultState = 0; // Default state of the block
        public string Data = ""; // Additional data for the block

        //Tool stuff
        public float Durrability = -1; // How much damage the block can take before breaking
        public int ItemID = 0; // Id ot the item in atlas
        public bool Placable = true;
        public float Light_Emission = 0f; // How much light the block emits

        public Color Color = Color.White;
        public Texture2D Texture { get; set; }
        public bool HasVariants = false;
        public string TexturePath { get; set; }

        public bool Item = false;
        public int MineLevel = 0; // How fast the block can be mined
        public float Damage = 2; // How fast the block can be mined
        public float Grip = 0f;

        public bool consumable = true;
        public Block ammo = null;
        public string ammoTag = "";
        public Block Smelt = null;
        //Constants for use types
        public float ChargeMax = 0f; // Time taken to charge the item
        public int TickUpdate = 10;
        public bool IgnoreUpdate = false; // If true, the block will not update every tick and only on UpdateChain
        public bool ConstantUpdate = false;

        public bool Solid = true;
        public float CooldownMax = 0f;//cooldown after using the item
        public float UseTimeMax = 0f; // Time taken to use/interact with the block
        //Dynamic use variables
        public float Charge = 0f;
        public bool CanFire = false;
        public float Cooldown = 0f;
        public float UseTime = 0f;

        public Action<TileGrid, Entity, Block> Interaction = null;
        public Action<TileGrid, string> Update;
        public Action<TileGrid, Entity> OnCollide;
    }

    public class BlockManager
    {
        public Game1 Game;
        public float BlockSize = 32;
        public List<Block> Blocks = new List<Block>();
        public BlockManager(Game1 game1)
        {
            Game = game1;
            Blocks = LoadBlocks();
        }
        public List<int[,]> World; // List of different layers of the world
        public static List<Block> LoadBlocks()
        {
            //Create Blocks here
            var list = new List<Block>()
            {

                new Block { Name = "Air", TexturePath = "air" ,Health = 1000, TickUpdate = 3,Solid = false},
                new Block { Name = "Dirt", TexturePath = "dirt",Health = 30, Tag="Dirt"},
                new Block { Name = "Grass", TexturePath = "grass_block_side",Health = 30,Tag="Dirt" },
                new Block { Name = "Cobblestone", TexturePath = "cobblestone", Health = 100,Tag="Stone"},
                new Block { Name = "Stone", TexturePath = "stone" ,Health = 100,Tag="Stone"},

                new Block { Name = "Wood", TexturePath = "oak_planks" ,Health = 60,Tag="Wood Fuel"},
                new Block { Name = "Log", TexturePath = "oak_log", Health = 60,Tag="Wood Fuel"},
                new Block { Name = "Leaves", TexturePath = "oak_leaves", Health = 13,Color = Color.SeaGreen,Transparent = true,Tag="Leaves Fuel"},
                new Block { Name = "Glass Block", TexturePath = "glass", Health = 4,Transparent = true,Tag="Glass"},
                new Block { Name = "Netherrack", TexturePath = "netherrack" ,Health = 90,Tag="Stone Fuel"},
                new Block { Name = "EndsStone", TexturePath = "end_stone" ,Health = 90,Tag="Stone"},
                new Block { Name = "Obsidian", TexturePath = "obsidian" ,Health = 290,Tag="Stone",MineLevel = 4},

                new Block { Name = "Coal Ore", TexturePath = "coal_ore" ,Health = 100,Tag="Stone Ore",MineLevel = 1},
                new Block { Name = "Iron Ore", TexturePath = "iron_ore" ,Health = 100,Tag="Stone Ore",MineLevel = 2},
                new Block { Name = "Gold Ore", TexturePath = "gold_ore" ,Health = 100,Tag="Stone Ore",MineLevel = 3},
                new Block { Name = "Diamond Ore", TexturePath = "diamond_ore" ,Health = 100,Tag="Stone Ore",MineLevel = 3},
                new Block { Name = "Torch", TexturePath = "torch",Color = Color.LightGoldenrodYellow,Light_Emission = 7f},
                new Block { Name = "Portal", TexturePath = "Animated/nether_portal",Health = 30000,Animated = true,TickUpdate = 5,Transparent = true,Solid = false,ConstantUpdate = true},

                new Block { Name = "Lava", TexturePath = "Animated/lava" ,Animated = true,Health = 100,Data = "5",TickUpdate = 76,ConstantUpdate = true,Solid = false,Tag = "Liquid"},
                new Block { Name = "Water", TexturePath = "Animated/WaterIdle" ,Animated = true,Health = 100,Data = "7",TickUpdate = 30,ConstantUpdate = true,Solid = false,Tag = "Liquid"},
                new Block { Name = "Gravel", TexturePath = "gravel" ,Health = 30,Tag = "Gravity Dirt"},

                new Block { Name = "Fire", TexturePath = "Animated/fire_1" ,Health = 10,Animated = true,Transparent = true,TickUpdate = 5,Solid = false,ConstantUpdate = true},
                new Block { Name = "Sand", TexturePath = "sand" ,Health = 30,Tag = "Gravity Dirt"},
                new Block { Name = "Chest", TexturePath = "ChestTesting" ,Interaction = null,Transparent = true,IgnoreUpdate = true, Tag="Wood Fuel"},
                new Block { Name = "Crafting Table", TexturePath = "crafting_table_front" ,Health = 60, Interaction = null,Tag="Wood Fuel"},
                new Block { Name = "Furnace", TexturePath = "furnace_front" ,Health = 100, Interaction = null,ConstantUpdate = true,Tag="Stone",HasVariants = true},

                new Block { Name = "TNT", TexturePath = "tnt_side", Health = 2,Transparent = false,Tag="Explosive"},
                new Block { Name = "Apple", TexturePath = "_item", Item = true,Placable = false,ItemID = 0,UseTimeMax = 3},

                new Block { Name = "Stick", TexturePath = "_item", Item = true,Placable = false,ItemID = 199,Tag = "Fuel"},
                new Block { Name = "Coal", TexturePath = "_item", Item = true,Placable = false,ItemID = 81,Tag = "Fuel"},
                new Block { Name = "Iron", TexturePath = "_item", Item = true,Placable = false,ItemID = 31},
                new Block { Name = "Gold", TexturePath = "_item", Item = true,Placable = false,ItemID = 61},
                new Block { Name = "Diamond", TexturePath = "_item", Item = true,Placable = false,ItemID = 85},

                new Block { Name = "Wooden Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 210,Damage = 0.5f,Tag="Pickaxe Fuel",MineLevel = 1},
                new Block { Name = "Stone Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 202,Damage = 0.6f,Tag="Pickaxe",MineLevel = 2},
                new Block { Name = "Iron Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 63,Damage = 0.7f,Tag="Pickaxe",MineLevel = 3},
                new Block { Name = "Gold Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 114,Damage = 0.8f,Tag="Pickaxe",MineLevel = 3},
                new Block { Name = "Diamond Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 101,Damage = 0.9f,Tag="Pickaxe",MineLevel = 4},

                new Block { Name = "Wooden Sword", TexturePath = "_item", Item = true,Placable = false,ItemID = 212,Damage = 2f,Tag="Sword Fuel",MineLevel = 0},
                new Block { Name = "Stone Sword", TexturePath = "_item", Item = true,Placable = false,ItemID = 204,Damage = 3f,Tag="Sword",MineLevel = 0},
                new Block { Name = "Iron Sword", TexturePath = "_item", Item = true,Placable = false,ItemID = 95,Damage = 4f,Tag="Sword",MineLevel = 0},
                new Block { Name = "Gold Sword", TexturePath = "_item", Item = true,Placable = false,ItemID = 14,Damage = 5f,Tag="Sword",MineLevel = 0},
                new Block { Name = "Diamond Sword", TexturePath = "_item", Item = true,Placable = false,ItemID = 103,Damage = 6f,Tag="Sword",MineLevel = 0},

                new Block { Name = "Bow", TexturePath = "_item", Item = true,Placable = false,ItemID = 36,Damage = 3f,Tag="Bow",Grip = 270f,ChargeMax = 4},
                new Block { Name = "Flint and Steel", TexturePath = "_item" ,ItemID= 28,Item = true,Placable = false},


            };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].ID = i;
            }



            return list;
        }
        public bool HasTag(string s, string t)
        {
            char Key = ' ';
            var Tags = s.Split(Key);
            foreach (string a in Tags)
            {
                if (a == t) return true;
            }
            return false;
        }
        public Block getBlock(int ID)
        {
            return Blocks[ID];
        }
        public Block getBlock(string Name)
        {
            var blocks = Blocks;
            return blocks.Find(x => x.Name == Name);
        }
        public Block getBlock(TileGrid tile)
        {
            if (tile == null) return Blocks[0];
            return Blocks[tile.ID];
        }
        public static Block getBlocks(string Name)
        {
            var blocks = LoadBlocks();
            return blocks.Find(x => x.Name == Name);
        }

        public bool Portal(TileGrid Pos)
        {



            //Find Margins By sending Lines in all directions,
            var pos = Pos.pos; //
            var Sides = LogicsClass.Sides;
            var Points = new TileGrid[6];
            bool Valid = true;
            for (int c = 0; c < 6; c++)
            {


                var s = Sides[c];
                TileGrid Hit = null;
                int Range = 1;
                while (Hit == null && Range <= 10)
                {

                    var t = Game._blockManager.GetTile(Pos.pos + new Vector3(0.5f, 0.5f, 0) + s * Range);

                    if (t != null)
                    {


                        if (t.ID == 0 && t.ID != getBlock("Fire").ID)
                        {
                            Range++;
                        }
                        else
                        {
                            Hit = t;
                            //t.ID = 9;
                            Points[c] = t;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }

            }


            //Reults are used to make edges, Up, down, left, right, front ,back

            //complicated, yes, its making the edges of the frame

            var up = Points[0];
            var down = Points[1];
            var left = Points[2];
            var right = Points[3];
            var front = Points[4];
            var back = Points[5];
            var Center = Pos;


            if ((up == null || down == null) ||
            ((left == null || right == null)/* || (front == null || back == null))*/))
            {
                Valid = false;
                return false;
            }
            bool Horizontal = (left != null && right != null);

            int xneg = 0;
            int yneg = 0;
            if ((Pos.pos).X < 0)
            {
                xneg = -1;
            }
            if ((Pos.pos).Y < 0)
            {
                yneg = -1;
            }
            //if (Horizontal)
            {
                var A = Combiner(left, up, Center);
                var B = Combiner(right, down, Center);

                int Lx = (int)(B.pos.X - A.pos.X);
                int Ly = (int)(A.pos.Y - B.pos.Y);
                //Check Frame 
                for (int i = 0; i <= Lx; i++)
                {
                    for (int j = 0; j <= Ly; j++)
                    {
                        var tile = Game._blockManager.GetTile(new Vector3(A.pos.X + i - xneg, B.pos.Y + j - yneg, A.pos.Z) + new Vector3(0.5f, 0.5f, 0));
                        //tile.ID = 4;
                        if (j == 0 || j == Ly || i == 0 || i == Lx)
                        {


                            if (Game._blockManager.getBlock(tile).Name != "Obsidian")
                            {

                                Valid = false;
                                //return false;
                            }
                            //tile.ID = 9;
                        }
                    }
                }


                //for Hallow loop to check for frame
                for (int i = 1; i < Lx; i++)
                {
                    for (int j = 1; j < Ly; j++)
                    {
                        var tile = Game._blockManager.GetTile(new Vector3(A.pos.X + i - xneg, B.pos.Y + j - yneg, A.pos.Z) + new Vector3(0.5f, 0.5f, 0));
                        if (tile.ID != 0 && Game._blockManager.getBlock(tile).Name != "Fire")
                        {

                            Valid = false;
                            return false;
                        }
                    }
                }





                //For loop for Fill
                for (int i = 1; i < Lx; i++)
                {
                    for (int j = 1; j < Ly; j++)
                    {
                        var tile = Game._blockManager.GetTile(new Vector3(A.pos.X + i - xneg, B.pos.Y + j - yneg, A.pos.Z) + new Vector3(0.5f, 0.5f, 0));
                        if (tile != null)
                        {
                            Game._blockManager.SetTile(tile, "Portal", "");
                            //tile.Color = Color.OrangeRed;
                        }
                    }
                }


            }














            return Valid;
        }
        public Vector3 Combiner(Vector3 X, Vector3 Y, Vector3 Z)
        {
            return new Vector3(X.X, Y.Y, Z.Z);
        }
        public TileGrid Combiner(TileGrid X, TileGrid Y, TileGrid Z)
        {
            return Game._blockManager.GetTile(new Vector3(X.pos.X, Y.pos.Y, Z.pos.Z));
        }
        public void LoadActions()
        {
            //getBlock("Flint and Steel").Interaction = (Pos, ent, item) =>
            //{
            getBlock("Iron Ore").Smelt = getBlock("Iron");
            getBlock("Gold Ore").Smelt = getBlock("Gold");





            getBlock("Air").Update = (Pos, data) =>
            {
                return;
                var pos = GetPosAtBlock(Pos); //
                var Sides = LogicsClass.SidesPosPlus(pos, Game);

                for (int i = 0; i < 27; i++)
                {
                    TileGrid tile = Sides[i];
                    if (tile == null) continue;

                    tile.brightness += Pos.brightness - 0.1f;
                    tile.updateLight = true;
                }
            };
            //};

            //getBlock("Torch").Update = (Pos, data) =>
            //{
            //    return;
            //    var pos = GetPosAtBlock(Pos); //
            //    var Sides = LogicsClass.SidesPos(pos, Game);

            //    for (int i = 0; i < 6; i++)
            //    {
            //        TileGrid tile = Sides[i];
            //        if (tile == null) continue;

            //        tile.brightness = 3f;
            //        tile.updateLight = true;
            //        //tile.MarkedForUpdate = true;
            //    }
            //};

            getBlock("Fire").Update = (Pos, data) =>
            {
                if (Portal(Pos)) { return; }

                return;


                var pos = GetPosAtBlock(Pos); //
                var Sides = LogicsClass.SidesPosPlus(pos, Game);
                bool Extinguish = true;
                for (int i = 0; i < 27; i++)
                {
                    TileGrid tile = Sides[i];
                    if (tile == null) continue;

                    if (GetBlockAtTile(tile).Tag == "Wood" || GetBlockAtTile(tile).Tag == "Planks" || GetBlockAtTile(tile).Tag == "Fuel")
                    {
                        if (tile.ID != 0)
                        {
                            Extinguish = false;
                        }
                        if (tile.MinedHealth > (GetBlockAtTile(tile).Health))
                        {
                            Game._actionManager.SetTile(tile, "Fire", "0");
                        }
                        else tile.MinedHealth += 1f;
                    }
                    if (GetBlockAtTile(tile).Name == "TNT")
                    {
                        var TNT = GetPosAtBlock(tile);
                        GetBlockAtTile(tile).Interaction(Pos, null, getBlock("Flint and Steel"));
                    }
                }
                if (Extinguish)
                {
                    Game._actionManager.SetTile(Pos, "Air");
                }


            };
            getBlock("Portal").Update = (Pos, data) =>
            {

                PerlinNoise NOISE = new(1);
                float b = Game.TimeSinceStart / 20f;
                float a = 2;
                float h = (float.Abs(MathF.Sin((Game.TimeSinceStart + (Pos.pos.X + Pos.pos.Y) * 40) / 455f)));
                float p = NOISE.Noise((Pos.pos.X + MathF.Sin(MathF.Sin(b + Pos.pos.Y))) * 3f * a * 0.05f, (Pos.pos.Y + Pos.pos.X + b) * a * 0.05f) / 2;
                Pos.Color = LogicsClass.HSL(p + MathF.Sin(b / 30), 1f, 0.5f) * (p + 0.6f);
                //Pos.Color = new Color(1, 1-p, 1);
            };
            getBlock("Portal").OnCollide = (Pos, data) =>
            {
                if (data.name != "Player") { return; }
                Game.Player.dimension = Game.Player.dimension == 0 ? 1 : 0;
                Game.Player.Plr.IFrame = 6f;

                Game.Chunks.Clear();

            };

            getBlock("Water").Update = (Pos, data) =>
            {
                var pos = GetPosAtBlock(Pos);
                //Game._blockManager.UpdateSurounding(Pos);

                if (data == "") data = "7";
                int Data = int.Parse(data);
                //Pos.MarkedForUpdate = true;
                var lower = GetBlockAtPos(new Vector2(pos.X, pos.Y + 1), (int)pos.Z, Game.Chunks);
                var top = GetBlockAtPos(new Vector2(pos.X, pos.Y - 1), (int)pos.Z, Game.Chunks);
                var Left = GetBlockAtPos(new Vector2(pos.X - 1, pos.Y), (int)pos.Z, Game.Chunks);
                var Right = GetBlockAtPos(new Vector2(pos.X + 1, pos.Y), (int)pos.Z, Game.Chunks);
                if (lower == null || top == null || Right == null || Left == null) return;

                var sides = LogicsClass.SidesPos(pos, Game);

                foreach (var side in sides)
                {
                    if (side == null) continue;
                    if (getBlock(side).Name == "Lava")
                    {
                        if (data == "7")
                        {
                            Game._actionManager.SetTile(Pos, "Stone", "");
                            return;
                        }
                        else
                        {
                            Game._actionManager.SetTile(Pos, "Cobblestone", ""); return;
                        }
                    }


                }



                //if (Data == 7)
                //{
                //    if (lower.ID == 0)
                //    {
                //        Game._actionManager.SetTile(lower, "Water", "6");
                //        //lower.MarkedForUpdate = false;

                //    }
                //}
                //if (Data < 7)
                //{
                //    if (top.ID != getBlock("Water").ID && !top.MarkedForUpdate)
                //    {
                //        Game._actionManager.SetTile(Pos, "Air", "");

                //    }
                //    else if (lower.ID == 0)
                //    {

                //        Game._actionManager.SetTile(lower, "Water", "6");


                //    }

                //}


                if (lower.ID == 0)
                {
                    Game._actionManager.SetTile(lower, "Water", "6");
                    //return;
                    lower.MarkedForUpdate = true;
                }






                if (lower.ID != 0 && lower.ID != getBlock("Water").ID && Data > 1)
                {

                    if (Left.ID == 0)
                    {
                        Game._actionManager.SetTile(Left, "Water", (Data - 1).ToString()); Left.MarkedForUpdate = true;
                    }
                    if (Right.ID == 0)
                    {
                        Game._actionManager.SetTile(Right, "Water", (Data - 1).ToString()); Right.MarkedForUpdate = true;
                    }

                }

                if (Data == 6)
                {
                    if (top.ID != getBlock("Water").ID)
                    {
                        Game._actionManager.SetTile(Pos, "Air", "");
                        lower.MarkedForUpdate = true;
                    }


                }
                if (Data < 6)
                {
                    if (Right.ID == getBlock("Water").ID && Left.ID == getBlock("Water").ID)
                    {
                        if (int.Parse(Right.Data) <= Data && int.Parse(Left.Data) <= Data)
                        {
                            Game._actionManager.SetTile(Pos, "Air", "");

                        }
                    }
                    else
                    {
                        Game._actionManager.SetTile(Pos, "Air", "");
                        ;
                    }


                }







            };

            getBlock("Lava").Update = (Pos, data) =>
            {
                var pos = GetPosAtBlock(Pos);
                //Game._blockManager.UpdateSurounding(Pos);

                if (data == "") data = "5";

                var sides = LogicsClass.SidesPos(pos, Game);

                foreach (var side in sides)
                {
                    if (side == null) continue;
                    if (getBlock(side).Name == "Water")
                    {
                        if (data == "5")
                        {
                            Game._actionManager.SetTile(Pos, "Obsidian", ""); return;
                        }
                        else
                        {
                            Game._actionManager.SetTile(Pos, "Cobblestone", ""); return;
                        }
                    }


                }

                int Data = int.Parse(data);
                //Pos.MarkedForUpdate = true;
                var lower = GetBlockAtPos(new Vector2(pos.X, pos.Y + 1), (int)pos.Z, Game.Chunks);
                var top = GetBlockAtPos(new Vector2(pos.X, pos.Y - 1), (int)pos.Z, Game.Chunks);
                var Left = GetBlockAtPos(new Vector2(pos.X - 1, pos.Y), (int)pos.Z, Game.Chunks);
                var Right = GetBlockAtPos(new Vector2(pos.X + 1, pos.Y), (int)pos.Z, Game.Chunks);
                if (lower == null || top == null || Right == null || Left == null) return;





                //if (Data == 7)
                //{
                //    if (lower.ID == 0)
                //    {
                //        Game._actionManager.SetTile(lower, "Water", "6");
                //        //lower.MarkedForUpdate = false;

                //    }
                //}
                //if (Data < 7)
                //{
                //    if (top.ID != getBlock("Water").ID && !top.MarkedForUpdate)
                //    {
                //        Game._actionManager.SetTile(Pos, "Air", "");

                //    }
                //    else if (lower.ID == 0)
                //    {

                //        Game._actionManager.SetTile(lower, "Water", "6");


                //    }

                //}


                if (lower.ID == 0)
                {
                    Game._actionManager.SetTile(lower, "Lava", "6");
                    //return;
                    lower.MarkedForUpdate = true;
                }






                if (lower.ID != 0 && lower.ID != getBlock("Lava").ID && Data > 1)
                {

                    if (Left.ID == 0)
                    {
                        Game._actionManager.SetTile(Left, "Lava", (Data - 1).ToString()); Left.MarkedForUpdate = true;
                    }
                    if (Right.ID == 0)
                    {
                        Game._actionManager.SetTile(Right, "Lava", (Data - 1).ToString()); Right.MarkedForUpdate = true;
                    }

                }

                if (Data == 6)
                {
                    if (top.ID != getBlock("Lava").ID)
                    {
                        Game._actionManager.SetTile(Pos, "Air", "");
                        lower.MarkedForUpdate = true;
                    }


                }
                if (Data < 5)
                {
                    if (Right.ID == getBlock("Lava").ID && Left.ID == getBlock("Lava").ID)
                    {
                        if (int.Parse(Right.Data) <= Data && int.Parse(Left.Data) <= Data)
                        {
                            Game._actionManager.SetTile(Pos, "Air", "");

                        }
                    }
                    else
                    {
                        Game._actionManager.SetTile(Pos, "Air", "");
                        ;
                    }


                }







            };


            getBlock("TNT").Interaction = (Pos, ent, item) =>
            {
                if (item == getBlock("Flint and Steel"))
                {
                    var pos = GetPosAtBlock(Pos);
                    var tnt = Game._entityManager.GravityBlock(Pos, true);
                    tnt.Health = 100;
                    tnt.name = "Tnt";
                    



                }
            };
            getBlock("Bow").Interaction = (Pos, ent, item) =>
            {

                int[] sprites = { 36, 51, 4, 20 };
                if (!item.CanFire && item.ChargeMax < item.Charge)
                {
                    if (item.Charge > 6)
                    {
                        return;
                    }
                    int I = (int)item.Charge - 2;
                    item.Charge += 0.03f;
                    Game.Player.DisplayID = sprites[I];//make this change itemslot isntead



                    return;
                }
                Entity Arrow = new Entity(-3, "Arrow", "_projectile", 1) { Texture = Game.Content.Load<Texture2D>("Projectiles/Arrow"), };
                Arrow.collisionBox = new CollisionBox() { Size = new Vector2(0.5f) };
                Arrow.position = Game.Player.Plr.position;
                Arrow.Mass = 0.4f;
                Arrow.velocity.velocity = Vector2.Normalize(Game.WorldMousePos - Game.Player.Plr.position) * item.Charge * item.Charge / 2;
                Arrow.Target = ent;
                Game._entityManager.Workspace.Add(Arrow);
                Game.Player.DisplayID = -1;
                item.Charge = 0f;
                item.CanFire = false;

            };

            getBlock("Apple").Interaction = (Pos, ent, item) =>
            {
                // Heal Player
                Game.Player.Plr.Health += 3;
                // Consume the apple

                if (Game.Player.Plr.Health > Game.Player.Plr.MaxHealth)
                {
                    Game.Player.Plr.Health = Game.Player.Plr.MaxHealth;
                }

            };

            getBlock("Stone").ItemDrop = getBlock("Cobblestone");
            getBlock("Leaves").ItemDrop = getBlock("Air");
            getBlock("Grass").ItemDrop = getBlock("Dirt");
            getBlock("Portal").ItemDrop = getBlock("Air");
            getBlock("Coal Ore").ItemDrop = getBlock("Coal");
            getBlock("Diamond Ore").ItemDrop = getBlock("Diamond");

            //getBlock("Furnace").Variants = [Game.Content.Load<Texture2D>("furnace_front_on")];


            getBlock("Sand").Update = (Pos, data) =>
            {
                //Pos.ID = 2;
                //Game._blockManager.UpdateSurounding(Pos);

                var pos = GetPosAtBlock(Pos);
                //Debuging.DebugPosWOrld(Game._spriteBatch,new Vector2(pos.X,pos.Y), Game);

                var lower = GetBlockAtPos(new Vector2(pos.X, pos.Y + 1), (int)pos.Z, Game.Chunks);
                if (lower == null) return;
                Debuging.DebugPosWOrld(Game._spriteBatch, new Vector2(pos.X, pos.Y + 1), Game);

                if (lower.ID == 0)
                    Game._entityManager.GravityBlock(new Vector2(pos.X, pos.Y), (int)pos.Z, true);
                //var top = Game._blockManager.GetTile(Pos.pos + new Vector3(0, -1, 0));
                //if (HasTag(Game._blockManager.getBlock(top).Tag, "Gravity"))
                //{
                //    getBlock("Sand").Update.Invoke(top, top.Data);
                //}




            };
            getBlock("Gravel").Update = getBlock("Sand").Update;
            getBlock("Chest").Interaction = (Pos, user, Item) =>
            {

                string Items = Pos.Data;
                var Window = Game._userInterfaceManager.GetWindow("Chest");
                var Inv = Game._userInterfaceManager.GetWindow("Inventory");

                Inv.Visible = true;
                Window.Visible = true;
                foreach (var slot in Window.ItemSlots)
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
                foreach (var item in Items.Split(','))
                {
                    if (item == "")
                    { continue; }
                    var a = item.Split(':');
                    var Slot = Window.ItemSlots[int.Parse(a[0])];
                    Slot.Item = Blocks[int.Parse(a[1])];
                    Slot.Count = int.Parse(a[2]);

                }
            };
            getBlock("Chest").Update = (Pos, data) =>
            {

                var Window = Game._userInterfaceManager.GetWindow("Chest");
                string Data = "";

                var block = Pos;


                foreach (var slot in Window.ItemSlots)
                {
                    if (slot.Item != null)
                    {
                        Data += $"{slot.ID}:{GetBlockID(slot.Item)}:{slot.Count},";
                    }
                }
                block.Data = Data;


            };
            getBlock("Crafting Table").Interaction = (Pos, user, item) =>
            {

                var Craft = Game._userInterfaceManager.GetWindow("Crafting3x3");
                var Inv = Game._userInterfaceManager.GetWindow("Inventory");
                Game._userInterfaceManager.GetWindow("Crafting2x2").Visible = false;
                Craft.Visible = true;
                Inv.Visible = true;


            };
            getBlock("Furnace").Update = (Pos, data) =>
            {
                //Pos.Counter = new(7);
                int FuelSources = 0;
                float fuelNext = 0;
                int Outputs;

                bool Ignited = false;
                if (Pos.Counter.Length > 7 && Pos.Counter[0] > 0)
                {

                    Pos.Counter[0] -= 1f;
                    Pos.Counter[1] -= 0.01f;


                    Ignited = true;
                }

                if (Ignited)
                {
                    Pos.Color = Color.Red;
                }
                else
                {
                    Pos.Color = Color.Cyan;
                }


                //Power, Fuel,Delay, FuelC,Current, CurrentC, Done, DoneC
                //Thread.Sleep(100);

            };
            getBlock("Furnace").Interaction = (Pos, user, Item) =>
            {
                var F = Game._userInterfaceManager.GetWindow("Furnace");
                var Inv = Game._userInterfaceManager.GetWindow("Inventory");
                //Load Items



                F.Visible = true;
                Inv.Visible = true;
            };

            //getBlock("Crafting Table").Update = (Pos) =>
            //{

            //    var Window = Game._userInterfaceManager.windows[4];
            //    var ItemSlots = Window.ItemSlots;
            //    foreach (CraftingRecipe Recipe in Game._RecipeManager.Recipes)
            //    {

            //        if (Recipe.RecipeGrid.GetLength(0) == 3)
            //        {
            //            if (Recipe.CheckRecipe(new ItemSlot[,]
            //        {
            //                { ItemSlots[0], ItemSlots[1],ItemSlots[2] },
            //                { ItemSlots[3], ItemSlots[4],ItemSlots[5] },
            //                { ItemSlots[6], ItemSlots[7],ItemSlots[8] },
            //        }))
            //            {
            //                ItemSlots[9].Item = Recipe.item.Item;
            //                ItemSlots[9].Count = Recipe.item.Count;
            //                break;

            //            }
            //            else
            //            {
            //                ItemSlots[9].Count = 0;
            //                ItemSlots[9].Item = null;
            //            }
            //        }
            //        else
            //        {

            //        }
            //        //if (ItemSlots[4].Item != null) break;


            //    }


            //};


        }

        public int GetBlockID(Block block)
        {
            return Blocks.IndexOf(block);
        }
        public Block GetBlockByName(string name)
        {
            var block = Blocks.Find(x => x.Name.ToLower() == name.ToLower());
            if (block == null)
                return GetBlockByName("Air");
            else
                return block;
        }

        public ItemSlot[,] RandomiseLoot()
        {
            Random random = new Random();
            ItemSlot[,] Chest = new ItemSlot[3, 9];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (random.Next(0, 100) < 20) // 20% chance to spawn an item
                    {
                        int itemId = random.Next(1, 6); // Assuming item IDs range from 1 to 5
                        int amount = random.Next(1, 65); // Random amount between 1 and 64
                        Chest[i, j] = new ItemSlot { Item = Blocks[itemId], Count = amount };
                    }
                }
            }
            return Chest;
        }








        public static TileGrid GetBlockAtPos(Vector2 pos, List<Chunk> Chunks)
        {
            return GetBlockAtPos(pos, 9, Chunks);
        }

        public static Vector3 GetPosAtBlock(TileGrid tile)
        {
            if (tile != null)
            {
                var pos = tile.pos + new Vector3(0.5f, 0.5f, 0.5f);

                return pos;
            }

            return Vector3.Zero;// Return zero vector if tile not found

        }
        public Block GetBlockAtTile(TileGrid tile)
        {
            if (tile == null) return Blocks[0];


            return Blocks[tile.ID];

        }


        public static TileGrid GetBlockAtPos(Vector2 pos, int z, List<Chunk> Chunks)
        {
            if (z < 0 || z > 9) return null;
            TileGrid Tile = null;
            if (Chunks.Count == 0) return null;

            int size = Chunks[0].Tiles.GetLength(1);
            int ChunkX = (int)Math.Ceiling((pos.X / size));
            int ChunkY = (int)Math.Ceiling((pos.Y / size));





            foreach (Chunk C in Chunks)
            {
                if (C.x == ChunkX && C.y == ChunkY)
                {
                    int x = (int)(pos.X % size);
                    int y = (int)(pos.Y % size);

                    if (pos.X < 0)
                    {
                        x = size - 1 - Math.Abs(x);
                    }
                    if (pos.Y < 0)
                    {
                        y = size - 1 - Math.Abs(y);
                    }

                    Tile = C.Tiles[z, y, x];
                }

            }


            return Tile;
        }



        public static TileGrid GetLastBlockAtPos(Vector2 pos, List<Chunk> Chunks)
        {

            TileGrid Tile = GetBlockAtPos(pos, Chunks);
            if (Tile == null) return null;
            for (int i = 0; i < 10; i++)
            {

                if (GetBlockAtPos(pos, i, Chunks).ID != 0)
                {
                    Tile = GetBlockAtPos(pos, i, Chunks);
                }
            }

            return Tile;
        }





        //Get Functions
        //Info
        //public Vector2 TilePos(TileGrid Tile)
        //{


        //    int size = 32;
        //    int ChunkX = (int)Math.Ceiling((Tile.pos.X / size));
        //    int ChunkY = (int)Math.Ceiling((Tile.pos.Y / size));

        //    return new(ChunkX, ChunkY);


        //}
        public void UpdateSurounding(TileGrid Tile)
        {
            //return;
            var pos = GetPosAtBlock(Tile);
            var Sides = LogicsClass.SidesPos(pos, Game);
            if (getBlock(Tile).Update != null)
            {
                getBlock(Tile).Update.Invoke(Tile, Tile.Data);
            }
            for (int i = 0; i < Sides.Length; i++)
            {
                TileGrid tile = Sides[i];
                if (tile == null) continue;
                if (getBlock(tile).Update != null)
                {
                    getBlock(tile).Update.Invoke(tile, tile.Data);
                    //UpdateSurounding(tile);
                    //Change(tile);
                }
                //tile.MarkedForUpdate = true;
                //tile.Color = Color.Red;

            }

        }
        public Vector2 TilePos(TileGrid Tile, Chunk chunk)
        {
            return new Vector2(Tile.pos.X % 32, Tile.pos.Y % 32);
        }



        ////object
        //public Block GetBlock(TileGrid tile)
        //{
        //    if (tile == null) return Blocks[0];


        //    return Blocks[tile.ID];
        //}
        //public Block GetBlock(string Name)
        //{
        //    var blocks = Blocks;
        //    return blocks.Find(x => x.Name == Name);
        //}
        //public int GetBlock(Block block)
        //{
        //    return Blocks.IndexOf(block);
        //}
        //public Block GetBlock(int ID)
        //{
        //    return Blocks[ID];
        //}

        //public TileGrid GetTile(Vector3 pos, Chunk chunk) // 0 -32
        //{

        //    int x = (int)(pos.X % 32);
        //    int y = (int)(pos.Y % 32);
        //    int z = (int)pos.Z;
        //    return chunk.Tiles[z, y, x];


        //}

        public TileGrid GetTile(Vector3 pos)
        {
            Vector2 Pos = new Vector2(pos.X, pos.Y);
            int z = (int)pos.Z;
            var Chunks = Game.Chunks;
            if (z < 0 || z > 9) return null;
            TileGrid Tile = null;
            if (Chunks.Count == 0) return null;
            int size = Chunks[0].Tiles.GetLength(1);
            int ChunkX = (int)Math.Ceiling((pos.X / size));
            int ChunkY = (int)Math.Ceiling((pos.Y / size));
            var C = GetChunk(Pos);
            int x = (int)(pos.X % size);
            int y = (int)(pos.Y % size);

            if (pos.X < 0)
            {
                x = size - 1 - Math.Abs(x);
            }
            if (pos.Y < 0)
            {
                y = size - 1 - Math.Abs(y);
            }

            Tile = C.Tiles[z, y, x];



            return Tile;
        }

        public Chunk GetChunk(TileGrid Tile)
        {
            return Tile.Parent;
        }
        public bool GetChunk(Vector2 pos, int non) //World Pos
        {
            int ChunkX = (int)Math.Ceiling((pos.X / 32));
            int ChunkY = (int)Math.Ceiling((pos.Y / 32));
            foreach (Chunk C in Game.Chunks)
            {
                if (C.x == ChunkX && C.y == ChunkY)
                {
                    return true;


                }

            }


            return false; //make exception to make chunk instead.
        }
        public Chunk GetChunk(Vector2 pos) //World Pos
        {
            int ChunkX = (int)Math.Ceiling((pos.X / 32));
            int ChunkY = (int)Math.Ceiling((pos.Y / 32));
            foreach (Chunk C in Game.Chunks)
            {
                if (C.x == ChunkX && C.y == ChunkY)
                {
                    return C;


                }

            }

            var c = new Chunk(ChunkX, ChunkY);
            Game.Chunks.Add(c);



            return c; //make exception to make chunk instead.
        }
        public Chunk GetChunk(int x, int y) //this is for given cordinates
        {

            int ChunkX = x;
            int ChunkY = y;
            foreach (Chunk C in Game.Chunks)
            {
                if (C.x == ChunkX && C.y == ChunkY)
                {
                    return C;


                }

            }

            var c = new Chunk(ChunkX, ChunkY);



            return c; //make exception to make chunk instead.
        }

        //public Vector2 ChunkPos(Vector2 pos)
        //{
        //    int size = 32;
        //    int ChunkX = (int)Math.Ceiling((pos.X / size));
        //    int ChunkY = (int)Math.Ceiling((pos.Y / size));

        //    return new Vector2(ChunkX, ChunkY);
        //}


        ////Set Functions
        //public void SpawnChunk(Vector2 Pos)
        //{
        //    if (GetChunk(Pos) != null) return;
        //    int size = 32;
        //    int ChunkX = (int)Math.Ceiling((Pos.X / size));
        //    int ChunkY = (int)Math.Ceiling((Pos.Y / size));
        //    Chunk c = new Chunk(ChunkX, ChunkY);
        //    Game.Chunks.Add(c);
        //}
        //public void SpawnChunk(TileGrid Tile) //UnNecessary?
        //{

        //}

        public void Change(TileGrid Tile)
        {
            EvaluateChange(Tile);
            if (Tile != null && Tile.ID != 0)
            {
                var b = Game._blockManager.getBlock(Tile);
                if (b.ConstantUpdate) Game.UpdateStack.Add(Tile);
            }
            if (false) //When generation robust use this.
            {
                return;
            }
            var heightMap = HeightMap.GetMap(Game.HeightMaps, GetChunk(Tile).x);

            float Sky = heightMap.GetHeight(Tile);

            if (float.Ceiling(Tile.pos.Y) == float.Ceiling(Sky))
            {
                //Change to the tallest
                if (Game._blockManager.getBlock(Tile).Transparent || Tile.ID == 0)
                {

                }
            }
            else if (float.Ceiling(Tile.pos.Y) <= float.Ceiling(Sky))
            {
                //New tallest
            }



            var q = new Event()

            {
                tile = Tile,

            };
            Game._actionManager.QueChange(q);
            EvaluateChange(Tile);

        }

        public void EvaluateChange(TileGrid tile)
        {
            if (tile == null) return;
            if (!Game.GameProgress.Contains(tile)) // assuming it updates because its an object
            {
                tile.SaveFile = true;
                Game.GameProgress.Add(tile);
            }
        }
        public void SetTile(TileGrid Tile, int ID, string Data)
        {
            Tile.ID = ID;
            Tile.Data = Data;
            Change(Tile);
        }
        public void SetTile(TileGrid Tile, int ID, bool ignore)
        {
            Tile.ID = ID;
            //Tile.Data = Data;
            //Change(Tile);
        }

        public void SetTile(Vector3 pos, int ID, string Data)
        {
            var e = GetTile(pos);
            if (e != null)
            {
                e.ID = ID;
                e.Data = Data;
                //e.brightness = 1;
            }
            Change(e);

        }

        public void SetTile(Vector3 pos, string block, string Data)
        {

            var e = GetTile(pos);

            if (e != null)
            {
                e.ID = GetBlockByName(block).ID;
                e.Data = Data;
            }
            Change(e);


        }
        public TileGrid SetTile(TileGrid Tile, TileGrid other, bool ignore)
        {
            Tile = other;
            Tile.ID = other.ID;
            Tile.Data = other.Data;
            Tile.state = other.state;
            Tile.pos = other.pos;

            return Tile;
        }
        public TileGrid SetTile(TileGrid Tile, TileGrid other)
        {

            var a = SetTile(Tile, other, true);
            Change(a);
            return a;


        }
        public TileGrid SetTile(TileGrid Tile, TileGrid other, int DeepCopy)
        {
            Tile.state = other.state;
            Tile.ID = other.ID;
            Tile.brightness = other.brightness;
            //Tile.LightSource = other.LightSource;
            Tile.MinedHealth = other.MinedHealth;
            Tile.Data = other.Data;
            Tile.MarkedForUpdate = other.MarkedForUpdate;
            Tile.updateLight = other.updateLight;
            Tile.SkyLight = other.SkyLight;
            Change(Tile);
            return Tile;

        }
        public TileGrid SetTile(TileGrid Tile, string block, bool ignore)
        {

            Tile.ID = GetBlockByName(block).ID;
            Tile.Data = "";
            //Change(Tile);
            return Tile;
        }
        public TileGrid SetTile(TileGrid Tile, string block)
        {

            Tile.ID = GetBlockByName(block).ID;
            Tile.Data = "";
            Change(Tile);
            return Tile;
        }
        public TileGrid SetTile(TileGrid Tile, string block, string Data)
        {
            Tile.ID = GetBlockByName(block).ID;
            Tile.Data = Data;
            Change(Tile);
            return Tile;
        }
        public void SetTile(TileGrid Tile, Block block)
        {
            Tile.ID = block.ID;
            Tile.Data = "";
        }
        public void SetTile(TileGrid Tile, Block block, string Data)
        {
            Tile.ID = block.ID;
            Tile.Data = Data;
        }
    }

    public class TileGrid
    {
        public Vector3 pos;
        public TileGrid() { }
        public int ID = 0;

        //this is for the placed blocks to have a direction, limited to complex blocks like logs, chest, bed, 
        //for blocks with changing textures
        //for multi-tile blocks

        public int state = 0; //For direction : left, right, up, down, front, back 
        public bool SaveFile = false;

        public float MinedHealth = 0; // How much health has been mined from this block
        public bool MarkedForUpdate = false;
        public bool updateLight = false;
        public Color Color = Color.White;

        public int SkyLight = 0;
        public float brightness = 1;

        public string Data { get; set; } = string.Empty;
        public float[] Counter = new float[10]; //For Changing Values 


        [JsonIgnore] public Chunk Parent;

    }

    public class Items
    {
        public Texture2D Atlas;
        public int itemSize = 16; // Size of each item in the atlas


        public void DrawItem(SpriteBatch _spriteBatch, Vector2 position, int itemID, float scale)
        {
            int itemSize = this.itemSize;
            int itemsPerRow = Atlas.Width / itemSize;

            int row = itemID / itemsPerRow;
            int column = itemID % itemsPerRow;

            Rectangle sourceRectangle = new Rectangle(column * itemSize, row * itemSize, itemSize, itemSize);
            _spriteBatch.Draw(Atlas, position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }



        public Rectangle GetRactangle(int itemID)
        {
            int itemSize = this.itemSize;
            int itemsPerRow = Atlas.Width / itemSize;

            int row = itemID / itemsPerRow;
            int column = itemID % itemsPerRow;

            return new Rectangle(column * itemSize, row * itemSize, itemSize, itemSize);

        }

    }





}
