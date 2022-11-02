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

The data is being scrapped from: *https://www.q88.com/Ships.aspx?letter=A&v=list*

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

- ASP.NET Core 5.0
- Entity Framework Core 5.0
- MS SQL Server
- Bootstrap 4
- Bootstrap Lazy Kit
- HTML5
- CSS
- JavaScript
- MS Visual Studio 2019
- MS SQL Server Management Studio 2018
- AngleSharp 0.14.0
- AutoMapper

# Images

- Register/Log in

![RegisterPNG](https://user-images.githubusercontent.com/97052397/199501388-3a6eb0ce-7d0e-42aa-b6d6-3a815af2f7e4.PNG)

![LogIn](https://user-images.githubusercontent.com/97052397/199501432-d67fafa2-9a79-495b-8fd9-4c9e077c9121.PNG)

