// StudyPlanner.jsx
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import {
  CheckSquare,
  BookMarked,
  Clock,
  Calendar as CalendarIcon,
  HelpCircle,
  ClipboardCheck,
  Target,
  TrendingUp
} from 'lucide-react';
import {
  todoService,
  plannerService,
  notesService,
  studySessionsService,
  attendanceService
} from '../services';
import Calendar from '../components/Calendar';

export default function StudyPlanner() {
  const navigate = useNavigate();
  const { user } = useUser();

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // State
  const [stats, setStats] = useState({
    todosPending: 0,
    todosTotal: 0,
    notesCount: 0,
    eventsCount: 0,
    upcomingEvents: 0,
    totalStudyTime: '0h 0m',
    attendanceRate: 0,
    progressPercentage: 0
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [calendarData, setCalendarData] = useState({
    events: [],
    tasks: []
  });

  // Fetch all dashboard data on mount
  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    setLoading(true);
    try {
      // Fetch all data in parallel
      const [
        todoSummaryRes,
        allTodosRes,
        notesRes,
        eventsRes,
        upcomingRes,
        studyTimeRes,
        attendanceRes
      ] = await Promise.all([
        todoService.getSummary(),
        todoService.getAllTodos('all'), // Get all todos for calendar
        notesService.getAllNotes(),
        plannerService.getAllEvents(),
        plannerService.getUpcomingEvents(),
        studySessionsService.getSummary(),
        attendanceService.getSummary()
      ]);

      // Update stats
      setStats({
        todosPending: todoSummaryRes.success ? todoSummaryRes.data.pendingTasks : 0,
        todosTotal: todoSummaryRes.success ? todoSummaryRes.data.totalTasks : 0,
        notesCount: notesRes.success ? notesRes.data.length : 0,
        eventsCount: eventsRes.success ? eventsRes.data.length : 0,
        upcomingEvents: upcomingRes.success ? upcomingRes.data.length : 0,
        totalStudyTime: studyTimeRes.success ? studyTimeRes.data.formatted : '0h 0m',
        attendanceRate: attendanceRes.success ? attendanceRes.data.attendanceRate : 0,
        progressPercentage: attendanceRes.success ? attendanceRes.data.progressPercentage : 0
      });

      // Update calendar data
      setCalendarData({
        events: eventsRes.success ? eventsRes.data : [],
        tasks: allTodosRes.success ? allTodosRes.data : []
      });
    } catch (err) {
      console.error('Error fetching dashboard data:', err);
      setError('Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  const handleDayClick = (date, data) => {
    console.log('Selected date:', date);
    console.log('Day data:', data);
    // You can add modal or navigate to specific page
  };

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen">
        <SharedHeader title="Mentora - Study Planner" />
        <div className="flex items-center justify-center h-screen">
          <div className="text-center">
            <Clock className="w-12 h-12 mx-auto mb-4 animate-spin" style={{ color: M.primary }} />
            <p style={{ color: M.muted }}>Loading your study data...</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div
      style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }}
      className="min-h-screen pb-24 text-[#2C3E3F]"
    >
      <SharedHeader title="Mentora - Study Planner" />
      <main className="container mx-auto px-4 mt-6">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Welcome Section */}
        <section
          className="bg-white rounded-3xl p-6 shadow-lg border mb-6"
          style={{ borderColor: M.bg3, background: `linear-gradient(135deg, ${M.bg1}, white)` }}
        >
          <h1 className="text-3xl font-bold text-[#2C3E3F] mb-2">
            Welcome back, {user?.firstName || 'Student'}! 👋
          </h1>
          <p className="text-[#5A7A6B] mb-4">Your personal study companion for academic excellence</p>
          
          {/* Quick Stats */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
            <StatCard
              icon={<Clock className="w-5 h-5" style={{ color: M.primary }} />}
              label="Study Time"
              value={stats.totalStudyTime}
              color={M.primary}
            />
            <StatCard
              icon={<Target className="w-5 h-5 text-green-600" />}
              label="Progress"
              value={`${stats.progressPercentage}%`}
              color="#4CAF50"
            />
            <StatCard
              icon={<CheckSquare className="w-5 h-5 text-blue-600" />}
              label="Tasks"
              value={`${stats.todosTotal - stats.todosPending}/${stats.todosTotal}`}
              color="#2196F3"
            />
            <StatCard
              icon={<CalendarIcon className="w-5 h-5 text-orange-600" />}
              label="Events"
              value={stats.upcomingEvents}
              color="#FF9800"
            />
          </div>
        </section>

        {/* Progress Overview */}
        <section className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center gap-2 mb-4">
            <TrendingUp className="w-6 h-6" style={{ color: M.primary }} />
            <h2 className="text-xl font-bold text-[#2C3E3F]">Overall Progress</h2>
          </div>
          
          {/* Progress Bar */}
          <div className="mb-4">
            <div className="flex justify-between items-center mb-2">
              <span className="text-sm font-medium" style={{ color: M.muted }}>
                {stats.progressPercentage}% Complete
              </span>
              <span className="text-sm" style={{ color: M.muted }}>
                50% Tasks + 50% Events
              </span>
            </div>
            <div className="w-full h-4 bg-gray-200 rounded-full overflow-hidden">
              <div
                className="h-full transition-all duration-500 rounded-full"
                style={{
                  width: `${stats.progressPercentage}%`,
                  background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})`
                }}
              />
            </div>
          </div>

          {/* Progress Details */}
          <div className="grid grid-cols-2 gap-4">
            <div className="text-center p-3 rounded-lg" style={{ background: M.bg1 }}>
              <p className="text-sm" style={{ color: M.muted }}>Tasks Completed</p>
              <p className="text-2xl font-bold" style={{ color: M.primary }}>
                {stats.todosTotal - stats.todosPending}/{stats.todosTotal}
              </p>
            </div>
            <div className="text-center p-3 rounded-lg" style={{ background: M.bg1 }}>
              <p className="text-sm" style={{ color: M.muted }}>Attendance Rate</p>
              <p className="text-2xl font-bold" style={{ color: M.primary }}>
                {stats.attendanceRate}%
              </p>
            </div>
          </div>
        </section>

        {/* Calendar Section */}
        <section className="mb-6">
          <Calendar
            events={calendarData.events}
            tasks={calendarData.tasks}
            onDayClick={handleDayClick}
          />
        </section>

        {/* Study Tools */}
        <section className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h2 className="text-xl font-bold text-[#2C3E3F] mb-4">Study Tools</h2>
          <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
            <ToolButton
              icon={<CheckSquare className="w-8 h-8" style={{ color: M.primary }} />}
              title="To-Do"
              subtitle={`${stats.todosPending} pending`}
              onClick={() => navigate('/todo')}
              borderColor={M.bg3}
            />
            <ToolButton
              icon={<BookMarked className="w-8 h-8" style={{ color: M.primary }} />}
              title="Notes"
              subtitle={`${stats.notesCount} saved`}
              onClick={() => navigate('/notes')}
              borderColor={M.bg3}
            />
            <ToolButton
              icon={<Clock className="w-8 h-8" style={{ color: M.primary }} />}
              title="Timer"
              subtitle="Pomodoro"
              onClick={() => navigate('/timer')}
              borderColor={M.bg3}
            />
            <ToolButton
              icon={<CalendarIcon className="w-8 h-8" style={{ color: M.primary }} />}
              title="Planner"
              subtitle={`${stats.upcomingEvents} upcoming`}
              onClick={() => navigate('/planner')}
              borderColor={M.bg3}
            />
            <ToolButton
              icon={<ClipboardCheck className="w-8 h-8" style={{ color: M.primary }} />}
              title="Attendance"
              subtitle={`${stats.attendanceRate}% rate`}
              onClick={() => navigate('/attendance')}
              borderColor={M.bg3}
            />
            <ToolButton
              icon={<HelpCircle className="w-8 h-8" style={{ color: M.primary }} />}
              title="Quiz *TODO* "
              subtitle="AI Assessment"
              onClick={() => navigate('/quiz')}
              borderColor={M.bg3}
            />
          </div>
        </section>
      </main>
    </div>
  );
}

function StatCard({ icon, label, value, color }) {
  return (
    <div className="flex items-center gap-3 p-3 bg-white rounded-lg shadow-sm">
      <div className="w-10 h-10 flex items-center justify-center rounded-lg" style={{ background: `${color}20` }}>
        {icon}
      </div>
      <div>
        <p className="text-xs text-gray-500">{label}</p>
        <p className="font-bold" style={{ color }}>{value}</p>
      </div>
    </div>
  );
}

function ToolButton({ icon, title, subtitle, onClick, borderColor }) {
  return (
    <button
      onClick={onClick}
      className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all"
      style={{ borderColor }}
    >
      {icon}
      <span className="text-sm font-medium">{title}</span>
      <span className="text-xs text-[#5A7A6B]">{subtitle}</span>
    </button>
  );
}
