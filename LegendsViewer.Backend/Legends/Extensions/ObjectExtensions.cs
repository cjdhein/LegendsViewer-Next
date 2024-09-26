using System.ComponentModel;

namespace LegendsViewer.Backend.Legends.Extensions;

public static class ObjectExtensions
{
    public static string GetDescription(this object enumerationValue)
    {
        if (enumerationValue is double doubleValue)
        {
            return doubleValue.ToString("R");
        }

        var type = enumerationValue.GetType();
        if (!type.IsEnum)
        {
            return enumerationValue.ToString();
        }

        //Tries to find a DescriptionAttribute for a potential friendly name
        //for the enum
        var memberInfo = type.GetMember(enumerationValue.ToString());
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
            {
                //Pull out the description value
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        //If we have no description attribute, just return the ToString of the enum
        return enumerationValue.ToString();
    }
}
