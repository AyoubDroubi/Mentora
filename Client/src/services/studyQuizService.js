import api from './api';

/**
 * Study Quiz Service - Study Planner Module
 * Handles all Study Quiz-related API calls
 */

const studyQuizService = {
  /**
   * Get quiz questions
   * @returns {Promise} List of quiz questions
   */
  getQuestions: async () => {
    try {
      const response = await api.get('/study-quiz/questions');
      return response.data;
    } catch (error) {
      console.error('Error fetching quiz questions:', error);
      throw error;
    }
  },

  /**
   * Submit quiz answers and get personalized study plan
   * @param {Object} answers - Quiz answers (key: question id, value: answer)
   * @returns {Promise} Quiz result with study plan
   */
  submitQuiz: async (answers) => {
    try {
      const response = await api.post('/study-quiz/submit', answers);
      return response.data;
    } catch (error) {
      console.error('Error submitting quiz:', error);
      throw error;
    }
  },

  /**
   * Get latest quiz attempt
   * @returns {Promise} Latest quiz attempt with study plan
   */
  getLatestAttempt: async () => {
    try {
      const response = await api.get('/study-quiz/latest');
      return response.data;
    } catch (error) {
      console.error('Error fetching latest quiz attempt:', error);
      throw error;
    }
  }
};

export default studyQuizService;
