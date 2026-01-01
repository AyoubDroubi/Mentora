import api from './api';

/**
 * User Profile Service - Professional implementation
 * Handles all profile-related API calls with proper error handling
 */

/**
 * Convert study level string to number (0-4)
 * Freshman=0, Sophomore=1, Junior=2, Senior=3, Graduate=4
 */
const convertStudyLevelToNumber = (level) => {
  if (typeof level === 'number') return level;
  
  const levelMap = {
    'Freshman': 0,
    'Sophomore': 1,
    'Junior': 2,
    'Senior': 3,
    'Graduate': 4
  };
  
  return levelMap[level] ?? 0;
};

/**
 * Convert study level number to string
 */
const convertStudyLevelToString = (level) => {
  const levels = ['Freshman', 'Sophomore', 'Junior', 'Senior', 'Graduate'];
  return levels[level] || 'Freshman';
};

const userProfileService = {
  /**
   * Get current user's profile
   */
  getProfile: async () => {
    try {
      const response = await api.get('/UserProfile');
      return response.data;
    } catch (error) {
      if (error.response?.status === 404) {
        return null; // Profile doesn't exist
      }
      console.error('Get profile error:', error);
      throw error;
    }
  },

  /**
   * Create or update user profile
   */
  updateProfile: async (profileData) => {
    try {
      // Prepare data for API
      const payload = {
        // Personal info (optional)
        bio: profileData.bio || null,
        location: profileData.location || null,
        phoneNumber: profileData.phoneNumber || null,
        dateOfBirth: profileData.dateOfBirth || null,
        
        // Academic info (required)
        university: profileData.university?.trim() || '',
        major: profileData.major?.trim() || '',
        expectedGraduationYear: parseInt(profileData.expectedGraduationYear) || new Date().getFullYear(),
        currentLevel: convertStudyLevelToNumber(profileData.currentLevel),
        
        // System config (required)
        timezone: profileData.timezone?.trim() || 'UTC',
        
        // Social links (optional)
        linkedInUrl: profileData.linkedInUrl || null,
        gitHubUrl: profileData.gitHubUrl || null,
        avatarUrl: profileData.avatarUrl || null
      };

      console.log('Sending profile update:', payload);

      const response = await api.put('/UserProfile', payload);
      
      console.log('Profile update successful:', response.data);
      return response.data;
    } catch (error) {
      console.error('Update profile error:', {
        status: error.response?.status,
        data: error.response?.data
      });
      throw error;
    }
  },

  /**
   * Check if user has a profile
   */
  hasProfile: async () => {
    try {
      const response = await api.get('/UserProfile/exists');
      return response.data.exists;
    } catch (error) {
      console.error('Has profile error:', error);
      return false;
    }
  },

  /**
   * Get profile completion percentage
   */
  getCompletion: async () => {
    try {
      const response = await api.get('/UserProfile/completion');
      return response.data.completionPercentage;
    } catch (error) {
      console.error('Get completion error:', error);
      return 0;
    }
  },

  /**
   * Get suggested timezones
   */
  getTimezones: async (location = null) => {
    try {
      const params = location ? { location } : {};
      const response = await api.get('/UserProfile/timezones', { params });
      return response.data;
    } catch (error) {
      console.error('Get timezones error:', error);
      return ['UTC', 'Asia/Amman'];
    }
  },

  /**
   * Validate timezone format
   */
  validateTimezone: async (timezone) => {
    try {
      const response = await api.get('/UserProfile/validate-timezone', {
        params: { timezone }
      });
      return response.data.isValid;
    } catch (error) {
      console.error('Validate timezone error:', error);
      return false;
    }
  },

  // Helper functions
  convertStudyLevelToNumber,
  convertStudyLevelToString
};

export default userProfileService;
