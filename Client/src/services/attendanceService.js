import api from './api';

/**
 * Attendance Service - Study Planner Module
 * Handles attendance tracking and progress calculation
 */

const attendanceService = {
  /**
   * Get attendance and progress summary
   * Includes tasks, events, and overall progress percentage
   * Progress = 50% tasks + 50% events
   * @returns {Promise} Attendance summary object
   */
  getSummary: async () => {
    try {
      const response = await api.get('/study-planner/attendance/summary');
      return response.data;
    } catch (error) {
      console.error('Error fetching attendance summary:', error);
      throw error;
    }
  },

  /**
   * Get attendance history
   * @param {number} days - Number of days to fetch (default: 30)
   * @returns {Promise} Attendance history with completed tasks and attended events
   */
  getHistory: async (days = 30) => {
    try {
      const response = await api.get('/study-planner/attendance/history', {
        params: { days }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching attendance history:', error);
      throw error;
    }
  },

  /**
   * Get weekly progress report
   * Shows tasks and events for current week
   * @returns {Promise} Weekly progress with daily breakdown
   */
  getWeeklyProgress: async () => {
    try {
      const response = await api.get('/study-planner/attendance/weekly');
      return response.data;
    } catch (error) {
      console.error('Error fetching weekly progress:', error);
      throw error;
    }
  }
};

export default attendanceService;
