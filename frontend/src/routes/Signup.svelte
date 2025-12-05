<script>
  import { navigate } from 'svelte-routing';
  import { authStore } from '../lib/stores/auth';

  let formData = $state({
    email: '',
    password: '',
    confirmPassword: '',
    role: 'Patient',
    firstname: '',
    lastname: '',
    specialty: '',
    brithdate: ''
  });

  let error = $state('');
  let loading = $state(false);

  async function handleSubmit(e) {
    e.preventDefault();
    error = '';

    if (formData.password !== formData.confirmPassword) {
      error = 'Passwords do not match';
      return;
    }

    if (formData.role === 'Doctor' && !formData.specialty) {
      error = 'Specialty is required for doctors';
      return;
    }

    if (formData.role === 'Patient' && !formData.brithdate) {
      error = 'Birth date is required for patients';
      return;
    }

    loading = true;

    const userData = {
      email: formData.email,
      password: formData.password,
      role: formData.role,
      firstname: formData.firstname,
      lastname: formData.lastname,
    };

    if (formData.role === 'Doctor') {
      userData.specialty = formData.specialty;
    } else if (formData.role === 'Patient') {
      userData.brithdate = formData.brithdate;
    }

    const result = await authStore.signup(userData);
    loading = false;

    if (result.success) {
      navigate('/login');
    } else {
      error = result.error;
    }
  }
</script>

<div class="signup-page">
  <div class="signup-container">
    <div class="signup-card card">
      <div class="signup-header">
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
        <h1>Create Account</h1>
        <p>Join MediCare today</p>
      </div>

      {#if error}
        <div class="alert alert-error">{error}</div>
      {/if}

      <form onsubmit={handleSubmit}>
        <div class="form-group">
          <label for="role">I am a</label>
          <select id="role" class="form-control" bind:value={formData.role}>
            <option value="Patient">Patient</option>
            <option value="Doctor">Doctor</option>
          </select>
        </div>

        <div class="grid grid-2">
          <div class="form-group">
            <label for="firstname">First Name</label>
            <input
              type="text"
              id="firstname"
              class="form-control"
              bind:value={formData.firstname}
              required
              placeholder="John"
            />
          </div>

          <div class="form-group">
            <label for="lastname">Last Name</label>
            <input
              type="text"
              id="lastname"
              class="form-control"
              bind:value={formData.lastname}
              required
              placeholder="Doe"
            />
          </div>
        </div>

        <div class="form-group">
          <label for="email">Email Address</label>
          <input
            type="email"
            id="email"
            class="form-control"
            bind:value={formData.email}
            required
            placeholder="john.doe@example.com"
          />
        </div>

        {#if formData.role === 'Doctor'}
          <div class="form-group">
            <label for="specialty">Specialty</label>
            <input
              type="text"
              id="specialty"
              class="form-control"
              bind:value={formData.specialty}
              required
              placeholder="e.g., Cardiology, Pediatrics"
            />
          </div>
        {/if}

        {#if formData.role === 'Patient'}
          <div class="form-group">
            <label for="brithdate">Birth Date</label>
            <input
              type="date"
              id="brithdate"
              class="form-control"
              bind:value={formData.brithdate}
              required
            />
          </div>
        {/if}

        <div class="form-group">
          <label for="password">Password</label>
          <input
            type="password"
            id="password"
            class="form-control"
            bind:value={formData.password}
            required
            placeholder="Create a strong password"
          />
        </div>

        <div class="form-group">
          <label for="confirmPassword">Confirm Password</label>
          <input
            type="password"
            id="confirmPassword"
            class="form-control"
            bind:value={formData.confirmPassword}
            required
            placeholder="Re-enter your password"
          />
        </div>

        <button type="submit" class="btn btn-primary btn-block" disabled={loading}>
          {loading ? 'Creating Account...' : 'Create Account'}
        </button>
      </form>

      <div class="signup-footer">
        <p>Already have an account? <a href="/login">Sign in</a></p>
      </div>
    </div>
  </div>
</div>

<style>
  .signup-page {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #0066cc 0%, #00a3e0 100%);
    padding: 2rem;
  }

  .signup-container {
    width: 100%;
    max-width: 600px;
  }

  .signup-card {
    animation: slideUp 0.5s ease;
  }

  .signup-header {
    text-align: center;
    margin-bottom: 2rem;
  }

  .signup-header svg {
    margin-bottom: 1rem;
  }

  .signup-header h1 {
    margin-bottom: 0.5rem;
  }

  .signup-header p {
    color: var(--text-secondary);
  }

  .btn-block {
    width: 100%;
    margin-top: 1rem;
  }

  .signup-footer {
    text-align: center;
    margin-top: 1.5rem;
    padding-top: 1.5rem;
    border-top: 1px solid var(--border);
  }

  .signup-footer a {
    font-weight: 600;
  }
</style>
