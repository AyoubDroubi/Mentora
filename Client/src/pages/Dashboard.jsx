import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import {
  CheckSquare,
  Clock,
  ChevronRight,
  Award,
  TrendingUp,
  Flame,
  Target,
  BookOpenCheck,
  User
} from 'lucide-react';
import { todoService, plannerService } from '../services';

export default function Dashboard() {
  const navigate = useNavigate();
  const { user, loading: userLoading } = useUser();

  // Theme colors
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // State for page-specific data only
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [upcomingEvents, setUpcomingEvents] = useState([]);
  const [pendingTasks, setPendingTasks] = useState([]);

  // Fetch only page-specific data (upcoming events and tasks)
  useEffect(() => {
    fetchPageData();
  }, []);

  const fetchPageData = async () => {
    setLoading(true);
    setError(null);
    
    try {
      // Only fetch page-specific data - stats come from UserContext
      const [upcomingEventsRes, todosRes] = await Promise.all([
        plannerService.getUpcomingEvents().catch(() => ({ success: false })),
        todoService.getAllTodos('active').catch(() => ({ success: false }))
      ]);

      // Update upcoming events
      if (upcomingEventsRes.success) {
        setUpcomingEvents(upcomingEventsRes.data.slice(0, 2));
      }

      // Update pending tasks
      if (todosRes.success) {
        setPendingTasks(todosRes.data.slice(0, 2));
      }

    } catch (err) {
      console.error('Error fetching dashboard data:', err);
      setError('Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  // Use stats from UserContext
  const stats = {
    totalHours: user.totalHours || '0h 0m',
    completedTasks: (user.todosTotal || 0) - (user.todosPending || 0),
    studyStreak: user.studyStreak || 0,
    attendancePercentage: user.attendanceRate || 0
  };

  const isLoading = userLoading || loading;

  if (isLoading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen">
        <SharedHeader title="Mentora - Dashboard" />
        <div className="flex items-center justify-center h-screen">
          <p style={{ color: M.muted }}>Loading dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen text-[#2C3E3F]">
      <SharedHeader title="Mentora - Dashboard" />
      <main className="container mx-auto px-4 mt-6">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Stats Grid - Using UserContext data */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-[#5A7A6B]">Study Hours</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{stats.totalHours}</p>
              </div>
              <Clock className="w-8 h-8" style={{ color: M.primary }} />
            </div>
          </div>
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-[#5A7A6B]">Completed</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{stats.completedTasks}</p>
              </div>
              <Award className="w-8 h-8" style={{ color: M.primary }} />
            </div>
          </div>
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-[#5A7A6B]">Streak</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{stats.studyStreak} days</p>
              </div>
              <Flame className="w-8 h-8 text-orange-500" />
            </div>
          </div>
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-[#5A7A6B]">Attendance</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{stats.attendancePercentage}%</p>
              </div>
              <TrendingUp className="w-8 h-8" style={{ color: M.primary }} />
            </div>
          </div>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
          <button
            onClick={() => navigate('/study-planner')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <Clock className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Study Planner</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Manage your schedule</p>
          </button>

          <button
            onClick={() => navigate('/career-builder')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <TrendingUp className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Career Builder</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Plan your future</p>
          </button>

          <button
            onClick={() => navigate('/skills')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <Award className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">My Skills</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Track your progress</p>
          </button>

          <button
            onClick={() => navigate('/profile')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <User className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Profile</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">View & edit profile</p>
          </button>
        </div>

        {/* Upcoming Events & Tasks */}
        <div className="bg-white rounded-2xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] flex items-center gap-2 mb-4">
            <BookOpenCheck className="w-5 h-5" style={{ color: M.primary }} />
            Upcoming Events & Tasks
          </h3>
          <div className="space-y-4">
            {/* Upcoming Events */}
            <div>
              <h4 className="text-sm font-semibold text-[#5A7A6B] mb-2 flex items-center gap-2">
                <Clock className="w-4 h-4" />
                Upcoming Events
              </h4>
              <div className="space-y-2">
                {upcomingEvents.length > 0 ? (
                  upcomingEvents.map(event => (
                    <div key={event.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-full flex items-center justify-center" style={{ background: M.bg3 }}>
                          <Clock className="w-4 h-4" style={{ color: M.primary }} />
                        </div>
                        <div>
                          <p className="font-medium text-[#2C3E3F] text-sm">{event.title}</p>
                          <p className="text-xs text-[#5A7A6B]">
                            {new Date(event.eventDateTime).toLocaleDateString()} at{' '}
                            {new Date(event.eventDateTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                          </p>
                        </div>
                      </div>
                      <ChevronRight className="w-4 h-4 text-[#5A7A6B]" />
                    </div>
                  ))
                ) : (
                  <p className="text-sm text-[#5A7A6B] text-center py-2">No upcoming events</p>
                )}
              </div>
            </div>

            {/* Pending Tasks */}
            <div>
              <h4 className="text-sm font-semibold text-[#5A7A6B] mb-2 flex items-center gap-2">
                <CheckSquare className="w-4 h-4" />
                Pending Tasks
              </h4>
              <div className="space-y-2">
                {pendingTasks.length > 0 ? (
                  pendingTasks.map(todo => (
                    <div key={todo.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-full flex items-center justify-center" style={{ background: M.bg3 }}>
                          <CheckSquare className="w-4 h-4" style={{ color: M.primary }} />
                        </div>
                        <div>
                          <p className="font-medium text-[#2C3E3F] text-sm">{todo.title}</p>
                          <p className="text-xs text-[#5A7A6B]">Pending</p>
                        </div>
                      </div>
                      <ChevronRight className="w-4 h-4 text-[#5A7A6B]" />
                    </div>
                  ))
                ) : (
                  <p className="text-sm text-[#5A7A6B] text-center py-2">No pending tasks</p>
                )}
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
