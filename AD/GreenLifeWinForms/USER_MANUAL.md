# GreenLife Organic Store — User Manual

> **Version:** 1.0  
> **Platform:** Windows Desktop (WinForms — .NET Framework 4.7.2)  
> **Database:** SQL Server Express  

---

## Table of Contents

1. [System Overview](#1-system-overview)  
2. [Getting Started](#2-getting-started)  
3. [Role Selection Screen](#3-role-selection-screen)  
4. [Customer Module](#4-customer-module)  
   - 4.1 [Customer Login](#41-customer-login)  
   - 4.2 [Customer Registration](#42-customer-registration)  
   - 4.3 [Customer Dashboard](#43-customer-dashboard)  
   - 4.4 [Browse Products](#44-browse-products)  
   - 4.5 [Shopping Cart & Checkout](#45-shopping-cart--checkout)  
   - 4.6 [Track Orders](#46-track-orders)  
   - 4.7 [My Reviews](#47-my-reviews)  
5. [Admin Module](#5-admin-module)  
   - 5.1 [Admin Login](#51-admin-login)  
   - 5.2 [Admin Dashboard](#52-admin-dashboard)  
   - 5.3 [Product Management](#53-product-management)  
   - 5.4 [Add / Edit Product](#54-add--edit-product)  
   - 5.5 [Customer Management](#55-customer-management)  
   - 5.6 [Order Management](#56-order-management)  
   - 5.7 [Reports](#57-reports)  
6. [Chatbot Assistant](#6-chatbot-assistant)  
7. [Email Notifications](#7-email-notifications)  
8. [Password Encryption](#8-password-encryption)  
9. [Multi-Field Search](#9-multi-field-search)  
10. [Troubleshooting](#10-troubleshooting)  

---

## 1. System Overview

GreenLife Organic Store is a desktop point-of-sale and management application for an organic food retail business. The system has two user roles:

| Role | Access |
|------|--------|
| **Administrator** | Product, customer, order management; reports; dashboard analytics |
| **Customer** | Browse products, place orders, track deliveries, write reviews |

### Key Features

- Product catalog with categories, images, discounts, and star ratings  
- Shopping cart with quantity editing and checkout  
- Order placement with automatic stock deduction  
- Multi-tab reports with chart visualization  
- CSV and PDF report download  
- Print preview with pagination  
- SHA-256 password encryption  
- SMTP email notifications (welcome, order confirmation, status updates)  
- AI chatbot assistant with knowledge-base matching  
- Multi-field SQL search across all major entities  
- Real-time dashboard graphs (Admin: 7-day sales; Customer: 6-month spending)  

---

## 2. Getting Started

### Prerequisites

1. **Windows 10/11** with .NET Framework 4.7.2 or later  
2. **SQL Server Express** installed and running  
3. Database `GreenLifeDB` created using the provided `DatabaseSchema.sql`  
4. Seed data loaded using `SeedData.sql` (optional, provides demo data)  

### Launching the Application

1. Double-click `GreenLifeWinForms.exe`  
2. The application tests the database connection on startup  
3. If the connection succeeds, a confirmation dialog appears  
4. Click **OK** to proceed to the Role Selection screen  

> **If connection fails:** Ensure SQL Server Express is running and the connection string in `DatabaseManager.cs` matches your server name.

---

## 3. Role Selection Screen

This is the entry point of the application.

| Element | Description |
|---------|-------------|
| **Administrator** button (green) | Opens the Admin Login form |
| **Customer** button (yellow) | Opens the Customer Login form |

- After logging out of either module, you return to this screen  
- Close this window to exit the application  

---

## 4. Customer Module

### 4.1 Customer Login

| Field | Description |
|-------|-------------|
| **Email** | Your registered email address |
| **Password** | Your account password (masked) |
| **Login** button | Authenticates and opens the Customer Dashboard |
| **Register here** link | Opens the registration form for new customers |

**Demo credentials:** `sarah.johnson@email.com` / `password123`

- Press **Enter** in the password field to submit  
- If login fails, the password field clears and email is focused  
- Only **active** accounts can log in (deactivated accounts are blocked)  

---

### 4.2 Customer Registration

Fill in all fields to create a new account:

| Field | Rules |
|-------|-------|
| **Full Name** | Required |
| **Email** | Required; must contain `@` and `.`; must be unique |
| **Phone** | Required |
| **Address** | Required; multi-line input supported |
| **Password** | Required; minimum 6 characters |
| **Confirm Password** | Must match the password field |

**Steps:**
1. Fill in all fields  
2. Click **Register**  
3. The system checks for duplicate emails  
4. Password is encrypted with SHA-256 before storage  
5. A welcome email is sent to your address  
6. You are redirected to the login screen  

---

### 4.3 Customer Dashboard

The dashboard shows:

| Section | Description |
|---------|-------------|
| **Welcome message** | Displays your name |
| **Navigation buttons** | Five green buttons for all features |
| **Spending Overview chart** | Bar chart (spending) + line chart (order count) for the last 6 months |
| **Assistant button** (blue) | Opens the AI chatbot |
| **Logout** button (red) | Returns to the Role Selection screen |

**Navigation buttons:**

| Button | Opens |
|--------|-------|
| Browse Products | Product catalog with search and categories |
| View Cart | Shopping cart (via Browse Products) |
| Track Orders | Order history and details |
| My Reviews | Your product reviews |
| My Profile | Profile information |

---

### 4.4 Browse Products

A visual product catalog with card-based layout.

#### Product Cards

Each card displays:
- **Product image** (loaded from file or auto-generated with category emoji)  
- **Discount badge** (red, top-right corner — shown only for discounted items)  
- **Product name** and **category**  
- **Star rating** (????? format with rating value and review count)  
- **Price** (green for regular, red for discounted; original price shown with strikethrough)  
- **Stock status** (green = In Stock, yellow = Low Stock, red = Out of Stock)  
- **Quantity selector** (spinner; max = available stock; disabled if out of stock)  
- **Add to Cart** button (green; disabled and grey if out of stock)  
- **View Details** button (blue; shows full product info in a dialog)  

#### Search & Filter

| Control | Function |
|---------|----------|
| **Search** text box | Searches across product name, supplier, description, and ID |
| **Category** dropdown | Filters by category (select "All" for everything) |
| Results update in **real-time** as you type |

#### Cart Badge

- The red badge next to **View Cart** shows the total item count  
- Click **View Cart** to proceed to the Shopping Cart  

---

### 4.5 Shopping Cart & Checkout

#### Cart View

| Column | Description |
|--------|-------------|
| ID | Product ID |
| Product Name | Item name |
| Unit Price | Original price (LKR) |
| Discount % | Applied discount |
| Discounted Price | Price after discount |
| Quantity | **Editable** — change directly in the grid |
| Line Total | Quantity × discounted price |

#### Summary Panel

| Field | Description |
|-------|-------------|
| **Subtotal** | Sum of all original prices × quantities |
| **Discount** | Total savings (red) |
| **Grand Total** | Final amount to pay (green, large font) |

#### Actions

| Button | Function |
|--------|----------|
| **Clear Cart** (red) | Removes all items after confirmation |
| **Remove Item** (yellow) | Removes the selected item after confirmation |
| **Proceed to Checkout** (green) | Shows order summary and places the order |

#### Checkout Process

1. Click **Proceed to Checkout**  
2. Review the order summary dialog  
3. Click **Yes** to confirm  
4. The system:  
   - Creates the order in the database  
   - Inserts all order items  
   - Deducts stock for each product  
   - Sends an order confirmation email  
5. Order ID is displayed in a success message  
6. Cart is cleared automatically  

---

### 4.6 Track Orders

Displays all your orders in a DataGridView.

| Column | Description |
|--------|-------------|
| Order ID | Unique order identifier |
| Order Date | When the order was placed |
| Status | Pending / Shipped / Delivered |
| Subtotal | Pre-discount total |
| Discount | Discount amount |
| Total | Final amount paid |

#### View Order Details

1. Select an order row  
2. Click **View Details**  
3. A details window opens showing:  
   - Order information (ID, date, status, customer)  
   - Item-by-item breakdown (product, quantity, unit price, discount, line total)  
   - Totals summary  

---

### 4.7 My Reviews

Manage your product reviews:

| Action | Description |
|--------|-------------|
| **View reviews** | All your reviews displayed in a DataGridView |
| **Add Review** | Select a product, set rating (1–5 stars), write comment |
| **Edit Review** | Modify an existing review's rating or comment |
| **Delete Review** | Remove a review after confirmation |

---

## 5. Admin Module

### 5.1 Admin Login

| Field | Description |
|-------|-------------|
| **Username** | Admin username |
| **Password** | Admin password (masked) |
| **Login** button | Authenticates and opens the Admin Dashboard |

**Demo credentials:** `admin` / `admin123`

- Press **Enter** in the password field to submit  
- Supports both hashed and legacy plain-text passwords  

---

### 5.2 Admin Dashboard

#### Metric Cards (top panel)

Four colored cards showing real-time statistics:

| Card | Color | Data Source |
|------|-------|-------------|
| **Total Products** | Green | Count of active products |
| **Low Stock** | Red | Products with stock < 50 |
| **Total Customers** | Blue | Active customer count |
| **Total Orders** | Yellow | Total order count |

#### Sales Overview Chart

- **Bar chart** (green): Daily revenue for the last 7 days  
- **Line chart** (yellow): Daily order count for the last 7 days  

#### Navigation Buttons

| Button | Opens |
|--------|-------|
| Product Management | Full product CRUD |
| Customer Management | Customer list with activate/deactivate |
| Order Management | Order status updates |
| Reports | Sales, stock, and customer reports |

#### Other Buttons

| Button | Function |
|--------|----------|
| **Assistant** (blue) | Opens the AI chatbot |
| **Logout** (red) | Returns to the Role Selection screen |

> **Note:** Metrics refresh automatically when you return from any management form.

---

### 5.3 Product Management

A full CRUD interface for products.

#### DataGridView Columns

| Column | Description |
|--------|-------------|
| ID | Product ID |
| Name | Product name |
| Category | Product category |
| Supplier | Supplier name |
| Price (LKR) | Unit price |
| Stock | Current stock level |
| Discount % | Discount percentage |
| Active | Whether the product is active |

#### Search

- Type in the search box to filter across **name, supplier, description, and ID**  
- Results update in real-time  

#### Action Buttons

| Button | Function |
|--------|----------|
| **Add Product** (green) | Opens the Add Product form |
| **Edit Product** (blue) | Opens the Edit form for the selected product |
| **Delete Product** (red) | Soft-deletes the product (sets IsActive = 0) after confirmation |
| **Refresh** (blue) | Reloads all products and clears search |
| **Close** (grey) | Closes the form |

---

### 5.4 Add / Edit Product

A form for creating or modifying products.

| Field | Rules |
|-------|-------|
| **Name** | Required |
| **Category** | Dropdown selection (Fruits, Vegetables, Dairy, Grains, Beverages, Snacks, Meat & Poultry, Bakery, Seafood, Spreads) |
| **Supplier** | Required |
| **Price** | Required; must be a valid positive number |
| **Stock** | Required; must be a whole number ? 0 |
| **Discount %** | 0–100; defaults to 0 |
| **Description** | Optional; multi-line |
| **Image** | Optional; file name or path |

- In **Edit** mode, all fields are pre-populated with existing data  
- Click **Save** to commit changes, **Cancel** to discard  

---

### 5.5 Customer Management

View and manage all customer accounts.

#### DataGridView Columns

| Column | Description |
|--------|-------------|
| ID | Customer ID |
| Full Name | Customer's name |
| Email | Email address |
| Phone | Phone number |
| Address | Delivery address |
| Active | Account status (True/False) |
| Registered | Registration date |

> The **Password** column is hidden for security.

#### Search

- Type in the search box to filter across **name, email, phone, address, and ID**  
- Results update in real-time  

#### Action Buttons

| Button | Function |
|--------|----------|
| **View Details** (blue) | Shows full customer information in a dialog |
| **View Orders** (green) | Shows a list of all orders placed by the customer |
| **Deactivate** (red) | Disables the customer's login ability after confirmation |
| **Activate** (green) | Re-enables a deactivated account after confirmation |
| **Close** (grey) | Closes the form |

---

### 5.6 Order Management

View and update all orders in the system.

#### DataGridView Columns

| Column | Description |
|--------|-------------|
| Order ID | Unique identifier |
| Customer | Customer name |
| Order Date | When the order was placed |
| Status | Current status |
| Subtotal | Pre-discount total |
| Discount | Discount amount |
| Grand Total | Final amount |

#### Search

- Search across **customer name, order ID, status, and customer ID**  

#### Action Buttons

| Button | Function |
|--------|----------|
| **View Details** | Opens order details with item breakdown |
| **Update Status** | Change status to Pending / Shipped / Delivered / Cancelled |
| **Refresh** | Reloads all orders |
| **Close** | Closes the form |

#### Status Update

1. Select an order row  
2. Click **Update Status**  
3. Choose the new status from the dropdown dialog  
4. Click **OK** to confirm  
5. An email notification is sent to the customer about the status change  

---

### 5.7 Reports

A comprehensive reporting interface with three tabs.

#### Report Tabs

| Tab | Description | Chart Type | Grid Columns |
|-----|-------------|------------|--------------|
| **Sales Report** | Revenue and order count grouped by date | Bar (revenue) + Line (orders) | Date, Orders, Revenue |
| **Stock Report** | Product stock levels sorted lowest first | Bar (stock levels, top 12) | Product, Category, Stock |
| **Customer History** | Customer spending ranked by total spent | Bar (spent) + Line (orders) | Customer, Email, Orders, Spent |

#### Export & Preview Buttons

| Button | Function |
|--------|----------|
| **Preview** | Opens a print preview dialog with paginated report |
| **Export CSV** | Saves the report data as a CSV file |
| **Export PDF** | Saves the report as a multi-page PDF file |

#### Report Preview Details

The print preview generates a professional report with:

- **Page header** (every page): Store name, report title, page number, generation date  
- **Green separator line** below the header  
- **Column headers**: Green background with white text  
- **Alternating row colors**: White and light green  
- **Summary footer** (last page): Total records count and grand total  
- **Page footer** (every page): Centered store name, report name, page number, and date  
- **Multi-page support**: Automatically paginates when data exceeds one page  

#### PDF Export Details

The exported PDF includes:

- Store name and report title header  
- Generation timestamp  
- Full data table with all rows  
- Summary footer with total records and grand total  
- Page numbers on each page (`Page 1 of N`)  
- Multi-page support for large datasets  

#### CSV Export Details

- Standard comma-separated format  
- Headers row followed by data rows  
- Proper escaping for values containing commas or quotes  
- Option to open the file immediately after export  

---

## 6. Chatbot Assistant

The AI-powered chatbot is accessible from both dashboards via the **?? Assistant** button.

### How to Use

1. Click the **?? Assistant** button on any dashboard  
2. The chatbot window opens with a welcome message  
3. Type your question in the input box and press **Enter** or click **?**  
4. The bot responds with relevant information  
5. Use the **quick-reply chips** at the bottom for common questions  

### Topics the Chatbot Can Help With

| Topic | Example Questions |
|-------|-------------------|
| **Products** | "What products do you have?", "Show me categories" |
| **Ordering** | "How do I place an order?", "How to buy?" |
| **Order Tracking** | "Track my order", "Where is my delivery?" |
| **Cart** | "How to use the shopping cart?" |
| **Account** | "How to register?", "How to login?" |
| **Password** | "How to change password?", "Forgot password" |
| **Payment** | "What payment methods?", "How to pay?" |
| **Returns** | "Return policy", "How to get a refund?" |
| **Delivery** | "How long does delivery take?", "Shipping info" |
| **Discounts** | "Any offers?", "Current promotions?" |
| **Contact** | "How to contact support?", "Phone number?" |
| **Store Info** | "About GreenLife", "Store hours?" |
| **Reviews** | "How to rate products?", "Write a review" |

### Quick Reply Chips

Pre-built buttons for instant questions:
- What products do you have?  
- How do I place an order?  
- Track my order  
- Payment methods  
- Return policy  
- Contact support  

---

## 7. Email Notifications

The system sends automated emails at key events:

| Event | Email Type | Content |
|-------|------------|---------|
| **New registration** | Welcome Email | Greeting, feature overview, call to action |
| **Order placed** | Order Confirmation | Order ID, item count, grand total |
| **Status change** | Status Update | New status with emoji indicator |
| **Password change** | Password Changed | Security notification |

### Email Configuration

To enable email notifications, update the SMTP settings in `Services/EmailNotificationService.cs`:

| Setting | Default Value | Description |
|---------|---------------|-------------|
| `smtpHost` | `smtp.gmail.com` | Your SMTP server |
| `smtpPort` | `587` | SMTP port (TLS) |
| `smtpUser` | `greenlife.organic.store@gmail.com` | Sender email |
| `smtpPass` | `your-app-password-here` | App password |
| `enableSsl` | `true` | SSL/TLS encryption |

> **Gmail users:** Generate an App Password at https://myaccount.google.com/apppasswords  
> **Note:** Emails fail silently — the application continues to work even if email sending fails.

---

## 8. Password Encryption

All passwords are encrypted using **SHA-256** hashing before storage.

### How It Works

1. User enters a plain-text password during registration or login  
2. The system computes the SHA-256 hash of the password  
3. The hash (64-character hex string) is stored in the database  
4. During login, the entered password is hashed and compared to the stored hash  
5. Passwords are **never stored in plain text**  

### Legacy Support

- The login system supports both hashed and plain-text passwords  
- If a stored password matches the hash ? authenticated  
- If a stored password matches plain text ? authenticated (backward compatibility)  
- New registrations always store hashed passwords  

---

## 9. Multi-Field Search

All search boxes in the application use multi-field SQL search for comprehensive results.

| Form | Fields Searched |
|------|----------------|
| **Product Management** / **Browse Products** | Name, Supplier, Description, Product ID |
| **Customer Management** | Full Name, Email, Phone, Address, Customer ID |
| **Order Management** | Customer Name, Order ID, Status, Customer ID |

### How It Works

- Type any keyword in the search box  
- The system searches across **all listed fields simultaneously** using SQL `LIKE` with wildcards  
- Results update in real-time as you type  
- Partial matches are supported (e.g., searching "john" finds "Johnson")  
- Clear the search box or click **Refresh** to show all records  

---

## 10. Troubleshooting

| Problem | Solution |
|---------|----------|
| **Database connection failed** | Ensure SQL Server Express is running; verify server name in `DatabaseManager.cs` |
| **Login fails with correct credentials** | Check if the account is active (Admin: Customer Management ? Activate) |
| **Products not showing** | Ensure products have `IsActive = 1` in the database |
| **Cart is empty after checkout** | This is expected — cart clears after successful order |
| **Email not received** | Update SMTP settings in `EmailNotificationService.cs` with valid credentials |
| **PDF looks empty** | Ensure there is data in the report grid before exporting |
| **Charts not showing data** | Verify that orders/products exist in the database |
| **Search returns no results** | Try a shorter/broader search term; check spelling |
| **Application won't start** | Ensure .NET Framework 4.7.2 is installed; check the Event Viewer for errors |
| **Chatbot doesn't understand** | Try simpler keywords; type "help" for topic list |

---

## Application Navigation Map

```
GreenLifeWinForms.exe
  ?? Role Selection
       ?? Administrator
       ?    ?? Admin Login
       ?         ?? Admin Dashboard
       ?              ?? Product Management
       ?              ?    ?? Add/Edit Product
       ?              ?? Customer Management
       ?              ?? Order Management
       ?              ?? Reports (Sales / Stock / Customer)
       ?              ?    ?? Preview
       ?              ?    ?? Export CSV
       ?              ?    ?? Export PDF
       ?              ?? Chatbot Assistant
       ?
       ?? Customer
            ?? Customer Login
            ?    ?? Customer Registration
            ?? Customer Dashboard
                 ?? Browse Products
                 ?    ?? Shopping Cart
                 ?         ?? Checkout
                 ?? Track Orders
                 ?    ?? Order Details
                 ?? My Reviews
                 ?? Chatbot Assistant
```

---

*© 2025 GreenLife Organic Store. All rights reserved.*
