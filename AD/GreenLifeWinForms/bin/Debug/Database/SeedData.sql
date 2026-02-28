-- Seed Data for GreenLife Organic Store
-- SQL Server (T-SQL)

USE GreenLifeDB;
GO

-- Insert Default Admin
IF NOT EXISTS (SELECT * FROM Admins WHERE Username = 'admin')
BEGIN
    INSERT INTO Admins (Username, Password, FullName) VALUES 
    ('admin', 'admin123', 'System Administrator');
END
GO

-- Insert Sample Customers
IF NOT EXISTS (SELECT * FROM Customers WHERE Email = 'sarah.johnson@email.com')
BEGIN
    INSERT INTO Customers (FullName, Email, Phone, Address, Password) VALUES 
    ('Sarah Johnson', 'sarah.johnson@email.com', '555-0101', '123 Green Street, Eco City', 'password123'),
    ('Michael Chen', 'michael.chen@email.com', '555-0102', '456 Organic Ave, Nature Town', 'password123'),
    ('Emily Davis', 'emily.davis@email.com', '555-0103', '789 Fresh Lane, Garden City', 'password123');
END
GO

-- Insert Sample Products
IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Organic Apples')
BEGIN
    -- Fruits
    INSERT INTO Products (Name, Category, Supplier, Price, Stock, Discount, Image, Description) VALUES 
    ('Organic Apples', 'Fruits', 'Green Valley Farms', 4.99, 150, 10, 'apples.jpg', 'Fresh organic apples from local farms'),
    ('Organic Bananas', 'Fruits', 'Tropical Growers', 3.49, 200, 0, 'bananas.jpg', 'Sweet organic bananas, rich in potassium'),
    ('Organic Strawberries', 'Fruits', 'Berry Best Farm', 6.99, 80, 15, 'strawberries.jpg', 'Juicy organic strawberries, perfect for desserts'),
    ('Organic Oranges', 'Fruits', 'Citrus Grove', 5.49, 120, 0, 'oranges.jpg', 'Fresh organic oranges, packed with vitamin C'),
    ('Organic Blueberries', 'Fruits', 'Berry Best Farm', 8.99, 60, 20, 'blueberries.jpg', 'Antioxidant-rich organic blueberries'),

    -- Vegetables
    ('Organic Carrots', 'Vegetables', 'Root Harvest Co', 2.99, 180, 0, 'carrots.jpg', 'Crunchy organic carrots, great for snacking'),
    ('Organic Broccoli', 'Vegetables', 'Green Valley Farms', 3.99, 100, 5, 'broccoli.jpg', 'Fresh organic broccoli, nutrient-dense'),
    ('Organic Spinach', 'Vegetables', 'Leafy Greens Ltd', 4.49, 90, 10, 'spinach.jpg', 'Tender organic spinach leaves, iron-rich'),
    ('Organic Tomatoes', 'Vegetables', 'Vine Fresh Farm', 5.99, 110, 0, 'tomatoes.jpg', 'Vine-ripened organic tomatoes'),
    ('Organic Bell Peppers', 'Vegetables', 'Rainbow Harvest', 4.99, 75, 15, 'peppers.jpg', 'Colorful organic bell peppers'),

    -- Dairy
    ('Organic Milk', 'Dairy', 'Happy Cow Dairy', 5.99, 200, 0, 'milk.jpg', 'Fresh organic whole milk from grass-fed cows'),
    ('Organic Cheese', 'Dairy', 'Artisan Cheese Co', 8.99, 85, 10, 'cheese.jpg', 'Handcrafted organic cheddar cheese'),
    ('Organic Yogurt', 'Dairy', 'Happy Cow Dairy', 4.99, 120, 5, 'yogurt.jpg', 'Creamy organic yogurt with live cultures'),
    ('Organic Butter', 'Dairy', 'Creamery Delights', 6.49, 95, 0, 'butter.jpg', 'Rich organic butter from grass-fed cows'),

    -- Grains
    ('Organic Brown Rice', 'Grains', 'Whole Grain Mills', 7.99, 150, 0, 'rice.jpg', 'Nutritious organic brown rice'),
    ('Organic Quinoa', 'Grains', 'Ancient Grains Co', 9.99, 70, 10, 'quinoa.jpg', 'Protein-rich organic quinoa'),
    ('Organic Oats', 'Grains', 'Whole Grain Mills', 5.99, 130, 5, 'oats.jpg', 'Hearty organic rolled oats'),
    ('Organic Whole Wheat Flour', 'Grains', 'Stone Mill Bakery', 6.99, 100, 0, 'flour.jpg', 'Freshly milled organic whole wheat flour'),

    -- Beverages
    ('Organic Green Tea', 'Beverages', 'Tea Garden Co', 8.99, 90, 15, 'tea.jpg', 'Premium organic green tea leaves'),
    ('Organic Apple Juice', 'Beverages', 'Pure Press Juices', 6.99, 110, 0, 'juice.jpg', 'Cold-pressed organic apple juice'),
    ('Organic Coffee', 'Beverages', 'Fair Trade Roasters', 12.99, 65, 20, 'coffee.jpg', 'Ethically sourced organic coffee beans'),

    -- Snacks
    ('Organic Almonds', 'Snacks', 'Nutty Delights', 11.99, 80, 10, 'almonds.jpg', 'Raw organic almonds, heart-healthy'),
    ('Organic Granola', 'Snacks', 'Crunchy Goodness', 7.99, 95, 5, 'granola.jpg', 'Homemade organic granola with honey'),
    ('Organic Dark Chocolate', 'Snacks', 'Cocoa Dreams', 5.99, 120, 15, 'chocolate.jpg', '70% organic dark chocolate bar'),

    -- Meat & Poultry
    ('Organic Chicken Breast', 'Meat & Poultry', 'Free Range Farms', 14.99, 45, 0, 'chicken.jpg', 'Organic free-range chicken breast'),
    ('Organic Ground Beef', 'Meat & Poultry', 'Grass Fed Ranch', 16.99, 40, 10, 'beef.jpg', 'Organic grass-fed ground beef');
END
GO

-- Insert Sample Orders
IF NOT EXISTS (SELECT * FROM Orders WHERE OrderId = 1)
BEGIN
    SET IDENTITY_INSERT Orders ON;
    
    INSERT INTO Orders (OrderId, CustomerId, CustomerName, OrderDate, Status, Subtotal, DiscountTotal, GrandTotal) VALUES 
    (1, 1, 'Sarah Johnson', DATEADD(day, -5, GETDATE()), 'Delivered', 45.95, 4.60, 41.35),
    (2, 1, 'Sarah Johnson', DATEADD(day, -2, GETDATE()), 'Shipped', 32.47, 2.50, 29.97),
    (3, 2, 'Michael Chen', DATEADD(day, -3, GETDATE()), 'Delivered', 58.96, 8.99, 49.97),
    (4, 2, 'Michael Chen', DATEADD(day, -1, GETDATE()), 'Pending', 28.97, 0, 28.97),
    (5, 3, 'Emily Davis', DATEADD(day, -4, GETDATE()), 'Shipped', 41.96, 5.99, 35.97);
    
    SET IDENTITY_INSERT Orders OFF;
END
GO

-- Insert Sample Order Items
IF NOT EXISTS (SELECT * FROM OrderItems WHERE OrderId = 1)
BEGIN
    -- Order 1 (Sarah - Delivered)
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) VALUES 
    (1, 1, 'Organic Apples', 2, 4.99, 10, 8.98),
    (1, 3, 'Organic Strawberries', 1, 6.99, 15, 5.94),
    (1, 11, 'Organic Milk', 3, 5.99, 0, 17.97),
    (1, 13, 'Organic Yogurt', 2, 4.99, 5, 9.48);

    -- Order 2 (Sarah - Shipped)
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) VALUES 
    (2, 7, 'Organic Broccoli', 2, 3.99, 5, 7.58),
    (2, 8, 'Organic Spinach', 1, 4.49, 10, 4.04),
    (2, 15, 'Organic Brown Rice', 2, 7.99, 0, 15.98);

    -- Order 3 (Michael - Delivered)
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) VALUES 
    (3, 5, 'Organic Blueberries', 2, 8.99, 20, 14.38),
    (3, 21, 'Organic Coffee', 1, 12.99, 20, 10.39),
    (3, 22, 'Organic Almonds', 2, 11.99, 10, 21.58);

    -- Order 4 (Michael - Pending)
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) VALUES 
    (4, 6, 'Organic Carrots', 3, 2.99, 0, 8.97),
    (4, 9, 'Organic Tomatoes', 2, 5.99, 0, 11.98),
    (4, 17, 'Organic Oats', 1, 5.99, 5, 5.69);

    -- Order 5 (Emily - Shipped)
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) VALUES 
    (5, 12, 'Organic Cheese', 1, 8.99, 10, 8.09),
    (5, 16, 'Organic Quinoa', 1, 9.99, 10, 8.99),
    (5, 24, 'Organic Dark Chocolate', 2, 5.99, 15, 10.18);
END
GO

-- Insert Sample Reviews
IF NOT EXISTS (SELECT * FROM Reviews WHERE ProductId = 1)
BEGIN
    INSERT INTO Reviews (ProductId, CustomerId, CustomerName, Rating, Comment, ReviewDate) VALUES 
    (1, 1, 'Sarah Johnson', 5, 'Absolutely fresh and delicious! Best apples I have had.', DATEADD(day, -4, GETDATE())),
    (3, 1, 'Sarah Johnson', 5, 'Sweet and juicy strawberries. Perfect for my smoothies!', DATEADD(day, -4, GETDATE())),
    (11, 1, 'Sarah Johnson', 4, 'Great quality milk, tastes very fresh.', DATEADD(day, -4, GETDATE())),
    (5, 2, 'Michael Chen', 5, 'These blueberries are amazing! So fresh and flavorful.', DATEADD(day, -2, GETDATE())),
    (21, 2, 'Michael Chen', 5, 'Best organic coffee I have ever tasted. Rich and smooth.', DATEADD(day, -2, GETDATE())),
    (22, 2, 'Michael Chen', 4, 'Good quality almonds, fresh and crunchy.', DATEADD(day, -2, GETDATE())),
    (12, 3, 'Emily Davis', 5, 'Excellent cheese! Great flavor and texture.', DATEADD(day, -3, GETDATE())),
    (24, 3, 'Emily Davis', 5, 'Rich dark chocolate, not too sweet. Perfect!', DATEADD(day, -3, GETDATE()));
END
GO

PRINT 'Sample data inserted successfully!';
