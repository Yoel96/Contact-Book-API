The Backend of this app was developed in C# and SQL Server. An API was developed in order to manage all the data necessary to manage the contacts data, this API has also an authorization system in order to make the data access more secure. 

### Requirements
To run the API the following requirements are needed:

- A SQL Server installation.
- Visual Studio with .Net tools installed.
- The Microsoft.AspNetCore.Authentication.JwtBearer nuget packet.
- The Microsoft.AspNetCore.Identity.EntityFrameworkCore nuget packet.
- The Microsoft.EntityFrameworkCore nuget packet.
- The Microsoft.EntityFrameworkCore.SqlServer nuget packet.
- The Microsoft.EntityFrameworkCore.Tools nuget packet.
- The Microsoft.VisualStudio.Web.CodeGeneration.Design nuget packet.

You will need to configure the appsettings.json file with the data of your data base. Just change the "Connection" property with the next format:

```
    "ConnectionStrings": {
        "Connection": "yourDBServer;Database=yourDBName;Trusted_Connection=True;TrustServerCertificate=true;"
    }
```

There is a migration folder with the files that are necessary to create the tables of the database. To make the migration to your data base use the next command in 
the Nuget package terminal.

```
Update-database
```

For better debug of the API endpoints. the Swagger packet is also activated in this program.
