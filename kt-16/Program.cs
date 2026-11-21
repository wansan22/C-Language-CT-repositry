
using System.Reflection;

[AttributeUsage(AttributeTargets.Class)]
public class LibraryMetaData : Attribute
    {
        public string Author {get; set;}
        public string Version {get; set;}

        public LibraryMetaData(string Author, string Version)
        {
            this.Author = Author;
            this.Version = Version;
        }
    }

// 7 классов с атрибутом LibraryMetaData
[LibraryMetaData("Антон", "1.0")]
public class MathUtilsLib { }

[LibraryMetaData("Елена Смирнова", "2.1.0")]
public class NetworkHelperLib { }   

[LibraryMetaData("Дмитрий Волков", "0.9-beta")]
public class JsonParserLib { }

[LibraryMetaData("Анна Ковалёва", "3.4")]
public class CryptoCoreLib { }

[LibraryMetaData("Максим Орлов", "1.2.5")]
public class LoggerEngineLib { }

[LibraryMetaData("София Морозова", "4.0")]
public class ImageProcessorLib { }

[LibraryMetaData("Игорь Лебедев", "1.0.1")]
public class DatabaseConnectorLib { }
public class PlainUserModel { }
public class PlainOrderService { }
public class PlainAppConfig { }



public class LibraryManager
{
    public void PrintLibraryInfo(Type type)
    {
        var x = type.GetCustomAttribute<LibraryMetaData>();
        if ( x == null)
        {
            Console.WriteLine("класс не является библиотекой");
        }
        else
        {
            Console.WriteLine($"Автор библиотеки {x.Author}. Версия библиотеки {x.Version} ");
        }
    }
    public void PrintLibraryInfo<T>()
    {
        var x = typeof(T);
        PrintLibraryInfo(x);
    }
    public void PrintLibrariesInfo(Assembly assembly)
    {
        Type[] types = assembly.GetTypes();
        var filter = types.Select(t => ( Type : t , attr : t.GetCustomAttribute<LibraryMetaData>())).Where(x => x.attr != null).ToArray();
        foreach ( var i in filter)
        {
            Console.WriteLine($"Автор библиотеки {i.attr.Author}. Версия библиотеки {i.attr.Version}");
        }   

    }
    public void PrintLibrariesInfo()
    {
        var x = typeof(Program).Assembly;
        PrintLibrariesInfo(x);
    }
    public void PrintLibrariesInfo<T>()
    {
        var taked = typeof(T).Assembly;
        PrintLibrariesInfo(taked);
    }
}
public class Program
{
    private static void Main(string[] array)
    {
        LibraryManager testfor = new LibraryManager();
 
        testfor.PrintLibraryInfo(typeof(MathUtilsLib));
        testfor.PrintLibraryInfo(typeof(PlainUserModel));

        testfor.PrintLibraryInfo<NetworkHelperLib>();
        testfor.PrintLibraryInfo<PlainOrderService>();

        Console.WriteLine();

        testfor.PrintLibrariesInfo();

        Console.WriteLine();

        testfor.PrintLibrariesInfo<CryptoCoreLib>();

        Console.WriteLine();

        testfor.PrintLibrariesInfo(typeof(Program).Assembly);

    }


    }