using System;

namespace EnumTypes
{
    [Serializable]
    public struct ShapeCombination : IEquatable<ShapeCombination> {
        public ShapeType Shape { get; }
        public ColorType Color { get; }
        public AnimalType Animal { get; }

        public ShapeCombination(ShapeType shape, ColorType color, AnimalType animal) {
            Shape = shape;
            Color = color;
            Animal = animal;
        }
        
        public override bool Equals(object obj) {
            return obj is ShapeCombination other && Equals(other);
        }

        public bool Equals(ShapeCombination other) {
            return this.Shape == other.Shape 
                   && this.Color == other.Color 
                   && this.Animal == other.Animal;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Shape, Color, Animal);
        }

        public static bool operator ==(ShapeCombination a, ShapeCombination b) {
            return a.Equals(b);
        }

        public static bool operator !=(ShapeCombination a, ShapeCombination b) {
            return !a.Equals(b);
        }

        public override string ToString() {
            return $"{Shape}-{Color}-{Animal}";
        }
    }
}