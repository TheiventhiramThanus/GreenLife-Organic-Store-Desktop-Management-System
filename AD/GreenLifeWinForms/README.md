# 🌱 GreenLife Organic Store - Windows Forms Application

A comprehensive desktop application for organic store management built with C# .NET Framework and Windows Forms.

## 📋 Project Overview

This is a **complete conversion** of the GreenLife Organic Store from a React/TypeScript web application to a C# Windows Forms desktop application. The application maintains all features and functionality while providing a native Windows desktop experience.

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
│   ├── Common/
│   │   ├── RoleSelectionForm.cs    # Admin/Customer selection
│   │   └── SplashForm.cs           # Startup screen
│   ├── Admin/
│   │   ├── AdminLoginForm.cs
│   │   ├── AdminDashboardForm.cs
│   │   ├── ProductManagementForm.cs
│   │   ├── CustomerManagementForm.cs
│   │   ├── OrderManagementForm.cs
│   │   └── ReportsForm.cs
│   └── Customer/
│       ├── CustomerLoginForm.cs
│       ├── CustomerRegisterForm.cs
│       ├── CustomerDashboardForm.cs
│       ├── BrowseProductsForm.cs
│       ├── CartForm.cs
│       ├── CheckoutForm.cs
│       ├── TrackOrdersForm.cs
│       ├── ProfileForm.cs
│       ├── ReviewsForm.cs
│       └── OrderHistoryForm.cs
├── Utils/
│   ├── ValidationHelper.cs         # Input validation
│   ├── ExportHelper.cs             # CSV export
│   └── NotificationHelper.cs       # User notifications
└── Resources/
    └── Images/                     # Product images, icons
```

## 🗄️ Database Schema

### Tables
1. **Products** - Product catalog with pricing, stock, and discounts
2. **Customers** - Customer accounts and information
3. **Orders** - Order headers with status tracking
4. **OrderItems** - Individual items in each order
5. **Reviews** - Product ratings and reviews
6. **Admins** - Administrator accounts

### Database File
- **Location**: `GreenLife.db` (created in application directory)
- **Type**: SQLite database
- **Auto-created**: Database and tables are created automatically on first run

## 🚀 Getting Started

### Prerequisites
- **Visual Studio 2019 or later**
- **.NET Framework 4.7.2 or later**
- **Windows OS** (Windows 7 or later)

### Installation Steps

1. **Open the Project**
   - Open `GreenLifeWinForms.sln` in Visual Studio

2. **Install NuGet Packages**
   ```
   Install-Package System.Data.SQLite
   ```

3. **Build the Solution**
   - Press `Ctrl+Shift+B` or go to Build → Build Solution

4. **Run the Application**
   - Press `F5` or click the Start button

### Demo Credentials

**Admin Portal**
- Username: `admin`
- Password: `admin123`

**Customer Portal**
- Email: `sarah.johnson@email.com`
- Password: `password123`

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
- **Forms**: Centered, responsive layouts

## 📊 Key Features Implementation

### 1. Product Management
- Full CRUD operations
- Category-based organization
- Stock level tracking with low stock alerts (< 50 units)
- Discount pricing support
- Product search and filtering

### 2. Order Processing
- Shopping cart with quantity management
- Discount calculation
- Order status workflow (Pending → Shipped → Delivered)
- Stock validation and automatic updates
- Order tracking timeline

### 3. Reporting & Analytics
- Sales reports with date filtering
- Stock level monitoring
- Customer purchase history
- CSV export functionality

### 4. User Management
- Role-based access (Admin/Customer)
- Secure authentication
- Profile management
- Customer registration with validation

### 5. Product Reviews
- 5-star rating system
- Review comments
- Average rating calculation
- Review count display

## 🔧 Development Guide

### Adding a New Form

1. **Create the Form**
   ```csharp
   public partial class MyNewForm : Form
   {
       public MyNewForm()
       {
           InitializeComponent();
       }
   }
   ```

2. **Design the UI**
   - Use the Visual Studio Designer
   - Follow the color scheme and design standards
   - Add controls (DataGridView, TextBox, Button, etc.)

3. **Implement Business Logic**
   - Use Services for data operations
   - Add validation using ValidationHelper
   - Handle errors gracefully

### Adding a New Service

1. **Create the Service Class**
   ```csharp
   public class MyService
   {
       private DatabaseManager db = DatabaseManager.Instance;
       
       public List<MyEntity> GetAll()
       {
           // Implementation
       }
   }
   ```

2. **Use the Service in Forms**
   ```csharp
   MyService service = new MyService();
   var data = service.GetAll();
   ```

## 📝 Code Examples

### Database Query Example
```csharp
string query = "SELECT * FROM Products WHERE IsActive = 1";
DataTable dt = DatabaseManager.Instance.ExecuteQuery(query);
```

### Insert with Parameters
```csharp
string query = "INSERT INTO Products (Name, Price, Stock) VALUES (@name, @price, @stock)";
SQLiteParameter[] parameters = {
    new SQLiteParameter("@name", productName),
    new SQLiteParameter("@price", price),
    new SQLiteParameter("@stock", stock)
};
DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
```

### Populate DataGridView
```csharp
dataGridView1.DataSource = DatabaseManager.Instance.ExecuteQuery("SELECT * FROM Products");
```

### CSV Export
```csharp
ExportHelper.ExportToCSV(dataTable, "products.csv");
```

## 🔒 Security Considerations

### Current Implementation (Demo)
- Plain text password storage
- No encryption
- Local database file

### Production Recommendations
- Hash passwords using BCrypt.Net or similar
- Encrypt sensitive data
- Use parameterized queries (already implemented)
- Implement user session management
- Add audit logging
- Regular database backups

## 🐛 Troubleshooting

### Database Not Created
- Ensure the application has write permissions in its directory
- Check that `DatabaseSchema.sql` and `SeedData.sql` are copied to output directory

### SQLite DLL Not Found
- Reinstall the System.Data.SQLite NuGet package
- Ensure the correct platform (x86/x64) is selected

### Forms Not Displaying Correctly
- Check that the form's `StartPosition` is set to `CenterScreen` or `CenterParent`
- Verify that `AutoScaleMode` is set to `Font`

## 📈 Performance Tips

1. **Database**
   - Use indexes on frequently queried columns (already implemented)
   - Use transactions for multiple operations
   - Close connections properly

2. **UI**
   - Load data asynchronously for large datasets
   - Implement pagination for DataGridView
   - Use virtual mode for very large datasets

3. **Memory**
   - Dispose of forms when closing
   - Clear DataGridView data sources when not needed
   - Implement IDisposable pattern

## 🎯 Future Enhancements

- Payment gateway integration
- Email notifications
- Barcode scanning support
- Multi-store support
- Advanced reporting with charts
- Inventory forecasting
- Backup and restore functionality
- Print receipts and invoices

## 📚 Additional Resources

- [Windows Forms Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [ADO.NET Tutorial](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)

## 📄 License

This project is for educational purposes.

## 👥 Support

For issues or questions, please refer to the documentation or contact the development team.

---

**Built with ❤️ for organic food enthusiasts**
