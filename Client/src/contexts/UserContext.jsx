import React, { createContext, useContext, useState, useEffect } from 'react';
import { 
  todoService, 
  plannerService, 
  notesService, 
  studySessionsService,
  attendanceService 
} from '../services';

const UserContext = createContext();

export const useUser = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(() => {
    // Load user from localStorage if available
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        return JSON.parse(storedUser);
      } catch (e) {
        console.error('Error parsing stored user:', e);
      }
    }
    
    // Default user state
    return {
      id: null,
      firstName: '',
      lastName: '',
      email: '',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=default',
      phone: '',
      location: '',
      bio: '',
      university: '',
      major: '',
      expectedGraduationYear: null,
      currentLevel: '',
      timezone: 'UTC',
      // Stats (loaded separately)
      studyStreak: 0,
      totalHours: '0h 0m',
      todosPending: 0,
      todosTotal: 0,
      notesCount: 0,
      eventsCount: 0,
      upcomingEvents: 0,
      attendanceRate: 0,
      progressPercentage: 0,
    };
  });

  const [loading, setLoading] = useState(false);

  // Update user and persist to localStorage
  const updateUser = (updates) => {
    setUser(prev => {
      const updated = { ...prev, ...updates };
      localStorage.setItem('user', JSON.stringify(updated));
      return updated;
    });
  };

  // Load user stats from API
  const loadUserStats = async () => {
    if (!user.id) return; // Don't load if no user ID

    try {
      setLoading(true);

      // Fetch all stats in parallel
      const [
        todoSummary,
        notesRes,
        eventsRes,
        upcomingRes,
        studyTime,
        attendance
      ] = await Promise.all([
        todoService.getSummary().catch(() => ({ success: false })),
        notesService.getAllNotes().catch(() => ({ success: false })),
        plannerService.getAllEvents().catch(() => ({ success: false })),
        plannerService.getUpcomingEvents().catch(() => ({ success: false })),
        studySessionsService.getSummary().catch(() => ({ success: false })),
        attendanceService.getSummary().catch(() => ({ success: false }))
      ]);

      // Update user stats
      updateUser({
        todosPending: todoSummary.success ? todoSummary.data.pendingTasks : 0,
        todosTotal: todoSummary.success ? todoSummary.data.totalTasks : 0,
        notesCount: notesRes.success ? notesRes.data.length : 0,
        eventsCount: eventsRes.success ? eventsRes.data.length : 0,
        upcomingEvents: upcomingRes.success ? upcomingRes.data.length : 0,
        totalHours: studyTime.success ? studyTime.data.formatted : '0h 0m',
        attendanceRate: attendance.success ? attendance.data.attendanceRate : 0,
        progressPercentage: attendance.success ? attendance.data.progressPercentage : 0,
      });
    } catch (error) {
      console.error('Error loading user stats:', error);
    } finally {
      setLoading(false);
    }
  };

  // Refresh stats when user changes
  useEffect(() => {
    if (user.id) {
      loadUserStats();
    }
  }, [user.id]);

  return (
    <UserContext.Provider value={{ 
      user, 
      setUser, 
      updateUser, 
      loadUserStats,
      loading 
    }}>
      {children}
    </UserContext.Provider>
  );
};
