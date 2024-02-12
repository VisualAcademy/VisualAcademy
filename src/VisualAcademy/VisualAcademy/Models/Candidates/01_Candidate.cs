using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualAcademy.Models.Candidates;

/// <summary>
/// 후보자, 지원자(Applicant)
/// </summary>
public class CandidateBase
{
    [Key] // Primary Key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Identity(1, 1)
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }

    public bool IsEnrollment { get; set; }

    /// <summary>
    /// Update 관련 동시성 충돌 처리를 위한 속성 
    /// </summary>
    [Timestamp] // rowversion
    public byte[]? ConcurrencyToken { get; set; }
}

public class Candidate : CandidateBase
{
    // Full Middle Name
    [StringLength(35)]
    public string? MiddleName { get; set; }

    // Suffix
    public string? NameSuffix { get; set; }

    // Alias(es): (oral or written)
    public string? AliasNames { get; set; }

    public string? SSN { get; set; }

    // Street
    [StringLength(70)]
    public string? Address { get; set; }

    [StringLength(70)]
    public string? City { get; set; }

    [StringLength(2)]
    public string? State { get; set; }

    // Zip 
    [DataType(DataType.PostalCode)]
    [StringLength(35)]
    public string? PostalCode { get; set; }

    // county
    public string? County { get; set; }

    // Telephone Number (Primary)
    [DataType(DataType.PhoneNumber)]
    [StringLength(35)]
    public string? PrimaryPhone { get; set; }

    // Telephone Number (Secondary)
    [DataType(DataType.PhoneNumber)]
    [StringLength(35)]
    public string? SecondaryPhone { get; set; }

    // Telephone Number (Work)
    //work_phone
    public string? WorkPhone { get; set; }


    // Email Address
    [DataType(DataType.EmailAddress)]
    [StringLength(254)]
    public string? Email { get; set; }

    // home_phone
    public string? HomePhone { get; set; }

    // mobile_phone
    public string? MobilePhone { get; set; }

    // Date of Birth
    public string? DOB { get; set; }

    // Age: age
    public int Age { get; set; }

    [StringLength(35)]
    public string? Gender { get; set; } // Male, Female 

    #region Birth Place
    [StringLength(70)]
    public string? BirthCity { get; set; }

    [StringLength(2)]
    public string? BirthState { get; set; }

    // birth_county
    public string? BirthCounty { get; set; }

    [StringLength(70)]
    public string? BirthCountry { get; set; }
    #endregion

    #region Driver's License
    // Driver's License Number: driver_license_number
    [StringLength(35)]
    public string? DriverLicenseNumber { get; set; }

    // State Issued: driver_license_state
    [StringLength(2)]
    public string? DriverLicenseState { get; set; }

    // Expiration Date: driver_license_expiration
    public DateTime? DriverLicenseExpiration { get; set; }
    #endregion

    public string? Photo { get; set; }

    [StringLength(35)]
    public string? LicenseNumber { get; set; }

    public string? OfficeAddress { get; set; }

    // office_city
    public string? OfficeCity { get; set; }

    // office_state
    public string? OfficeState { get; set; }

    // work_fax
    public string? WorkFax { get; set; }

    public string? BirthPlace { get; set; }

    // us_citizen
    public string? UsCitizen { get; set; }

    // marital_status
    public string? MaritalStatus { get; set; }

    // eye_color
    public string? EyeColor { get; set; }

    // hair_color
    public string? HairColor { get; set; }

    public string? Height { get; set; }
    public string? HeightFeet { get; set; }
    public string? HeightInches { get; set; }

    // business_structure
    public string? BusinessStructure { get; set; }

    // business_structure_other
    public string? BusinessStructureOther { get; set; }

    // weight
    public string? Weight { get; set; }

    // physical_marks
    public string? PhysicalMarks { get; set; }
}
