using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShopApp;

public enum OrderStatus
{
    Draft,
    Confirmed,
    Shipped,
    Cancelled
}

public class Product
{
    private string _name = null!;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название товара не может быть пустым или состоять только из пробелов.", nameof(value));
            _name = value;
        }
    }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public string Category { get; init; } = "General";

    public bool IsAvailable => Stock > 0;

    public Product(string name, decimal initialPrice, int initialStock = 0, string category = "General")
    {
        Name = name;
        ChangePrice(initialPrice);
        if (initialStock > 0)
        {
            AddStock(initialStock);
        }
        Category = category;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Цена должна быть строго больше нуля.");
        Price = newPrice;
    }

    public void AddStock(int qty)
    {
        if (qty <= 0)
            throw new ArgumentOutOfRangeException(nameof(qty), "Количество пополнения должно быть строго больше нуля.");
        Stock += qty;
    }

    public bool TryReserve(int qty)
    {
        if (qty <= 0 || Stock < qty)
            return false;

        Stock -= qty;
        return true;
    }
}

public class OrderItem
{
    private int _quantity;

    public Product Product { get; init; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Количество товара в позиции должно быть больше нуля.");
            _quantity = value;
        }
    }

    public decimal Subtotal => Product.Price * Quantity;

    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        Quantity = quantity;
    }
}

public class Order
{
    private readonly List<OrderItem> _items = new();
    private string? _cachedSummary;

    public string Number { get; init; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.Subtotal);

    public string Summary
    {
        get
        {
            if (_cachedSummary == null)
            {
                int totalQuantity = _items.Sum(i => i.Quantity);
                _cachedSummary = string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order #{Number}: {totalQuantity} items, total {Total:F2}, status {Status}"
                );
            }
            return _cachedSummary;
        }
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Нельзя добавить товар: заказ #{Number} находится в статусе {Status}, а не Draft.");

        if (!item.Product.TryReserve(item.Quantity))
        {
            throw new InvalidOperationException(
                $"Недостаточно товара '{item.Product.Name}' на складе (запрошено: {item.Quantity}, доступно: {item.Product.Stock})."
            );
        }

        _items.Add(item);
        ResetSummaryCache();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Переход в Confirmed возможен только из статуса Draft (текущий: {Status}).");

        Status = OrderStatus.Confirmed;
        ResetSummaryCache();
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Отгрузка (Shipped) возможна только из статуса Confirmed (текущий: {Status}).");

        Status = OrderStatus.Shipped;
        ResetSummaryCache();
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Draft && Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Отмена невозможна: заказ уже в терминальном статусе {Status}.");

        Status = OrderStatus.Cancelled;
        ResetSummaryCache();
    }

    private void ResetSummaryCache() => _cachedSummary = null;
}

internal class Program
{
    private static void Main()
    {

        var laptop = new Product("Ноутбук", 75000.00m, initialStock: 5, category: "Электроника");
        var mouse = new Product("Беспроводная мышь", 1500.50m, initialStock: 10, category: "Аксессуары");
        var coffee = new Product("Зерновой кофе", 800.00m, initialStock: 2, category: "Продукты");

        Console.WriteLine($"{laptop.Name} (Id: {laptop.Id}): Stock = {laptop.Stock}, Price = {laptop.Price}");
        Console.WriteLine($"{mouse.Name} (Id: {mouse.Id}): Stock = {mouse.Stock}, Price = {mouse.Price}");
        Console.WriteLine($"{coffee.Name} (Id: {coffee.Id}): Stock = {coffee.Stock}, Price = {coffee.Price}\n");

     
        var order = new Order { Number = "ORD-2026-001" };
        order.AddItem(new OrderItem(laptop, 1));
        order.AddItem(new OrderItem(mouse, 2));

   
        Console.WriteLine($"Total: {order.Total}");
        Console.WriteLine($"Summary: {order.Summary}\n");

        try
        {
            order.AddItem(new OrderItem(coffee, 5));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[Ожидаемая ошибка]: {ex.Message}");
            Console.WriteLine($"Остаток кофе не изменился: {coffee.Stock}");
            Console.WriteLine($"Позиций в заказе по-прежнему: {order.Items.Count}\n");
        }

        order.Confirm();
        Console.WriteLine($"Новый статус заказа: {order.Status}");
        try
        {
            order.AddItem(new OrderItem(coffee, 1));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[Ожидаемая ошибка]: {ex.Message}\n");
        }

     
        order.Ship();
        Console.WriteLine($"Финальный Summary: {order.Summary}");
        Console.WriteLine($"Остаток '{laptop.Name}': {laptop.Stock} (было 5)");
        Console.WriteLine($"Остаток '{mouse.Name}': {mouse.Stock} (было 10)\n");
 
    }
}