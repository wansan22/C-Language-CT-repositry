using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

public class Program 
{
     public static IEnumerable<T> Order<T>(IEnumerable<T> array, Func<T, T, int> delegat)
    {
     
    List<T> resultlist = new List<T>();

    using (var enumerator = array.GetEnumerator())
    {
        while (enumerator.MoveNext())
        {
            T current = enumerator.Current;

        
            int insertIndex = 0;
            while (insertIndex < resultlist.Count && delegat(resultlist[insertIndex], current) <= 0)
            {
                insertIndex++;
            }

        
            resultlist.Insert(insertIndex, current);
        }
    }

    return resultlist;
    }

    private static void Main()
    {
        Func<int,int,int> sorted = (int x , int y) => x.CompareTo(y);
        int[] arrayfortest = [5,4,3,2,1];
        var x = Program.Order<int>(arrayfortest,sorted);
        foreach( var i in x)
        {
            Console.Write(i + " ");
        }
      
    }
}

 