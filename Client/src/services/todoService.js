import api from './api';

/**
 * Todo Service - Study Planner Module
 * Handles all Todo-related API calls
 */

const todoService = {
  /**
   * Get all todos with optional filtering
   * @param {string} filter - 'all', 'active', or 'completed'
   * @returns {Promise} List of todos
   */
  getAllTodos: async (filter = 'all') => {
    try {
      const response = await api.get('/todo', {
        params: { filter }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching todos:', error);
      throw error;
    }
  },

  /**
   * Get todo summary statistics
   * @returns {Promise} Summary object with task counts
   */
  getSummary: async () => {
    try {
      const response = await api.get('/todo/summary');
      return response.data;
    } catch (error) {
      console.error('Error fetching todo summary:', error);
      throw error;
    }
  },

  /**
   * Create a new todo
   * @param {string} title - Todo title
   * @returns {Promise} Created todo object
   */
  createTodo: async (title) => {
    try {
      const response = await api.post('/todo', { title });
      return response.data;
    } catch (error) {
      console.error('Error creating todo:', error);
      throw error;
    }
  },

  /**
   * Toggle todo completion status
   * @param {string} id - Todo ID (GUID)
   * @returns {Promise} Updated todo object
   */
  toggleTodo: async (id) => {
    try {
      const response = await api.patch(`/todo/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error toggling todo:', error);
      throw error;
    }
  },

  /**
   * Delete a todo
   * @param {string} id - Todo ID (GUID)
   * @returns {Promise} Success message
   */
  deleteTodo: async (id) => {
    try {
      const response = await api.delete(`/todo/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting todo:', error);
      throw error;
    }
  }
};

export default todoService;
