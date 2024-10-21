namespace LegendsViewer.Backend.Legends.Parser;

public class Property
{
    public string Name { get; set; } = string.Empty;
    public bool Known { get; set; }

    public List<Property>? SubProperties { get; set; }

    private string _value = string.Empty;
    public string Value
    {
        get
        {
            Known = true;
            return _value;
        }
        set => _value = value ?? string.Empty;
    }

    public int ValueAsInt()
    {
        return Convert.ToInt32(Value);
    }
}