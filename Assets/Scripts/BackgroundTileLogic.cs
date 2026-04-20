using UnityEngine;

public class BackgroundTileLogic
{
    readonly float _parallaxFactor;
    readonly float _tileWidth;
    readonly int _tileCount;

    public BackgroundTileLogic(float parallaxFactor, float tileWidth, int tileCount)
    {
        _parallaxFactor = parallaxFactor;
        _tileWidth = tileWidth;
        _tileCount = tileCount;
    }

    public static int TileColorIndex(float tileLeftEdgeX, float tileWidth)
    {
        return Mathf.Abs(Mathf.RoundToInt(tileLeftEdgeX / tileWidth)) % 2;
    }

    public float[] GetTilePositions(float cameraX)
    {
        float phase = ((cameraX * _parallaxFactor % _tileWidth) + _tileWidth) % _tileWidth;
        float totalWidth = _tileCount * _tileWidth;
        float startX = Mathf.Floor((cameraX - totalWidth * 0.5f) / _tileWidth) * _tileWidth + phase;

        float[] positions = new float[_tileCount];
        for (int i = 0; i < _tileCount; i++)
            positions[i] = startX + i * _tileWidth;
        return positions;
    }
}
