using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IFoodCategoryRepo
    {
        public List<FoodCategory> GetAllFoodCategories();
    }
}
