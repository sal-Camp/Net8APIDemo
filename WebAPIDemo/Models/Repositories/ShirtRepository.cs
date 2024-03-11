namespace WebAPIDemo.Models.Repositories;

public static class ShirtRepository
{
    private static List<Shirt> _shirts = new List<Shirt>()
    {
        new Shirt { ShirtId = 1, Brand = "H&M", Color = "Blue", Size = 6, Gender = "women", Price = 10.99 },
        new Shirt { ShirtId = 2, Brand = "H&M", Color = "Red", Size = 8, Gender = "men", Price = 12.99 },
        new Shirt { ShirtId = 3, Brand = "AE", Color = "Green", Size = 10, Gender = "women", Price = 14.99 },
        new Shirt { ShirtId = 4, Brand = "AE", Color = "Yellow", Size = 12, Gender = "men", Price = 16.99 }
    };

    public static List<Shirt> GetShirts()
    {
        return _shirts;
    }

    public static bool ShirtExists(int id)
    {
        return _shirts.Any(x => x.ShirtId == id);
    }

    public static Shirt? GetShirtById(int id)
    {
        return _shirts.FirstOrDefault(x => x.ShirtId == id);
    }

    public static void AddShirt(Shirt shirt)
    {
        int maxId = _shirts.Max(x => x.ShirtId);
        shirt.ShirtId = maxId + 1;
        _shirts.Add(shirt);
    }

    public static Shirt? GetShirtByProperties(string? brand, string? gender, string? color, int? size)
    {
        return _shirts.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(brand) &&
            !string.IsNullOrWhiteSpace(x.Brand) &&
            x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(gender) &&
            !string.IsNullOrWhiteSpace(x.Gender) &&
            x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(color) &&
            !string.IsNullOrWhiteSpace(x.Color) &&
            x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
            size.HasValue &&
            x.Size.HasValue &&
            size.Value == x.Size.Value);
    }

    public static void UpdateShirt(Shirt shirt)
    {
        var shirtToUpdate = _shirts.First(x => x.ShirtId == shirt.ShirtId);
        shirtToUpdate.Brand = shirt.Brand;
        shirtToUpdate.Price = shirt.Price;
        shirtToUpdate.Size = shirt.Size;
        shirtToUpdate.Color = shirt.Color;
        shirtToUpdate.Gender = shirt.Gender;
    }

    public static void DeleteShirt(int shirtId)
    {
        var shirt = GetShirtById(shirtId);
        if (shirt is not null)
        {
            _shirts.Remove(shirt);
        }
    }
}