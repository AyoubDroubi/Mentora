import api from './api';

/**
 * User Profile Service
 * Handles all user profile-related API calls per SRS Module 2
 * All endpoints use /UserProfile (with capital U and P)
 */

const userProfileService = {
  /**
   * Get current user's profile
   * GET /api/UserProfile
   * @returns {Promise<Object>} User profile data
   */
  getProfile: async () => {
    try {
      const response = await api.get('/UserProfile');
      return response.data;
    } catch (error) {
      if (error.response?.status === 404) {
        return null; // Profile doesn't exist yet
      }
      throw error;
    }
  },

  /**
   * Create or update user profile
   * PUT /api/UserProfile
   * @param {Object} profileData - Profile data including academic attributes
   * @returns {Promise<Object>} Updated profile
   */
    updateProfile: async (profileData) => {
        debugger
    const response = await api.put('/UserProfile', profileData);
    return response.data;
  },

  /**
   * Check if user has a profile
   * GET /api/UserProfile/exists
   * @returns {Promise<boolean>}
   */
  hasProfile: async () => {
    const response = await api.get('/UserProfile/exists');
    return response.data.exists;
  },

  /**
   * Get profile completion percentage
   * GET /api/UserProfile/completion
   * @returns {Promise<number>} Completion percentage (0-100)
   */
  getCompletion: async () => {
    const response = await api.get('/UserProfile/completion');
    return response.data.completionPercentage;
  },

  /**
   * Get suggested timezones based on location
   * GET /api/UserProfile/timezones
   * @param {string} location - Optional location string
   * @returns {Promise<Array<string>>} List of timezone identifiers
   */
  getTimezones: async (location = null) => {
    const params = location ? { location } : {};
    const response = await api.get('/UserProfile/timezones', { params });
    return response.data;
  },

  /**
   * Validate timezone format
   * GET /api/UserProfile/validate-timezone
   * @param {string} timezone - Timezone to validate
   * @returns {Promise<boolean>}
   */
  validateTimezone: async (timezone) => {
    const response = await api.get('/UserProfile/validate-timezone', {
      params: { timezone }
    });
    return response.data.isValid;
  }
};

export default userProfileService;
