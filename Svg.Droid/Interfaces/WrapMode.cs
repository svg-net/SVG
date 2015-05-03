namespace Svg
{
    // Summary:
    //     Specifies how a texture or gradient is tiled when it is smaller than the
    //     area being filled.
    public enum WrapMode
    {
        // Summary:
        //     Tiles the gradient or texture.
        Tile = 0,
        //
        // Summary:
        //     Reverses the texture or gradient horizontally and then tiles the texture
        //     or gradient.
        TileFlipX = 1,
        //
        // Summary:
        //     Reverses the texture or gradient vertically and then tiles the texture or
        //     gradient.
        TileFlipY = 2,
        //
        // Summary:
        //     Reverses the texture or gradient horizontally and vertically and then tiles
        //     the texture or gradient.
        TileFlipXY = 3,
        //
        // Summary:
        //     The texture or gradient is not tiled.
        Clamp = 4,
    }
}