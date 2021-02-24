# Frontend Technologies &#128187; - Activity Scheduler 

- ## Angular - CLI version 8.3.0 
    - ### Typescript 
<br /><br />

## *Application Components* 
<br />

**1.** Authentication<br />
- **Entities** - application interfaces and models.
- **Guards** - application guards.
- **Login** - the **core** logic of the **login** component.
- **Register** - the **core** logic of the **register** component.<br />
*Note:* - here **auth-interceptor** and **auth.service** are also located.<br /><br />


**2.** Header<br />
- The main header component.<br /><br />


**3.** Main<br />
Here are all of the **core** components of the application located.
- **activityEntities** - contains all of the activity entity related components.
    - activity-container - serving as a wrapper around the other components.
    - view-activity - responsible for the view.
    - new-activity - responsible for creating new activity entity.
    - edit-activity - responsible for editing an existing activity entity.
- **schedule-activity** - responsible for **scheduling** an activity. <br />
    *Note:* - the **schedule-activity.service** is also located here.
- **my-activities** - contains user specific activities.
- **manage-users** - contains components related to managing a user.
    - user-container - serving as a wrapper around the other components.
    - view-user - responsible for the view.
    - register-admin - responsible for registering an **admin**.
    - edit-user - responsible for editing an existing user.
- **user-account** - contains user specific account information.
<br /><br />


**4.** Services<br />
- Contain the **core** logic connected to the **backend**, logic for **error handling**, **validation** etc..<br /><br />

**5.** Footer<br />
- The footer component.<br /><br />

**6.** Shared<br />
Contains some commonly used components.
- **error** - the error component.
- **loading-spinner** - the loading-spinner component.
- **not-found** - the not found component.
<br /><br />

**7.** Styling <br />
In the Angular application, **assets/scss** contains the core styling for all of the components, the methodology used for styling was **BEM** (Block, Element & Modifier).
- **Base** - typography, utilities, variables and setup.
- **Elements** - buttons, forms, inputs and other reusable elements.
- **Pages** - login, register, activity and all of the core components styling is to be found here.<br />
**Note:** *styles.css* - **core** file contains all of the **scss** imports.
