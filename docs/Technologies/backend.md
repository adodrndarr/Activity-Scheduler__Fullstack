# Backend Technologies &#128187; - Activity Scheduler 

- ## .NET Core version 3.1
    - ### C#
- ## Architecture
    - ### 3-Tier Architecture
- ## .NET Core Web API 
- ## Microsoft SQL Server
    - ### Entity Framework - Code First Approach
- ## Testing
    - ### nUnit Framework
<br /><br />

## *Application Components* 
<br />

**1.** Domain<br />
Responsible for the data access, domain entities and repository logic. 
- **DataAcess** - database context, migrations and some initial configuration is to be found here.
- **Models** - contains the **core** domain entities.
- **Repositories** - repository pattern logic, interfaces and the implementation.
<br /><br />

**2.** Presentation<br />
Responsible for providing more detailed entities, which will be used as Data Transfer Objects (DTOs).
- **EntitiesDTO** - contains all the entities used for incoming requests or reponses from the API.
<br /><br />

**3.** Services<br />
Responsible for the **main** business logic, regarding the retrieval of data, processing and handing over the 'ready' data to other components as needed.
- **Extensions** - contains some useful custom extension methods for mapping common objects from one type to another.
- **Implementations** - contains the actual implementation of the services such as the **AccountService**, **ActivityService** etc...
- **Interfaces** - the corresponding service interfaces are to be found here.
<br /><br />

**4.** Services.Test<br />
- **Services** - contains some useful **unit-tests**, which cover most of the services main logic functionality.
<br /><br />

**5.** WebAPI.ActivityScheduler<br />
The **core** API project where all the others are used as needed.
- **Controllers** - the **main** API endpoints such as *Account*, *Activities* etc...
- **Logging** - all of the internal and functioning application logs are located here.
- **Mappings** - mapping profiles from **AutoMapper**, to easily map from one type to another.
<br /><br />
