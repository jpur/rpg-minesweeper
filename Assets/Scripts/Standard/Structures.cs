using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IVector2 {
    public int X, Y;

    public IVector2(int x, int y) {
        X = x;
        Y = y;
    }

    public static int Distance(IVector2 a, IVector2 b) {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    public static IVector2 operator +(IVector2 a, IVector2 b) {
        return new IVector2(a.X + b.X, a.Y + b.Y);
    }

    public static IVector2 operator -(IVector2 a, IVector2 b) {
        return new IVector2(a.X - b.X, a.Y - b.Y);
    }

    public static bool operator ==(IVector2 a, IVector2 b) {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(IVector2 a, IVector2 b) {
        return a.X != b.X || a.Y != b.Y;
    }

    public bool Equals(IVector2 other) {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        return obj is IVector2 && Equals((IVector2)obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (X * 397) ^ Y;
        }
    }

    public override string ToString() {
        return string.Format("{0}, {1}", X, Y);
    }
}

