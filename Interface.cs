class UIButtons
{
public Vector2 Position = Vector2(0,0);
public Vector2 Scale = Vector2(0,0);

Public Texture2D Background;

public string Name = "Button";
public string Text = "TextButton";

public void IsInBounds( Vector2 Pos)
{

if (Pos.X >= Position.X && Pos.X <= Position.X+Scale.X)
if (Pos.Y > position.Y && Pos.Y <= Position.Y + Scale.Y)
{
return true;
//Place Design shit here
}
}


