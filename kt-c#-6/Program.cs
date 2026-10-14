using System;
using System.Collections.Generic;

public class Program
{
    public static IEnumerable<T> Order<T>(IEnumerable<T> consequency, Func<T, T, int> delegat)
    {
        List<T> resultlist = new List<T>();

        using (var enumenator = consequency.GetEnumerator())
        {
            while (enumenator.MoveNext())
            {
                resultlist.Add(enumenator.Current);
            }

            try
            {
                enumenator.Reset();
            }
            catch (NotSupportedException)
            {
            }
        }

        for (int i = 0; i < resultlist.Count - 1; i++)
        {
            for (int j = i + 1; j < resultlist.Count; j++)
            {
                if (delegat(resultlist[i], resultlist[j]) > 0)
                {
                    var temp = resultlist[j];
                    resultlist[j] = resultlist[i];
                    resultlist[i] = temp;
                }
            }
        }

        return resultlist;
    }

    public class Cat
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public Cat(int age, string name)
        {
            Age = age;
            Name = name;
        }
    }

    private static void Main()
    {
        int[] data = { 9, 2, 7, 1, 5 };
        var sortedNumbers = Order(data, (a, b) => a.CompareTo(b));

        using (var e = sortedNumbers.GetEnumerator())
        {
            while (e.MoveNext())
            {
                Console.Write(e.Current + " ");
            }
        }
        Console.WriteLine();

        Cat[] cats = 
        [
            new Cat(10, "Буся"), 
            new Cat(5, "Марк"), 
            new Cat(2, "Барсик") 
        ];
        

        var sortedCats = Order(cats, (c1, c2) => c1.Age.CompareTo(c2.Age));

        using (var e = sortedCats.GetEnumerator())
        {
            while (e.MoveNext())
            {
                Console.WriteLine(e.Current.Age);
            }
        }
    }
}