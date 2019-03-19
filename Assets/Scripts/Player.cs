using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : Entity {
    public static Player Instance {
        get { return _inst ?? (_inst = FindObjectOfType<Player>()); }
    }

    private static Player _inst;

    public event System.Action ExpChanged;
    public event System.Action LeveledUp;

    public int Experience {
        get { return _exp; }
        set {
            _exp = value;
            if (ExpChanged != null) {
                ExpChanged();
            }
        }
    }

    private const int BasePlayerExp = 60;
    private const float PlayerExpFactor = 2.53f;

    private const int BaseKillExp = 60;
    private const float KillExpFactor = 1.82f;

    private int _revealCount;
    private int _exp;

    public int ExperienceRemaining {
        get {
            return ExperienceToLevel - Experience;
        }
    }
    public int ExperienceToLevel {
        get {
            return CalculateExpForLevel(Level + 1);
        }
    }

    public PlayerHealth Health;

    void Awake() {
        _inst = this;
        Reset();
    }

    public void Reset() {
        Level = 1;
        Experience = 0;
        Health.CurrentHealth = Health.MaxHealth;
        _revealCount = 0;
    }

    public int GetNearby(Tile tile) {
        Tile[] neighbors = Board.Instance.GetNeighbors(tile);
        int nearbyEnemyLevelSum = 0;
        foreach (Tile neighbor in neighbors) {
            if (neighbor.IsOccupied) {
                nearbyEnemyLevelSum += neighbor.Occupant.Level;
            }
        }
        return nearbyEnemyLevelSum;
    }

    public void RecursiveRevealEmptyTile(Tile tile, bool firstTile) {
        if (tile.IsOccupied || (tile.IsRevealed && !firstTile)) return;
        if (!firstTile) tile.Reveal(false);

        int nearbyEnemyLevelSum = GetNearby(tile);
        if (nearbyEnemyLevelSum != 0) tile.SetNearbyCount(nearbyEnemyLevelSum);
        else {
            foreach (Tile neighbor in Board.Instance.GetNeighbors(tile)) {
                RecursiveRevealEmptyTile(neighbor, false);
            }
        }
    }

    public void OnReveal(Tile tile, bool playerClicked) {
        if (playerClicked) {
            if (tile.IsOccupied && _revealCount > 0) {
                Enemy enemy = tile.Occupant;
                enemy.OnReveal(true);

                int levelDiff = Mathf.Max(0, enemy.Level - Level);
                int damage = CalculateDamage(levelDiff);
                Health.TakeDamage(damage);

                if (Health.CurrentHealth > 0) {
                    int exp = CalculateExpFromKill(enemy.Level);
                    AddExp(exp);
                }
            } else {
                if (tile.IsOccupied) {
                    Destroy(tile.Occupant.gameObject);
                    tile.Occupant = null;
                }
                RecursiveRevealEmptyTile(tile, true);
            }
        } else {
            Enemy enemy = tile.Occupant;
            if (enemy != null) enemy.OnReveal(playerClicked);
            int nearby = GetNearby(tile);
            if (nearby != 0) tile.SetNearbyCount(nearby);
        }
        _revealCount++;
        if (_revealCount == Board.Instance.GetTiles().Length) {
            Messenger.SendMessage(Message.AllTilesRevealed);
        }
    }

    public void AddExp(int exp) {
        Experience += exp;
        if (Experience >= CalculateExpForLevel(Level + 1)) {
            int overflow = Experience - CalculateExpForLevel(Level + 1);
            LevelUp();
            AddExp(overflow);
        }
    }

    public void LevelUp() {
        Level++;
        Experience = 0;

        if (LeveledUp != null) {
            LeveledUp();
        }
    }

    public int CalculateExpFromKill(int killedLevel) {
        return (int)(BaseKillExp * (Mathf.Pow(killedLevel, KillExpFactor)));
    }

    public int CalculateExpForLevel(int level) {
        return level < 2 ? 0 : (int)(BasePlayerExp * (Mathf.Pow(level, PlayerExpFactor)));
    }

    int CalculateDamage(int levelDiff) {
        return levelDiff == 0 ? 0 : 2 + (int)Mathf.Pow(levelDiff, 2);
    }
}
