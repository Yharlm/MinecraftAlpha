using MinecraftAlpha;
using System.Collections.Generic;

public class RecipeManager
{
    public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();
    

    public List<CraftingRecipe> LoadRecipes(BlockManager blocksManager)
    {
        var List = new List<CraftingRecipe>()

        {
            new CraftingRecipe(new int[,] {
                { 4, 4 },
                { 4, 4 }} ,6,4,blocksManager),
            new CraftingRecipe(new int[,] {
                { 2, 0 },
                { 0, 0 }} ,3,1,blocksManager),
            new CraftingRecipe(new int[,] {
                { 0, 0 },
                { 0, 0 }} ,0,1, blocksManager),
        };




        return List;
    }


}

public class CraftingRecipe
{
    public ItemSlot item = null;
    public ItemSlot[,] RecipeGrid = new ItemSlot[2, 2];

    public bool Typebased = false;

    public CraftingRecipe(int[,] Grid, int Result, int count,BlockManager manager)
    {
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
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
                }

            }
        }
        return confirm;
    }
}
