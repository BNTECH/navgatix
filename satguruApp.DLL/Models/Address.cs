using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Address
{
    public long Id { get; set; }

    public int? Ct_Table_Id { get; set; }

    public int? Table_Row_Id { get; set; }

    public int? Ct_Type_Id { get; set; }

    public double? Radius_In_Miles { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string Location_Code { get; set; }

    public bool? Is_Enable_Geo_Fencing { get; set; }

    public string Name { get; set; }

    public bool? Is_Deleted { get; set; }

    public string Address1 { get; set; }

    public string Zip_Code { get; set; }

    public int? Country_Id { get; set; }

    public int? State_Id { get; set; }

    public int? City_Id { get; set; }

    public int? Created_By { get; set; }

    public int? Updated_By { get; set; }

    public DateTimeOffset? Created_Datetime { get; set; }

    public DateTimeOffset? Updated_Datetime { get; set; }
}
