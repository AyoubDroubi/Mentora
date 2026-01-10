import api from './api';

/**
 * Master Skills Service - Skills Library Management
 * Provides access to the master skills repository
 */

const masterSkillsService = {
  /**
   * Get all skills from master library
   * GET /api/skills?category=Technical&search=react
   */
  getAllSkills: async (filters = {}) => {
    try {
      const params = new URLSearchParams();
      
      if (filters.category) {
        params.append('category', filters.category);
      }
      if (filters.search) {
        params.append('search', filters.search);
      }

      const queryString = params.toString();
      const url = queryString ? `/skills?${queryString}` : '/skills';
      
      const response = await api.get(url);
      return response.data;
    } catch (error) {
      console.error('Get all skills error:', error);
      throw error;
    }
  },

  /**
   * Get skill by ID
   * GET /api/skills/{id}
   */
  getSkillById: async (skillId) => {
    try {
      const response = await api.get(`/skills/${skillId}`);
      return response.data;
    } catch (error) {
      console.error('Get skill by ID error:', error);
      throw error;
    }
  },

  /**
   * Get skills by category
   * GET /api/skills/category/{category}
   */
  getSkillsByCategory: async (category) => {
    try {
      const response = await api.get(`/skills/category/${category}`);
      return response.data;
    } catch (error) {
      console.error('Get skills by category error:', error);
      throw error;
    }
  },

  /**
   * Get skill categories
   * GET /api/skills/categories
   */
  getCategories: async () => {
    try {
      const response = await api.get('/skills/categories');
      return response.data;
    } catch (error) {
      console.error('Get categories error:', error);
      throw error;
    }
  },

  /**
   * Create a new skill (Admin/User feature)
   * POST /api/skills
   */
  createSkill: async (skillData) => {
    try {
      const response = await api.post('/skills', {
        name: skillData.name,
        category: skillData.category,
        description: skillData.description || ''
      });
      return response.data;
    } catch (error) {
      console.error('Create skill error:', error);
      throw error;
    }
  }
};

export default masterSkillsService;
