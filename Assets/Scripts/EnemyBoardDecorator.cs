using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyBoardDecorator : MonoBehaviour, IBoardDecorator {
    //public TextAsset EnemyDataAsset;
    public Enemy EnemyPrefab;
    public EnemyData[] EnemyData;

    public int[] Variation;

    private readonly Dictionary<int, List<EnemyData>> _sortedData = new Dictionary<int, List<EnemyData>>(); 

    void Awake() {
        foreach (EnemyData enemyData in EnemyData) {
            List<EnemyData> list;
            if (!_sortedData.TryGetValue(enemyData.Level - 1, out list)) {
                list = new List<EnemyData>();
                _sortedData.Add(enemyData.Level - 1, list);
            }
            list.Add(enemyData);
        }
    }

    public Stack<int> GetLevelsToPlace() {
        Stack<int> levels = new Stack<int>();
        int leftoverExp = 0;
        for (int i = 0; i < Variation.Length; i++) {
            int lvl = i + 1;
            int amountToLevel = Mathf.CeilToInt((float)(Player.Instance.CalculateExpForLevel(lvl + 1) - leftoverExp)/ Player.Instance.CalculateExpFromKill(lvl));
            int amountToPlace = amountToLevel + (Random.Range(0, Variation[i]) * GetDifficultyVariation());
            leftoverExp = (amountToPlace * Player.Instance.CalculateExpFromKill(lvl)) - Player.Instance.CalculateExpForLevel(lvl + 1);
            for (int j = 0; j < amountToPlace; j++) {
                levels.Push(i);
            }
        }

        return levels;
    }

    public void Decorate(Board board) {
        Stack<int> placements = GetLevelsToPlace();

        List<Tile> tiles = board.GetTiles().ToList();
        while (placements.Count > 0 && tiles.Count > 0) {
            Tile tile = tiles[Random.Range(0, tiles.Count)];
            tiles.Remove(tile);

            int level = placements.Pop();

            EnemyData data = _sortedData[level][Random.Range(0, _sortedData[level].Count)];
            Enemy enemy = CreateEnemy(data);
            tile.Occupant = enemy;
        }
    }

    Enemy CreateEnemy(EnemyData data) {
        Enemy enemy = (Enemy)Instantiate(EnemyPrefab);
        enemy.SetData(data);
        return enemy;
    }

    public int GetDifficultyVariation() {
        return Game.Difficulty * 2;
    }
}
