import React, { createContext, useContext, useState, useEffect } from 'react';
import userProfileService from '../services/userProfileService';
import { useAuth } from './AuthContext';

const ProfileContext = createContext();

export const useProfile = () => {
  const context = useContext(ProfileContext);
  if (!context) {
    throw new Error('useProfile must be used within a ProfileProvider');
  }
  return context;
};

export const ProfileProvider = ({ children }) => {
  const { user, isAuthenticated } = useAuth();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [completion, setCompletion] = useState(0);
  const [hasProfile, setHasProfile] = useState(false);

  // Load profile when user authenticates
  useEffect(() => {
    if (isAuthenticated) {
      loadProfile();
      checkProfileExists();
    } else {
      // Clear profile on logout
      setProfile(null);
      setHasProfile(false);
      setCompletion(0);
    }
  }, [isAuthenticated]);

  /**
   * Load user profile from API
   */
  const loadProfile = async () => {
    try {
      setLoading(true);
      const profileData = await userProfileService.getProfile();
      setProfile(profileData);
      
      if (profileData) {
        setHasProfile(true);
        await loadCompletion();
      }
    } catch (error) {
      console.error('Error loading profile:', error);
      setProfile(null);
      setHasProfile(false);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Check if profile exists
   */
  const checkProfileExists = async () => {
    try {
      const exists = await userProfileService.hasProfile();
      setHasProfile(exists);
    } catch (error) {
      console.error('Error checking profile:', error);
    }
  };

  /**
   * Load profile completion percentage
   */
  const loadCompletion = async () => {
    try {
      const percentage = await userProfileService.getCompletion();
      setCompletion(percentage);
    } catch (error) {
      console.error('Error loading completion:', error);
    }
  };

  /**
   * Update user profile
   * @param {Object} profileData - Profile data to update
   */
  const updateProfile = async (profileData) => {
    try {
      setLoading(true);
      const updatedProfile = await userProfileService.updateProfile(profileData);
      setProfile(updatedProfile);
      setHasProfile(true);
      await loadCompletion();
      return { success: true, data: updatedProfile };
    } catch (error) {
      console.error('Error updating profile:', error);
      return { 
        success: false, 
        error: error.response?.data?.message || 'Failed to update profile' 
      };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Get suggested timezones
   * @param {string} location - Optional location
   */
  const getTimezones = async (location = null) => {
    try {
      return await userProfileService.getTimezones(location);
    } catch (error) {
      console.error('Error getting timezones:', error);
      return [];
    }
  };

  /**
   * Validate timezone
   * @param {string} timezone - Timezone to validate
   */
  const validateTimezone = async (timezone) => {
    try {
      return await userProfileService.validateTimezone(timezone);
    } catch (error) {
      console.error('Error validating timezone:', error);
      return false;
    }
  };

  /**
   * Refresh profile data
   */
  const refreshProfile = async () => {
    await loadProfile();
  };

  const value = {
    profile,
    loading,
    completion,
    hasProfile,
    updateProfile,
    refreshProfile,
    getTimezones,
    validateTimezone,
    // Helper getters
    needsOnboarding: isAuthenticated && !hasProfile,
    isProfileComplete: completion === 100,
  };

  return (
    <ProfileContext.Provider value={value}>
      {children}
    </ProfileContext.Provider>
  );
};
