using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class node
{
    public bool active  = true;
    public bool visible = true;

    // Your universal methods
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch sprite_batch);
}