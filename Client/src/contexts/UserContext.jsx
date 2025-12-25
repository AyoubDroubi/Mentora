import React, { createContext, useContext, useState } from 'react';

const UserContext = createContext();

export const useUser = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState({
    name: 'Bijaya Kumar Behera',
    email: 'kumarbijayebehera07@gmail.com',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Bijaya',
    phone: '+962 79 123 4567',
    location: 'Amman, Jordan',
    joinDate: 'January 2024',
    studyStreak: 7,
    totalHours: 124,
    completedTasks: 45,
    todosPending: 3,
    notesCount: 12,
    eventsCount: 5,
    attendanceRate: 95,
    bio: 'Computer Science student passionate about learning and personal growth.',
    careerGoal: 'Software Developer',
    skills: ['JavaScript', 'React', 'Node.js'],
    level: 'Beginner',
    careerPlan: null, // Will store the generated career plan
  });

  const updateUser = (updates) => {
    setUser(prev => ({ ...prev, ...updates }));
  };

  return (
    <UserContext.Provider value={{ user, setUser, updateUser }}>
      {children}
    </UserContext.Provider>
  );
};
