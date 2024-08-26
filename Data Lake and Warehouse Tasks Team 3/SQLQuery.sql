CREATE TABLE PatientAdmissionsFact (
    AdmissionID SERIAL PRIMARY KEY,
    PatientID INT,
    PhysicianID INT,
    DepartmentID INT,
    AdmissionDateID INT,
    DischargeDateID INT,
    LengthOfStay INT,
    AdmissionType VARCHAR(50),
    AdmissionReason TEXT,
    DischargeStatus VARCHAR(50),
) PARTITION BY RANGE (AdmissionDateID);

CREATE TABLE TreatmentOutcomesFact (
    OutcomeID SERIAL PRIMARY KEY,
    PatientID INT,
    PhysicianID INT,
    DepartmentID INT,
    TreatmentDateID INT,
    TreatmentType VARCHAR(50),
    OutcomeStatus VARCHAR(50),
    FollowUpDateID INT,
    ReadmissionWithin30Days BOOLEAN,
) PARTITION BY RANGE (TreatmentDateID);

CREATE TABLE ResourceUtilizationFact (
    ResourceUsageID SERIAL PRIMARY KEY,
    PatientID INT,
    DepartmentID INT,
    ResourceID INT,
    UsageDateID INT,
    QuantityUsed INT,
    Cost DECIMAL(18,2),
) PARTITION BY RANGE (UsageDateID);


CREATE TABLE PatientsDimension (
    PatientID SERIAL PRIMARY KEY,
    PatientNaturalKey VARCHAR(50) UNIQUE,
    Name VARCHAR(100),
    Gender CHAR(1),
    DateOfBirth DATE,
    Address VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    ZipCode VARCHAR(10),
    InsuranceProvider VARCHAR(100),
    MaritalStatus VARCHAR(50),
    Ethnicity VARCHAR(50)
);

CREATE TABLE PhysiciansDimension (
    PhysicianID SERIAL PRIMARY KEY,
    PhysicianNaturalKey VARCHAR(50) UNIQUE,
    Name VARCHAR(100),
    Specialization VARCHAR(100),
    Gender CHAR(1),
    YearsOfExperience INT,
    DepartmentID INT,
    ContactDetails VARCHAR(255),
    FOREIGN KEY (DepartmentID) REFERENCES DepartmentsDimension(DepartmentID)
);

CREATE TABLE DepartmentsDimension (
    DepartmentID SERIAL PRIMARY KEY,
    DepartmentName VARCHAR(100),
    HeadOfDepartment VARCHAR(100),
    Location VARCHAR(100)
);

CREATE TABLE TimeDimension (
    DateID SERIAL PRIMARY KEY,
    Date DATE,
    DayOfWeek VARCHAR(10),
    Month VARCHAR(10),
    Quarter VARCHAR(2),
    Year INT,
    IsWeekend BOOLEAN
);

CREATE TABLE ResourcesDimension (
    ResourceID SERIAL PRIMARY KEY,
    ResourceName VARCHAR(100),
    ResourceType VARCHAR(50),
    Supplier VARCHAR(100),
    CostPerUnit DECIMAL(18,2)
);


