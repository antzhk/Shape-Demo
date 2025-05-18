using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine.AddressableAssets;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private float spawnAreaWidth = 8f;  
    [SerializeField] private float spawnHeight = 10f;
    
    public List<GameObject> allPieces = new List<GameObject>();

    public bool IsSpawning { get; private set; }
    
    private GameObject _piecePrefab;

    private const string PiecePrefab = "Assets/Prefabs/BaseShape.prefab";

    public void Awake()
    {
        this._piecePrefab = Addressables.LoadAssetAsync<GameObject>(PiecePrefab).WaitForCompletion();
    }
    
    public int SpawnPieces(int typesCount)
    {
        return Spawn(typesCount);
    }

    public void ClearAllPieces()
    {
        foreach (var obj in allPieces)
        {
            if (obj != null)
                Destroy(obj);
        }
        
        allPieces.Clear();
    }

    private int Spawn(int typesToUse)
    {
        var comboSet = new List<ShapeCombination>();

        for (int i = 0; i < typesToUse; i++) 
        {
            comboSet.Add(GetRandomCombination());
        }
        
        List<ShapeCombination> spawnList = new List<ShapeCombination>();
        
        foreach (ShapeCombination combo in comboSet) 
        {
            spawnList.Add(combo);
            spawnList.Add(combo);
            spawnList.Add(combo);
        }
        
        for (int i = 0; i < spawnList.Count; i++)
        {
            ShapeCombination temp = spawnList[i];
            int j = Random.Range(i, spawnList.Count);
            spawnList[i] = spawnList[j];
            spawnList[j] = temp;
        }

        IsSpawning = true;
        StartCoroutine(SpawnSequence(spawnList));
        
        return spawnList.Count; 
    }

    private ShapeCombination GetRandomCombination()
    {
        ShapeType randShape = (ShapeType) Random.Range(0, System.Enum.GetValues(typeof(ShapeType)).Length);
        ColorType randColor = (ColorType) Random.Range(0, System.Enum.GetValues(typeof(ColorType)).Length);
        AnimalType randAnimal = (AnimalType) Random.Range(0, System.Enum.GetValues(typeof(AnimalType)).Length);

        return new ShapeCombination(randShape, randColor, randAnimal);
    }


    private IEnumerator SpawnSequence(List<ShapeCombination> combos) 
    {
        foreach (ShapeCombination combo in combos) 
        {
            Vector2 position = GetRandomSpawnPosition();

            GameObject pieceObj = Instantiate(_piecePrefab, position, Quaternion.identity);

            pieceObj.transform.localScale = Vector3.one * 0.5f;
            
            allPieces.Add(pieceObj);
            
            BaseShape shape = pieceObj.GetComponent<BaseShape>();
            
            if (shape != null) 
            {
                shape.Init(combo.Shape, combo.Color, combo.Animal);
            }

            yield return new WaitForSeconds(0.05f);  
        }

        IsSpawning = false;
    }


    private Vector2 GetRandomSpawnPosition() 
    {
        float halfWidth = spawnAreaWidth / 2f;
        float x = Random.Range(-halfWidth, halfWidth);
        float y = spawnHeight;
        return new Vector2(x, y);
    }
}
