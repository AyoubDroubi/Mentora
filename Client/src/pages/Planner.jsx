import React, { useState } from 'react';
import { Calendar, Clock, Trash2, Plus, CheckSquare } from 'lucide-react';

export default function Planner() {
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

  // Events state
  const [events, setEvents] = useState([
    { id: 1, title: 'Web Exam', date: '2024-12-20', time: '10:00', duration: '2h' },
    { id: 2, title: 'Java Lab', date: '2024-12-21', time: '14:00', duration: '3h' },
  ]);
  const [evTitle, setEvTitle] = useState('');
  const [evDate, setEvDate] = useState('');
  const [evTime, setEvTime] = useState('');

  const addEvent = (title, date, time) => {
    if (!title || !date || !time) return;
    setEvents(e => [...e, { id: Date.now(), title, date, time, duration: '1h' }]);
  };

  const deleteEvent = (id) => setEvents(e => e.filter(x => x.id !== id));

  // Header component
  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <CheckSquare className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Planner</span>
      </div>
    </header>
  );

  return (
    <div
      style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }}
      className="min-h-screen pb-24"
    >
      <Header />
      <main className="container mx-auto px-4 mt-6">

        {/* Add Event Card */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-3 flex items-center gap-2">
            <Calendar className="w-5 h-5" style={{ color: M.primary }} />
            Add New Event
          </h3>
          <div className="space-y-3">
            <input
              value={evTitle}
              onChange={e => setEvTitle(e.target.value)}
              placeholder="Event title"
              className="w-full px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
              style={{ borderColor: M.bg3 }}
            />
            <div className="grid grid-cols-2 gap-3">
              <input
                type="date"
                value={evDate}
                onChange={e => setEvDate(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
              />
              <input
                type="time"
                value={evTime}
                onChange={e => setEvTime(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
              />
            </div>
            <button
              onClick={() => {
                addEvent(evTitle, evDate, evTime);
                setEvTitle('');
                setEvDate('');
                setEvTime('');
              }}
              className="w-full px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center justify-center gap-2"
              style={{ background: M.primary }}
            >
              <Plus className="w-5 h-5" />
              Add Event
            </button>
          </div>
        </div>

        {/* Upcoming Events */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-4">Upcoming Events</h3>
          <div className="space-y-3">
            {events.length === 0 ? (
              <div className="text-center py-8">
                <Calendar className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
                <p className="text-[#5A7A6B]">No events scheduled</p>
              </div>
            ) : (
              events.map(ev => (
                <div
                  key={ev.id}
                  className="flex items-center justify-between p-4 rounded-xl hover:shadow-md transition-all"
                  style={{ background: M.bg1 }}
                >
                  <div className="flex items-center gap-3">
                    <div className="w-12 h-12 rounded-lg flex items-center justify-center" style={{ background: M.primary }}>
                      <Calendar className="w-6 h-6 text-white" />
                    </div>
                    <div>
                      <p className="font-medium text-[#2C3E3F]">{ev.title}</p>
                      <p className="text-sm text-[#5A7A6B] flex items-center gap-2 mt-1">
                        <Clock className="w-3 h-3" />
                        {ev.date} at {ev.time} ({ev.duration})
                      </p>
                    </div>
                  </div>
                  <button
                    onClick={() => deleteEvent(ev.id)}
                    className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              ))
            )}
          </div>
        </div>

      </main>
    </div>
  );
}
