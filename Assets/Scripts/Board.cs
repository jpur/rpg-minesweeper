using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    public static Board Instance {
        get { return _inst ?? (_inst = FindObjectOfType<Board>()); }
    }

    public int Width;
    public int Height;

    public Tile BaseTile;

    private static Board _inst;

    private RectTransform _trans;
    private Grid<Tile> _grid;

    private Vector3 _tileSize;
    private Vector3 _tileOffset;

    void Awake() {
        _inst = this;
        _trans = GetComponent<RectTransform>();
    }

    public void Populate() {
        _tileSize = new Vector2(_trans.rect.width / Width, _trans.rect.height / Height);
        _tileOffset = new Vector2(-_tileSize.x * (Width / 2f - 0.5f), -_tileSize.y * (Height / 2f - 0.5f));

        if (_grid != null) {
            foreach (Tile tile in _grid) {
                tile.Remove();
            }
        }
        _grid = new Grid<Tile>(Width, Height);

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                CreateTile(x, y);
            }
        }

        IBoardDecorator[] decorators = GetComponents<EnemyBoardDecorator>();
        foreach (IBoardDecorator decorator in decorators) {
            decorator.Decorate(this);
        }
    }
    
    Tile CreateTile(int x, int y) {
        Tile tile = (Tile)Instantiate(BaseTile);
        tile.transform.position = Vector3.Scale(new Vector3(x, y, 0), _tileSize) + _tileOffset;
        tile.SetSize(_tileSize);
        tile.transform.SetParent(transform, false);
        tile.Position = new IVector2(x, y);
        _grid.Set(x, y, tile);
        return tile;
    }

    public Tile[] GetTiles() {
        return _grid.Cells;
    }

    public Tile[] GetNeighbors(Tile tile) {
        List<Tile> neighbors = new List<Tile>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (Mathf.Abs(x) + Mathf.Abs(y) == 0) continue;
                Tile neighbor = _grid.Get(tile.Position.X + x, tile.Position.Y + y);
                if (neighbor == null) continue;
                neighbors.Add(neighbor);
            }
        }
        return neighbors.ToArray();
    }
}
