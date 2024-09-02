To migrate data from an Excel file (`emp_history.xlsx`) to an Azure SQL Server database using Azure Data Factory (ADF) and Azure Storage Account, follow these steps:

### Prerequisites
1. **Azure Storage Account**: You need an Azure Storage Account where you will upload the Excel file.
2. **Azure SQL Server Database**: Ensure that your Azure SQL Server database is set up and that you have access credentials.
3. **Azure Data Factory**: Create an instance of Azure Data Factory if you don’t already have one.

### Step-by-Step Solution

#### Step 1: Prepare the Excel File
1. **Check and Clean Data**: Ensure that your Excel file (`emp_history.xlsx`) is clean, with no empty rows or irrelevant data in the columns you want to migrate.
2. **Upload Excel File to Azure Blob Storage**:
   - Go to your Azure Storage Account in the Azure portal.
   - Navigate to the **Blob service** and create a container (if not already existing).
   - Upload the `emp_history.xlsx` file to the container.

#### Step 2: Set Up Azure Data Factory (ADF)
1. **Create a New Pipeline**:
   - Go to your Azure Data Factory instance in the Azure portal.
   - Click on **Author & Monitor**.
   - Create a new pipeline by clicking on the **+** icon and selecting **Pipeline**.

2. **Create Linked Services**:
   - **Azure Blob Storage Linked Service**:
     - Go to **Manage** > **Linked Services** > **New**.
     - Choose **Azure Blob Storage**.
     - Name the linked service (e.g., `AzureBlobStorageLS`).
     - Configure it to connect to your Azure Storage Account where the Excel file is uploaded.
   - **Azure SQL Database Linked Service**:
     - Create another linked service, but this time choose **Azure SQL Database**.
     - Name it (e.g., `AzureSQLDatabaseLS`).
     - Provide the necessary connection details (Server name, Database name, Username, and Password).

3. **Create Datasets**:
   - **Excel Dataset**:
     - Go to **Author** > **Datasets** > **New Dataset**.
     - Choose **Azure Blob Storage** and then select **Excel**.
     - Configure the dataset to point to the `emp_history.xlsx` file in the Blob Storage.
     - Set up the **Sheet name** or **Table** from which you want to read the data.
   - **SQL Table Dataset**:
     - Create another dataset for the Azure SQL Database.
     - Choose **Azure SQL Database**.
     - Name it and link it to the Azure SQL Database Linked Service.
     - Specify the table (or create a new table) where the data will be migrated.

4. **Set Up Data Flow**:
   - Add a **Copy Data** activity in the pipeline.
   - In the **Source** tab, select the **Excel dataset**.
   - In the **Sink** tab, select the **SQL Table dataset**.
   - Configure the **Mapping** tab to map the Excel columns (`EmpID`, `Emp_Last_Name`, and `Salary`) to the corresponding SQL table columns.

#### Step 3: Configure Data Types in Azure SQL Database
1. **Create the Target Table**:
   - If the target table does not exist, run the following SQL command in your Azure SQL Database to create it:

   ```sql
   CREATE TABLE EmployeeHistory (
       EmpID INT,
       Emp_Last_Name VARCHAR(255),
       Salary INT
   );
   ```

   Ensure the data types match the requirements: `EmpID` and `Salary` as `INT`, `Emp_Last_Name` as `VARCHAR`.

#### Step 4: Execute the Pipeline
1. **Run the Pipeline**:
   - Click on **Debug** to test the pipeline.
   - Once it runs successfully in debug mode, **publish** all changes.
   - Schedule or manually trigger the pipeline as required.

2. **Monitor the Pipeline**:
   - Go to **Monitor** in the Azure Data Factory to check the status of the pipeline run.
   - Ensure that the pipeline runs successfully and the data is correctly loaded into your Azure SQL Database.

#### Step 5: Validate Data in Azure SQL Database
1. **Verify Data Migration**:
   - Connect to your Azure SQL Database using SQL Server Management Studio (SSMS) or Azure Data Studio.
   - Run a `SELECT` query to ensure that the data has been migrated correctly:

   ```sql
   SELECT * FROM EmployeeHistory;
   ```

### Conclusion
By following these steps, you have successfully migrated the specific columns (`EmpID`, `Emp_Last_Name`, `Salary`) from an Excel file to an Azure SQL Server database using Azure Data Factory and Azure Storage Account. Make sure to validate the data and ensure that all data types are correctly configured.