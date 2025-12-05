import { writable } from 'svelte/store';
import { api } from '../services/api';

function createAuthStore() {
  const { subscribe, set, update } = writable({
    user: null,
    role: null,
    isAuthenticated: false,
    loading: true,
  });

  return {
    subscribe,
    login: async (email, password) => {
      try {
        const data = await api.login(email, password);
        localStorage.setItem('role', data.role);
        update(state => ({
          ...state,
          user: { email },
          role: data.role,
          isAuthenticated: true,
          loading: false,
        }));
        return { success: true };
      } catch (error) {
        return { success: false, error: error.message };
      }
    },
    signup: async (userData) => {
      try {
        await api.signup(userData);
        return { success: true };
      } catch (error) {
        return { success: false, error: error.message };
      }
    },
    logout: () => {
      api.logout();
      localStorage.removeItem('role');
      set({
        user: null,
        role: null,
        isAuthenticated: false,
        loading: false,
      });
    },
    checkAuth: () => {
      const token = localStorage.getItem('token');
      if (token) {
        api.setToken(token);
        const role = localStorage.getItem('role');
        update(state => ({
          ...state,
          isAuthenticated: true,
          role: role,
          loading: false,
        }));
      } else {
        update(state => ({
          ...state,
          loading: false,
        }));
      }
    },
  };
}

export const authStore = createAuthStore();
