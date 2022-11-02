# Vessel Info

Vessel Info is a simple ASP.NET Core app for creating and storing tanker ships. 

# Project Description

The project is divided into two main sections:
- **An admin user** is responsible for managing basic CRUD operations as well as scraping a large amount of tankers related data.
- **A user** can look through more than 8000 tankers. All important information related to a specific vessel is shown on a dedicated page.

---

Apart from doing some basic CRUD operations on a given ship when logged as an admin you have an option for data scraping. Once the whole data is scrapped, it would automatically be persisted into the project's database. This process usually lasts quite some time due to a significant amount of information being processed. 

A user is granted access to detailed information about ship specifications like:

- *IMO (identification number)*
- *DWT (deadweight tonnage)*
- *LOA (length overall)*
- *Call Sign (a unique identifier)*
- *Flag and Port of Registry*
- *Type of Vessel*
- *Type of Hull*
- *Classification Society*
- *Owner*
- *Operator*

The data has been scrapped from: *https://www.q88.com/Ships.aspx?letter=A&v=list*

![Q88Sample](https://user-images.githubusercontent.com/97052397/199532689-5f47cf6e-6155-4a53-b998-5e805833418c.PNG)

The database's backup file can be found here: *https://github.com/george-pench/Vessel-Info/tree/main/Data/Vassel-Info.Data.Backup*

# Database Diagram
![DatabaseDiagram](https://user-images.githubusercontent.com/97052397/199499995-f137e550-b8e5-4967-9526-bf81d2422ad2.png)

# Roles

- Admin
- Authorized User
- Non-Authorized User

# Admin Credentials

| Email            | Password |
| ---------------- | -------- |
| Admin@vessel.com | admin12  |

# Technologies Used

- *ASP.NET Core 5.0*
- *Entity Framework Core 5.0*
- *MS SQL Server*
- *Bootstrap 4*
- *Bootstrap Lazy Kit*
- *HTML5*
- *CSS*
- *JavaScript*
- *MS Visual Studio 2019*
- *MS SQL Server Management Studio 2018*
- *AngleSharp 0.14.0*
- *AutoMapper*

# Images

- Register/Log in

![RegisterPNG](https://user-images.githubusercontent.com/97052397/199501388-3a6eb0ce-7d0e-42aa-b6d6-3a815af2f7e4.PNG)

![LogIn](https://user-images.githubusercontent.com/97052397/199501432-d67fafa2-9a79-495b-8fd9-4c9e077c9121.PNG)

- Homepage

![Homepage](https://user-images.githubusercontent.com/97052397/199531996-7245eb0d-23ff-4bfa-8458-99a84c3771df.PNG)

- Admin Create Page

![Create](https://user-images.githubusercontent.com/97052397/199533347-a8711fbd-c9ae-457a-8bba-4e4dd435b8d3.PNG)

- Details Page

![Details](https://user-images.githubusercontent.com/97052397/199534664-2ea33a93-7b9f-45a3-9077-d9cb5b1d5fc0.PNG)

- All Page

![All](https://user-images.githubusercontent.com/97052397/199534727-77a2903b-dfc8-4342-b3f3-f21dcc12a405.PNG)

