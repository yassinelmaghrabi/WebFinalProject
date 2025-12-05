<script>
  import { onMount } from 'svelte';
  import { api } from '../lib/services/api';
  import Calendar from '../lib/components/Calendar.svelte';
  import { format } from 'date-fns';

  let appointments = $state([]);
  let patients = $state([]);
  let loading = $state(true);
  let selectedDate = $state(new Date());
  let showPatientModal = $state(false);
  let selectedPatient = $state(null);
  let medicalRecords = $state([]);

  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    try {
      loading = true;
      const [appts, pts] = await Promise.all([
        api.getMyAppointments(),
        api.getPatients()
      ]);
      appointments = appts;
      patients = pts;
    } catch (err) {
      console.error('Failed to load data:', err);
    } finally {
      loading = false;
    }
  }

  async function viewPatientRecords(patient) {
    selectedPatient = patient;
    try {
      const records = await api.getMedicalRecords();
      medicalRecords = records.filter(r => r.patientId === patient.id);
      showPatientModal = true;
    } catch (err) {
      console.error('Failed to load medical records:', err);
    }
  }

  let todayAppointments = $derived(
    appointments.filter(apt => {
      const aptDate = new Date(apt.start);
      const today = new Date();
      return aptDate.toDateString() === today.toDateString();
    })
  );

  let upcomingAppointments = $derived(
    appointments.filter(apt => new Date(apt.start) > new Date())
  );

  let stats = $derived({
    totalAppointments: appointments.length,
    todayAppointments: todayAppointments.length,
    totalPatients: patients.length,
  });
</script>

<div class="dashboard">
  <div class="container">
    <div class="dashboard-header">
      <h1>Doctor Dashboard</h1>
    </div>

    {#if loading}
      <div class="loading">
        <div class="spinner"></div>
      </div>
    {:else}
      <div class="stats-grid grid grid-3">
        <div class="stat-card card">
          <div class="stat-icon" style="background: #e3f2fd;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#0066cc">
              <rect x="8" y="4" width="16" height="24" rx="2" stroke="#0066cc" fill="none" stroke-width="2"/>
              <line x1="12" y1="10" x2="20" y2="10" stroke="#0066cc" stroke-width="2"/>
              <line x1="12" y1="16" x2="20" y2="16" stroke="#0066cc" stroke-width="2"/>
              <line x1="12" y1="22" x2="16" y2="22" stroke="#0066cc" stroke-width="2"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.totalAppointments}</h3>
            <p>Total Appointments</p>
          </div>
        </div>

        <div class="stat-card card">
          <div class="stat-icon" style="background: #e8f5e9;">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="#4caf50">
              <circle cx="16" cy="16" r="12" stroke="#4caf50" fill="none" stroke-width="2"/>
              <path d="M11 16L14 19L21 12" stroke="#4caf50" stroke-width="2" fill="none"/>
            </svg>
          </div>
          <div class="stat-content">
            <h3>{stats.todayAppointments}</h3>
            <p>Today's Appointments</p>
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
      </div>

      <div class="dashboard-grid">
        <div class="calendar-section">
          <div class="card">
            <h2>Schedule</h2>
            <Calendar {appointments} {selectedDate} />
          </div>
        </div>

        <div class="appointments-section">
          <div class="card">
            <h2>Today's Appointments</h2>
            {#if todayAppointments.length === 0}
              <p class="empty-state">No appointments scheduled for today</p>
            {:else}
              <div class="appointments-list">
                {#each todayAppointments as appointment}
                  <div class="appointment-item">
                    <div class="appointment-time">
                      {format(new Date(appointment.start), 'h:mm a')}
                    </div>
                    <div class="appointment-details">
                      <h4>{appointment.patient?.firstName} {appointment.patient?.lastName}</h4>
                      <p>Duration: {format(new Date(appointment.start), 'h:mm a')} - {format(new Date(appointment.end), 'h:mm a')}</p>
                      <span class="badge badge-{appointment.status === 'Booked' ? 'success' : 'warning'}">
                        {appointment.status}
                      </span>
                    </div>
                  </div>
                {/each}
              </div>
            {/if}
          </div>

          <div class="card">
            <h2>Recent Patients</h2>
            {#if patients.length === 0}
              <p class="empty-state">No patients found</p>
            {:else}
              <div class="table-container">
                <table>
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Birth Date</th>
                      <th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {#each patients.slice(0, 10) as patient}
                      <tr>
                        <td>{patient.firstName} {patient.lastName}</td>
                        <td>{format(new Date(patient.birthDate), 'MMM d, yyyy')}</td>
                        <td>
                          <button class="btn btn-primary" onclick={() => viewPatientRecords(patient)}>
                            View Records
                          </button>
                        </td>
                      </tr>
                    {/each}
                  </tbody>
                </table>
              </div>
            {/if}
          </div>
        </div>
      </div>
    {/if}
  </div>
</div>

{#if showPatientModal && selectedPatient}
  <div class="modal-overlay" onclick={() => showPatientModal = false}>
    <div class="modal" onclick={(e) => e.stopPropagation()}>
      <h2>Patient: {selectedPatient.firstName} {selectedPatient.lastName}</h2>
      <p><strong>Birth Date:</strong> {format(new Date(selectedPatient.birthDate), 'MMMM d, yyyy')}</p>

      <h3>Medical Records</h3>
      {#if medicalRecords.length === 0}
        <p class="empty-state">No medical records found</p>
      {:else}
        <div class="records-list">
          {#each medicalRecords as record}
            <div class="record-item">
              <p><strong>Date:</strong> {format(new Date(record.createdAt), 'MMM d, yyyy')}</p>
              <p><strong>Diagnosis:</strong> {record.diagnosis}</p>
              <p><strong>Notes:</strong> {record.notes}</p>
            </div>
          {/each}
        </div>
      {/if}

      <div class="modal-buttons">
        <button class="btn btn-secondary" onclick={() => showPatientModal = false}>
          Close
        </button>
      </div>
    </div>
  </div>
{/if}

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

  .dashboard-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 2rem;
  }

  .calendar-section,
  .appointments-section {
    display: flex;
    flex-direction: column;
    gap: 2rem;
  }

  .appointments-list {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin-top: 1rem;
  }

  .appointment-item {
    display: flex;
    gap: 1rem;
    padding: 1rem;
    border: 2px solid var(--border);
    border-radius: 8px;
    transition: all 0.3s ease;
  }

  .appointment-item:hover {
    border-color: var(--primary);
    background: var(--bg-secondary);
  }

  .appointment-time {
    background: var(--primary);
    color: white;
    padding: 0.75rem;
    border-radius: 8px;
    text-align: center;
    font-weight: 600;
    min-width: 80px;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .appointment-details {
    flex: 1;
  }

  .appointment-details h4 {
    margin-bottom: 0.25rem;
  }

  .appointment-details p {
    color: var(--text-secondary);
    margin-bottom: 0.5rem;
    font-size: 0.875rem;
  }

  .empty-state {
    text-align: center;
    color: var(--text-secondary);
    padding: 2rem;
  }

  .records-list {
    margin-top: 1rem;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .record-item {
    padding: 1rem;
    background: var(--bg-secondary);
    border-radius: 8px;
  }

  .record-item p {
    margin-bottom: 0.5rem;
  }

  .modal-buttons {
    display: flex;
    gap: 1rem;
    justify-content: flex-end;
    margin-top: 2rem;
  }

  @media (max-width: 968px) {
    .dashboard-grid {
      grid-template-columns: 1fr;
    }
  }
</style>
