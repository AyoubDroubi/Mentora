import api from './api';

/**
 * Assessment Service
 * Handles all API calls related to the Assessment Module
 */

// ============================================
// Assessment Questions APIs
// ============================================

/**
 * Get all active assessment questions
 * @param {string} targetMajor - Optional: Filter by major (e.g., "ComputerScience")
 * @returns {Promise} List of assessment questions
 */
export const getAssessmentQuestions = async (targetMajor = null) => {
  try {
    const params = targetMajor ? { targetMajor } : {};
    const response = await api.get('/assessment/questions', { params });
    return response.data;
  } catch (error) {
    console.error('Error fetching assessment questions:', error);
    throw error;
  }
};

// ============================================
// Assessment Attempt APIs
// ============================================

/**
 * Start a new assessment attempt
 * @param {Object} data - { major, studyLevel }
 * @returns {Promise} Created assessment attempt
 */
export const startAssessment = async (data) => {
  try {
    // Send as query parameters to match backend expectation
    const response = await api.post('/assessment/start', null, {
      params: {
        major: data.major,
        studyLevel: data.studyLevel
      }
    });
    return response.data;
  } catch (error) {
    console.error('Error starting assessment:', error);
    throw error;
  }
};

/**
 * Submit responses for an assessment attempt (bulk)
 * @param {string} attemptId - Assessment attempt ID
 * @param {Array} responses - Array of { questionId, responseValue, responseTimeSeconds?, notes? }
 * @returns {Promise} Assessment completion response
 */
export const submitAssessmentResponses = async (attemptId, responses) => {
  try {
    // Use bulk submission endpoint to send all responses at once
    const response = await api.post('/assessment/responses/bulk', {
      assessmentAttemptId: attemptId,
      responses: responses
    });
    return response.data;
  } catch (error) {
    console.error('Error submitting assessment responses:', error);
    throw error;
  }
};

/**
 * Complete an assessment attempt
 * @param {string} attemptId - Assessment attempt ID
 * @returns {Promise} Completed assessment attempt with AI study plan
 */
export const completeAssessment = async (attemptId) => {
  try {
    const response = await api.post(`/assessment/${attemptId}/complete`);
    return response.data;
  } catch (error) {
    console.error('Error completing assessment:', error);
    throw error;
  }
};

/**
 * Get user's assessment attempts
 * @param {string} status - Optional: Filter by status (InProgress, Completed, Abandoned)
 * @returns {Promise} List of assessment attempts
 */
export const getUserAssessmentAttempts = async (status = null) => {
  try {
    const params = status ? { status } : {};
    const response = await api.get('/assessment/my-attempts', { params });
    return response.data;
  } catch (error) {
    console.error('Error fetching user assessment attempts:', error);
    throw error;
  }
};

/**
 * Get specific assessment attempt by ID
 * @param {string} attemptId - Assessment attempt ID
 * @returns {Promise} Assessment attempt details with responses
 */
export const getAssessmentAttempt = async (attemptId) => {
  try {
    const response = await api.get(`/assessment/${attemptId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching assessment attempt:', error);
    throw error;
  }
};

// ============================================
// Study Plan APIs
// ============================================

/**
 * Get user's study plans
 * @param {string} status - Optional: Filter by status (NotStarted, InProgress, Completed, OnHold)
 * @returns {Promise} List of study plans
 */
export const getUserStudyPlans = async (status = null) => {
  try {
    const params = status ? { status } : {};
    const response = await api.get('/studyplan/my-plans', { params });
    return response.data;
  } catch (error) {
    console.error('Error fetching user study plans:', error);
    throw error;
  }
};

/**
 * Get specific study plan by ID with all details
 * @param {string} planId - Study plan ID
 * @returns {Promise} Complete study plan with steps, skills, and resources
 */
export const getStudyPlan = async (planId) => {
  try {
    const response = await api.get(`/studyplan/${planId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching study plan:', error);
    throw error;
  }
};

/**
 * Update study plan status
 * @param {string} planId - Study plan ID
 * @param {string} status - New status (NotStarted, InProgress, Completed, OnHold)
 * @returns {Promise} Updated study plan
 */
export const updateStudyPlanStatus = async (planId, status) => {
  try {
    const response = await api.put(`/studyplan/${planId}/status`, { status });
    return response.data;
  } catch (error) {
    console.error('Error updating study plan status:', error);
    throw error;
  }
};

/**
 * Update study plan step status
 * @param {string} planId - Study plan ID
 * @param {string} stepId - Step ID
 * @param {string} status - New status (NotStarted, InProgress, Completed)
 * @returns {Promise} Updated step
 */
export const updateStepStatus = async (planId, stepId, status) => {
  try {
    const response = await api.put(`/studyplan/${planId}/steps/${stepId}/status`, { status });
    return response.data;
  } catch (error) {
    console.error('Error updating step status:', error);
    throw error;
  }
};

/**
 * Mark checkpoint as completed
 * @param {string} planId - Study plan ID
 * @param {string} checkpointId - Checkpoint ID
 * @returns {Promise} Updated checkpoint
 */
export const markCheckpointCompleted = async (planId, checkpointId) => {
  try {
    const response = await api.put(`/studyplan/${planId}/checkpoints/${checkpointId}/complete`);
    return response.data;
  } catch (error) {
    console.error('Error marking checkpoint as completed:', error);
    throw error;
  }
};

/**
 * Mark resource as completed
 * @param {string} planId - Study plan ID
 * @param {string} resourceId - Resource ID
 * @param {Object} data - Optional: { userRating, userNotes }
 * @returns {Promise} Updated resource
 */
export const markResourceCompleted = async (planId, resourceId, data = {}) => {
  try {
    const response = await api.put(`/studyplan/${planId}/resources/${resourceId}/complete`, data);
    return response.data;
  } catch (error) {
    console.error('Error marking resource as completed:', error);
    throw error;
  }
};

/**
 * Get study plan progress summary
 * @param {string} planId - Study plan ID
 * @returns {Promise} Progress statistics
 */
export const getStudyPlanProgress = async (planId) => {
  try {
    const response = await api.get(`/studyplan/${planId}/progress`);
    return response.data;
  } catch (error) {
    console.error('Error fetching study plan progress:', error);
    throw error;
  }
};

/**
 * Get all study plan steps for a specific plan
 * @param {string} planId - Study plan ID
 * @returns {Promise} List of steps with checkpoints and resources
 */
export const getStudyPlanSteps = async (planId) => {
  try {
    const response = await api.get(`/studyplan/${planId}/steps`);
    return response.data;
  } catch (error) {
    console.error('Error fetching study plan steps:', error);
    throw error;
  }
};

// ============================================
// Helper Functions
// ============================================

/**
 * Calculate assessment progress percentage
 * @param {number} answeredQuestions - Number of answered questions
 * @param {number} totalQuestions - Total number of questions
 * @returns {number} Progress percentage (0-100)
 */
export const calculateAssessmentProgress = (answeredQuestions, totalQuestions) => {
  if (totalQuestions === 0) return 0;
  return Math.round((answeredQuestions / totalQuestions) * 100);
};

/**
 * Format time duration from seconds to readable string
 * @param {number} seconds - Duration in seconds
 * @returns {string} Formatted time (e.g., "2h 30m")
 */
export const formatDuration = (seconds) => {
  if (!seconds) return '0m';
  
  const hours = Math.floor(seconds / 3600);
  const minutes = Math.floor((seconds % 3600) / 60);
  
  if (hours > 0) {
    return `${hours}h ${minutes}m`;
  }
  return `${minutes}m`;
};

/**
 * Get status badge color class
 * @param {string} status - Status value
 * @returns {string} Tailwind CSS color classes
 */
export const getStatusColor = (status) => {
  const statusColors = {
    NotStarted: 'bg-gray-100 text-gray-800',
    InProgress: 'bg-blue-100 text-blue-800',
    Completed: 'bg-green-100 text-green-800',
    OnHold: 'bg-yellow-100 text-yellow-800',
    Abandoned: 'bg-red-100 text-red-800',
  };
  
  return statusColors[status] || 'bg-gray-100 text-gray-800';
};

/**
 * Get difficulty badge color class
 * @param {string} difficulty - Difficulty level
 * @returns {string} Tailwind CSS color classes
 */
export const getDifficultyColor = (difficulty) => {
  const difficultyColors = {
    Beginner: 'bg-green-100 text-green-800',
    Intermediate: 'bg-yellow-100 text-yellow-800',
    Advanced: 'bg-orange-100 text-orange-800',
    Expert: 'bg-red-100 text-red-800',
  };
  
  return difficultyColors[difficulty] || 'bg-gray-100 text-gray-800';
};

export default {
  // Questions
  getAssessmentQuestions,
  
  // Attempts
  startAssessment,
  submitAssessmentResponses,
  completeAssessment,
  getUserAssessmentAttempts,
  getAssessmentAttempt,
  
  // Study Plans
  getUserStudyPlans,
  getStudyPlan,
  updateStudyPlanStatus,
  updateStepStatus,
  markCheckpointCompleted,
  markResourceCompleted,
  getStudyPlanProgress,
  getStudyPlanSteps,
  
  // Helpers
  calculateAssessmentProgress,
  formatDuration,
  getStatusColor,
  getDifficultyColor,
};
