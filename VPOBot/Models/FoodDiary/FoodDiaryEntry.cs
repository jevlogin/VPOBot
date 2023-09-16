using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class FoodDiaryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public double? Weight { get; set; }
        public int MealTimeHours { get; set; }
        public int MealTimeMinutes { get; set; }
        public MealType MealType { get; set; }
        public string DishName { get; set; }
        public EatPurposeType? EatPurpose { get; set; }
        public double? WaterAmount { get; set; }
        public double? ProteinAmount { get; set; }
        public double? FatAmount { get; set; }
        public double? CarbohydrateAmount { get; set; }
        public double Cost { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            if (Date != null)
            {
                stringBuilder.Append("Дата: ");
                stringBuilder.Append(Date.ToString("dd MMMM yyyy года", CultureInfo.GetCultureInfo("ru-RU")));
            }


            if (Weight != null)
            {
                stringBuilder.Append(",\n Вес: ");
                stringBuilder.Append(Weight);
            }

            stringBuilder.Append(",\n Время приема пищи: ");
            stringBuilder.Append(MealTimeHours.ToString("D2"));
            stringBuilder.Append(":");
            stringBuilder.Append(MealTimeMinutes.ToString("D2"));


            if (MealType == MealType.Water)
            {
                stringBuilder.Append(",\n Типа блюда: ");
                stringBuilder.Append(GetMealTypeDescription(MealType));
            }

            if (WaterAmount != null)
            {
                stringBuilder.Append(",\n Количество воды: ");
                stringBuilder.Append(WaterAmount);
            }

            if (!string.IsNullOrEmpty(DishName))
            {
                stringBuilder.Append(",\n Название блюда: ");
                stringBuilder.Append(DishName);
            }

            if (EatPurpose != null)
            {
                stringBuilder.Append(",\n Цель употребления в пищу: ");
                stringBuilder.Append(GetEatPurpose(EatPurpose));
            }

            if (ProteinAmount != null)
            {
                stringBuilder.Append(",\n Количество белка: ");
                stringBuilder.Append(ProteinAmount);
            }

            if (FatAmount != null)
            {
                stringBuilder.Append(",\n Количество жира: ");
                stringBuilder.Append(FatAmount);
            }

            if (CarbohydrateAmount != null)
            {
                stringBuilder.Append(",\n Количество углеводов: ");
                stringBuilder.Append(CarbohydrateAmount);
            }

            stringBuilder.Append(",\n Стоимость блюда: ");
            stringBuilder.Append(Cost);

            return stringBuilder.ToString();
        }

        private string GetEatPurpose(EatPurposeType? eatPurpose)
        {
            if (eatPurpose is { } eat)
            {
                switch (eat)
                {
                    case EatPurposeType.Hunger:
                        return "Для утоления голода";
                    case EatPurposeType.Health:
                        return "Для улучшения здоровья";
                    case EatPurposeType.Weight:
                        return "Для контроля веса";
                    default:
                        return "Ничего";
                }
            }
            return "Ничего";
        }

        private string GetMealTypeDescription(MealType mealType)
        {
            switch (mealType)
            {
                case MealType.None:
                    return "Ничего";
                case MealType.Water:
                    return "Вода";
                case MealType.Eat:
                    return "Еда";
                default:
                    return mealType.ToString();
            }
        }
    }
}
