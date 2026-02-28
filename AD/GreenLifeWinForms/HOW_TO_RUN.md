# 🚀 How to Run the GreenLife Windows Forms Application

## ✅ Prerequisites

Before running the application, ensure you have:

1. **Visual Studio 2019 or later** installed
2. **SQL Server Express** installed and running
3. **SQL Server instance:** `MSI\SQLEXPRESS` (already configured)
4. **.NET Framework 4.7.2** or later

---

## 🎯 Quick Start (3 Steps)

### Step 1: Open the Solution
1. Navigate to: `c:\Users\thanu\OneDrive\Desktop\AD\GreenLifeWinForms\`
2. Double-click: **`GreenLifeWinForms.sln`**
3. Visual Studio will open the project

### Step 2: Build the Project
1. In Visual Studio, press **`Ctrl + Shift + B`** (or click Build → Build Solution)
2. Wait for the build to complete
3. Check the Output window for any errors

### Step 3: Run the Application
1. Press **`F5`** (or click Debug → Start Debugging)
2. The application will:
   - Connect to SQL Server
   - Create the database automatically (if it doesn't exist)
   - Create all tables
   - Insert sample data
   - Show a success message
   - Launch the Role Selection screen

---

## 🎮 Using the Application

### First Launch
When you run the application for the first time:
1. ✅ Database connection test runs automatically
2. ✅ Success message appears if connected
3. ✅ Role Selection screen appears

### Role Selection Screen
You'll see two options:
- **👤 Administrator** - For admin users
- **🛒 Customer** - For customers

---

## 🔐 Login Credentials

### Admin Portal
Click "Administrator" and use:
- **Username:** `admin`
- **Password:** `admin123`

### Customer Portal
Click "Customer" and use:
- **Email:** `sarah.johnson@email.com`
- **Password:** `password123`

**Other demo customers:**
- `michael.chen@email.com` / `password123`
- `emily.davis@email.com` / `password123`

---

## 📊 What You'll See

### Admin Dashboard
After admin login, you'll see:
- **Metrics Cards:**
  - Total Products: 25
  - Low Stock: (varies)
  - Total Customers: 3
  - Total Orders: 5

- **Navigation Buttons:**
  - 📦 Product Management (coming soon)
  - 👥 Customer Management (coming soon)
  - 📋 Order Management (coming soon)
  - 📊 Reports (coming soon)

### Customer Dashboard
After customer login, you'll see:
- **Welcome message** with customer name
- **Navigation Buttons:**
  - 🛒 Browse Products (coming soon)
  - 🛍️ View Cart (coming soon)
  - 📦 Track Orders (coming soon)
  - ⭐ My Reviews (coming soon)
  - 👤 My Profile (coming soon)

---

## 🗄️ Database Auto-Creation

The application automatically:
1. **Checks** if `GreenLifeDB` database exists
2. **Creates** the database if needed
3. **Creates** all 6 tables:
   - Products
   - Customers
   - Orders
   - OrderItems
   - Reviews
   - Admins
4. **Inserts** sample data:
   - 1 admin account
   - 3 customer accounts
   - 25 products
   - 5 orders
   - 8 reviews

---

## 🔧 Troubleshooting

### Error: "Cannot connect to database"
**Solution:**
1. Verify SQL Server Express is running
2. Open **SQL Server Configuration Manager**
3. Start **SQL Server (SQLEXPRESS)** service
4. Try running the app again

### Error: "Login failed"
**Solution:**
1. Ensure Windows Authentication is enabled
2. Your Windows account needs permissions
3. Try connecting via SQL Server Management Studio first

### Error: "Database already exists"
**This is normal!** The app detects existing database and skips creation.

### Build Errors
**Solution:**
1. Right-click solution → **Restore NuGet Packages**
2. Clean Solution (**Build → Clean Solution**)
3. Rebuild Solution (**Build → Rebuild Solution**)

---

## 📁 Project Structure

```
GreenLifeWinForms/
├── GreenLifeWinForms.sln          ← Open this file
├── GreenLifeWinForms.csproj       ← Project file
├── Program.cs                     ← Entry point
├── App.config                     ← Configuration
├── Database/
│   ├── DatabaseManager.cs         ← Database operations
│   ├── DatabaseSchema.sql         ← Table definitions
│   └── SeedData.sql               ← Sample data
├── Models/                        ← Entity classes
├── Services/                      ← Business logic
└── Forms/                         ← UI forms
    ├── Common/
    │   └── RoleSelectionForm.cs
    ├── Admin/
    │   ├── AdminLoginForm.cs
    │   └── AdminDashboardForm.cs
    └── Customer/
        ├── CustomerLoginForm.cs
        └── CustomerDashboardForm.cs
```

---

## ✅ Verification Checklist

After running the application:

- [ ] Application starts without errors
- [ ] Database connection success message appears
- [ ] Role Selection screen displays
- [ ] Admin login works with demo credentials
- [ ] Admin dashboard shows metrics
- [ ] Customer login works with demo credentials
- [ ] Customer dashboard displays

---

## 🎯 Current Features (Working)

✅ **Database Connection** - SQL Server Express  
✅ **Auto Database Creation** - Creates DB and tables  
✅ **Sample Data** - Pre-populated with demo data  
✅ **Role Selection** - Choose Admin or Customer  
✅ **Admin Login** - Username/password authentication  
✅ **Customer Login** - Email/password authentication  
✅ **Admin Dashboard** - Metrics and navigation  
✅ **Customer Dashboard** - Welcome and navigation  

---

## 🚧 Coming Soon

⏳ Product Management  
⏳ Customer Management  
⏳ Order Management  
⏳ Shopping Cart  
⏳ Reports  
⏳ Customer Registration  
⏳ Profile Management  

---

## 🎨 UI Design

The application uses an **eco-friendly green theme**:
- **Primary Color:** Green (RGB 22, 163, 74)
- **Secondary Color:** Yellow (RGB 251, 191, 36)
- **Background:** Light Green (RGB 240, 253, 244)
- **Text:** Dark Green (RGB 20, 83, 45)
- **Font:** Segoe UI

---

## 📞 Quick Commands

### Build
```
Ctrl + Shift + B
```

### Run (Debug)
```
F5
```

### Run (Without Debug)
```
Ctrl + F5
```

### Stop
```
Shift + F5
```

---

## 🎉 Success!

If you see the Role Selection screen, **congratulations!** Your application is running successfully.

You can now:
1. Test admin login
2. Test customer login
3. Explore the dashboards
4. Verify database was created in SQL Server

---

**Need help?** Check the troubleshooting section or review the SQL_SERVER_MIGRATION.md file.

**Ready to continue development?** See IMPLEMENTATION_STATUS.md for next steps.
