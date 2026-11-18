using System;
using System.Collections.Generic;
using System.Linq;
using Game_Bracket.Contracts;

namespace Game_Bracket.Domain;

public class Map
{
    public string Name { get; }
    public string ImagePath { get; }

    public Map(string name, string? imagePath = "")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Maps must have a name", nameof(name));
        }

        Name = name;
        ImagePath = imagePath ?? string.Empty;
    }
    public string GetMapDetails()
    {
        return string.IsNullOrEmpty(ImagePath) ? Name : $"{Name} ({ImagePath})";
    }
    public override string ToString()
    {
        return GetMapDetails();
    }
}
public class MapVetoService : IMapVetoService
{
    private readonly List<Map> _mapPool;
    private readonly IMapVetoFactory _formatFactory;
    private readonly string _format;
    private Map? _selectedMap;

    public MapVetoService(IEnumerable<Map> mapPool, string format)
        : this(mapPool, format, new MapVetoFactory())
    { }
    public MapVetoService(IEnumerable<Map> mapPool, string format, IMapVetoFactory formatFactory)
    {
        if (mapPool == null)
        {
            throw new ArgumentNullException(nameof(mapPool));
        }
        if (formatFactory == null)
        {
            throw new ArgumentNullException(nameof(formatFactory));
        }
        _mapPool = mapPool.ToList();
        if (_mapPool.Any(map => map is null))
        {
            throw new ArgumentException("The map pool cannot contain null maps.", nameof(mapPool));
        }

        _format = format;
        _formatFactory = formatFactory;
    }

    public void RunMapVeto()
    {
        IMapVetoService service = _formatFactory.CreateService(_format);
        if (service is not MapVetoFormatService formatService)
        {
            throw new InvalidOperationException("Factory returned invalid service");
        }

        formatService.SetMapPool(_mapPool);
        formatService.RunMapVeto();
        _selectedMap = formatService.GetSelectedMap();
    }

    public Map GetSelectedMap()
    {
        if (_selectedMap == null)
        {
            throw new InvalidOperationException("Must run veto service before using a map");
        }

        return _selectedMap;
    }
}
public class MapVetoFactory : IMapVetoFactory
{
    public IMapVetoService CreateService(string format)
    {
        string normalizedFormat = NormalizeFormat(format);

        switch (normalizedFormat)
        {
            case "bestofone":
                return new BestOfOneMapVetoService();
            case "bestofthree":
                return new BestOfThreeMapVetoService();
            case "bestoffive":
                return new BestOfFiveMapVetoService();
            default:
                throw new ArgumentException(
                    "Only use one of the provided Veto formats",
                    nameof(format));
        }
    }

    private static string NormalizeFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            throw new ArgumentException("Map veto format cannot be empty.", nameof(format));
        }

        return new string(format.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
    }
}

public abstract class MapVetoFormatService : IMapVetoService
{
    private List<Map> _mapPool = new List<Map>();
    private Map? _selectedMap;

    internal void SetMapPool(IEnumerable<Map> maps)
    {
        _mapPool = maps.ToList();
        _selectedMap = null;
    }

    public abstract void RunMapVeto();

    public Map GetSelectedMap()
    {
        if (_selectedMap == null)
        {
            throw new InvalidOperationException("Must run veto service before using a map");
        }

        return _selectedMap;
    }

    protected void SelectFirstAvailableMap()
    {
        _selectedMap = _mapPool.FirstOrDefault();
        if (_selectedMap == null)
        {
            throw new InvalidOperationException("Must have at least one map in map pool.");
        }
    }
}

public sealed class BestOfOneMapVetoService : MapVetoFormatService
{
    public override void RunMapVeto()
    {
        SelectFirstAvailableMap();
    }
}
public sealed class BestOfThreeMapVetoService : MapVetoFormatService
{
    public override void RunMapVeto()
    {
        SelectFirstAvailableMap();
    }
}
public sealed class BestOfFiveMapVetoService : MapVetoFormatService
{
    public override void RunMapVeto()
    {
        SelectFirstAvailableMap();
    }
}