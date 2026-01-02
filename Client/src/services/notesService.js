import api from './api';

/**
 * Notes Service - Study Planner Module
 * Handles all Note-related API calls
 */

const notesService = {
  /**
   * Get all notes
   * @returns {Promise} List of notes
   */
  getAllNotes: async () => {
    try {
      const response = await api.get('/notes');
      return response.data;
    } catch (error) {
      console.error('Error fetching notes:', error);
      throw error;
    }
  },

  /**
   * Get a specific note by ID
   * @param {string} id - Note ID (GUID)
   * @returns {Promise} Note object
   */
  getNoteById: async (id) => {
    try {
      const response = await api.get(`/notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching note:', error);
      throw error;
    }
  },

  /**
   * Create a new note
   * @param {Object} noteData - Note details
   * @param {string} noteData.title - Note title
   * @param {string} noteData.content - Note content
   * @returns {Promise} Created note object
   */
  createNote: async (noteData) => {
    try {
      const response = await api.post('/notes', noteData);
      return response.data;
    } catch (error) {
      console.error('Error creating note:', error);
      throw error;
    }
  },

  /**
   * Update an existing note
   * @param {string} id - Note ID (GUID)
   * @param {Object} noteData - Updated note details
   * @param {string} noteData.title - Note title (optional)
   * @param {string} noteData.content - Note content (optional)
   * @returns {Promise} Updated note object
   */
  updateNote: async (id, noteData) => {
    try {
      const response = await api.put(`/notes/${id}`, noteData);
      return response.data;
    } catch (error) {
      console.error('Error updating note:', error);
      throw error;
    }
  },

  /**
   * Delete a note
   * @param {string} id - Note ID (GUID)
   * @returns {Promise} Success message
   */
  deleteNote: async (id) => {
    try {
      const response = await api.delete(`/notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting note:', error);
      throw error;
    }
  }
};

export default notesService;
