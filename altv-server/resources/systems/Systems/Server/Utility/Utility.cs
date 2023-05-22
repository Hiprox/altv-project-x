using AltV.Net.ColoredConsole;
using AltV.Net.Data;
using AltV.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class UI
{
    enum Type
    {
        List = 0,
        Checkbox,
        Slider
    }
}

public class TxtClr
{
    private TxtClr(string value) { Value = value; }
    public string Value { get; private set; }
    public static TxtClr Gray = new TxtClr("{A5A5A5}");
    public static TxtClr Warn = new TxtClr("{FFDC33}");
    public static TxtClr Err = new TxtClr("{FFFF00}");
    public static TxtClr AdmCht = new TxtClr("{FFFF00}");
    public static TxtClr AdmErr = new TxtClr("{E32636}");
    public static TxtClr AdmErrVar = new TxtClr("{FFA420}");
    public static TxtClr AdmEvnt = new TxtClr("{A5260A}");
    public override string ToString()
    {
        return Value;
    }
}

/* Интерьеры */
public sealed class InteriorsManager : AltServer.Server
{
    private string FilePath { get; } = @".\configs\interiors.json";
    private static InteriorsManager _instance;
    private static List<dynamic> _interiors = new List<dynamic>();
    private InteriorsManager()
    {
        Alt.Log("[Интерьеры] Идет загрузка...");
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
            _interiors = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? _interiors;
            if (_interiors.Count == 0)
            {
                throw new FileLoadException($"Файл конфигураций {Path.GetFileName(FilePath)} пуст!");
            }
        }
        catch(JsonReaderException)
        {
            throw new JsonReaderException($"Файл конфигураций {Path.GetFileName(FilePath)} поврежден!");
        }
        Alt.Log("[Интерьеры] Загружены!");
    }
    public static InteriorsManager Instance()
    {
        if (_instance == null)
        {
            _instance = new InteriorsManager();
        }
        return _instance;
    }
    public int Count { get { return _interiors.Count; } }
    public AltV.Net.Data.Position GetPosition(int interior)
    {
        var pos = _interiors[interior];
        return new((float)pos.X, (float)pos.Y, (float)pos.Z + 2.0f);
    }
    public string GetName(int interior)
    {
        return _interiors[interior].Name;
    }

}


/*interface IUiItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public UiMenu Submenu { get; set; }
    public string EventCallback { get; set; }
}

class UiListItem : IUiItem
{
    public UiListItem() { }
    public
    public string Name
    { get; set; }
    public string Description { get; set; }
    public string EventCallback { get; set; }
    public UiMenu Submenu { get; set; }
}

sealed class UiMenu
{
    public IUiItem Items { get; set; }
}*/