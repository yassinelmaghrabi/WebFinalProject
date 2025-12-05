<script>
  import { Link } from 'svelte-routing';
  import { authStore } from '../stores/auth';

  let { isAuthenticated, role } = $state(null);

  $effect(() => {
    const unsubscribe = authStore.subscribe(value => {
      isAuthenticated = value.isAuthenticated;
      role = value.role;
    });
    return unsubscribe;
  });

  function handleLogout() {
    authStore.logout();
    window.location.href = '/';
  }
</script>

<nav class="navbar">
  <div class="navbar-container">
    <Link to="/" class="navbar-brand">
      <svg width="32" height="32" viewBox="0 0 32 32" fill="none">
        <rect width="32" height="32" rx="8" fill="url(#gradient)"/>
        <path d="M16 8L16 24M10 16H22" stroke="white" stroke-width="3" stroke-linecap="round"/>
        <defs>
          <linearGradient id="gradient" x1="0" y1="0" x2="32" y2="32">
            <stop offset="0%" stop-color="#0066cc"/>
            <stop offset="100%" stop-color="#0052a3"/>
          </linearGradient>
        </defs>
      </svg>
      <span>MediCare</span>
    </Link>

    <div class="navbar-menu">
      {#if isAuthenticated}
        {#if role === 'Patient'}
          <Link to="/patient/dashboard" class="navbar-link">Dashboard</Link>
          <Link to="/patient/appointments" class="navbar-link">Appointments</Link>
          <Link to="/patient/records" class="navbar-link">Medical Records</Link>
        {:else if role === 'Doctor'}
          <Link to="/doctor/dashboard" class="navbar-link">Dashboard</Link>
          <Link to="/doctor/schedule" class="navbar-link">Schedule</Link>
          <Link to="/doctor/patients" class="navbar-link">Patients</Link>
        {:else if role === 'Admin'}
          <Link to="/admin/dashboard" class="navbar-link">Dashboard</Link>
          <Link to="/admin/users" class="navbar-link">Users</Link>
          <Link to="/admin/reports" class="navbar-link">Reports</Link>
        {/if}
        <button class="btn btn-secondary" onclick={handleLogout}>Logout</button>
      {:else}
        <Link to="/login" class="navbar-link">Login</Link>
        <Link to="/signup">
          <button class="btn btn-primary">Sign Up</button>
        </Link>
      {/if}
    </div>
  </div>
</nav>

<style>
  .navbar {
    background: white;
    box-shadow: 0 2px 8px rgba(0, 102, 204, 0.1);
    position: sticky;
    top: 0;
    z-index: 100;
  }

  .navbar-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 1rem 2rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .navbar-brand {
    display: flex;
    align-items: center;
    gap: 0.75rem;
    font-size: 1.5rem;
    font-weight: 700;
    color: var(--primary);
    text-decoration: none;
  }

  .navbar-menu {
    display: flex;
    align-items: center;
    gap: 1.5rem;
  }

  .navbar-link {
    color: var(--text-primary);
    font-weight: 500;
    transition: color 0.2s;
  }

  .navbar-link:hover {
    color: var(--primary);
  }

  @media (max-width: 768px) {
    .navbar-container {
      padding: 1rem;
    }

    .navbar-menu {
      gap: 0.75rem;
    }

    .navbar-link {
      font-size: 0.9rem;
    }
  }
</style>
