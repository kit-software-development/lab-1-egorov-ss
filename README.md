# Лабораторная работа №1

## Цели и задачи

Настоящая лабораторная работа ставит целью получение практических
умений, связанных с разработкой событийно-ориентированного 
программного обеспечения.

Поскольку, в языке программирования С#, представление о событии 
неразрывно связано с понятием о делегате, в рамках работы уделяется
внимание теоретическим и практическим аспектам использования 
делегатов.

В результате выполнения лабораторной работы слушатель должен 
сформировать следующие умения:
1. умение объявлять делегаты;
2. умение создавать объекты делегатов;
3. умение вызывать метод с использованием делегата;
4. умение объявлять и инициировать события класса;
5. при реализации модели событий класса, уметь использовать, 
предназначенные для этого, библиотечные типы данных.

## Делегаты

Делегаты можно определить, как ссылки на методы. Но делегаты, с C# -
это много больше, чем просто ссылка на метод. Объявляя делегат, мы 
создаём тип данных, по функциональности сравнимый с классом.

Объявление делегата очень похоже на объявление абстрактного метода 
класса. За тем исключением, что вместо ключевого слова `abstract` 
используется ключевое слово `delegate`.
```c#
delegate int Calculation(int x, int y);
```
После объявления делегата мы получаем тип данных, позволяющий 
адресовать любой метод с подходящей сигнатурой. Например:
```c#
static int Sum(int x, int y)
{
    return x + y;   
}

static int Substruction(int x, int y)
{
    return x - y;
}

static void Main(string[] args)
{
    Calculation calculation = Sum;
    int result = calculation(4, 5);
}
```
Более того, делегат может адресовать несколько методов. Например:
```c#
// объявляем делегат с требуемой сигнатурой
delegate void MessageConsumer(string message);

// ...

// открываем поток журнала
FileStream stream = File.OpenWrite("programm.log");
StreamWriter log = new StreamWriter(stream);

// инициализируем делегат двумя подходящими методами
MessageConsumer consumer = log.WriteLine;
consumer += Console.WriteLine;

// выводим сообщение сразу в два потока
consumer("Some message");
```
Также, делегаты поддерживают обобщение:
```c#
delegate T Calculation<T>(T x, T y);

// ...

static int Sum(int x, int y)
{
    return x + y;   
}

static float Sum(float x, float y)
{
    return x + y;
}

// ...

Calculation<float> calclulation = Sum;
```

## События

Можно сказать, что событие - это экземпляр делегата. Но не всякий 
экземпляр делегата является событием. 

Событие - специальный компонент класса, по семантике использования 
похожий на свойство. 

Событие объявляется с использованием ключевого слова event, как 
показано в примере:
```c#
class Point
{
    // ...
    public event EventHandler LocationChanged;
    // ...
}
```
После объявления событию могут быть присвоены обработчики. При этом
следует сказать, что добавить обработчик события можно в любом 
контексте, в котором доступен экземпляр класса. В то время как
инициировать событие можно только в пределах класса.

### События, не имеющие аргументов

Отдельно следует заметить, что хоть синтаксис и позволяет нам 
объявлять события от любого делегата, рекомендуется использовать
библиотечные событийные делегаты, разработанные специально для 
этих целей.

Таким образом, в библиотеке определён событийный делегат 
`EventHandler`, объявление которого показано ниже:
```c#
namespace System
{
  /// <summary>
  ///   Represents the method that will handle an event 
  ///   that has no event data.
  /// </summary>
  /// <param name="sender">
  ///   The source of the event. 
  /// </param>
  /// <param name="e">
  ///   An object that contains no event data. 
  /// </param>
  public delegate void EventHandler(object sender, EventArgs e);
}
```
Предполагается, что в качестве второго аргумента событий следует
передавать тип, родственный классу `EventArgs`. Определение данного
класса показано ниже:
```c#
namespace System
{
  /// <summary>
  ///   Represents the base class for classes that 
  ///   contain event data, and provides a value to use for 
  ///   events that do not include event data. 
  /// </summary>
  public class EventArgs
  {
    /// <summary>
    ///     Provides a value to use with events that do not 
    ///     have event data.
    /// </summary>
    public static readonly EventArgs Empty = new EventArgs();

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.EventArgs" /> class.
    /// </summary>
    public EventArgs() {}
  }
}
```
Как можно видеть, для удобства, в этом классе определена константа,
которую можно использовать для инициации событий, не имеющих аргументов.

Пример определения события, не имеющего аргументов показан ниже:
```c#
class Person
{
    /// <summary>
    ///     Событие, сигнализирубщее о том, что новый экземпляр
    ///     класса Person был создан в системе.
    /// </summary>
    public static event EventHandler PersonCreated;

    public Person()
    {
        // ...
        if (PersonCreated != null)
        {
            PersonCreated(this, EventArgs.Empty);
        }
    }
    
    // ...
}

static void Main(string[] args)
{
    Person.PersonCreated += OnPersonCreated;
}

/// <summary>
///     Обработчик события создания экземпляра класса Person
/// </summary>
private static void OnPersonCreated(object sender, EventArgs e)
{
    throw new NotImplementedException();
}
```
Теперь, при создании экземпляра класса `Person`, всякий раз, будет 
вызван метод `OnPersonCreated`, и мы получаем возможность 
реагировать на это событие.

### Параметризованные события

Также существует обобщённая версия делегата `EventHandler`, позволяющая 
определить, какой тип данных будет описывать аргументы события:
```c#
namespace System
{
  /// <summary>
  ///   Represents the method that will handle an event 
  ///   when the event provides data. 
  /// </summary>
  /// <param name="sender">
  ///   The source of the event.
  /// </param>
  /// <param name="e">
  ///   An object that contains the event data. 
  /// </param>
  /// <typeparam name="TEventArgs">
  ///   The type of the event data generated by the event.
  /// </typeparam>
  public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
}
```
С использованием этого делегата можно описывать параметризованные 
события. Ниже показан пример, в котором описывается параметризованное
событие изменения имени человека. Для реализации параметризованного
события, следует объявить класс, описывающий параметры события:
```c#
/// <summary>
///     Аргументы события изменения имени.
/// </summary>
class NameChangeEventArgs : EventArgs
{
    /// <summary>
    ///     Имя до события.
    /// </summary>
    public string Old { get; }
    
    /// <summary>
    ///     Имя, после события.
    /// </summary>
    public string Current { get; }

    public NameChangeEventArgs(string old, string current)
    {
        Old = old;
        Current = current;
    }
}
```
Далее, с использованием этого класса, следует объявить 
параметризованное событие и инициировать его всякий раз, когда 
интересующее нас свойство изменяется. 

При этом не следует забывать, что событие может быть пустым, то 
есть не адресовать ни одного обработчика. В это случае событие
будет содержать значение `null`.
```c#
/// <summary>
///     Представление о человеке
/// </summary>
class Person
{
    /// <summary>
    ///     Переменная, для хранения имени человека
    /// </summary>
    private string _name;
    /// <summary>
    ///     Свойство, обеспечивающее доступ к инкапсулированому имени.
    /// </summary>
    public string Name
    {
        get { return _name; }
        set
        {
            var args = new NameChangeEventArgs(_name, value);
            _name = value;
            if (NameChanged != null)
            {
                NameChanged(this, args);
            }
        }
    }
    /// <summary>
    ///     Событие, сигнализирубщее о том, что имя человека было изменено.
    /// </summary>
    public event EventHandler<NameChangeEventArgs> NameChanged;
   
    // ...
}
```
После того, как событие было объявлено, мы можем добавить обработчик
этого события для любого экземпляра класса `Person` и описать
в нём нашу реакцию на это событие.
```c#
static void Main(string[] args)
{
    Person person = new Person();
    person.NameChanged += OnPersonNameChanged;
}

/// <summary>
///     Обработчик события изменения имени человека
/// </summary>
private static void OnPersonNameChanged(object sender, NameChangeEventArgs e)
{
    throw new NotImplementedException();
}
```
