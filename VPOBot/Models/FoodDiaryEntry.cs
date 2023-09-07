namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class FoodDiaryEntry
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public double? Weight { get; set; }
        public int MealTimeHours { get; set; }
        public int MealTimeMinutes { get; set; }
        public string MealType { get; set; }
        public DishType DishName { get; set; }
        public string? EatPurpose { get; set; }
        public double? WaterAmount { get; set; }
        public double? ProteinAmount { get; set; }
        public double? FatAmount { get; set; }
        public double? CarbohydrateAmount { get; set; }
        public double Cost { get; set; }
    }
}
