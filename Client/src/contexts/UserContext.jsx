import React, { createContext, useContext, useState, useEffect } from 'react';
import authService from '../services/authService';

const UserContext = createContext();

export const useUser = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // Fetch basic user profile data on mount (independent of auth flow)
  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        // Check if authenticated
        if (authService.isAuthenticated()) {
          // Fetch basic user info from API
          const result = await authService.getCurrentUser();
          if (result.success) {
            // Store only basic profile data
            const basicUserData = {
              id: result.data.userId,
              firstName: result.data.firstName || '',
              lastName: result.data.lastName || '',
              email: result.data.email || '',
              avatar: result.data.avatar || 'https://api.dicebear.com/7.x/avataaars/svg?seed=default',
            };
            setUser(basicUserData);
            localStorage.setItem('user', JSON.stringify(basicUserData));
          }
        }
      } catch (error) {
        console.error('Error fetching user profile:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  // Update user profile data
  const updateUser = (updates) => {
    setUser(prev => {
      const updated = { ...prev, ...updates };
      localStorage.setItem('user', JSON.stringify(updated));
      return updated;
    });
  };

  // Clear user data (called on logout)
  const clearUser = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  return (
    <UserContext.Provider value={{ 
      user, 
      loading,
      setUser, 
      updateUser,
      clearUser
    }}>
      {children}
    </UserContext.Provider>
  );
};
