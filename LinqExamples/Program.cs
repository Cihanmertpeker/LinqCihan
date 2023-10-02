using LinqExamples;


class Propgram
{

    public static void Main(string[] args)
    {
        /// < summary >
        ///  ÖDEV => 
        ///  Kategorisi Beyaz Eşya olan ürünlerin Adları ile beraber, satış toplamları : Buzdolabı - 50.000
        ///            Televizyon - 200.0000
        ///            
        ///  En çok satışı olan 3 ürünün adı ve kategorisini
        ///  
        ///  Aynı örneğin yapılması
        ///  
        ///  En az satılan 2 ürünün adı ve kategorisi
        /// 
        /// 
        ///  kategorilere göre ortalama satış fiyatı ... 
        ///  Beyaz Eşya -> 50.000 
        ///  Elektronik -> 300.000
        ///  Giyim -> 500.000
        ///  
        /// 
        /// </summary>
       


        List< Category> categories = new List<Category>()
        {
        new Category { Id = 1, Name= "Telefon"},
        new Category { Id = 2,Name="Beyaz Eşya"},
        new Category { Id = 3,Name="Mobilya"}
        };

        List<Product> products = new List<Product>()
        {
        new Product { Id = 1,Name="Iphone 14",Price=50000,CategoryId=1},
        new Product { Id = 2,Name="Iphone 12",Price=40000,CategoryId=1},
        new Product { Id = 3,Name="Iphone 15",Price=80000,CategoryId=1},
        new Product { Id = 4,Name="Çamaşır Makinesi",Price=15000,CategoryId=2},
        new Product { Id = 5,Name="Bulaşık Makinesi",Price=14000,CategoryId=2},
        new Product { Id = 6,Name="Kurutma Makinesi",Price=12000,CategoryId=2},
        new Product { Id = 7,Name="Sandalye",Price=300,CategoryId=3},
        new Product { Id = 8,Name="Kanepe",Price=1000,CategoryId=3},
        new Product { Id = 9,Name="Televizyon ünitesi",Price=750,CategoryId=3},
        };

        List<Sales> sales = new List<Sales>()
        {
        new Sales {Amount=142,ProductId=1},
        new Sales {Amount=57,ProductId=2},
        new Sales {Amount=26,ProductId=3},
        new Sales {Amount=44,ProductId=4},
        new Sales {Amount=32,ProductId=5},
        new Sales {Amount=11,ProductId=6},
        new Sales {Amount=7,ProductId=7},
        new Sales {Amount=48,ProductId=8},
        new Sales {Amount=89,ProductId=9},
        };


        // Kategorisi Beyaz Eşya olan ürünlerin Adları ile beraber, satış toplamları
        var householdAppliances = from product in products                                      
                                  join sale in sales on product.Id equals sale.ProductId
                                  where product.CategoryId == 2 
                                  select new
                                  {
                                      ProductName = product.Name,
                                      TotalSales = sale.Amount
                                  };

        foreach (var item in householdAppliances)
        {
            Console.WriteLine($"Ürün Adı: {item.ProductName}, - Satış Toplamı: {item.TotalSales}");
        }

        Console.WriteLine("***************************************************************************");

        //En çok satışı olan 3 ürünün adı ve kategorisi
        var topThreeProducts = (from product in products
                                join sale in sales on product.Id equals sale.ProductId
                                group new { product, sale } by new { product.Name, product.CategoryId } into grouped
                                orderby grouped.Sum(item => item.sale.Amount) descending
                                select new
                                {
                                    ProductName = grouped.Key.Name,
                                    CategoryId = grouped.Key.CategoryId,
                                    TotalSales = grouped.Sum(item => item.sale.Amount)
                                }).Take(3);

        foreach (var item in topThreeProducts)
        {
            var category = categories.FirstOrDefault(cat => cat.Id == item.CategoryId);
            Console.WriteLine($"Kategori: {category?.Name}, - Ürün Adı: {item.ProductName}");
        }

       
        // En az satılan 2 ürünün adı ve kategorisi
        var bottomTwoProducts = (from product in products
                                 join sale in sales on product.Id equals sale.ProductId into productSales
                                 from sale in productSales.DefaultIfEmpty()
                                 group new { product, sale } by new { product.Name, product.CategoryId } into grouped
                                 orderby grouped.Sum(item => item.sale?.Amount ?? 0)
                                 select new
                                 {
                                     ProductName = grouped.Key.Name,
                                     CategoryId = grouped.Key.CategoryId,
                                     TotalSales = grouped.Sum(item => item.sale?.Amount ?? 0)
                                 }).Take(2);

        foreach (var item in bottomTwoProducts)
        {
            var category = categories.FirstOrDefault(cat => cat.Id == item.CategoryId);
            Console.WriteLine($"Kategori: {category?.Name}, - Ürün Adı: {item.ProductName},");
        }

        Console.WriteLine("***************************************************************************");

        //  kategorilere göre ortalama satış fiyatı
        var categoryAveragePrices = from product in products
                                    join sale in sales on product.Id equals sale.ProductId
                                    group new { product, sale } by product.CategoryId into grouped
                                    select new
                                    {
                                        CategoryId = grouped.Key,
                                        AveragePrice = ((int)grouped.Average(item => item.product.Price))
                                    };

        foreach (var item in categoryAveragePrices.OrderByDescending(k => k.AveragePrice))
        {
            var category = categories.FirstOrDefault(cat => cat.Id == item.CategoryId);
            Console.WriteLine($"Kategori: {category?.Name}, - Ortalama Satış Fiyatı: {item.AveragePrice}");
        }

    }




}
