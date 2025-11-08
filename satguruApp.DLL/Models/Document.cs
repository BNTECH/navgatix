using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Document
{
    public Guid? VehicleId { get; set; }

    public string DocType { get; set; }

    public string DocumentUrl { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CTTableId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdateDatetime { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public string DocumentName { get; set; }

    public string DocumentExt { get; set; }

    public string DocumentPathKey { get; set; }

    public string DocumentPath { get; set; }

    public byte[] DocStream { get; set; }

    public int? CategoryType { get; set; }

    public int? CTDocumentStatusId { get; set; }

    public DateTime? DocumentIssueDate { get; set; }

    public bool? IsJpass { get; set; }

    public bool? Form312 { get; set; }

    public string Comment { get; set; }

    public int? TableRowId { get; set; }

    public bool? IsDefault { get; set; }

    public int Id { get; set; }

    public virtual Vehicle Vehicle { get; set; }
}
