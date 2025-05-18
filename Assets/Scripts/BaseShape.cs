using System;
using EnumTypes;
using UnityEngine;

public class BaseShape : MonoBehaviour
{
    [SerializeField] private ShapesMap shapesMap;
    [SerializeField] private ColorMap colorMap;
    [SerializeField] private AnimalMap animalMap;

    public ShapeType Figure { get; private set; }
    public ColorType Color { get; private set; }
    public AnimalType Animal { get; private set; }

    public void Init(ShapeType shape, ColorType color, AnimalType animal)
    {
        foreach (var key in shapesMap.Keys)
        {
            shapesMap[key].SetActive(key == shape);
        }

        foreach (var key in animalMap.Keys)
        {
            animalMap[key].SetActive(key == animal);
        }

        shapesMap[shape].transform.GetChild(0).GetComponent<SpriteRenderer>().color = colorMap[color];

        this.Figure = shape;
        this.Color = color;
        this.Animal = animal;
    }

    public bool CanCombine(BaseShape baseShape)
    {
        return baseShape.Figure == this.Figure && baseShape.Color == this.Color && baseShape.Animal == this.Animal;
    }

    public void OnMouseDown()
    {
        GameManager.Instance.OnPieceClicked(this);
    }

    public bool CanTake()
    {
        return true;
    }
}

[Serializable]
public class ShapesMap : EnumDictionary<ShapeType, GameObject>{}

[Serializable]
public class ColorMap : EnumDictionary<ColorType, Color>{}

[Serializable]
public class AnimalMap : EnumDictionary<AnimalType, GameObject> {}
