import api from './api';

/**
 * Planner Service - Study Planner Module
 * Handles all Event/Calendar-related API calls
 */

const plannerService = {
  /**
   * Get all events
   * @param {string} date - Optional date filter (YYYY-MM-DD)
   * @returns {Promise} List of events
   */
  getAllEvents: async (date = null) => {
    try {
      const response = await api.get('/planner/events', {
        params: date ? { date } : {}
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching events:', error);
      throw error;
    }
  },

  /**
   * Get upcoming events
   * @returns {Promise} List of upcoming events
   */
  getUpcomingEvents: async () => {
    try {
      const response = await api.get('/planner/events/upcoming');
      return response.data;
    } catch (error) {
      console.error('Error fetching upcoming events:', error);
      throw error;
    }
  },

  /**
   * Create a new event
   * @param {Object} eventData - Event details
   * @param {string} eventData.title - Event title
   * @param {string} eventData.eventDateTime - Event date/time (ISO 8601)
   * @returns {Promise} Created event object
   */
  createEvent: async (eventData) => {
    try {
      const response = await api.post('/planner/events', eventData);
      return response.data;
    } catch (error) {
      console.error('Error creating event:', error);
      throw error;
    }
  },

  /**
   * Mark event as attended
   * @param {string} id - Event ID (GUID)
   * @returns {Promise} Updated event object
   */
  markAttended: async (id) => {
    try {
      const response = await api.patch(`/planner/events/${id}/attend`);
      return response.data;
    } catch (error) {
      console.error('Error marking event as attended:', error);
      throw error;
    }
  },

  /**
   * Delete an event
   * @param {string} id - Event ID (GUID)
   * @returns {Promise} Success message
   */
  deleteEvent: async (id) => {
    try {
      const response = await api.delete(`/planner/events/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting event:', error);
      throw error;
    }
  }
};

export default plannerService;
