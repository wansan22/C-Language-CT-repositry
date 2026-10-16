using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
public class Program
{
    public enum Temperaturequality
    {
        high ,
        middle,
        low ,
        Unknown
    }

    public enum Windowquality
    {
        opened,
        closed,
        Unknown
    }
   public class TemperatureController
    {
        public int TemperatureValue { get; private set; }
        private bool _isInitialized;

        public event Action<int, int>? TemperatureChanged;

        public void SetTemperature(int value)
        {
            if (!_isInitialized || TemperatureValue != value)
            {
                int oldTemp = TemperatureValue;
                TemperatureValue = value;
                _isInitialized = true;

                TemperatureChanged?.Invoke(oldTemp, TemperatureValue);
            }
        }
    }
    public class AirConditioner
        {
            private Temperaturequality _currentQuality = Temperaturequality.Unknown;
            public AirConditioner(TemperatureController controller)
            {
                controller.TemperatureChanged += OnTemperatureChanged;
            }
            public void OnTemperatureChanged(int oldTemp,int newTemp)
            {
                Temperaturequality newQuality ;
                switch (newTemp) 
                {
                    case > 25:
                        newQuality = Temperaturequality.high;
                        break;
                    case >= 15: 
                        newQuality = Temperaturequality.middle;
                        break;
                    default :
                        newQuality = Temperaturequality.low;
                        break;
                }
                if (newQuality == _currentQuality)
            {
                return ;
            }
                 _currentQuality = newQuality;
                 switch ( _currentQuality )
            {
                case Temperaturequality.high :
                Console.WriteLine("кондиционер включен");
                break;
                case Temperaturequality.middle :
                Console.WriteLine("Комфортная температура, ничего не делаю");
                break;
                case Temperaturequality.low :
                Console.WriteLine("Обогреватель включен");
                break;
            }

            }
        }
        public class Window
    {
        private Windowquality _currentquality = Windowquality.Unknown;
        public Window(TemperatureController controller)
        {
            controller.TemperatureChanged += OnTemperatureChanged;
        }
        public void OnTemperatureChanged(int oldTemp, int newTemp)
        {
            Windowquality newquality = Windowquality.Unknown;
            switch (newTemp)
            {
                case > 35 :
                newquality = Windowquality.opened;
                break;
                case < 35 :
                newquality = Windowquality.closed;
                break;
            }
            if (newquality == _currentquality)
            {
                return ;
            }
            _currentquality = newquality;
            switch (newquality)
            {
                case Windowquality.opened :
                Console.WriteLine("Окно открыто");
                break;
                case Windowquality.closed :
                Console.WriteLine("Окно закрыто");
                break;
                case Windowquality.Unknown :
                Console.WriteLine("Изменение температуры для окна неопределенно при новом значении newTemp");
                break;
            }
        }

    }
    public class Logger
    {
        public List<string> logs = new List<string>();
        public Logger(TemperatureController controller)
        {
            controller.TemperatureChanged += OnTemperatureChanged;
        }
        public void OnTemperatureChanged(int oldTemp, int newTemp)
        {
            if (logs.Count == 0)
            {
                DateTime z = DateTime.UtcNow;
                string record = $"Добавление первой записи {newTemp} и время сохранения {z}";
                logs.Add(record);
            }
            else
            {
            DateTime x = DateTime.UtcNow;
            string record2 = $"Старая запись {oldTemp} новая запись {newTemp} и время сохранения {x}";
            logs.Add(record2);
            }
            
         
        }
    }
     public static void Main()
    {
        TemperatureController controller = new TemperatureController();

       
        AirConditioner ac = new AirConditioner(controller);
        Window window = new Window(controller);
        Logger logger = new Logger(controller);

        
        controller.SetTemperature(16);
        controller.SetTemperature(17);
        controller.SetTemperature(38); 
        Console.WriteLine("\n--- Сохранённые логи: ---");
        foreach (var entry in logger.logs)
        {
            Console.WriteLine(entry);
        }
    }
}

