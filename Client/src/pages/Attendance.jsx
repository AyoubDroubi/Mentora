import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { ClipboardCheck, TrendingUp, Calendar, CheckSquare, Target } from 'lucide-react';
import { attendanceService } from '../services';

export default function Attendance() {
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
  const [summary, setSummary] = useState(null);
  const [history, setHistory] = useState(null);
  const [weekly, setWeekly] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [historyDays, setHistoryDays] = useState(30);

  // Fetch data on mount
  useEffect(() => {
    fetchAllData();
  }, [historyDays]);

  const fetchAllData = async () => {
    setLoading(true);
    try {
      const [summaryRes, historyRes, weeklyRes] = await Promise.all([
        attendanceService.getSummary(),
        attendanceService.getHistory(historyDays),
        attendanceService.getWeeklyProgress()
      ]);

      if (summaryRes.success) setSummary(summaryRes.data);
      if (historyRes.success) setHistory(historyRes.data);
      if (weeklyRes.success) setWeekly(weeklyRes.data);
    } catch (err) {
      console.error('Error fetching attendance data:', err);
      setError(err.response?.data?.message || 'Failed to load attendance data');
    } finally {
      setLoading(false);
    }
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <ClipboardCheck className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Attendance & Progress</span>
      </div>

      <nav className="flex items-center gap-4">
        <button
          onClick={() => navigate('/study-planner')}
          className="text-white hover:bg-white/20 px-3 py-2 rounded-lg transition-colors"
        >
          Dashboard
        </button>
        <button
          onClick={() => navigate('/profile')}
          className="w-10 h-10 rounded-full border-2 border-white overflow-hidden hover:scale-110 transition-transform"
          title="Open profile"
        >
          <img
            src={user?.avatar || 'https://api.dicebear.com/7.x/avataaars/svg?seed=default'}
            alt="avatar"
            className="w-full h-full object-cover"
          />
        </button>
      </nav>
    </header>
  );

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen">
        <Header />
        <div className="flex items-center justify-center h-screen">
          <p style={{ color: M.muted }}>Loading attendance data...</p>
        </div>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />

      <main className="container mx-auto px-4 mt-6">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Overall Progress */}
        {summary && (
          <div className="bg-white rounded-2xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-2 mb-4">
              <Target className="w-6 h-6" style={{ color: M.primary }} />
              <h2 className="text-xl font-bold" style={{ color: M.text }}>Overall Progress</h2>
            </div>

            {/* Main Progress Circle */}
            <div className="flex flex-col md:flex-row items-center gap-6 mb-6">
              {/* Progress Circle */}
              <div className="relative">
                <svg width="200" height="200" className="transform -rotate-90">
                  <circle
                    cx="100"
                    cy="100"
                    r="90"
                    stroke={M.bg3}
                    strokeWidth="20"
                    fill="none"
                  />
                  <circle
                    cx="100"
                    cy="100"
                    r="90"
                    stroke={M.primary}
                    strokeWidth="20"
                    fill="none"
                    strokeDasharray={`${2 * Math.PI * 90}`}
                    strokeDashoffset={`${2 * Math.PI * 90 * (1 - summary.progressPercentage / 100)}`}
                    strokeLinecap="round"
                    className="transition-all duration-1000"
                  />
                </svg>
                <div className="absolute inset-0 flex items-center justify-center">
                  <div className="text-center">
                    <div className="text-4xl font-bold" style={{ color: M.primary }}>
                      {summary.progressPercentage}%
                    </div>
                    <div className="text-sm" style={{ color: M.muted }}>Progress</div>
                  </div>
                </div>
              </div>

              {/* Progress Breakdown */}
              <div className="flex-1 space-y-4 w-full">
                <div>
                  <div className="flex justify-between mb-1">
                    <span className="text-sm font-medium">Tasks Progress (50%)</span>
                    <span className="text-sm font-bold" style={{ color: M.primary }}>
                      {summary.taskCompletionRate}%
                    </span>
                  </div>
                  <div className="w-full h-3 bg-gray-200 rounded-full overflow-hidden">
                    <div
                      className="h-full bg-blue-500 transition-all"
                      style={{ width: `${summary.taskCompletionRate}%` }}
                    />
                  </div>
                  <p className="text-xs mt-1" style={{ color: M.muted }}>
                    {summary.completedTasks} of {summary.totalTasks} tasks completed
                  </p>
                </div>

                <div>
                  <div className="flex justify-between mb-1">
                    <span className="text-sm font-medium">Events Attendance (50%)</span>
                    <span className="text-sm font-bold" style={{ color: M.primary }}>
                      {summary.attendanceRate}%
                    </span>
                  </div>
                  <div className="w-full h-3 bg-gray-200 rounded-full overflow-hidden">
                    <div
                      className="h-full bg-green-500 transition-all"
                      style={{ width: `${summary.attendanceRate}%` }}
                    />
                  </div>
                  <p className="text-xs mt-1" style={{ color: M.muted }}>
                    {summary.attendedEvents} of {summary.totalPastEvents} events attended
                  </p>
                </div>
              </div>
            </div>

            {/* Stats Grid */}
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <StatBox
                label="Total Tasks"
                value={summary.totalTasks}
                icon={<CheckSquare className="w-5 h-5 text-blue-500" />}
              />
              <StatBox
                label="Completed"
                value={summary.completedTasks}
                icon={<CheckSquare className="w-5 h-5 text-green-500" />}
              />
              <StatBox
                label="Pending"
                value={summary.pendingTasks}
                icon={<CheckSquare className="w-5 h-5 text-orange-500" />}
              />
              <StatBox
                label="Upcoming Events"
                value={summary.upcomingEvents}
                icon={<Calendar className="w-5 h-5 text-purple-500" />}
              />
            </div>
          </div>
        )}

        {/* Weekly Progress */}
        {weekly && (
          <div className="bg-white rounded-2xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-2 mb-4">
              <Calendar className="w-6 h-6" style={{ color: M.primary }} />
              <h2 className="text-xl font-bold" style={{ color: M.text }}>This Week's Progress</h2>
            </div>
            <p className="text-sm mb-4" style={{ color: M.muted }}>
              {new Date(weekly.weekStart).toLocaleDateString()} - {new Date(weekly.weekEnd).toLocaleDateString()}
            </p>

            <div className="grid grid-cols-7 gap-2">
              {weekly.days.map((day, index) => (
                <div
                  key={index}
                  className="text-center p-3 rounded-lg border"
                  style={{
                    borderColor: M.bg3,
                    background: day.tasksCompleted > 0 || day.eventsAttended > 0 ? M.bg1 : 'white'
                  }}
                >
                  <div className="text-xs font-medium mb-1" style={{ color: M.muted }}>
                    {new Date(day.date).toLocaleDateString('en-US', { weekday: 'short' })}
                  </div>
                  <div className="text-lg font-bold" style={{ color: M.primary }}>
                    {day.tasksCompleted + day.eventsAttended}
                  </div>
                  <div className="text-xs" style={{ color: M.muted }}>
                    {day.tasksCompleted}T / {day.eventsAttended}E
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* History */}
        {history && (
          <div className="bg-white rounded-2xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between mb-4">
              <div className="flex items-center gap-2">
                <TrendingUp className="w-6 h-6" style={{ color: M.primary }} />
                <h2 className="text-xl font-bold" style={{ color: M.text }}>Attendance History</h2>
              </div>
              
              <select
                value={historyDays}
                onChange={(e) => setHistoryDays(Number(e.target.value))}
                className="px-3 py-2 border rounded-lg"
                style={{ borderColor: M.bg3 }}
              >
                <option value={7}>Last 7 days</option>
                <option value={30}>Last 30 days</option>
                <option value={90}>Last 90 days</option>
              </select>
            </div>

            <p className="text-sm mb-4" style={{ color: M.muted }}>
              Period: {history.period.from} to {history.period.to} ({history.period.days} days)
            </p>

            <div className="grid md:grid-cols-2 gap-4 mb-4">
              <div className="p-4 rounded-lg" style={{ background: M.bg1 }}>
                <h3 className="font-semibold mb-2">Tasks Summary</h3>
                <div className="space-y-1 text-sm">
                  <div className="flex justify-between">
                    <span>Total:</span>
                    <span className="font-bold">{history.summary.tasksTotal}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Completed:</span>
                    <span className="font-bold text-green-600">{history.summary.tasksCompleted}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Pending:</span>
                    <span className="font-bold text-orange-600">{history.summary.tasksPending}</span>
                  </div>
                </div>
              </div>

              <div className="p-4 rounded-lg" style={{ background: M.bg1 }}>
                <h3 className="font-semibold mb-2">Events Summary</h3>
                <div className="space-y-1 text-sm">
                  <div className="flex justify-between">
                    <span>Total:</span>
                    <span className="font-bold">{history.summary.eventsTotal}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Attended:</span>
                    <span className="font-bold text-green-600">{history.summary.eventsAttended}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Missed:</span>
                    <span className="font-bold text-red-600">{history.summary.eventsMissed}</span>
                  </div>
                </div>
              </div>
            </div>

            {/* Recent Activities */}
            <div className="mt-6">
              <h3 className="font-semibold mb-3">Recent Activities</h3>
              
              {/* Completed Tasks */}
              {history.tasks.length > 0 && (
                <div className="mb-4">
                  <h4 className="text-sm font-medium mb-2" style={{ color: M.muted }}>Completed Tasks</h4>
                  <div className="space-y-2">
                    {history.tasks.slice(0, 5).map((task) => (
                      <div key={task.id} className="p-2 rounded border" style={{ borderColor: M.bg3 }}>
                        <div className="flex items-center justify-between">
                          <span className="text-sm">{task.title}</span>
                          <span className="text-xs" style={{ color: M.muted }}>
                            {new Date(task.completedAt).toLocaleDateString()}
                          </span>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              )}

              {/* Events */}
              {history.events.length > 0 && (
                <div>
                  <h4 className="text-sm font-medium mb-2" style={{ color: M.muted }}>Events</h4>
                  <div className="space-y-2">
                    {history.events.slice(0, 5).map((event) => (
                      <div 
                        key={event.id} 
                        className="p-2 rounded border" 
                        style={{ 
                          borderColor: M.bg3,
                          background: event.isAttended ? '#E8F5E9' : 'white'
                        }}
                      >
                        <div className="flex items-center justify-between">
                          <div className="flex items-center gap-2">
                            <span className="text-sm">{event.title}</span>
                            {event.isAttended && <span className="text-xs text-green-600">✓ Attended</span>}
                          </div>
                          <span className="text-xs" style={{ color: M.muted }}>
                            {new Date(event.eventDateTime).toLocaleDateString()}
                          </span>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>
          </div>
        )}
      </main>
    </div>
  );
}

function StatBox({ label, value, icon }) {
  return (
    <div className="text-center p-4 rounded-lg border" style={{ borderColor: '#E8F3E8' }}>
      <div className="flex justify-center mb-2">{icon}</div>
      <div className="text-2xl font-bold" style={{ color: '#6B9080' }}>{value}</div>
      <div className="text-xs" style={{ color: '#5A7A6B' }}>{label}</div>
    </div>
  );
}
