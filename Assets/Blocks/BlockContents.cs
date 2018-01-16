using UnityEngine;

public class BlockContents {
    string _contentsType;   // Name of the contents (ie, "Dirt", "Rock", "Dirt with Iron Ore In It")
    float _contentsHealth;  // Health of contents (ie how hard it is to break the block)
    Color _contentsColor;   // Color the block should be.  This could be a texture in the future.

    public BlockContents (string type, float startingHealth, Color color)
    {
        _contentsType = type;
        _contentsHealth = startingHealth;
        _contentsColor = color;
    }

    public BlockContents(BlockContents prototype)
    {
        _contentsColor = prototype._contentsColor;
        _contentsHealth = prototype._contentsHealth;
        _contentsType = prototype._contentsType;
    }

    public Color color {
        get { return _contentsColor; }
    }

    public float Health
    {
        get { return _contentsHealth; }
    }

    public void ReduceHealth (float amountToReduceHealthBy)
    {
        if (_contentsHealth != int.MaxValue)
        {
            _contentsHealth -= amountToReduceHealthBy;
            if (_contentsHealth < 0)
                _contentsHealth = 0;
        }
    }
}
