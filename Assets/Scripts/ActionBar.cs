using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using UnityEngine.AddressableAssets;

public class ActionBar : MonoBehaviour
{
    public List<Transform> containers;
    public event Action OnTripleMatch;
    public event Action OnTrayFull;

    private List<ShapeCombination> tray = new();
    private List<GameObject> icons = new();

    private const string ShapePrefabAddress = "Assets/Prefabs/BaseShape.prefab";

    public Vector3 GetWorldPositionFreeContainer()
    {
        return containers[GetFreeContainer()].position;
    }
    
    public async void AddPiece(ShapeCombination combo)
    {
        tray.Add(combo);

        if (tray.Count <= 7)
        {
            GameObject icon = await CreateIconFromCombination(combo, containers[GetFreeContainer()]);
            icons.Add(icon);
        }

        int count = 0;
        foreach (var c in tray)
        {
            if (c == combo) count++;
        }

        if (count == 3)
        {
            for (int i = icons.Count - 1; i >= 0 && count > 0; i--)
            {
                if (tray[i] == combo)
                {
                    Destroy(icons[i]);
                    icons.RemoveAt(i);
                    tray.RemoveAt(i);
                    
                    count--;
                }
            }

            OnTripleMatch?.Invoke();
        }

        if (tray.Count >= 7)
        {
            OnTrayFull?.Invoke();
        }
    }
    
    private int GetFreeContainer()
    {
        for (int i = 0; i < this.containers.Count; i++)
        {
            if (this.containers[i].transform.childCount == 0)
                return i;
        }

        return -1;
    }

    private async System.Threading.Tasks.Task<GameObject> CreateIconFromCombination(ShapeCombination combo, Transform parent)
    {
        var prefab = Addressables.LoadAssetAsync<GameObject>(ShapePrefabAddress).WaitForCompletion();
        
        GameObject icon = Instantiate(prefab, parent);

        var rb = icon.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        var col = icon.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        icon.transform.localScale = Vector3.one * 0.5f;
        icon.transform.DOScale(Vector3.one * 0.5f, 1f);
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localRotation = Quaternion.identity;

        var shape = icon.GetComponent<BaseShape>();
        
        if (shape != null)
        {
            shape.Init(combo.Shape, combo.Color, combo.Animal);
        }

        return icon;
    }

    public void ResetBar()
    {
        foreach (var icon in icons)
        {
            if (icon != null) Destroy(icon);
        }

        tray.Clear();
        icons.Clear();
    }
}
