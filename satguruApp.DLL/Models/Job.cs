using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Job
{
    public int Id { get; set; }

    public string Job_Title { get; set; }

    public int? Country_Id { get; set; }

    public int? State_Id { get; set; }

    public int? City_Id { get; set; }

    public string Location { get; set; }

    public string Salary { get; set; }

    public DateTime? Posted_Date { get; set; }

    public DateTime? Closed_Date { get; set; }

    public int? Job_Status_Id { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Datetime { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Datetime { get; set; }

    public bool? Is_Deleted { get; set; }

    public string Description { get; set; }

    public int? Min_Experience { get; set; }

    public int? Max_Experience { get; set; }

    public int? CtEmployment_Type { get; set; }

    public int? Working_Hours { get; set; }

    public int? No_Of_Opening { get; set; }

    public int? Hiring_Time { get; set; }

    public string Schedule_Job { get; set; }

    public int? Company_Id { get; set; }

    public int? Division_Id { get; set; }

    public string Requirement { get; set; }

    public string Responsibility { get; set; }

    public int? Ct_Lab_Cat_Id { get; set; }

    public string Required_Training_Url { get; set; }

    public int? Positions_Filled { get; set; }

    public int? LocationId { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<Job> InverseLocationNavigation { get; set; } = new List<Job>();

    public virtual Job LocationNavigation { get; set; }
}
