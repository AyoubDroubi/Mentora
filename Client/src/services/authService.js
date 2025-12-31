import api from './api';

// Get device info for tracking
const getDeviceInfo = () => {
  const userAgent = navigator.userAgent;
  let browserName = 'Unknown';
  let osName = 'Unknown';

  // Detect browser
  if (userAgent.indexOf('Chrome') > -1) browserName = 'Chrome';
  else if (userAgent.indexOf('Safari') > -1) browserName = 'Safari';
  else if (userAgent.indexOf('Firefox') > -1) browserName = 'Firefox';
  else if (userAgent.indexOf('Edge') > -1) browserName = 'Edge';

  // Detect OS
  if (userAgent.indexOf('Win') > -1) osName = 'Windows';
  else if (userAgent.indexOf('Mac') > -1) osName = 'macOS';
  else if (userAgent.indexOf('Linux') > -1) osName = 'Linux';
  else if (userAgent.indexOf('Android') > -1) osName = 'Android';
  else if (userAgent.indexOf('iOS') > -1) osName = 'iOS';

  return `${browserName}/${osName}`;
};

// Authentication Service
const authService = {
  /**
   * Register a new user
   * @param {Object} userData - { firstName, lastName, email, password }
   */
  register: async (userData) => {
    try {
        const response = await api.post('/Auth/register', userData);
      return {
        success: true,
        data: response.data,
        message: response.data.message,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Registration failed',
        errors: error.response?.data?.errors || [error.message],
      };
    }
  },

  /**
   * Login user
   * @param {Object} credentials - { email, password }
   */
  login: async (credentials) => {
    try {
        const response = await api.post('/Auth/login', {
        ...credentials,
        deviceInfo: getDeviceInfo(),
      });

      if (response.data.isSuccess) {
        // Store tokens
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        
        // Store user info
        const userInfo = {
          email: credentials.email,
          tokenExpiration: response.data.tokenExpiration,
        };
        localStorage.setItem('user', JSON.stringify(userInfo));

        return {
          success: true,
          data: response.data,
          message: response.data.message,
        };
      }

      return {
        success: false,
        message: response.data.message,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Login failed',
        errors: error.response?.data?.errors || [error.message],
      };
    }
  },

  /**
   * Logout user
   */
  logout: async () => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      if (refreshToken) {
          await api.post('/Auth/logout', { refreshToken });
      }
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      // Clear local storage
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
    }
  },

  /**
   * Logout from all devices
   */
  logoutAll: async () => {
    try {
        await api.post('/Auth/logout-all');
    } catch (error) {
      console.error('Logout all error:', error);
    } finally {
      // Clear local storage
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
    }
  },

  /**
   * Refresh access token
   */
  refreshToken: async () => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      if (!refreshToken) {
        throw new Error('No refresh token available');
      }

      const response = await api.post('/Auth/refresh-token', { refreshToken });

      if (response.data.isSuccess) {
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        return {
          success: true,
          data: response.data,
        };
      }

      return {
        success: false,
        message: response.data.message,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Token refresh failed',
      };
    }
  },

  /**
   * Forgot password - Request reset token
   * @param {string} email
   */
  forgotPassword: async (email) => {
    try {
        const response = await api.post('/Auth/forgot-password', { email });
      return {
        success: true,
        message: response.data.message,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Request failed',
      };
    }
  },

  /**
   * Reset password using token
   * @param {Object} resetData - { email, token, newPassword, confirmPassword }
   */
  resetPassword: async (resetData) => {
    try {
        const response = await api.post('/Auth/reset-password', resetData);
      return {
        success: true,
        message: response.data.message,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Password reset failed',
        errors: error.response?.data?.errors || [error.message],
      };
    }
  },

  /**
   * Get current user info
   */
  getCurrentUser: async () => {
    try {
        const response = await api.get('/Auth/me');
      return {
        success: true,
        data: response.data,
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to get user info',
      };
    }
  },

  /**
   * Check if user is authenticated
   */
  isAuthenticated: () => {
    const token = localStorage.getItem('accessToken');
    const user = localStorage.getItem('user');
    return !!token && !!user;
  },

  /**
   * Get stored user info
   */
  getUser: () => {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  /**
   * Get access token
   */
  getAccessToken: () => {
    return localStorage.getItem('accessToken');
  },
};

export default authService;
