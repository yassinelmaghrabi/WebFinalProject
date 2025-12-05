# MediCare Frontend

A modern, responsive medical clinic appointment system built with Svelte 5.

## Features

- **User Authentication**: Secure login and signup for Patients, Doctors, and Admins
- **Patient Portal**:
  - Book appointments with doctors
  - View appointment history
  - Calendar view of scheduled appointments
  - Access medical records

- **Doctor Portal**:
  - Manage daily schedule
  - View patient information
  - Access patient medical records
  - Track appointments

- **Admin Dashboard**:
  - User management
  - System-wide reports
  - Appointment analytics
  - Doctor and patient statistics

## Tech Stack

- **Framework**: Svelte 5
- **Build Tool**: Vite
- **Routing**: svelte-routing
- **Date Utilities**: date-fns
- **API Integration**: REST API (ASP.NET Core backend)

## Getting Started

### Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Backend API running on port 5000

### Installation

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm run dev
```

3. Build for production:
```bash
npm run build
```

## Project Structure

```
frontend/
├── src/
│   ├── lib/
│   │   ├── components/     # Reusable components
│   │   ├── services/       # API service layer
│   │   └── stores/         # Svelte stores (state management)
│   ├── routes/             # Page components
│   ├── app.css             # Global styles
│   ├── App.svelte          # Main app component with routing
│   └── main.js             # Entry point
├── public/                 # Static assets
└── package.json
```

## Color Scheme

The application uses a modern blue color palette:
- Primary: #0066cc
- Secondary: #00a3e0
- Accent: #4fc3f7
- Success: #4caf50
- Warning: #ff9800
- Error: #f44336

## API Configuration

The frontend is configured to proxy API requests to `http://localhost:5000`. Update `vite.config.js` if your backend runs on a different port.

## User Roles

### Patient
- View and book appointments
- Access personal medical records
- Manage profile

### Doctor
- View daily schedule
- Access patient information
- Manage appointments

### Admin
- Manage all users
- Generate reports
- View system statistics

## Responsive Design

The application is fully responsive and optimized for:
- Desktop (1200px+)
- Tablet (768px - 1199px)
- Mobile (< 768px)

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
