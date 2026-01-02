import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { Calendar, Clock, Trash2, Plus, CheckSquare, CheckCircle } from 'lucide-react';
import { plannerService } from '../services';

export default function Planner() {
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
  const [events, setEvents] = useState([]);
  const [upcomingEvents, setUpcomingEvents] = useState([]);
  const [evTitle, setEvTitle] = useState('');
  const [evDate, setEvDate] = useState('');
  const [evTime, setEvTime] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Fetch events on mount
  useEffect(() => {
    fetchEvents();
    fetchUpcomingEvents();
  }, []);

  // Fetch all events
  const fetchEvents = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await plannerService.getAllEvents();
      
      if (response.success) {
        setEvents(response.data || []);
      }
    } catch (err) {
      console.error('Error fetching events:', err);
      setError(err.response?.data?.message || 'Failed to load events');
    } finally {
      setLoading(false);
    }
  };

  // Fetch upcoming events
  const fetchUpcomingEvents = async () => {
    try {
      const response = await plannerService.getUpcomingEvents();
      if (response.success) {
        setUpcomingEvents(response.data || []);
      }
    } catch (err) {
      console.error('Error fetching upcoming events:', err);
    }
  };

  // Add event
  const addEvent = async () => {
    if (!evTitle.trim() || !evDate || !evTime) {
      setError('Please fill in all fields');
      return;
    }

    try {
      setLoading(true);
      setError(null);

      // Combine date and time to create ISO string
      const eventDateTime = new Date(`${evDate}T${evTime}`).toISOString();

      const response = await plannerService.createEvent({
        title: evTitle.trim(),
        eventDateTime
      });

      if (response.success) {
        setEvents([response.data, ...events]);
        setEvTitle('');
        setEvDate('');
        setEvTime('');
        fetchUpcomingEvents(); // Refresh upcoming
      }
    } catch (err) {
      console.error('Error creating event:', err);
      setError(err.response?.data?.message || 'Failed to create event');
    } finally {
      setLoading(false);
    }
  };

  // Mark event as attended
  const markAttended = async (id) => {
    try {
      const response = await plannerService.markAttended(id);

      if (response.success) {
        setEvents(events.map(e => e.id === id ? response.data : e));
        setUpcomingEvents(upcomingEvents.map(e => e.id === id ? response.data : e));
      }
    } catch (err) {
      console.error('Error marking attended:', err);
      setError(err.response?.data?.message || 'Failed to mark attended');
    }
  };

  // Delete event
  const deleteEvent = async (id) => {
    if (!window.confirm('Are you sure you want to delete this event?')) return;

    try {
      const response = await plannerService.deleteEvent(id);

      if (response.success) {
        setEvents(events.filter(e => e.id !== id));
        setUpcomingEvents(upcomingEvents.filter(e => e.id !== id));
      }
    } catch (err) {
      console.error('Error deleting event:', err);
      setError(err.response?.data?.message || 'Failed to delete event');
    }
  };

  // Format date/time for display
  const formatDateTime = (isoString) => {
    const date = new Date(isoString);
    const dateStr = date.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric' 
    });
    const timeStr = date.toLocaleTimeString('en-US', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
    return { dateStr, timeStr };
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <Calendar className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Planner</span>
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
          <img src={user?.avatar || 'https://api.dicebear.com/7.x/avataaars/svg?seed=default'} alt="avatar" className="w-full h-full object-cover" />
        </button>
      </nav>
    </header>
  );

  return (
    <div
      style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }}
      className="min-h-screen pb-24"
    >
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Add Event Card */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-3 flex items-center gap-2">
            <Plus className="w-5 h-5" style={{ color: M.primary }} />
            Add New Event
          </h3>
          <div className="space-y-3">
            <input
              value={evTitle}
              onChange={(e) => setEvTitle(e.target.value)}
              placeholder="Event title (e.g., Midterm Exam)"
              className="w-full px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
              style={{ borderColor: M.bg3 }}
              disabled={loading}
            />
            <div className="grid grid-cols-2 gap-3">
              <input
                type="date"
                value={evDate}
                onChange={(e) => setEvDate(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
                disabled={loading}
              />
              <input
                type="time"
                value={evTime}
                onChange={(e) => setEvTime(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
                disabled={loading}
              />
            </div>
            <button
              onClick={addEvent}
              disabled={loading}
              className="w-full px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center justify-center gap-2 disabled:opacity-50"
              style={{ background: M.primary }}
            >
              <Plus className="w-5 h-5" />
              {loading ? 'Adding...' : 'Add Event'}
            </button>
          </div>
        </div>

        {/* Upcoming Events */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
            <Calendar className="w-5 h-5" style={{ color: M.primary }} />
            Upcoming Events
          </h3>
          {loading && <p className="text-center py-8" style={{ color: M.muted }}>Loading...</p>}
          
          {!loading && upcomingEvents.length === 0 && (
            <div className="text-center py-8">
              <Calendar className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
              <p className="text-[#5A7A6B]">No upcoming events</p>
              <p className="text-sm text-gray-400 mt-2">Create your first event above!</p>
            </div>
          )}

          <div className="space-y-3">
            {!loading && upcomingEvents.map((ev) => {
              const { dateStr, timeStr } = formatDateTime(ev.eventDateTime);
              return (
                <div
                  key={ev.id}
                  className="flex items-center justify-between p-4 rounded-xl hover:shadow-md transition-all"
                  style={{ background: ev.isAttended ? '#E8F5E9' : M.bg1 }}
                >
                  <div className="flex items-center gap-3 flex-1">
                    <div 
                      className="w-12 h-12 rounded-lg flex items-center justify-center" 
                      style={{ background: ev.isAttended ? '#4CAF50' : M.primary }}
                    >
                      {ev.isAttended ? (
                        <CheckCircle className="w-6 h-6 text-white" />
                      ) : (
                        <Calendar className="w-6 h-6 text-white" />
                      )}
                    </div>
                    <div className="flex-1">
                      <p className="font-medium text-[#2C3E3F]">{ev.title}</p>
                      <p className="text-sm text-[#5A7A6B] flex items-center gap-2 mt-1">
                        <Clock className="w-3 h-3" />
                        {dateStr} at {timeStr}
                      </p>
                    </div>
                  </div>
                  <div className="flex gap-2">
                    {!ev.isAttended && (
                      <button
                        onClick={() => markAttended(ev.id)}
                        className="text-green-600 hover:bg-green-50 p-2 rounded-lg transition-colors"
                        title="Mark as attended"
                      >
                        <CheckCircle className="w-5 h-5" />
                      </button>
                    )}
                    <button
                      onClick={() => deleteEvent(ev.id)}
                      className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                    >
                      <Trash2 className="w-4 h-4" />
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        </div>

        {/* All Events */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-4">All Events</h3>
          
          {!loading && events.length === 0 && (
            <p className="text-center py-4 text-[#5A7A6B]">No events yet</p>
          )}
          
          <div className="space-y-2">
            {!loading && events.map((ev) => {
              const { dateStr, timeStr } = formatDateTime(ev.eventDateTime);
              const isPast = new Date(ev.eventDateTime) < new Date();
              
              return (
                <div
                  key={ev.id}
                  className="flex items-center justify-between p-3 rounded-lg hover:bg-gray-50 transition-all border"
                  style={{ 
                    borderColor: M.bg3,
                    background: ev.isAttended ? '#E8F5E9' : (isPast && !ev.isAttended ? '#FFEBEE' : 'white')
                  }}
                >
                  <div className="flex items-center gap-3 flex-1">
                    <div 
                      className="w-10 h-10 rounded-lg flex items-center justify-center" 
                      style={{ 
                        background: ev.isAttended ? '#4CAF50' : (isPast ? '#EF5350' : M.primary)
                      }}
                    >
                      {ev.isAttended ? (
                        <CheckCircle className="w-5 h-5 text-white" />
                      ) : isPast ? (
                        <Calendar className="w-5 h-5 text-white" />
                      ) : (
                        <Calendar className="w-5 h-5 text-white" />
                      )}
                    </div>
                    <div className="flex-1">
                      <p className={`font-medium ${ev.isAttended ? 'text-green-600' : 'text-[#2C3E3F]'}`}>
                        {ev.title}
                        {ev.isAttended && ' ?'}
                        {isPast && !ev.isAttended && ' (Missed)'}
                      </p>
                      <p className="text-xs text-[#5A7A6B]">{dateStr} at {timeStr}</p>
                    </div>
                  </div>
                  <div className="flex gap-2">
                    {!ev.isAttended && !isPast && (
                      <button
                        onClick={() => markAttended(ev.id)}
                        className="text-green-600 hover:bg-green-50 p-2 rounded-lg transition-colors"
                        title="Mark as attended"
                      >
                        <CheckCircle className="w-4 h-4" />
                      </button>
                    )}
                    <button
                      onClick={() => deleteEvent(ev.id)}
                      className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                    >
                      <Trash2 className="w-3 h-3" />
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </main>
    </div>
  );
}
