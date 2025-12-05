<script>
  import { Link } from 'svelte-routing';
</script>

<div class="home-page">
  <section class="hero">
    <div class="hero-content">
      <h1>Your Health, Simplified</h1>
      <p>Book appointments, manage medical records, and stay connected with your healthcare providers - all in one place.</p>
      <div class="hero-buttons">
        <Link to="/signup">
          <button class="btn btn-primary btn-lg">Get Started</button>
        </Link>
        <Link to="/login">
          <button class="btn btn-secondary btn-lg">Sign In</button>
        </Link>
      </div>
    </div>
    <div class="hero-image">
      <svg viewBox="0 0 400 300" fill="none">
        <rect x="50" y="50" width="300" height="200" rx="12" fill="#f5f9fc" stroke="#0066cc" stroke-width="2"/>
        <circle cx="200" cy="120" r="30" fill="#0066cc"/>
        <path d="M200 105L200 135M185 120H215" stroke="white" stroke-width="4" stroke-linecap="round"/>
        <rect x="80" y="170" width="240" height="10" rx="5" fill="#e0e7ee"/>
        <rect x="80" y="190" width="180" height="10" rx="5" fill="#e0e7ee"/>
        <rect x="80" y="210" width="200" height="10" rx="5" fill="#e0e7ee"/>
      </svg>
    </div>
  </section>

  <section class="features">
    <div class="container">
      <h2>Why Choose MediCare?</h2>
      <div class="grid grid-3">
        <div class="feature-card card">
          <div class="feature-icon">
            <svg width="48" height="48" viewBox="0 0 48 48" fill="none">
              <circle cx="24" cy="24" r="24" fill="#e3f2fd"/>
              <path d="M24 16V32M16 24H32" stroke="#0066cc" stroke-width="3" stroke-linecap="round"/>
            </svg>
          </div>
          <h3>Easy Booking</h3>
          <p>Schedule appointments with your preferred doctors in just a few clicks.</p>
        </div>

        <div class="feature-card card">
          <div class="feature-icon">
            <svg width="48" height="48" viewBox="0 0 48 48" fill="none">
              <circle cx="24" cy="24" r="24" fill="#e8f5e9"/>
              <rect x="16" y="18" width="16" height="16" rx="2" stroke="#4caf50" stroke-width="2"/>
              <path d="M20 26L23 29L29 23" stroke="#4caf50" stroke-width="2" stroke-linecap="round"/>
            </svg>
          </div>
          <h3>Secure Records</h3>
          <p>Your medical records are encrypted and accessible anytime, anywhere.</p>
        </div>

        <div class="feature-card card">
          <div class="feature-icon">
            <svg width="48" height="48" viewBox="0 0 48 48" fill="none">
              <circle cx="24" cy="24" r="24" fill="#fff3e0"/>
              <circle cx="24" cy="20" r="6" stroke="#ff9800" stroke-width="2"/>
              <path d="M14 34C14 29 18 26 24 26C30 26 34 29 34 34" stroke="#ff9800" stroke-width="2" stroke-linecap="round"/>
            </svg>
          </div>
          <h3>Expert Doctors</h3>
          <p>Connect with qualified healthcare professionals across various specialties.</p>
        </div>
      </div>
    </div>
  </section>

  <section class="cta">
    <div class="cta-content">
      <h2>Ready to get started?</h2>
      <p>Join thousands of patients who trust MediCare for their healthcare needs.</p>
      <Link to="/signup">
        <button class="btn btn-primary btn-lg">Sign Up Now</button>
      </Link>
    </div>
  </section>
</div>

<style>
  .home-page {
    min-height: 100vh;
  }

  .hero {
    background: linear-gradient(135deg, #0066cc 0%, #00a3e0 100%);
    padding: 4rem 2rem;
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 4rem;
    align-items: center;
    min-height: 80vh;
  }

  .hero-content {
    color: white;
    max-width: 600px;
  }

  .hero-content h1 {
    font-size: 3.5rem;
    margin-bottom: 1.5rem;
    color: white;
  }

  .hero-content p {
    font-size: 1.25rem;
    margin-bottom: 2rem;
    opacity: 0.95;
  }

  .hero-buttons {
    display: flex;
    gap: 1rem;
  }

  .btn-lg {
    padding: 1rem 2rem;
    font-size: 1.125rem;
  }

  .hero-image {
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .hero-image svg {
    max-width: 100%;
    height: auto;
  }

  .features {
    padding: 6rem 2rem;
    text-align: center;
  }

  .features h2 {
    font-size: 2.5rem;
    margin-bottom: 3rem;
  }

  .feature-card {
    text-align: center;
    padding: 2rem;
  }

  .feature-icon {
    margin-bottom: 1.5rem;
    display: flex;
    justify-content: center;
  }

  .feature-card h3 {
    margin-bottom: 1rem;
  }

  .feature-card p {
    color: var(--text-secondary);
  }

  .cta {
    background: linear-gradient(135deg, #0066cc 0%, #00a3e0 100%);
    padding: 4rem 2rem;
    text-align: center;
  }

  .cta-content {
    color: white;
  }

  .cta-content h2 {
    font-size: 2.5rem;
    margin-bottom: 1rem;
    color: white;
  }

  .cta-content p {
    font-size: 1.25rem;
    margin-bottom: 2rem;
    opacity: 0.95;
  }

  @media (max-width: 768px) {
    .hero {
      grid-template-columns: 1fr;
      padding: 3rem 1rem;
      min-height: auto;
    }

    .hero-content h1 {
      font-size: 2.5rem;
    }

    .hero-buttons {
      flex-direction: column;
    }

    .hero-image {
      display: none;
    }

    .features {
      padding: 4rem 1rem;
    }

    .cta {
      padding: 3rem 1rem;
    }
  }
</style>
