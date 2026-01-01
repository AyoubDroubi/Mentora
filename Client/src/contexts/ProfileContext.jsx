import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
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
    } else {
      // Clear state on logout
      setProfile(null);
      setHasProfile(false);
      setCompletion(0);
    }
  }, [isAuthenticated]);

  /**
   * Load user profile
   */
  const loadProfile = useCallback(async () => {
    try {
      setLoading(true);
      const profileData = await userProfileService.getProfile();
      
      if (profileData) {
        setProfile(profileData);
        setHasProfile(true);
        await loadCompletion();
      } else {
        setProfile(null);
        setHasProfile(false);
        setCompletion(0);
      }
    } catch (error) {
      console.error('Error loading profile:', error);
      setProfile(null);
      setHasProfile(false);
    } finally {
      setLoading(false);
    }
  }, []);

  /**
   * Load completion percentage
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
   * Update profile with proper error handling
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
      // Extract error details
      const errorData = error.response?.data;
      let errorMessage = 'Failed to update profile';
      let validationErrors = null;

      if (errorData) {
        if (errorData.errors) {
          // Format validation errors
          validationErrors = errorData.errors;
          const errorList = Object.entries(errorData.errors)
            .map(([field, messages]) => {
              const msgs = Array.isArray(messages) ? messages : [messages];
              return `${field}: ${msgs.join(', ')}`;
            })
            .join('\n');
          errorMessage = `Validation failed:\n${errorList}`;
        } else if (errorData.message) {
          errorMessage = errorData.message;
        }
      }

      console.error('Error updating profile:', {
        message: errorMessage,
        validationErrors,
        originalError: error
      });

      return {
        success: false,
        error: errorMessage,
        validationErrors
      };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Get timezones
   */
  const getTimezones = async (location = null) => {
    try {
      return await userProfileService.getTimezones(location);
    } catch (error) {
      console.error('Error getting timezones:', error);
      return ['UTC', 'Asia/Amman'];
    }
  };

  /**
   * Validate timezone
   */
  const validateTimezone = async (timezone) => {
    try {
      return await userProfileService.validateTimezone(timezone);
    } catch (error) {
      console.error('Error validating timezone:', error);
      return false;
    }
  };

  const value = {
    profile,
    loading,
    completion,
    hasProfile,
    updateProfile,
    refreshProfile: loadProfile,
    getTimezones,
    validateTimezone,
    needsOnboarding: isAuthenticated && !hasProfile,
    isProfileComplete: completion === 100,
  };

  return (
    <ProfileContext.Provider value={value}>
      {children}
    </ProfileContext.Provider>
  );
};
