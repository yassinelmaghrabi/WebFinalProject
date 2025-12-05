<script>
  import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, addMonths, subMonths, startOfWeek, endOfWeek } from 'date-fns';

  let { onDateSelect, selectedDate = new Date(), appointments = [] } = $props();

  let currentMonth = $state(new Date());

  function getDaysInMonth() {
    const start = startOfWeek(startOfMonth(currentMonth));
    const end = endOfWeek(endOfMonth(currentMonth));
    return eachDayOfInterval({ start, end });
  }

  let days = $derived(getDaysInMonth());

  function nextMonth() {
    currentMonth = addMonths(currentMonth, 1);
  }

  function prevMonth() {
    currentMonth = subMonths(currentMonth, 1);
  }

  function selectDate(day) {
    if (onDateSelect) {
      onDateSelect(day);
    }
  }

  function hasAppointment(day) {
    return appointments.some(apt => isSameDay(new Date(apt.start), day));
  }
</script>

<div class="calendar">
  <div class="calendar-header">
    <button class="calendar-nav" onclick={prevMonth}>
      &#8592;
    </button>
    <h3>{format(currentMonth, 'MMMM yyyy')}</h3>
    <button class="calendar-nav" onclick={nextMonth}>
      &#8594;
    </button>
  </div>

  <div class="calendar-weekdays">
    {#each ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'] as day}
      <div class="calendar-weekday">{day}</div>
    {/each}
  </div>

  <div class="calendar-days">
    {#each days as day}
      <button
        class="calendar-day"
        class:other-month={!isSameMonth(day, currentMonth)}
        class:selected={selectedDate && isSameDay(day, selectedDate)}
        class:has-appointment={hasAppointment(day)}
        onclick={() => selectDate(day)}
      >
        {format(day, 'd')}
      </button>
    {/each}
  </div>
</div>

<style>
  .calendar {
    background: var(--bg-card);
    border-radius: 12px;
    padding: 1.5rem;
    box-shadow: var(--shadow);
  }

  .calendar-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1.5rem;
  }

  .calendar-nav {
    background: var(--primary);
    color: white;
    border: none;
    width: 40px;
    height: 40px;
    border-radius: 8px;
    cursor: pointer;
    font-size: 1.25rem;
    transition: all 0.3s ease;
  }

  .calendar-nav:hover {
    background: var(--primary-dark);
    transform: scale(1.1);
  }

  .calendar-weekdays {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 0.5rem;
    margin-bottom: 0.5rem;
  }

  .calendar-weekday {
    text-align: center;
    font-weight: 600;
    color: var(--text-secondary);
    padding: 0.5rem;
  }

  .calendar-days {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 0.5rem;
  }

  .calendar-day {
    aspect-ratio: 1;
    border: 2px solid var(--border);
    background: var(--bg-primary);
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.2s;
    font-weight: 500;
  }

  .calendar-day:hover {
    border-color: var(--primary);
    background: var(--primary-light);
    color: white;
  }

  .calendar-day.other-month {
    color: var(--text-secondary);
    opacity: 0.5;
  }

  .calendar-day.selected {
    background: var(--primary);
    color: white;
    border-color: var(--primary);
  }

  .calendar-day.has-appointment {
    background: var(--accent);
    color: white;
  }
</style>
