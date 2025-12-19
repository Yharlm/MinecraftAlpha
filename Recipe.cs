using MinecraftAlpha;
using System.Collections.Generic;

public class RecipeManager
{
    public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();
    
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
                { 0, stick,0  }} ,stick,1,blocksManager),

        };




        return List;
    }


}

public class CraftingRecipe
{
    public ItemSlot item = null;
    public ItemSlot[,] RecipeGrid;
   
    public bool Typebased = false;

    public CraftingRecipe(int[,] Grid, int Result, int count,BlockManager manager)
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

                if (RecipeGrid[i, j].Item != Grid[i, j].Item)
                {
                    confirm = false;
                    //break;
                }

            }
        }
        return confirm;
    }
}
