using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Utils;



public class EnumUtil
{
    public static IEnumerable<EnumData> GetEnumData<TEnum>() where TEnum : Enum
    {
        var enums = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        foreach (var item in enums)
        {
            yield return new EnumData()
            {
                Id = Convert.ToInt32(item),
                Name = item.ToString()
            };
        }

    }
}


public class EnumData
{
    public int Id { get; set; }
    public string Name { get; set; }
}
