using MinecraftAlpha;

public class CraftingRecipe
{
    public ItemSlot item = null;
    public ItemSlot[,] RecipeGrid = new ItemSlot[3, 3];

    public bool Typebased = false;

    public ItemSlot Craft(ItemSlot[,] Input)
    {
        for (int i = 0; i < Input.GetLength(0); i++)
        {
            for (int j = 0; i < Input.GetLength(1); j++)
            {
                if (RecipeGrid[i, j].Item == Input[i, j].Item)
                {
                    foreach (var item in Input)
                    {
                        item.Count -= 1;
                    }
                    return this.item;
                }
            }
        }
        return null;

    }
}
