/*using AltV.Net.Client;
using Newtonsoft.Json;

public class ShapesManager
{
    public enum Gender : byte
    {
        Male = 0,
        Female
    }
    private string FilePath { get; } = @".\shapes.json";
    private static ShapesManager _instance;
    private static Dictionary<string, dynamic> _data = new ();
    private ShapesManager()
    {
        Alt.Log("[Shapes] Идет загрузка...");
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException($"Файл конфигураций {Path.GetFileName(FilePath)} не существует!");
        }
        var json = File.ReadAllText(FilePath);
        if (string.IsNullOrEmpty(json))
        {
            throw new FileLoadException($"Файл конфигураций {Path.GetFileName(FilePath)} пуст!");
        }
        try
        {
            _data = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(json) ?? _data;
            if (_data.Count == 0)
            {
                throw new FileLoadException($"Файл конфигураций {Path.GetFileName(FilePath)} пуст!");
            }
        }
        catch (JsonReaderException)
        {
            throw new JsonReaderException($"Файл конфигураций {Path.GetFileName(FilePath)} поврежден!");
        }
        Alt.Log("[Shapes] Загружены!");
    }
    public static ShapesManager Instance()
    {
        if (_instance == null)
        {
            _instance = new ShapesManager();
        }
        return _instance;
    }
    public static void Reset()
    {
        _instance = null;
    }
    public int CountMale { get { return _data["Male"].Count; } }
    public int CountFemale { get { return _data["Female"].Count; } }
    
    public string GetName(Gender gender, int index)
    {
        return gender switch
        {
            Gender.Male => _data["Male"][index].Name,
            Gender.Female => _data["Female"][index].Name,
            _ => throw new NotImplementedException($"Файл конфигураций {Path.GetFileName(FilePath)} поврежден!")
        };
    }
    public int GetId(Gender gender, int index)
    {
        return gender switch
        {
            Gender.Male => _data["Male"][index].Id,
            Gender.Female => _data["Female"][index].Id,
            _ => throw new NotImplementedException($"Файл конфигураций {Path.GetFileName(FilePath)} поврежден!")
        };
    }
    public string GetImage(Gender gender, int index)
    {
        return gender switch
        {
            Gender.Male => _data["Male"][index].Image,
            Gender.Female => _data["Female"][index].Image,
            _ => throw new NotImplementedException($"Файл конфигураций {Path.GetFileName(FilePath)} поврежден!")
        };
    }
    public IEnumerable<int> GetIds(Gender gender)
    {
        for (int i = 0; i < (gender == Gender.Male ? CountMale : CountFemale); ++i)
        {
            yield return GetId(gender, i);
        }
    }
}
*/