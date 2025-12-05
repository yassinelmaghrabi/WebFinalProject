<script>
  import { onMount } from 'svelte';
  import { api } from '../lib/services/api';
  import { format, startOfWeek, endOfWeek, isWithinInterval } from 'date-fns';

  let users = $state([]);
  let appointments = $state([]);
  let doctors = $state([]);
  let patients = $state([]);
  let loading = $state(true);
  let activeTab = $state('overview');

  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    try {
      loading = true;
      const [usersData, appts, docs, pts] = await Promise.all([
        api.getUsers(),
        api.getAppointments(),
        api.getDoctors(),
        api.getPatients()
      ]);
      users = usersData;
      appointments = appts;
      doctors = docs;
      patients = pts;
    } catch (err) {
      console.error('Failed to load data:', err);
    } finally {
      loading = false;
    }
  }

  let stats = $derived({
    totalUsers: users.length,
    totalDoctors: doctors.length,
    totalPatients: patients.length,
    totalAppointments: appointments.length,
    weeklyAppointments: appointments.filter(apt => {
      const aptDate = new Date(apt.start);
      const now = new Date();
      return isWithinInterval(aptDate, {
        start: startOfWeek(now),
        end: endOfWeek(now)
      });
    }).length,
  });
</script>

<div class="dashboard">
  <div class="container">
    <div class="dashboard-header">
      <h1>Admin Dashboard</h1>
    </div>

    {#if loading}
      <div class="loading">
        <div class="spinner"></div>
      </div>
    {:else}
      <div class="stats-grid grid grid-4">
        <div class="stat-card card">
          <div class="stat-icon" style="background: #e3f2fd;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#0066cc">
              <circle cx="16" cy="12" r="5" stroke="#0066cc" fill="none" stroke-width="2"/>
              <path d="M8 26C8 21 11 18 16 18C21 18 24 21 24 26" stroke="#0066cc" stroke-width="2" fill="none"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.totalUsers}</h3>
            <p>Total Users</p>
          </div>
        </div>

        <div class="stat-card card">
          <div class="stat-icon" style="background: #e8f5e9;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#4caf50">
              <rect x="8" y="10" width="16" height="16" rx="2" stroke="#4caf50" fill="none" stroke-width="2"/>
              <path d="M16 6L16 10M12 10L20 10" stroke="#4caf50" stroke-width="2"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.totalDoctors}</h3>
            <p>Total Doctors</p>
          </div>
        </div>

        <div class="stat-card card">
          <div class="stat-icon" style="background: #fff3e0;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#ff9800">
              <circle cx="16" cy="12" r="5" stroke="#ff9800" fill="none" stroke-width="2"/>
              <path d="M8 26C8 21 11 18 16 18C21 18 24 21 24 26" stroke="#ff9800" stroke-width="2" fill="none"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.totalPatients}</h3>
            <p>Total Patients</p>
          </div>
        </div>

        <div class="stat-card card">
          <div class="stat-icon" style="background: #f3e5f5;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#9c27b0">
              <rect x="6" y="8" width="20" height="18" rx="2" stroke="#9c27b0" fill="none" stroke-width="2"/>
              <line x1="6" y1="14" x2="26" y2="14" stroke="#9c27b0" stroke-width="2"/>
              <circle cx="11" cy="5" r="1" fill="#9c27b0"/>
              <circle cx="16" cy="5" r="1" fill="#9c27b0"/>
              <circle cx="21" cy="5" r="1" fill="#9c27b0"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.weeklyAppointments}</h3>
            <p>This Week</p>
          </div>
        </div>
      </div>

      <div class="tabs">
        <button
          class="tab"
          class:active={activeTab === 'overview'}
          onclick={() => activeTab = 'overview'}
        >
          Overview
        </button>
        <button
          class="tab"
          class:active={activeTab === 'users'}
          onclick={() => activeTab = 'users'}
        >
          Users
        </button>
        <button
          class="tab"
          class:active={activeTab === 'appointments'}
          onclick={() => activeTab = 'appointments'}
        >
          Appointments
        </button>
        <button
          class="tab"
          class:active={activeTab === 'reports'}
          onclick={() => activeTab = 'reports'}
        >
          Reports
        </button>
      </div>

      <div class="tab-content">
        {#if activeTab === 'overview'}
          <div class="grid grid-2">
            <div class="card">
              <h2>Recent Doctors</h2>
              <div class="table-container">
                <table>
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Specialty</th>
                    </tr>
                  </thead>
                  <tbody>
                    {#each doctors.slice(0, 5) as doctor}
                      <tr>
                        <td>{doctor.firstName} {doctor.lastName}</td>
                        <td><span class="badge badge-primary">{doctor.specialty}</span></td>
                      </tr>
                    {/each}
                  </tbody>
                </table>
              </div>
            </div>

            <div class="card">
              <h2>Recent Patients</h2>
              <div class="table-container">
                <table>
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Birth Date</th>
                    </tr>
                  </thead>
                  <tbody>
                    {#each patients.slice(0, 5) as patient}
                      <tr>
                        <td>{patient.firstName} {patient.lastName}</td>
                        <td>{format(new Date(patient.birthDate), 'MMM d, yyyy')}</td>
                      </tr>
                    {/each}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        {:else if activeTab === 'users'}
          <div class="card">
            <h2>All Users</h2>
            <div class="table-container">
              <table>
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Email</th>
                    <th>Role</th>
                  </tr>
                </thead>
                <tbody>
                  {#each users as user}
                    <tr>
                      <td>{user.id}</td>
                      <td>{user.email}</td>
                      <td>
                        <span class="badge badge-{user.role === 'Doctor' ? 'success' : user.role === 'Patient' ? 'info' : 'warning'}">
                          {user.role}
                        </span>
                      </td>
                    </tr>
                  {/each}
                </tbody>
              </table>
            </div>
          </div>
        {:else if activeTab === 'appointments'}
          <div class="card">
            <h2>All Appointments</h2>
            <div class="table-container">
              <table>
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Patient</th>
                    <th>Doctor</th>
                    <th>Date</th>
                    <th>Time</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {#each appointments as appointment}
                    <tr>
                      <td>{appointment.id}</td>
                      <td>{appointment.patient?.firstName} {appointment.patient?.lastName}</td>
                      <td>Dr. {appointment.doctor?.firstName} {appointment.doctor?.lastName}</td>
                      <td>{format(new Date(appointment.start), 'MMM d, yyyy')}</td>
                      <td>{format(new Date(appointment.start), 'h:mm a')}</td>
                      <td>
                        <span class="badge badge-{appointment.status === 'Booked' ? 'success' : 'warning'}">
                          {appointment.status}
                        </span>
                      </td>
                    </tr>
                  {/each}
                </tbody>
              </table>
            </div>
          </div>
        {:else if activeTab === 'reports'}
          <div class="grid grid-2">
            <div class="card">
              <h2>Weekly Appointment Report</h2>
              <div class="report-content">
                <p><strong>Total Appointments This Week:</strong> {stats.weeklyAppointments}</p>
                <p><strong>Total Appointments:</strong> {stats.totalAppointments}</p>
                <p><strong>Average per Week:</strong> {Math.round(stats.totalAppointments / 4)}</p>
              </div>
            </div>

            <div class="card">
              <h2>User Statistics</h2>
              <div class="report-content">
                <p><strong>Total Users:</strong> {stats.totalUsers}</p>
                <p><strong>Doctors:</strong> {stats.totalDoctors} ({Math.round((stats.totalDoctors / stats.totalUsers) * 100)}%)</p>
                <p><strong>Patients:</strong> {stats.totalPatients} ({Math.round((stats.totalPatients / stats.totalUsers) * 100)}%)</p>
              </div>
            </div>

            <div class="card">
              <h2>Doctor Specialties</h2>
              <div class="report-content">
                {#each [...new Set(doctors.map(d => d.specialty))] as specialty}
                  <p><strong>{specialty}:</strong> {doctors.filter(d => d.specialty === specialty).length} doctors</p>
                {/each}
              </div>
            </div>

            <div class="card">
              <h2>Appointment Status</h2>
              <div class="report-content">
                {#each [...new Set(appointments.map(a => a.status))] as status}
                  <p><strong>{status}:</strong> {appointments.filter(a => a.status === status).length} appointments</p>
                {/each}
              </div>
            </div>
          </div>
        {/if}
      </div>
    {/if}
  </div>
</div>

<style>
  .dashboard {
    min-height: 100vh;
    padding: 2rem 0;
  }

  .dashboard-header {
    margin-bottom: 2rem;
  }

  .stats-grid {
    margin-bottom: 2rem;
  }

  .stat-card {
    display: flex;
    gap: 1rem;
    align-items: center;
  }

  .stat-icon {
    width: 64px;
    height: 64px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .stat-content h3 {
    font-size: 2rem;
    margin-bottom: 0.25rem;
  }

  .stat-content p {
    color: var(--text-secondary);
    font-size: 0.875rem;
  }

  .tabs {
    display: flex;
    gap: 0.5rem;
    margin-bottom: 2rem;
    border-bottom: 2px solid var(--border);
  }

  .tab {
    padding: 1rem 2rem;
    background: none;
    border: none;
    border-bottom: 3px solid transparent;
    font-weight: 500;
    color: var(--text-secondary);
    cursor: pointer;
    transition: all 0.3s ease;
  }

  .tab:hover {
    color: var(--primary);
  }

  .tab.active {
    color: var(--primary);
    border-bottom-color: var(--primary);
  }

  .tab-content {
    animation: fadeIn 0.3s ease;
  }

  .report-content {
    padding: 1rem;
  }

  .report-content p {
    margin-bottom: 1rem;
    padding: 0.75rem;
    background: var(--bg-secondary);
    border-radius: 8px;
  }

  @media (max-width: 968px) {
    .tabs {
      overflow-x: auto;
    }

    .tab {
      padding: 0.75rem 1rem;
      white-space: nowrap;
    }
  }
</style>
