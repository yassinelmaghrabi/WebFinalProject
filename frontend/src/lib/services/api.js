const API_BASE = '/api';

class ApiService {
  constructor() {
    this.token = localStorage.getItem('token');
  }

  setToken(token) {
    this.token = token;
    if (token) {
      localStorage.setItem('token', token);
    } else {
      localStorage.removeItem('token');
    }
  }

  async request(endpoint, options = {}) {
    const headers = {
      'Content-Type': 'application/json',
      ...options.headers,
    };

    if (this.token) {
      headers['Authorization'] = `Bearer ${this.token}`;
    }

    const config = {
      ...options,
      headers,
    };

    if (options.body) {
      config.body = JSON.stringify(options.body);
    }

    const response = await fetch(`${API_BASE}${endpoint}`, config);

    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Request failed');
    }

    return response.json();
  }

  async login(email, password) {
    const data = await this.request('/auth/login', {
      method: 'POST',
      body: { email, password },
    });
    this.setToken(data.token);
    return data;
  }

  async signup(userData) {
    return this.request('/users/signup', {
      method: 'POST',
      body: userData,
    });
  }

  async getAppointments() {
    return this.request('/appointments');
  }

  async getMyAppointments() {
    return this.request('/appointments/for');
  }

  async bookAppointment(appointmentData) {
    return this.request('/appointments/book', {
      method: 'POST',
      body: appointmentData,
    });
  }

  async getPatients() {
    return this.request('/patients');
  }

  async getDoctors() {
    return this.request('/doctors');
  }

  async getMedicalRecords() {
    return this.request('/medicalrecords');
  }

  async createMedicalRecord(record) {
    return this.request('/medicalrecords', {
      method: 'POST',
      body: record,
    });
  }

  async getUsers() {
    return this.request('/users');
  }

  logout() {
    this.setToken(null);
  }
}

export const api = new ApiService();
