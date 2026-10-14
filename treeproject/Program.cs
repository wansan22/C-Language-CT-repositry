using System;
public class Program
{
    public class Node
    {
        public int Value { get; set; }
        public Node? nodeleft { get; set; }
        public Node? noderight { get; set; }
        public int Height { get; set; } = 1;

        public Node(int value, Node? nodeleft = null, Node? noderight = null)
        {
            Value = value;
            this.nodeleft = nodeleft;
            this.noderight = noderight;
        }
    }

public static Node? GetNodeByIndex(Node? root, int targetIndex)
    {
        if (targetIndex < 0) return null;

        int currentIndex = 0;
        return FindInOrder(root, targetIndex, ref currentIndex);
    }
    private static Node? FindInOrder(Node? node, int targetIndex, ref int currentIndex)
    {
        if (node == null) return null;

        Node? left = FindInOrder(node.nodeleft, targetIndex, ref currentIndex);
        if (left != null) return left;

        if (currentIndex == targetIndex)
        {
            return node;
        }
        currentIndex++;

        return FindInOrder(node.noderight, targetIndex, ref currentIndex);
    }

public static Node? BuildBstFromConsole()
{
    Console.WriteLine("Введите числа для дерева поиска через пробел (например: 17 6 5 20 19):");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) return null;

    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    Node? root = null;

    foreach (string part in parts)
    {
        if (int.TryParse(part, out int val))
        {
            root = Insert(root, val);
        }
    }

    Console.WriteLine("BST успешно построено!\n");
    return root;
}


public static Node? Delete(Node? root, int value)
{
    // 0. Базовый случай: дошли до конца и не нашли
    if (root == null) return null;

    // 1. Спускаемся к нужному числу
    if (value < root.Value)
    {
        root.nodeleft = Delete(root.nodeleft, value);
    }
    else if (value > root.Value)
    {
        root.noderight = Delete(root.noderight, value);
    }
    else
    {
        // 2. Нашли узел (root.Value == value)! Разбираем 3 случая:

        // Случай 1: Лист (нет детей) -> вернется null
        // Случай 2а: Только правый ребенок -> отдаем правого наверх
        if (root.nodeleft == null)
        {
            return root.noderight;
        }

        // Случай 2б: Только левый ребенок -> отдаем левого наверх
        if (root.noderight == null)
        {
            return root.nodeleft;
        }

        // Случай 3: Есть ОБА ребенка (как раз ваши 5 - 6 - 7)
        // 3.1. Ищем минимальный элемент в правом поддереве (преемника)
        root.Value = MinValue(root.noderight);

        // 3.2. Удаляем старый узел преемника из правого поддерева
        root.noderight = Delete(root.noderight, root.Value);
    }

    return root;
}

// Вспомогательный метод: самое маленькое число всегда в самом левом углу
private static int MinValue(Node node)
{
    int min = node.Value;
    while (node.nodeleft != null)
    {
        min = node.nodeleft.Value;
        node = node.nodeleft;
    }
    return min;
}


// Поиск узла по значению (возвращает найденный Node или null)
public static Node? Find(Node? root, int value)
{
    // Базовый случай: дошли до конца (null) или нашли значение
    if (root == null || root.Value == value)
    {
        return root;
    }

    // Идем строго в нужную ветку
    if (value < root.Value)
    {
        return Find(root.nodeleft, value);
    }
    
    return Find(root.noderight, value);
}

    public static Node Insert(Node? root, int value) // для дерева поиска
    {
        if (root == null)
        {
            return new Node(value);
        }
        if (value < root.Value)
        {
            root.nodeleft = Insert(root.nodeleft, value);
        }
        else if (value > root.Value)
        {
            root.noderight = Insert(root.noderight,value);
        }
        else
            return root;
    

    UpdateHeight(root);

   
    int balance = GetBalance(root);

    // Случай LL (перевес влево, вставили влево) -> один правый поворот
    if (balance > 1 && value < root.nodeleft!.Value)
        return RotateRight(root);

    // Случай RR (перевес вправо, вставили вправо) -> один левый поворот
    if (balance < -1 && value > root.noderight!.Value)
        return RotateLeft(root);
        // Случай LR (перевес влево, но вставили вправо) -> большой поворот
    if (balance > 1 && value > root.nodeleft!.Value)
    {
        root.nodeleft = RotateLeft(root.nodeleft);
        return RotateRight(root);
    }
    // Случай RL (перевес вправо, но вставили влево) -> большой поворот
    if (balance < -1 && value < root.noderight!.Value)
    {
        root.noderight = RotateRight(root.noderight);
        return RotateLeft(root);
    }
    return root;
    }

    private static int GetHeight(Node? node) => node?.Height ?? 0;
    private static int GetBalance(Node? node)
    {
    return node == null ? 0 : GetHeight(node.nodeleft) - GetHeight(node.noderight);
    }
    private static void UpdateHeight(Node node)
    {
    node.Height = 1 + Math.Max(GetHeight(node.nodeleft), GetHeight(node.noderight));
    }
    private static Node RotateLeft(Node a)
{
    Node b = a.noderight!;
    Node? temp = b.nodeleft;

    b.nodeleft = a;
    a.noderight = temp;

    UpdateHeight(a);
    UpdateHeight(b);

    return b; // b становится новым корнем поддерева
}

// Малый правый поворот (тянет левого ребенка вверх-вправо)
private static Node RotateRight(Node a)
{
    Node b = a.nodeleft!;
    Node? temp = b.noderight;

    b.noderight = a;
    a.nodeleft = temp;

    UpdateHeight(a);
    UpdateHeight(b);

    return b; // b становится новым корнем поддерева
}

    // Прямой обход (Pre-order): Корень -> Левый -> Правый
    public static void PrintPreOrder(Node? node)
    {
        if (node == null) return;

        Console.Write($"{node.Value} ");
        PrintPreOrder(node.nodeleft);
        PrintPreOrder(node.noderight);
    }

    // Центрированный обход (In-order): Левый -> Корень -> Правый (выведет числа по возрастанию)
    public static void PrintInOrder(Node? node)
    {
        if (node == null) return;

        PrintInOrder(node.nodeleft);
        Console.Write($"{node.Value} ");
        PrintInOrder(node.noderight);
    }

    // Обратный центрированный обход: левый правый корень (по убыванию)
    public static void PrintReverseInOrder(Node? node)
    {
        if (node == null) return;

        PrintReverseInOrder(node.nodeleft);
        PrintReverseInOrder(node.noderight);
        Console.Write($"{node.Value} ");
    }

    public static void Main()
    {
        // Создаем корректное BST дерево:
        //          10
        //        /    \
        //       5      20
        //      / \    /  \
        //     3   8  15   30
        Node node7 = new Node(30);
        Node node6 = new Node(15);
        Node node5 = new Node(8);
        Node node4 = new Node(3); // исправлено с 6 на 3 для соблюдения BST
        Node node3 = new Node(20, node6, node7);
        Node node2 = new Node(5, node4, node5);
        Node root = new Node(10, node2, node3);

        Console.WriteLine("Сортировка по возрастанию (In-Order):");
        PrintInOrder(root);

        Console.WriteLine("\n\nСортировка по убыванию:");
        PrintReverseInOrder(root); 

        Console.WriteLine("\n\nПрямой обход (Pre-Order):");
        PrintPreOrder(root);
        Console.WriteLine();
    }
}