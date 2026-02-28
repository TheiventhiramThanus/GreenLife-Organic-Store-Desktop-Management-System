# 🚀 Quick Start Guide - GreenLife Windows Forms

## ⚡ Fast Setup (5 Minutes)

### Step 1: Create the Visual Studio Project

1. **Open Visual Studio**
2. **Create New Project**
   - File → New → Project
   - Select "Windows Forms App (.NET Framework)"
   - Name: `GreenLifeWinForms`
   - Location: `C:\Users\thanu\OneDrive\Desktop\AD\GreenLifeWinForms`
   - Framework: `.NET Framework 4.7.2` or later
   - Click "Create"

### Step 2: Install Required NuGet Package

1. **Open NuGet Package Manager**
   - Tools → NuGet Package Manager → Manage NuGet Packages for Solution

2. **Install SQLite**
   - Search for: `System.Data.SQLite`
   - Select the package
   - Check your project
   - Click "Install"
   - Accept the license

### Step 3: Set Up Project Structure

1. **Add Folders to Project**
   - Right-click on project → Add → New Folder
   - Create these folders:
     - `Database`
     - `Models`
     - `Services`
     - `Forms`
     - `Utils`
     - `Resources`

2. **Add Sub-folders in Forms**
   - Right-click on `Forms` → Add → New Folder
   - Create:
     - `Common`
     - `Admin`
     - `Customer`

### Step 4: Add Existing Files

1. **Copy Files to Project**
   - Copy all `.cs` files from the provided structure into your project folders
   - Copy `DatabaseSchema.sql` and `SeedData.sql` to the `Database` folder

2. **Add Files to Project**
   - Right-click on each folder → Add → Existing Item
   - Select all files in that folder
   - Click "Add"

3. **Set SQL Files to Copy**
   - Select `DatabaseSchema.sql` and `SeedData.sql`
   - In Properties window, set:
     - "Copy to Output Directory" → "Copy if newer"

### Step 5: Build and Run

1. **Build the Solution**
   - Press `Ctrl+Shift+B`
   - Or: Build → Build Solution
   - Ensure no errors

2. **Run the Application**
   - Press `F5`
   - Or: Debug → Start Debugging

## 📋 File Checklist

Make sure you have these files in your project:

### Database (2 files)
- [ ] `DatabaseManager.cs`
- [ ] `DatabaseSchema.sql`
- [ ] `SeedData.sql`

### Models (6 files)
- [ ] `Product.cs`
- [ ] `Customer.cs`
- [ ] `Order.cs`
- [ ] `OrderItem.cs`
- [ ] `Review.cs`
- [ ] `Admin.cs`

### Services (5 files - TO BE CREATED)
- [ ] `AuthService.cs`
- [ ] `ProductService.cs`
- [ ] `CustomerService.cs`
- [ ] `OrderService.cs`
- [ ] `ReviewService.cs`

### Utils (3 files - TO BE CREATED)
- [ ] `ValidationHelper.cs`
- [ ] `ExportHelper.cs`
- [ ] `NotificationHelper.cs`

### Forms (18 files - TO BE CREATED)
- [ ] Common: `RoleSelectionForm.cs`, `SplashForm.cs`
- [ ] Admin: 6 forms
- [ ] Customer: 10 forms

## 🎯 Current Progress

✅ **Completed:**
- Project structure created
- Database schema designed
- Seed data prepared
- All model classes created
- DatabaseManager implemented
- Documentation completed

⏳ **Next Steps:**
1. Create Service classes (business logic)
2. Create Utility classes (helpers)
3. Design and implement Forms (UI)
4. Test all functionality
5. Polish and deploy

## 🔧 Quick Commands

### Build
```
Ctrl+Shift+B
```

### Run
```
F5
```

### Clean Solution
```
Build → Clean Solution
```

### Rebuild
```
Build → Rebuild Solution
```

## 🐛 Common Issues & Fixes

### Issue: "SQLite DLL not found"
**Fix:** Reinstall System.Data.SQLite NuGet package

### Issue: "Database file not created"
**Fix:** Ensure SQL files are set to "Copy if newer"

### Issue: "Namespace not found"
**Fix:** Ensure all files are added to the project (not just copied to folder)

### Issue: "Form designer not working"
**Fix:** Ensure the form inherits from `System.Windows.Forms.Form`

## 📞 Need Help?

Refer to:
- `README.md` - Full documentation
- `CONVERSION_PLAN.md` - Detailed implementation plan
- Visual Studio documentation
- SQLite documentation

## 🎉 Success Criteria

Your setup is complete when:
- ✅ Project builds without errors
- ✅ Application starts and shows the first form
- ✅ Database file (`GreenLife.db`) is created
- ✅ No missing references or errors

---

**Estimated Setup Time:** 5-10 minutes
**Difficulty:** Easy to Medium

Good luck! 🚀
