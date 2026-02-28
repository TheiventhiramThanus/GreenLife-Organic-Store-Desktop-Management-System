# GreenLife Organic Store — Test Plan

> **Project:** GreenLife Organic Store  
> **Version:** 1.0  
> **Database:** SQL Server Express — GreenLifeDB  
> **Total Test Cases:** 15 (5 Database + 10 System)  

---

## A. Database Test Cases (5)

| Test ID | Module / Feature | Test Scenario | Test Data | Expected Outcome |
|---------|-----------------|---------------|-----------|------------------|
| DB-01 | Database — Schema | Verify all 6 tables are created with correct columns and constraints | Run `DatabaseSchema.sql` on a fresh SQL Server instance | Tables `Products`, `Customers`, `Orders`, `OrderItems`, `Reviews`, `Admins` are created. `IDENTITY` keys auto-increment. `UNIQUE` constraint on `Customers.Email` and `Admins.Username`. `CHECK` constraint on `Orders.Status` allows only `Pending`, `Shipped`, `Delivered`. `CHECK` constraint on `Reviews.Rating` allows only 1–5. All foreign keys enforced. |
| DB-02 | Database — Referential Integrity | Insert an `OrderItem` referencing a non-existent `ProductId` | `INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, LineTotal) VALUES (1, 99999, 'Ghost', 1, 100.00, 100.00)` | SQL Server returns foreign key violation error. Row is **not** inserted. Referential integrity is preserved. |
| DB-03 | Database — Unique Constraint | Insert two customers with the same email address | First: `INSERT INTO Customers (FullName, Email, Phone, Address, Password) VALUES ('User A', 'dup@test.com', '0770000000', 'Colombo', 'hash1')` — Second: same email `'dup@test.com'` with different name | First insert succeeds. Second insert fails with `UNIQUE constraint violation` on `Email`. Only one row exists for that email. |
| DB-04 | Database — Index Performance | Search customers by email using the indexed column | `SELECT * FROM Customers WHERE Email = 'sarah.johnson@email.com'` — Run `SET STATISTICS IO ON` before execution | Query uses `idx_customers_email` index seek (verify in execution plan). Logical reads are minimal (typically 2–3). Result returns the correct customer row. |
| DB-05 | Database — Cascade & Default Values | Insert a new order without specifying `Status` and `OrderDate` | `INSERT INTO Orders (CustomerId, CustomerName, Subtotal, DiscountTotal, GrandTotal) VALUES (1, 'Test User', 500.00, 50.00, 450.00)` | Row is inserted successfully. `Status` defaults to `'Pending'`. `OrderDate` defaults to `GETDATE()` (current timestamp). Verify with `SELECT * FROM Orders WHERE CustomerId = 1 ORDER BY OrderId DESC`. |

---

## B. System Test Cases (10)

| Test ID | Module / Feature | Test Scenario | Test Data / Steps | Expected Outcome |
|---------|-----------------|---------------|-------------------|------------------|
| SYS-01 | Authentication — Admin Login | Login with valid admin credentials | Username: `admin`, Password: `admin123`. Click **Login**. | Success dialog: `"Welcome, System Administrator!"`. Admin Dashboard opens with 4 metric cards, sales chart, and navigation buttons. |
| SYS-02 | Authentication — Customer Login | Login with invalid credentials and verify error handling | Email: `wrong@test.com`, Password: `wrongpass`. Click **Login**. | Error dialog: `"Invalid email or password."`. Password field clears. User remains on Login form. No crash or unhandled exception. |
| SYS-03 | Customer Registration | Register a new customer with full validation | Full Name: `Test User`, Email: `testuser@greenlife.com`, Phone: `0771234567`, Address: `123 Green St, Colombo`, Password: `test123`, Confirm: `test123`. Click **Register**. | Success dialog: `"Registration successful!"`. Password stored as 64-character SHA-256 hash in database (verify: `SELECT Password FROM Customers WHERE Email = 'testuser@greenlife.com'`). Form closes with `DialogResult.OK`. |
| SYS-04 | Password Encryption | Verify password is hashed before storage and plain text is never stored | Register with Password: `mypassword`. After registration, query: `SELECT Password FROM Customers WHERE Email = '...'` | Stored value is `89e01536ac207279409d4de1e5253e01f4a1769e696db0d6062ca9b8f56767c8` (SHA-256 of `mypassword`). Value is exactly 64 hex characters. Plain text `mypassword` does **not** appear anywhere in the database. |
| SYS-05 | Browse Products & Multi-Field Search | Search products using supplier name instead of product name | Navigate: Customer Dashboard ? Browse Products. Type `"Organic Farms"` in search box. | Products from supplier `"Organic Farms"` are displayed. Search works across Name, Supplier, Description, and ProductId fields simultaneously. Category filter combines with text search correctly. |
| SYS-06 | Shopping Cart & Checkout | Add items to cart, modify quantity, and complete checkout | Browse Products ? Add `2x` Apple, `3x` Banana ? View Cart ? Change Apple qty to `5` in grid ? Click **Proceed to Checkout** ? Confirm **Yes**. | Cart shows updated quantities and recalculated totals. Order confirmation dialog shows Order ID and Grand Total. Stock is deducted in database (verify: `SELECT Stock FROM Products WHERE Name = 'Apple'`). Cart clears after success. |
| SYS-07 | Customer Dashboard Graph | Verify the spending overview chart displays correct data for the last 6 months | Login as a customer who has orders. Navigate to Customer Dashboard. | Chart displays 6 bars (one per month) with green bars for spending amounts and yellow line for order count. Months with no orders show 0. Data matches `SELECT SUM(GrandTotal), COUNT(*) FROM Orders WHERE CustomerId = X GROUP BY MONTH(OrderDate)`. |
| SYS-08 | Admin Dashboard Graph | Verify the sales chart displays the last 7 days of data | Login as Admin. View the Admin Dashboard. | Chart shows 7 data points (last 7 days). Green bars represent daily revenue. Yellow line represents daily order count. Metric cards show correct counts for Products, Low Stock, Customers, and Orders. |
| SYS-09 | Report Download — CSV & PDF | Export a Sales Report as both CSV and PDF and verify content | Admin Dashboard ? Reports ? Sales Report tab ? Click **Export CSV** ? Save ? Click **Export PDF** ? Save. | CSV file: Opens in Excel/text editor with headers `Date,Orders,Revenue` and all data rows. Values with commas are properly quoted. PDF file: Opens in PDF reader with title `"GreenLife Organic Store"`, report name, date, data table, `"Total Records: N"`, and `"Grand Total: LKR X.XX"`. Multi-page PDF generated if data exceeds 45 lines. |
| SYS-10 | Report Preview — Print Preview | Generate a print preview for the Stock Report with many products | Admin Dashboard ? Reports ? Stock Report tab ? Click **Preview**. | PrintPreviewDialog opens. Page 1 shows: store name header, `"Stock Report"` title, `"Page 1"` top-right, generation date, green separator line, green column headers (Product, Category, Stock), alternating row colors. If data exceeds one page: Page 2+ shows same header with updated page number. Last page shows green separator, `"Total Records: N"`, and total stock count. Every page has centered footer: `"GreenLife Organic Store | Stock Report | Page N | date"`. |

---

## Test Execution Summary Template

| Test ID | Tester | Date | Status | Remarks |
|---------|--------|------|--------|---------|
| DB-01   |        |      | Pass / Fail |   |
| DB-02   |        |      | Pass / Fail |   |
| DB-03   |        |      | Pass / Fail |   |
| DB-04   |        |      | Pass / Fail |   |
| DB-05   |        |      | Pass / Fail |   |
| SYS-01  |        |      | Pass / Fail |   |
| SYS-02  |        |      | Pass / Fail |   |
| SYS-03  |        |      | Pass / Fail |   |
| SYS-04  |        |      | Pass / Fail |   |
| SYS-05  |        |      | Pass / Fail |   |
| SYS-06  |        |      | Pass / Fail |   |
| SYS-07  |        |      | Pass / Fail |   |
| SYS-08  |        |      | Pass / Fail |   |
| SYS-09  |        |      | Pass / Fail |   |
| SYS-10  |        |      | Pass / Fail |   |

---

*© 2025 GreenLife Organic Store — Test Plan Document*
