using System.Collections.Generic;
using System.Text;

///<summary>
/// Class to Save the World Information
///</summary>
public class SaveGame
{
    public int LevelIndex;
    public List<PositionedTile> SeaTiles, BeachTiles, GrassTiles, MountainTiles, FarmableTiles, DecoTiles, UnitTiles;

    ///<summary>
    /// translates the Tile Data into String
    ///</summary>
    public string Serialize()
    {
        var builder = new StringBuilder();
        char[] tilemaplabels = { 's', 'b', 'g', 'm', 'f', 'd', 'u' };
        List<PositionedTile>[] tilemaps =
            { SeaTiles, BeachTiles, GrassTiles, MountainTiles, FarmableTiles, DecoTiles, UnitTiles };
        for (int i = 0; i < tilemaps.Length; i++)
        {
            builder.Append(tilemaplabels[i]);
            builder.Append("[");
            foreach (var tile in tilemaps[i])
            {
                builder.Append($"{(int)tile.Tile.Type}({tile.Position.x},{tile.Position.y})");
            }

            builder.Append("]");
        }

        return builder.ToString();
    }
}