using System;
using System.Reflection;
using System.Linq;
public class Program
{
    public class Cat
    {
        public int Age {get; set;}
        public string Name {get; set;}
        
        public string Korm {get; set;}
        
        public Cat(int age, string name, string korm)
        {
            Age = age;
            Name = name;
            Korm = korm;
        }
    }
    public static string SimpleSerialize(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj),"В метод передали нулевой объект");
        }
        PropertyInfo[] v = obj.GetType().GetProperties();
        var x = v.Where(p => p.CanRead).Select((p, index) => $"Prop{index+1}: {p.GetValue(obj) ?? "null"}");
        string result = string.Join(", ",x);
        return result;
        
    }
    private static void Main()
    {
       var catt = new Cat(10,"аня","китикет");
       Console.WriteLine(SimpleSerialize(null));
    }
}