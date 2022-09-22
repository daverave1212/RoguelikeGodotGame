using Godot;
using Godot.Collections;

public class TileManager : Node
{
    private static readonly Dictionary<string, int> tileNameToID = new Dictionary<string, int>();

    private static TileMap tilemap;

    public override void _Ready()
    {
        tilemap = GetNode<TileMap>(new NodePath("/root/World/ground"));

        MapTileNameToID();
    }

    public static void SetTile(string tileName, int x, int y, int collectionX = 0, int collectionY = 0)
    {
        tilemap.SetCell(x, y, tileNameToID[tileName], autotileCoord: new Vector2(collectionX, collectionY));
    }

    private void MapTileNameToID()
    {
        var tileIDs = tilemap.TileSet.GetTilesIds();
        for(int i = 0; i < tileIDs.Count; i++)
        {
            var tileID = (int)tileIDs[i];
            var tileName = tilemap.TileSet.TileGetName(tileID);
            tileNameToID[tileName] = tileID;
        }
    }
}
