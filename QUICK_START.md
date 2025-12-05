# Quick Start Guide - Medical Clinic Appointment System

This guide will help you get the complete application running quickly.

## Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Node.js v18+** - [Download](https://nodejs.org/)
- **SQL Server** - Local or Docker instance

## Setup Steps

### 1. Database Setup

#### Option A: Using Docker (Recommended)

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword!" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

#### Option B: Using Local SQL Server

Ensure SQL Server is running and accessible on `localhost,1433`

### 2. Backend Setup

```bash
# Navigate to API directory
cd Api

# Restore dependencies
dotnet restore

# Run database migrations
dotnet ef database update

# Run the API
dotnet run
```

The API will start on `http://localhost:5000`

### 3. Frontend Setup

Open a new terminal:

```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will start on `http://localhost:5173`

### 4. Access the Application

Open your browser and navigate to: **http://localhost:5173**

## Default Test Accounts

### Create Test Accounts

Use the signup page to create accounts for each role:

#### Patient Account
- Email: patient@test.com
- Password: Test123!
- Role: Patient
- First Name: John
- Last Name: Doe
- Birth Date: 1990-01-01

#### Doctor Account
- Email: doctor@test.com
- Password: Test123!
- Role: Doctor
- First Name: Jane
- Last Name: Smith
- Specialty: General Practice

#### Admin Account
You'll need to manually set a user's role to "Admin" in the database.

## Application Features Tour

### As a Patient

1. **Login** with your patient credentials
2. **Dashboard** - View your upcoming appointments
3. **Book Appointment**:
   - Click "Book Appointment"
   - Select a date on the calendar
   - Choose a doctor
   - Select time slot
   - Confirm booking
4. **View Records** - Access your medical history

### As a Doctor

1. **Login** with your doctor credentials
2. **Dashboard** - View statistics and today's schedule
3. **Schedule** - See all appointments in calendar view
4. **Patients** - View patient list and medical records

### As an Admin

1. **Login** with admin credentials
2. **Dashboard** - View system overview
3. **Users Tab** - Manage all users
4. **Appointments Tab** - View all appointments
5. **Reports Tab** - Generate system reports

## Common Tasks

### Booking an Appointment (Patient)

1. Go to Dashboard
2. Click "Book Appointment" button
3. Click on desired date in calendar
4. Select doctor from dropdown
5. Choose start and end time
6. Click "Book Appointment"

### Viewing Patient Records (Doctor)

1. Go to Dashboard
2. Scroll to "Recent Patients" section
3. Click "View Records" on any patient
4. Review medical history

### Generating Reports (Admin)

1. Go to Dashboard
2. Click "Reports" tab
3. View weekly and overall statistics
4. Analyze doctor specialties and appointment status

## Development Workflow

### Making Frontend Changes

1. Edit files in `frontend/src/`
2. Changes auto-reload in browser
3. Check console for errors
4. Test across different screen sizes

### Making Backend Changes

1. Edit files in `Api/`
2. Save changes (hot reload enabled)
3. If models change, create new migration:
   ```bash
   dotnet ef migrations add MigrationName
   dotnet ef database update
   ```

### Adding New API Endpoints

1. Create/update controller in `Api/Controllers/`
2. Add service method in `Api/Services/`
3. Update frontend API service in `frontend/src/lib/services/api.js`
4. Use in components

## Troubleshooting

### Backend Issues

**Database Connection Failed**
```bash
# Check connection string in Api/appsettings.json
# Ensure SQL Server is running
# Verify credentials
```

**Migration Errors**
```bash
# Drop database and recreate
dotnet ef database drop
dotnet ef database update
```

### Frontend Issues

**API Calls Failing**
- Check backend is running on port 5000
- Verify proxy config in `vite.config.js`
- Check browser console for CORS errors

**Authentication Not Working**
- Clear localStorage
- Check token in localStorage
- Verify API login endpoint

**UI Not Updating**
- Clear browser cache
- Restart dev server
- Check for JavaScript errors in console

### Port Conflicts

**Backend Port 5000 in Use**
- Change in `Api/Properties/launchSettings.json`
- Update proxy in `frontend/vite.config.js`

**Frontend Port 5173 in Use**
```bash
# Vite will automatically use next available port
# Or specify port: npm run dev -- --port 3000
```

## Production Deployment

### Backend

```bash
cd Api
dotnet publish -c Release -o ./publish
# Deploy to IIS, Azure, or other hosting
```

### Frontend

```bash
cd frontend
npm run build
# Deploy dist/ folder to static hosting (Netlify, Vercel, etc.)
```

### Environment Variables

Update production settings:

**Backend** (`appsettings.Production.json`):
- Database connection string
- JWT secret key
- Email SMTP settings

**Frontend**:
- Update API base URL in proxy config
- Set production environment variables

## Architecture Overview

```
┌─────────────────┐
│   Browser       │
│  (Svelte App)   │
└────────┬────────┘
         │ HTTP/REST
         ▼
┌─────────────────┐
│   Vite Proxy    │
│  (Development)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  .NET Web API   │
│  (Port 5000)    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   SQL Server    │
│  (Port 1433)    │
└─────────────────┘
```

## Tech Stack Summary

### Frontend
- **Svelte 5** - UI Framework
- **Vite** - Build Tool
- **svelte-routing** - Routing
- **date-fns** - Date Utilities

### Backend
- **.NET 9** - API Framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **JWT** - Authentication

### Features
- Role-based access control
- Appointment scheduling
- Medical records management
- Real-time notifications (email)
- Admin reporting

## Next Steps

1. **Explore the codebase** - Review file structure
2. **Read documentation** - Check FRONTEND_GUIDE.md
3. **Test features** - Try all user roles
4. **Customize** - Modify to your needs
5. **Deploy** - Follow production deployment guide

## Support & Resources

- Frontend Documentation: `FRONTEND_GUIDE.md`
- Backend Documentation: `Api/README.md`
- Database Schema: `SQL/schema.sql`
- ERD Diagram: `SQL/ERD.png`

## Tips for Development

1. **Use Browser DevTools** - Monitor network requests and console
2. **Check API Documentation** - Swagger UI at `http://localhost:5000/swagger`
3. **Test Responsive Design** - Use browser device emulation
4. **Monitor Database** - Use SQL Server Management Studio
5. **Version Control** - Commit changes frequently

Enjoy building with MediCare!
