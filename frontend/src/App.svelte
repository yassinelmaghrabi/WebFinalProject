<script>
  import { Router, Route } from 'svelte-routing';
  import { onMount } from 'svelte';
  import { authStore } from './lib/stores/auth';
  import Navbar from './lib/components/Navbar.svelte';
  import Home from './routes/Home.svelte';
  import Login from './routes/Login.svelte';
  import Signup from './routes/Signup.svelte';
  import PatientDashboard from './routes/PatientDashboard.svelte';
  import DoctorDashboard from './routes/DoctorDashboard.svelte';
  import AdminDashboard from './routes/AdminDashboard.svelte';

  onMount(() => {
    authStore.checkAuth();
  });
</script>

<Router>
  <Navbar />
  <Route path="/" component={Home} />
  <Route path="/login" component={Login} />
  <Route path="/signup" component={Signup} />
  <Route path="/patient/dashboard" component={PatientDashboard} />
  <Route path="/patient/appointments" component={PatientDashboard} />
  <Route path="/patient/records" component={PatientDashboard} />
  <Route path="/doctor/dashboard" component={DoctorDashboard} />
  <Route path="/doctor/schedule" component={DoctorDashboard} />
  <Route path="/doctor/patients" component={DoctorDashboard} />
  <Route path="/admin/dashboard" component={AdminDashboard} />
  <Route path="/admin/users" component={AdminDashboard} />
  <Route path="/admin/reports" component={AdminDashboard} />
</Router>
