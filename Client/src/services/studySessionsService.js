import api from './api';

/**
 * Study Sessions Service - Study Planner Module
 * Handles all Study Session (Pomodoro) related API calls
 */

const studySessionsService = {
  /**
   * Get all study sessions
   * @param {number} limit - Maximum number of sessions to fetch (default: 50)
   * @returns {Promise} List of study sessions
   */
  getAllSessions: async (limit = 50) => {
    try {
      const response = await api.get('/study-sessions', {
        params: { limit }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching study sessions:', error);
      throw error;
    }
  },

  /**
   * Get study time summary
   * @returns {Promise} Summary object with total hours and minutes
   */
  getSummary: async () => {
    try {
      const response = await api.get('/study-sessions/summary');
      return response.data;
    } catch (error) {
      console.error('Error fetching study summary:', error);
      throw error;
    }
  },

  /**
   * Get sessions by date range
   * @param {string} from - Start date (YYYY-MM-DD)
   * @param {string} to - End date (YYYY-MM-DD)
   * @returns {Promise} Sessions in date range with summary
   */
  getSessionsByRange: async (from, to) => {
    try {
      const response = await api.get('/study-sessions/range', {
        params: { from, to }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching sessions by range:', error);
      throw error;
    }
  },

  /**
   * Save a completed study session
   * @param {Object} sessionData - Session details
   * @param {number} sessionData.durationMinutes - Session duration in minutes
   * @param {string} sessionData.startTime - Session start time (ISO 8601) - optional
   * @param {number} sessionData.pauseCount - Number of pauses - optional (default: 0)
   * @param {number} sessionData.focusScore - Focus score (0-100) - optional (default: 100)
   * @returns {Promise} Created session object
   */
  saveSession: async (sessionData) => {
    try {
      const response = await api.post('/study-sessions', sessionData);
      return response.data;
    } catch (error) {
      console.error('Error saving study session:', error);
      throw error;
    }
  },

  /**
   * Delete a study session
   * @param {string} id - Session ID (GUID)
   * @returns {Promise} Success message
   */
  deleteSession: async (id) => {
    try {
      const response = await api.delete(`/study-sessions/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting study session:', error);
      throw error;
    }
  }
};

export default studySessionsService;
