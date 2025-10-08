public class CraftingRecipe 
{
  public Itemframe item = null;
  public Itemframe[,] RecipeGrid = new Itemframe[3,3];

  public bool Typebased = false;

  public Itemframe Craft(Itemframe[,] Input,)
  {
    for(int i = 0;i < Input.GetLength(0);i++)
    {
      for(int j = 0;i < Input.Getlength(1);j++)
      {
        if(RecipeGrid[i,j].Item == Input[i,j].Item)
        {
          foreach (var item in Input)
          {
            item.Amount -= 1;
          }
          return this.item;
        }
      }
    }
    return null;
    
  }
}
