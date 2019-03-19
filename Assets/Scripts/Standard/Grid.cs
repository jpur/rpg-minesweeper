using UnityEngine;
using System.Collections;

public class Grid<T> : IEnumerable {
    public int Width { get; private set; }
    public int Height { get; private set; }

    public readonly T[] Cells;

    public Grid(int width, int height) {
        Width = width;
        Height = height;

        Cells = new T[width * height];
    }

    public T Get(int x, int y) {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return default(T);
        int index = GetIndex(x, y);
        return index < 0 || index >= Cells.Length ? default(T) : Cells[index];
    }

    public T Get(IVector2 iv2) {
        return Get(iv2.X, iv2.Y);
    }

    public void Set(int x, int y, T value) {
        int index = GetIndex(x, y);
        if (index >= 0 && index < Cells.Length) {
            Cells[index] = value;
        }
    }

    public void Set(IVector2 iv2, T value) {
        Set(iv2.X, iv2.Y, value);
    }

    private int GetIndex(int x, int y) {
        return y * Width + x;
    }

    public IEnumerator GetEnumerator() {
        foreach (T tile in Cells) {
            yield return tile;
        }
    }
}
