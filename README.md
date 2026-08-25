# 🛍️ IdealShop

**IdealShop** is a full-stack e-commerce web application built with **React** and **ASP.NET Core**. It provides a complete online shopping experience for customers together with an administration dashboard for managing products, categories, customers, and administrators.

The project uses a React single-page application for the frontend and an ASP.NET Core Web API connected to SQL Server for the backend.

---

## ✨ Features

### 👤 Customer

* Create a customer account
* Login and logout
* Browse available products
* Browse products by category
* View detailed product information
* Add products to the shopping cart
* Manage cart items
* Checkout and create an order
* View an order confirmation after checkout

### 🛒 Shopping

* Product catalog
* Product details
* Category-based product filtering
* Shopping cart
* Quantity and cart management
* Checkout workflow
* Order creation

### 🛠️ Admin Panel

* Admin authentication
* Admin dashboard
* Create, edit, and manage products
* Manage product categories
* Manage customers
* Manage administrator accounts
* Manage product images and information

---

## 🧰 Tech Stack

### Frontend

* **React 19**
* **React Router DOM 7**
* **Axios**
* **Bootstrap 5**
* **Create React App**
* HTML5
* CSS3
* JavaScript

### Backend

* **ASP.NET Core / .NET 9**
* **C#**
* **Entity Framework Core 9**
* **ASP.NET Core Controllers**
* **Cookie-based Authentication**
* **REST API**

### Database

* **Microsoft SQL Server**
* **Entity Framework Core Migrations**

---

## 🏗️ Project Architecture

```text
IdealShop-TWA-Project/
│
├── IdealShop-TWA-Project/
│   ├── IdealShop.sln
│   │
│   └── IdealShop/
│       ├── Controllers/
│       │   ├── AdminController.cs
│       │   ├── CartItemsController.cs
│       │   ├── CategoriesController.cs
│       │   ├── CustomersController.cs
│       │   ├── OrdersController.cs
│       │   └── ProductsController.cs
│       │
│       ├── Data/
│       ├── Migrations/
│       ├── Models/
│       ├── Properties/
│       ├── wwwroot/
│       ├── Program.cs
│       ├── appsettings.json
│       └── IdealShop.csproj
│
├── idealshop-frontend/
│   ├── public/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   │   ├── admin/
│   │   │   └── customers/
│   │   ├── App.js
│   │   └── index.js
│   │
│   ├── package.json
│   └── package-lock.json
│
└── Doc/
```

---

## 🚀 Getting Started

### Prerequisites

Make sure the following tools are installed:

* [.NET 9 SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/)
* npm
* Microsoft SQL Server
* Git

You may also use:

* Visual Studio 2022+
* Visual Studio Code
* SQL Server Management Studio

---

## 📥 Clone the Repository

```bash
git clone https://github.com/reza-akbari-dev/IdealShop-TWA-Project.git
cd IdealShop-TWA-Project
```

---

## 🗄️ Database Setup

The backend uses **SQL Server** and Entity Framework Core.

The default development connection is configured for a local SQL Server instance using the database:

```text
IdealShop
```

The connection string can be configured in:

```text
IdealShop-TWA-Project/IdealShop/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=IdealShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

Change this connection string if your SQL Server installation uses another server name, authentication method, or database configuration.

### Apply Entity Framework Migrations

Move into the backend project:

```bash
cd IdealShop-TWA-Project/IdealShop
```

Restore dependencies:

```bash
dotnet restore
```

Apply the database migrations:

```bash
dotnet ef database update
```

If the `dotnet ef` command is unavailable, install the Entity Framework CLI tool:

```bash
dotnet tool install --global dotnet-ef
```

---

## ⚙️ Run the Backend

From:

```text
IdealShop-TWA-Project/IdealShop
```

run:

```bash
dotnet run
```

The development profiles are configured for:

```text
https://localhost:7138
```

and:

```text
http://localhost:5076
```

If your browser does not trust the ASP.NET development HTTPS certificate, run:

```bash
dotnet dev-certs https --trust
```

---

## 💻 Run the Frontend

Open another terminal and move to the frontend:

```bash
cd idealshop-frontend
```

Install dependencies:

```bash
npm install
```

Start the React development server:

```bash
npm start
```

The application will normally be available at:

```text
http://localhost:3000
```

---

## 🔗 Frontend & Backend Communication

The React application communicates with the ASP.NET Core backend through HTTP API requests using **Axios**.

During development:

```text
React Frontend
http://localhost:3000
        │
        │ Axios / HTTP
        ▼
ASP.NET Core API
https://localhost:7138
        │
        │ Entity Framework Core
        ▼
SQL Server
IdealShop Database
```

The backend currently includes a CORS policy allowing requests from:

```text
http://localhost:3000
```

Authentication uses cookies, so frontend requests that require authentication should send credentials with the request.

---

## 🗺️ Application Routes

Some of the main frontend routes include:

| Route                      | Description           |
| -------------------------- | --------------------- |
| `/`                        | Home page             |
| `/products`                | Product catalog       |
| `/products/:id`            | Product details       |
| `/category/:categoryId`    | Products by category  |
| `/about`                   | About page            |
| `/register`                | Customer registration |
| `/login`                   | Customer login        |
| `/cart`                    | Shopping cart         |
| `/checkout`                | Checkout              |
| `/order-placed`            | Order confirmation    |
| `/admin-login`             | Admin login           |
| `/admin-panel`             | Admin dashboard       |
| `/admin/products`          | Manage products       |
| `/admin/products/create`   | Create product        |
| `/admin/products/edit/:id` | Edit product          |
| `/admin/categories`        | Manage categories     |
| `/admin/customers`         | Manage customers      |
| `/admin/admins`            | Manage administrators |

---

## 🔌 Backend API Areas

The ASP.NET Core backend separates the main functionality into controllers for:

```text
Admin
Customers
Products
Categories
Cart Items
Orders
```

This keeps the frontend and backend separated and allows the React application to communicate with the server using API requests.

---

## 🔐 Authentication

IdealShop uses **ASP.NET Core cookie authentication**.

The backend authentication scheme is:

```text
MyCookieAuth
```

Cookies are configured for cross-origin development between the React frontend and ASP.NET Core API.

The backend also enables:

* Authentication middleware
* Authorization middleware
* CORS
* HTTPS redirection
* Static file serving

---

## 📦 Frontend Commands

Inside `idealshop-frontend`:

### Start development server

```bash
npm start
```

### Create production build

```bash
npm run build
```

### Run tests

```bash
npm test
```

---

## 🧪 Backend Commands

Inside `IdealShop-TWA-Project/IdealShop`:

### Restore packages

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

### Update database

```bash
dotnet ef database update
```

---

## 📌 Current Development Configuration

| Service           | Address                  |
| ----------------- | ------------------------ |
| React Frontend    | `http://localhost:3000`  |
| ASP.NET HTTPS API | `https://localhost:7138` |
| ASP.NET HTTP API  | `http://localhost:5076`  |
| Database          | `IdealShop`              |
| Database Engine   | Microsoft SQL Server     |

---

## 🔮 Possible Future Improvements

* JWT or ASP.NET Identity authentication
* Role-based authorization
* Online payment integration
* Order history for customers
* Product search and advanced filtering
* Product reviews and ratings
* Wishlist functionality
* Inventory and stock management
* Email order confirmations
* Docker support
* Automated tests
* CI/CD deployment pipeline
* Cloud database and production deployment

---

## 👨‍💻 Author

**Reza Akbari**

GitHub: [@reza-akbari-dev](https://github.com/reza-akbari-dev)

Project Repository:
[IdealShop-TWA-Project](https://github.com/reza-akbari-dev/IdealShop-TWA-Project)

---

## 🤝 Contributing

Contributions, suggestions, and improvements are welcome.

1. Fork the repository.
2. Create a new branch.

```bash
git checkout -b feature/your-feature
```

3. Commit your changes.

```bash
git commit -m "Add your feature"
```

4. Push your branch.

```bash
git push origin feature/your-feature
```

5. Open a Pull Request.

---

## ⭐ Support

If you find this project useful, consider giving the repository a **star ⭐** on GitHub.
