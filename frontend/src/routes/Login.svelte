<script>
  import { navigate } from 'svelte-routing';
  import { authStore } from '../lib/stores/auth';

  let email = $state('');
  let password = $state('');
  let error = $state('');
  let loading = $state(false);

  async function handleSubmit(e) {
    e.preventDefault();
    error = '';
    loading = true;

    const result = await authStore.login(email, password);
    loading = false;

    if (result.success) {
      const role = localStorage.getItem('role');
      if (role === 'Patient') {
        navigate('/patient/dashboard');
      } else if (role === 'Doctor') {
        navigate('/doctor/dashboard');
      } else if (role === 'Admin') {
        navigate('/admin/dashboard');
      }
    } else {
      error = result.error;
    }
  }
</script>

<div class="login-page">
  <div class="login-container">
    <div class="login-card card">
      <div class="login-header">
        <svg width="64" height="64" viewBox="0 0 64 64" fill="none">
          <rect width="64" height="64" rx="16" fill="url(#gradient)"/>
          <path d="M32 16L32 48M20 32H44" stroke="white" stroke-width="4" stroke-linecap="round"/>
          <defs>
            <linearGradient id="gradient" x1="0" y1="0" x2="64" y2="64">
              <stop offset="0%" stop-color="#0066cc"/>
              <stop offset="100%" stop-color="#0052a3"/>
            </linearGradient>
          </defs>
        </svg>
        <h1>Welcome Back</h1>
        <p>Sign in to your MediCare account</p>
      </div>

      {#if error}
        <div class="alert alert-error">{error}</div>
      {/if}

      <form onsubmit={handleSubmit}>
        <div class="form-group">
          <label for="email">Email Address</label>
          <input
            type="email"
            id="email"
            class="form-control"
            bind:value={email}
            required
            placeholder="Enter your email"
          />
        </div>

        <div class="form-group">
          <label for="password">Password</label>
          <input
            type="password"
            id="password"
            class="form-control"
            bind:value={password}
            required
            placeholder="Enter your password"
          />
        </div>

        <button type="submit" class="btn btn-primary btn-block" disabled={loading}>
          {loading ? 'Signing in...' : 'Sign In'}
        </button>
      </form>

      <div class="login-footer">
        <p>Don't have an account? <a href="/signup">Sign up</a></p>
      </div>
    </div>
  </div>
</div>

<style>
  .login-page {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #0066cc 0%, #00a3e0 100%);
    padding: 2rem;
  }

  .login-container {
    width: 100%;
    max-width: 450px;
  }

  .login-card {
    animation: slideUp 0.5s ease;
  }

  .login-header {
    text-align: center;
    margin-bottom: 2rem;
  }

  .login-header svg {
    margin-bottom: 1rem;
  }

  .login-header h1 {
    margin-bottom: 0.5rem;
  }

  .login-header p {
    color: var(--text-secondary);
  }

  .btn-block {
    width: 100%;
    margin-top: 1rem;
  }

  .login-footer {
    text-align: center;
    margin-top: 1.5rem;
    padding-top: 1.5rem;
    border-top: 1px solid var(--border);
  }

  .login-footer a {
    font-weight: 600;
  }
</style>
