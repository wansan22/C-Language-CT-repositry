using System.Linq.Expressions;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

public class Program
{
        public class createdList<T> where T : struct
    {
        private T[] array;
        private int count;
        public createdList()
        {
            array = new T[4];
            count = 0;
        }
        public createdList(T[] initialarray)
        {
            array = new T[initialarray.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = initialarray[i];
            }
            count = array.Length;
        }
        public void AddElementinList(T value)
        {
            if (count >= array.Length)
            {
                T[] template = array;
                array = new T[array.Length * 2];
                for (int i = 0; i < template.Length; i++)
                {
                    array[i] = template[i];
                }   
            }
            array[count] = value;
            count++;
        }
        public void Removeat(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new IndexOutOfRangeException("Индекс вне массива");
            }

            for (int i = index; i < count-1; i++)
            {
                array[i] = array[i+1];
            }
            count--;
            array[count] = default;
            
            }
            public bool Remove(T value)

            {
                var index = Array.IndexOf(array,value);

                if ( index < 0)
                {
                    return false;
                }
                else
                {
                    Removeat(index);
                    return true;
                }
            }
            public int IndexOf(T value)
            {   
                int index = -1;
                for (int i = 0; i < count ; i++)
                {
                    if( value.Equals(array[i]))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
            public int LastIndexOf(T value)
            {
                int index = -1;
                for (int i = 0; i < count ; i++)
                {
                    if( value.Equals(array[i]))
                    {
                        if (i > index)
                        {
                        index = i;
                        }
                    }
                }
                
                return index;
            }
            public int Count => count; // это мой метод каунт.
           
            public T this[int index]
        
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException("Выход за границы массива");
                }
                return array[index];
            }
            set
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException("Выход за границы массива");
                }
                array[index] = value;
            }
        }

        }
    
     private static void Main()
    {
        int[] massive = new int[5] {1,3,3,4,2};
        createdList<int> example = new createdList<int>(massive);
        Console.WriteLine(example.Count);
        Console.WriteLine(example[0]);
       

    }
}