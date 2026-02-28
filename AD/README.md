# 🌱 GreenLife Organic Store - C# Windows Forms Application

A comprehensive desktop application for organic store management built with **C# .NET Framework** and **Windows Forms**.

## 📋 Project Overview

This is a complete desktop application for managing an organic food store, featuring separate portals for administrators and customers, with full product management, order processing, and reporting capabilities.

## 🎯 Features

### Admin Portal
- **Dashboard**: View key metrics (total sales, products, orders, low stock alerts)
- **Product Management**: Add, edit, delete, and search products with category filtering
- **Customer Management**: View and manage customer information
- **Order Management**: Process orders and update order status (Pending → Shipped → Delivered)
- **Reports**: Generate and export sales reports, stock reports, and customer order history

### Customer Portal
- **User Registration & Login**: Secure authentication system
- **Browse Products**: Search and filter products by name, category, and price
- **Shopping Cart**: Add, update, and remove items with real-time total calculation
- **Checkout**: Place orders with automatic stock updates
- **Track Orders**: Monitor order status with visual timeline
- **Profile Management**: Update personal information
- **Product Reviews**: Rate and review purchased products
- **Order History**: View and export complete order history to CSV

## 🛠️ Technology Stack

- **Framework**: .NET Framework 4.7.2+
- **Language**: C# 7.0+
- **UI**: Windows Forms
- **Database**: SQLite (lightweight, file-based)
- **Data Access**: ADO.NET with System.Data.SQLite
- **Export**: CSV export functionality

## 📁 Project Structure

```
GreenLifeWinForms/
├── Database/
│   ├── DatabaseManager.cs          # Database connection & operations
│   ├── DatabaseSchema.sql          # Database schema
│   └── SeedData.sql                # Initial data
├── Models/
│   ├── Product.cs                  # Product entity
│   ├── Customer.cs                 # Customer entity
│   ├── Order.cs                    # Order entity
│   ├── OrderItem.cs                # Order item entity
│   ├── Review.cs                   # Review entity
│   └── Admin.cs                    # Admin entity
├── Services/
│   ├── AuthService.cs              # Authentication logic
│   ├── ProductService.cs           # Product CRUD operations
│   ├── CustomerService.cs          # Customer management
│   ├── OrderService.cs             # Order processing
│   └── ReviewService.cs            # Review management
├── Forms/
│   ├── Common/                     # Shared forms
│   ├── Admin/                      # Admin portal forms
│   └── Customer/                   # Customer portal forms
├── Utils/
│   ├── ValidationHelper.cs         # Input validation
│   ├── ExportHelper.cs             # CSV export
│   └── NotificationHelper.cs       # User notifications
└── Resources/
    └── Images/                     # Product images, icons
```

## 🚀 Getting Started

### Prerequisites
- **Visual Studio 2019 or later**
- **.NET Framework 4.7.2 or later**
- **Windows OS** (Windows 7 or later)

### Quick Start

1. **Open Visual Studio**
2. **Create New Project**
   - File → New → Project
   - Select "Windows Forms App (.NET Framework)"
   - Name: `GreenLifeWinForms`
   - Framework: `.NET Framework 4.7.2` or later

3. **Install NuGet Package**
   ```
   Install-Package System.Data.SQLite
   ```

4. **Add Project Files**
   - Copy all files from the `GreenLifeWinForms` folder
   - Add them to your Visual Studio project

5. **Build and Run**
   - Press `F5` to build and run

### Demo Credentials

**Admin Portal**
- Username: `admin`
- Password: `admin123`

**Customer Portal**
- Email: `sarah.johnson@email.com`
- Password: `password123`

## 📚 Documentation

- **[INDEX.md](GreenLifeWinForms/INDEX.md)** - Documentation navigation hub
- **[QUICK_START.md](GreenLifeWinForms/QUICK_START.md)** - 5-minute setup guide
- **[ARCHITECTURE.md](GreenLifeWinForms/ARCHITECTURE.md)** - System architecture
- **[CONVERSION_PLAN.md](CONVERSION_PLAN.md)** - Implementation roadmap
- **[REQUIREMENTS.md](REQUIREMENTS.md)** - Functional requirements

## 🗄️ Database

### Technology: SQLite
- Lightweight, no installation required
- File-based database (`GreenLife.db`)
- Automatic creation on first run
- Pre-populated with sample data

### Tables
1. **Products** - Product catalog with pricing and stock
2. **Customers** - Customer accounts
3. **Orders** - Order headers
4. **OrderItems** - Order line items
5. **Reviews** - Product ratings and reviews
6. **Admins** - Administrator accounts

## 🎨 UI Design

### Color Scheme (Eco-Friendly Green Theme)
- **Primary**: RGB(22, 163, 74) - Green
- **Secondary**: RGB(251, 191, 36) - Yellow
- **Background**: RGB(240, 253, 244) - Light Green
- **Text**: RGB(20, 83, 45) - Dark Green

### Design Standards
- **Font**: Segoe UI (10pt body, 12pt headers, 14pt titles)
- **Spacing**: Consistent padding and margins
- **DataGridView**: Alternating row colors, styled headers
- **Buttons**: Consistent sizing with hover effects

## 📊 Key Features

### Product Management
- Full CRUD operations
- Category-based organization
- Stock level tracking with low stock alerts (< 50 units)
- Discount pricing support
- Product search and filtering

### Order Processing
- Shopping cart with quantity management
- Discount calculation
- Order status workflow (Pending → Shipped → Delivered)
- Stock validation and automatic updates
- Order tracking timeline

### Reporting & Analytics
- Sales reports with date filtering
- Stock level monitoring
- Customer purchase history
- CSV export functionality

### User Management
- Role-based access (Admin/Customer)
- Secure authentication
- Profile management
- Customer registration with validation

### Product Reviews
- 5-star rating system
- Review comments
- Average rating calculation
- Review count display

## 🔒 Security

### Current Implementation
- Parameterized queries (SQL injection prevention)
- Input validation
- Role-based access control

### Production Recommendations
- Hash passwords using BCrypt.Net
- Encrypt sensitive data
- Implement session management
- Add audit logging
- Regular database backups

## 📈 Performance

- Database indexes on frequently queried columns
- Connection pooling
- Transaction support for multi-step operations
- Efficient data binding

## 🐛 Troubleshooting

### Database Not Created
- Ensure the application has write permissions
- Check that SQL files are copied to output directory

### SQLite DLL Not Found
- Reinstall System.Data.SQLite NuGet package
- Verify correct platform (x86/x64)

### Forms Not Displaying
- Check `StartPosition` property
- Verify `AutoScaleMode` is set to `Font`

## 🎯 Implementation Status

**Current Progress: 40%**

✅ **Completed:**
- Database schema and seed data
- All entity models
- Authentication service
- Product service
- Comprehensive documentation

⏳ **In Progress:**
- Customer service
- Order service
- Review service
- Utility classes
- Windows Forms UI

See [IMPLEMENTATION_STATUS.md](GreenLifeWinForms/IMPLEMENTATION_STATUS.md) for detailed progress.

## 📝 Future Enhancements

- Payment gateway integration
- Email notifications
- Barcode scanning support
- Multi-store support
- Advanced reporting with charts
- Inventory forecasting
- Backup and restore functionality
- Print receipts and invoices

## 📄 License

This project is for educational purposes.

---

**Built with ❤️ for organic food enthusiasts**

For detailed setup instructions, see [QUICK_START.md](GreenLifeWinForms/QUICK_START.md)