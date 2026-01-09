import React, { createContext, useContext, useState, useEffect, useCallback, useRef } from 'react';
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

// Cache configuration
const CACHE_DURATION = 5 * 60 * 1000; // 5 minutes
const statsCache = {
  data: null,
  timestamp: null,
  isLoading: false,
  requestPromise: null
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
  const hasLoadedRef = useRef(false);
  const lastUserIdRef = useRef(null);

  // Update user and persist to localStorage
  const updateUser = useCallback((updates) => {
    setUser(prev => {
      const updated = { ...prev, ...updates };
      localStorage.setItem('user', JSON.stringify(updated));
      return updated;
    });
  }, []);

  // Check if cache is valid
  const isCacheValid = useCallback(() => {
    if (!statsCache.data || !statsCache.timestamp) return false;
    const now = Date.now();
    return (now - statsCache.timestamp) < CACHE_DURATION;
  }, []);

  // Load user stats from API with caching
  const loadUserStats = useCallback(async (forceRefresh = false) => {
    if (!user.id) return; // Don't load if no user ID

    // Return cached data if valid and not forcing refresh
    if (!forceRefresh && isCacheValid() && statsCache.data) {
      updateUser(statsCache.data);
      return;
    }

    // Return existing request if already loading
    if (statsCache.isLoading && statsCache.requestPromise) {
      return statsCache.requestPromise;
    }

    try {
      statsCache.isLoading = true;
      setLoading(true);

      // Create and store the request promise
      statsCache.requestPromise = Promise.all([
        todoService.getSummary().catch(() => ({ success: false })),
        notesService.getAllNotes().catch(() => ({ success: false })),
        plannerService.getAllEvents().catch(() => ({ success: false })),
        plannerService.getUpcomingEvents().catch(() => ({ success: false })),
        studySessionsService.getSummary().catch(() => ({ success: false })),
        attendanceService.getSummary().catch(() => ({ success: false }))
      ]);

      // Fetch all stats in parallel
      const [
        todoSummary,
        notesRes,
        eventsRes,
        upcomingRes,
        studyTime,
        attendance
      ] = await statsCache.requestPromise;

      // Prepare stats update
      const statsUpdate = {
        todosPending: todoSummary.success ? todoSummary.data.pendingTasks : 0,
        todosTotal: todoSummary.success ? todoSummary.data.totalTasks : 0,
        notesCount: notesRes.success ? notesRes.data.length : 0,
        eventsCount: eventsRes.success ? eventsRes.data.length : 0,
        upcomingEvents: upcomingRes.success ? upcomingRes.data.length : 0,
        totalHours: studyTime.success ? studyTime.data.formatted : '0h 0m',
        attendanceRate: attendance.success ? attendance.data.attendanceRate : 0,
        progressPercentage: attendance.success ? attendance.data.progressPercentage : 0,
      };

      // Update cache
      statsCache.data = statsUpdate;
      statsCache.timestamp = Date.now();

      // Update user stats
      updateUser(statsUpdate);
    } catch (error) {
      console.error('Error loading user stats:', error);
    } finally {
      statsCache.isLoading = false;
      statsCache.requestPromise = null;
      setLoading(false);
    }
  }, [user.id, isCacheValid, updateUser]);

  // Clear cache function
  const clearStatsCache = useCallback(() => {
    statsCache.data = null;
    statsCache.timestamp = null;
    statsCache.isLoading = false;
    statsCache.requestPromise = null;
  }, []);

  // Load stats only once when user.id is first set or changes
  useEffect(() => {
    // Skip if no user ID
    if (!user.id) {
      hasLoadedRef.current = false;
      lastUserIdRef.current = null;
      return;
    }

    // Skip if already loaded for this user
    if (hasLoadedRef.current && lastUserIdRef.current === user.id) {
      return;
    }

    // Mark as loaded and store user ID
    hasLoadedRef.current = true;
    lastUserIdRef.current = user.id;

    // Load stats
    loadUserStats();
  }, [user.id, loadUserStats]);

  return (
    <UserContext.Provider value={{ 
      user, 
      setUser, 
      updateUser, 
      loadUserStats,
      clearStatsCache,
      loading,
      isCacheValid: isCacheValid()
    }}>
      {children}
    </UserContext.Provider>
  );
};
