-- GreenLife Organic Store Database Schema
-- SQL Server (T-SQL)

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GreenLifeDB')
BEGIN
    CREATE DATABASE GreenLifeDB;
END
GO

USE GreenLifeDB;
GO

-- Products Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(200) NOT NULL,
        Category NVARCHAR(100) NOT NULL,
        Supplier NVARCHAR(200) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,
        Discount DECIMAL(5,2) DEFAULT 0,
        IsActive BIT DEFAULT 1,
        Image NVARCHAR(500),
        Description NVARCHAR(MAX),
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Customers Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerId INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(200) NOT NULL,
        Email NVARCHAR(200) UNIQUE NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        Address NVARCHAR(500) NOT NULL,
        Password NVARCHAR(200) NOT NULL,
        IsActive BIT DEFAULT 1,
        RegisteredDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Orders Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderId INT PRIMARY KEY IDENTITY(1,1),
        CustomerId INT NOT NULL,
        CustomerName NVARCHAR(200) NOT NULL,
        OrderDate DATETIME DEFAULT GETDATE(),
        Status NVARCHAR(20) CHECK(Status IN ('Pending', 'Shipped', 'Delivered')) DEFAULT 'Pending',
        Subtotal DECIMAL(10,2) NOT NULL,
        DiscountTotal DECIMAL(10,2) DEFAULT 0,
        GrandTotal DECIMAL(10,2) NOT NULL,
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- OrderItems Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        OrderItemId INT PRIMARY KEY IDENTITY(1,1),
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        ProductName NVARCHAR(200) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        Discount DECIMAL(5,2) DEFAULT 0,
        LineTotal DECIMAL(10,2) NOT NULL,
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
    );
END
GO

-- Reviews Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        ReviewId INT PRIMARY KEY IDENTITY(1,1),
        ProductId INT NOT NULL,
        CustomerId INT NOT NULL,
        CustomerName NVARCHAR(200) NOT NULL,
        Rating INT CHECK(Rating >= 1 AND Rating <= 5) NOT NULL,
        Comment NVARCHAR(MAX),
        ReviewDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- Admins Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Admins')
BEGIN
    CREATE TABLE Admins (
        AdminId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(100) UNIQUE NOT NULL,
        Password NVARCHAR(200) NOT NULL,
        FullName NVARCHAR(200) NOT NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Indexes for Performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_products_category')
    CREATE INDEX idx_products_category ON Products(Category);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_products_name')
    CREATE INDEX idx_products_name ON Products(Name);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_customers_email')
    CREATE INDEX idx_customers_email ON Customers(Email);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_orders_customer')
    CREATE INDEX idx_orders_customer ON Orders(CustomerId);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_orders_status')
    CREATE INDEX idx_orders_status ON Orders(Status);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_orderitems_order')
    CREATE INDEX idx_orderitems_order ON OrderItems(OrderId);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_reviews_product')
    CREATE INDEX idx_reviews_product ON Reviews(ProductId);
GO

PRINT 'Database schema created successfully!';
