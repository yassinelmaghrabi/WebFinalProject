<script>
  import { onMount } from 'svelte';
  import { api } from '../lib/services/api';
  import Calendar from '../lib/components/Calendar.svelte';
  import { format } from 'date-fns';

  let appointments = $state([]);
  let doctors = $state([]);
  let loading = $state(true);
  let showBookModal = $state(false);
  let selectedDate = $state(new Date());
  let bookingData = $state({
    doctorId: '',
    startTime: '09:00',
    endTime: '10:00',
  });
  let error = $state('');
  let success = $state('');

  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    try {
      loading = true;
      const [appts, docs] = await Promise.all([
        api.getMyAppointments(),
        api.getDoctors()
      ]);
      appointments = appts;
      doctors = docs;
    } catch (err) {
      error = 'Failed to load data';
    } finally {
      loading = false;
    }
  }

  function handleDateSelect(date) {
    selectedDate = date;
    showBookModal = true;
  }

  async function handleBookAppointment(e) {
    e.preventDefault();
    error = '';
    success = '';

    const startDateTime = new Date(selectedDate);
    const [startHour, startMinute] = bookingData.startTime.split(':');
    startDateTime.setHours(parseInt(startHour), parseInt(startMinute));

    const endDateTime = new Date(selectedDate);
    const [endHour, endMinute] = bookingData.endTime.split(':');
    endDateTime.setHours(parseInt(endHour), parseInt(endMinute));

    try {
      const patientId = 1;
      await api.bookAppointment({
        patientId,
        doctorId: parseInt(bookingData.doctorId),
        start: startDateTime.toISOString(),
        end: endDateTime.toISOString(),
      });
      success = 'Appointment booked successfully!';
      showBookModal = false;
      await loadData();
    } catch (err) {
      error = 'Failed to book appointment. Time slot may be taken.';
    }
  }

  let upcomingAppointments = $derived(
    appointments.filter(apt => new Date(apt.start) > new Date())
  );

  let pastAppointments = $derived(
    appointments.filter(apt => new Date(apt.start) <= new Date())
  );
</script>

<div class="dashboard">
  <div class="container">
    <div class="dashboard-header">
      <h1>Patient Dashboard</h1>
      <button class="btn btn-primary" onclick={() => showBookModal = true}>
        Book Appointment
      </button>
    </div>

    {#if success}
      <div class="alert alert-success">{success}</div>
    {/if}

    {#if error && !showBookModal}
      <div class="alert alert-error">{error}</div>
    {/if}

    {#if loading}
      <div class="loading">
        <div class="spinner"></div>
      </div>
    {:else}
      <div class="dashboard-grid">
        <div class="calendar-section">
          <div class="card">
            <h2>Calendar</h2>
            <Calendar {appointments} {selectedDate} onDateSelect={handleDateSelect} />
          </div>
        </div>

        <div class="appointments-section">
          <div class="card">
            <h2>Upcoming Appointments</h2>
            {#if upcomingAppointments.length === 0}
              <p class="empty-state">No upcoming appointments</p>
            {:else}
              <div class="appointments-list">
                {#each upcomingAppointments as appointment}
                  <div class="appointment-item">
                    <div class="appointment-date">
                      <div class="date-day">{format(new Date(appointment.start), 'd')}</div>
                      <div class="date-month">{format(new Date(appointment.start), 'MMM')}</div>
                    </div>
                    <div class="appointment-details">
                      <h4>Dr. {appointment.doctor?.firstName} {appointment.doctor?.lastName}</h4>
                      <p>{format(new Date(appointment.start), 'h:mm a')} - {format(new Date(appointment.end), 'h:mm a')}</p>
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
            <h2>Past Appointments</h2>
            {#if pastAppointments.length === 0}
              <p class="empty-state">No past appointments</p>
            {:else}
              <div class="appointments-list">
                {#each pastAppointments.slice(0, 5) as appointment}
                  <div class="appointment-item">
                    <div class="appointment-date">
                      <div class="date-day">{format(new Date(appointment.start), 'd')}</div>
                      <div class="date-month">{format(new Date(appointment.start), 'MMM')}</div>
                    </div>
                    <div class="appointment-details">
                      <h4>Dr. {appointment.doctor?.firstName} {appointment.doctor?.lastName}</h4>
                      <p>{format(new Date(appointment.start), 'h:mm a')}</p>
                      <span class="badge badge-info">Completed</span>
                    </div>
                  </div>
                {/each}
              </div>
            {/if}
          </div>
        </div>
      </div>
    {/if}
  </div>
</div>

{#if showBookModal}
  <div class="modal-overlay" onclick={() => showBookModal = false}>
    <div class="modal" onclick={(e) => e.stopPropagation()}>
      <h2>Book Appointment</h2>
      <p>Selected Date: <strong>{format(selectedDate, 'MMMM d, yyyy')}</strong></p>

      {#if error}
        <div class="alert alert-error">{error}</div>
      {/if}

      <form onsubmit={handleBookAppointment}>
        <div class="form-group">
          <label for="doctor">Select Doctor</label>
          <select id="doctor" class="form-control" bind:value={bookingData.doctorId} required>
            <option value="">Choose a doctor</option>
            {#each doctors as doctor}
              <option value={doctor.id}>
                Dr. {doctor.firstName} {doctor.lastName} - {doctor.specialty}
              </option>
            {/each}
          </select>
        </div>

        <div class="grid grid-2">
          <div class="form-group">
            <label for="startTime">Start Time</label>
            <input
              type="time"
              id="startTime"
              class="form-control"
              bind:value={bookingData.startTime}
              required
            />
          </div>

          <div class="form-group">
            <label for="endTime">End Time</label>
            <input
              type="time"
              id="endTime"
              class="form-control"
              bind:value={bookingData.endTime}
              required
            />
          </div>
        </div>

        <div class="modal-buttons">
          <button type="button" class="btn btn-secondary" onclick={() => showBookModal = false}>
            Cancel
          </button>
          <button type="submit" class="btn btn-primary">
            Book Appointment
          </button>
        </div>
      </form>
    </div>
  </div>
{/if}

<style>
  .dashboard {
    min-height: 100vh;
    padding: 2rem 0;
  }

  .dashboard-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
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

  .appointment-date {
    background: var(--primary);
    color: white;
    padding: 0.75rem;
    border-radius: 8px;
    text-align: center;
    min-width: 60px;
  }

  .date-day {
    font-size: 1.5rem;
    font-weight: 700;
  }

  .date-month {
    font-size: 0.875rem;
    text-transform: uppercase;
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
  }

  .empty-state {
    text-align: center;
    color: var(--text-secondary);
    padding: 2rem;
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

    .dashboard-header {
      flex-direction: column;
      gap: 1rem;
      align-items: stretch;
    }
  }
</style>
