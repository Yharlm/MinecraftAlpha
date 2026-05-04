using System.Collections.Generic;
using MinecraftAlpha;

public class RecipeManager
{
    public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();
    public List<FurnaceRecipe> smelting = new List<FurnaceRecipe>();
    public static int GetStartIndex(ItemSlot[,] grid)
    {
        foreach (var slot in grid)
        {
            if (slot.Item != null)
            {
                return slot.Item.ID;
            }
        }
        return -1;
    }
    public List<CraftingRecipe> LoadRecipes(BlockManager blocksManager)
    {
        int log = blocksManager.GetBlockByName("Log").ID;
        int stick = blocksManager.GetBlockByName("Stick").ID;
        int wood = blocksManager.GetBlockByName("Wood").ID;
        int Cstone = blocksManager.GetBlockByName("Cobblestone").ID;
        int iron = blocksManager.GetBlockByName("Iron").ID;
        int gold = blocksManager.GetBlockByName("Gold").ID;
        int Diamond = blocksManager.GetBlockByName("Diamond").ID;
        int CraftT = blocksManager.GetBlockByName("Crafting Table").ID;
        var List = new List<CraftingRecipe>()

        {
            new CraftingRecipe(new int[,] {
                { log, 0 },
                { 0, 0 }} ,wood,4,blocksManager),
            new CraftingRecipe(new int[,] {
                { wood, 0 },
                { wood, 0 }} ,stick,4,blocksManager),
            new CraftingRecipe(new int[,] {
                { wood, wood },
                { wood, wood }} ,CraftT,1,blocksManager),
            new CraftingRecipe(new int[,] {
                { wood, wood,wood },
                { 0, stick,0  },
                { 0, stick,0  }} ,blocksManager.GetBlockByName("Wooden Pickaxe").ID,1,blocksManager),
            new CraftingRecipe(new int[,] {
                { Cstone, Cstone,Cstone },
                { 0, stick,0  },
                { 0, stick,0  }} ,blocksManager.GetBlockByName("Stone Pickaxe").ID,1,blocksManager),

            new CraftingRecipe(new int[,] {
                { iron, iron,iron },
                { 0, stick,0  },
                { 0, stick,0  }} ,blocksManager.GetBlockByName("Iron Pickaxe").ID,1,blocksManager),

            new CraftingRecipe(new int[,] {
                { gold, gold,gold },
                { 0, stick,0  },
                { 0, stick,0  }} ,blocksManager.GetBlockByName("Gold Pickaxe").ID,1,blocksManager),


        };




        return List;
    }
    public List<FurnaceRecipe> LoadFurnace(BlockManager blocksManager)
    {



        var List = new List<FurnaceRecipe>()
        {
            new(blocksManager.getBlock("Iron Ore"), blocksManager.getBlock("Iron"), 1),
            new(blocksManager.getBlock("Iron Ore"), blocksManager.getBlock("Iron"), 1)

        };



        return List;

    }
    
}
public class FurnaceRecipe
{

    public float luck = 1;
    public int Input = 0;
    public int Output = 0;
    public FurnaceRecipe(Block I, Block O, float luck)
    {
        Input = I.ID;
        Output = O.ID;
        this.luck = luck;
    }
    public Block Confirm(ItemSlot input, ItemSlot Output,Game1 game)
    {

        int a = input.Item.ID;
        foreach(var recipe in game._RecipeManager.smelting)
        {
            if(recipe.Input == a)
            {
                if(recipe.Output == Output.Item.ID)
                {
                    
                    return game._blockManager.getBlock(this.Output);
                }
            }
        }

        return null;
    }

}

public class CraftingRecipe
{
    public ItemSlot item = null;
    public ItemSlot[,] RecipeGrid;

    public bool Typebased = false;

    public CraftingRecipe(int[,] Grid, int Result, int count, BlockManager manager)
    {

        RecipeGrid = new ItemSlot[Grid.GetLength(0), Grid.GetLength(1)];

        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                if (Grid[x, y] == 0)
                {
                    RecipeGrid[x, y] = new ItemSlot() { Item = null };
                    continue;
                }

                RecipeGrid[x, y] = new ItemSlot() { Item = manager.Blocks[Grid[x, y]] };

            }
        }
        item = new ItemSlot() { Item = manager.Blocks[Result], Count = count };
    }




    public bool CheckRecipe(ItemSlot[,] Grid)
    {
        bool confirm = true;
        for (int i = 0; i < RecipeGrid.GetLength(0); i++)
        {
            for (int j = 0; j < RecipeGrid.GetLength(1); j++)
            {
                if (RecipeGrid[j, i].Item != null && RecipeGrid[j, i].Item.Name == "Log")
                {

                }
                if (RecipeGrid[j, i].Item != Grid[i, j].Item)
                {
                    confirm = false;
                    //break;
                }
                else
                {

                }

            }
        }
        return confirm;
    }
}
