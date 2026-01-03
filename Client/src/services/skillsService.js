import api from './api';

/**
 * Skills Service - User Profile Skills Management
 * Per SRS 2.3: Skills Portfolio Management
 */

const skillsService = {
  // ==================== CRUD Operations ====================

  /**
   * Get all skills for current user
   * GET /api/userprofile/skills?proficiencyLevel=2&isFeatured=true&sortBy=proficiency
   */
  getSkills: async (filters = {}) => {
    try {
      const params = new URLSearchParams();
      
      // Only add parameters if they have actual values (not null/undefined)
      if (filters.proficiencyLevel !== null && filters.proficiencyLevel !== undefined) {
        params.append('proficiencyLevel', filters.proficiencyLevel);
      }
      if (filters.isFeatured !== null && filters.isFeatured !== undefined) {
        params.append('isFeatured', filters.isFeatured);
      }
      if (filters.sortBy) {
        params.append('sortBy', filters.sortBy);
      }

      const queryString = params.toString();
      const url = queryString ? `/userprofile/skills?${queryString}` : '/userprofile/skills';
      
      const response = await api.get(url);
      return response.data;
    } catch (error) {
      console.error('Get skills error:', error);
      throw error;
    }
  },

  /**
   * Get featured skills
   * GET /api/userprofile/skills/featured
   */
  getFeaturedSkills: async () => {
    try {
      const response = await api.get('/userprofile/skills/featured');
      return response.data;
    } catch (error) {
      console.error('Get featured skills error:', error);
      throw error;
    }
  },

  /**
   * Add a skill
   * POST /api/userprofile/skills
   */
  addSkill: async (skillData) => {
    try {
      const payload = {
        skillId: skillData.skillId,
        proficiencyLevel: skillData.proficiencyLevel || 0,
        acquisitionMethod: skillData.acquisitionMethod || null,
        startedDate: skillData.startedDate || null,
        yearsOfExperience: skillData.yearsOfExperience || null,
        isFeatured: skillData.isFeatured || false,
        notes: skillData.notes || null,
        displayOrder: skillData.displayOrder || 0
      };

      const response = await api.post('/userprofile/skills', payload);
      return response.data;
    } catch (error) {
      console.error('Add skill error:', error);
      throw error;
    }
  },

  /**
   * Add multiple skills in bulk
   * POST /api/userprofile/skills/bulk
   */
  addSkillsBulk: async (skills) => {
    try {
      const response = await api.post('/userprofile/skills/bulk', {
        skills: skills
      });
      return response.data;
    } catch (error) {
      console.error('Add skills bulk error:', error);
      throw error;
    }
  },

  /**
   * Update a skill
   * PATCH /api/userprofile/skills/{id}
   */
  updateSkill: async (skillId, updates) => {
    try {
      const response = await api.patch(`/userprofile/skills/${skillId}`, updates);
      return response.data;
    } catch (error) {
      console.error('Update skill error:', error);
      throw error;
    }
  },

  /**
   * Delete a skill
   * DELETE /api/userprofile/skills/{id}
   */
  deleteSkill: async (skillId) => {
    try {
      const response = await api.delete(`/userprofile/skills/${skillId}`);
      return response.data;
    } catch (error) {
      console.error('Delete skill error:', error);
      throw error;
    }
  },

  /**
   * Delete multiple skills
   * DELETE /api/userprofile/skills/bulk
   */
  deleteSkillsBulk: async (skillIds) => {
    try {
      const response = await api.delete('/userprofile/skills/bulk', {
        data: { skillIds }
      });
      return response.data;
    } catch (error) {
      console.error('Delete skills bulk error:', error);
      throw error;
    }
  },

  /**
   * Toggle featured status
   * PATCH /api/userprofile/skills/{id}/featured
   */
  toggleFeatured: async (skillId) => {
    try {
      const response = await api.patch(`/userprofile/skills/${skillId}/featured`);
      return response.data;
    } catch (error) {
      console.error('Toggle featured error:', error);
      throw error;
    }
  },

  /**
   * Reorder skills
   * PATCH /api/userprofile/skills/reorder
   */
  reorderSkills: async (skillOrders) => {
    try {
      const response = await api.patch('/userprofile/skills/reorder', {
        skillOrders: skillOrders.map(s => ({
          skillId: s.skillId,
          displayOrder: s.displayOrder
        }))
      });
      return response.data;
    } catch (error) {
      console.error('Reorder skills error:', error);
      throw error;
    }
  },

  // ==================== Analytics ====================

  /**
   * Get skills summary
   * GET /api/userprofile/skills/summary
   */
  getSummary: async () => {
    try {
      const response = await api.get('/userprofile/skills/summary');
      return response.data;
    } catch (error) {
      console.error('Get summary error:', error);
      throw error;
    }
  },

  /**
   * Get skills distribution
   * GET /api/userprofile/skills/distribution
   */
  getDistribution: async () => {
    try {
      const response = await api.get('/userprofile/skills/distribution');
      return response.data;
    } catch (error) {
      console.error('Get distribution error:', error);
      throw error;
    }
  },

  /**
   * Get skills timeline
   * GET /api/userprofile/skills/timeline
   */
  getTimeline: async () => {
    try {
      const response = await api.get('/userprofile/skills/timeline');
      return response.data;
    } catch (error) {
      console.error('Get timeline error:', error);
      throw error;
    }
  },

  /**
   * Get skills coverage analysis
   * GET /api/userprofile/skills/coverage
   */
  getCoverage: async () => {
    try {
      const response = await api.get('/userprofile/skills/coverage');
      return response.data;
    } catch (error) {
      console.error('Get coverage error:', error);
      throw error;
    }
  },

  // ==================== Helper Functions ====================

  /**
   * Get proficiency level name
   */
  getProficiencyName: (level) => {
    const levels = ['Beginner', 'Intermediate', 'Advanced', 'Expert'];
    return levels[level] || 'Unknown';
  },

  /**
   * Get proficiency color
   */
  getProficiencyColor: (level) => {
    const colors = {
      0: 'bg-gray-500',
      1: 'bg-blue-500',
      2: 'bg-purple-500',
      3: 'bg-green-500'
    };
    return colors[level] || 'bg-gray-500';
  },

  /**
   * Get category icon
   */
  getCategoryIcon: (category) => {
    const icons = {
      Technical: '??',
      Soft: '??',
      Business: '??',
      Creative: '??'
    };
    return icons[category] || '??';
  }
};

export default skillsService;
