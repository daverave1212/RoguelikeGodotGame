using Godot;
using Godot.Collections;

public class TileGeneration : TileMap
{
    private readonly Dictionary<string, int> tileNameToID = new Dictionary<string, int>();
    private OccluderPolygon2D occluder;

    public override void _Ready()
    {
        occluder = GetChild<LightOccluder2D>(0).Occluder;
        MapTileNameToID();

        SetTiles();
    }
    private void SetTiles()
    {
        for(int y = 0; y < 4; y++)
            for(int x = 0; x < 4; x++)
                SetTile("Tree1", 10 + x, 5 + y, true);
    }

    private void SetTile(string tileName, int x, int y, bool castShadow)
    {
        SetCell(x, y, tileNameToID[tileName]);

        if(castShadow)
            SetTileShadowPolygon(x, y, 4, 4, 28, 4);
    }
    private void MapTileNameToID()
    {
        var tileIDs = TileSet.GetTilesIds();
        for(int i = 0; i < tileIDs.Count; i++)
        {
            var tileID = (int)tileIDs[i];
            var tileName = TileSet.TileGetName(tileID);
            tileNameToID[tileName] = tileID;
        }
    }
    private void SetTileShadowPolygon(int x, int y, float reduceLeft = 0f, float reduceRight = 0f, float reduceTop = 0f, float reduceBottom = 0f)
    {
        var worldPos = MapToWorld(new Vector2(x, y));
        var polygon = occluder.Polygon;
        System.Array.Resize(ref polygon, polygon.Length + 4);

        var sz = CellSize;
        var len = polygon.Length;

        // order is counter-clockwise, topLeft -> bottomLeft -> bottomRight -> topRight
        polygon[len - 4] = worldPos + new Vector2(reduceLeft, reduceTop);
        polygon[len - 3] = worldPos + new Vector2(reduceLeft, sz.y);
        polygon[len - 2] = worldPos + new Vector2(sz.x - reduceRight, sz.y - reduceBottom);
        polygon[len - 1] = worldPos + new Vector2(sz.x - reduceRight, reduceTop);

        occluder.Polygon = polygon;
    }
}
