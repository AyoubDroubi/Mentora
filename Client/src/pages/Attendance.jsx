import React, { useState } from 'react';
import { Calendar, Clock, Trash2, Plus, CheckSquare } from 'lucide-react';

export default function Planner() {
  /* 🎨 Colors */
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  /* 📅 Events State */
  const [events, setEvents] = useState([
    { id: 1, title: 'Math Exam', date: '2024-12-20', time: '10:00', duration: '2h' },
    { id: 2, title: 'Physics Lab', date: '2024-12-21', time: '14:00', duration: '3h' },
  ]);

  const [title, setTitle] = useState('');
  const [date, setDate] = useState('');
  const [time, setTime] = useState('');

  /* ➕ Add Event */
  const addEvent = () => {
    if (!title || !date || !time) return;

    setEvents([
      ...events,
      {
        id: Date.now(),
        title,
        date,
        time,
        duration: '1h',
      },
    ]);

    setTitle('');
    setDate('');
    setTime('');
  };

  /* 🗑 Delete Event */
  const deleteEvent = (id) => {
    setEvents(events.filter(e => e.id !== id));
  };

  /* 🔝 Header */
  const Header = () => (
    <header
      className="px-6 py-4 flex items-center gap-3 shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md">
        <CheckSquare className="w-6 h-6" style={{ color: M.primary }} />
      </div>
      <span className="text-white text-xl font-bold">Mentora - Planner</span>
    </header>
  );

  return (
    <div
      className="min-h-screen pb-24"
      style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }}
    >
      <Header />

      <main className="container mx-auto px-4 mt-6 space-y-6">

        {/* ➕ Add Event */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold mb-4 flex items-center gap-2" style={{ color: M.text }}>
            <Calendar className="w-5 h-5" style={{ color: M.primary }} />
            Add New Event
          </h3>

          <div className="space-y-3">
            <input
              value={title}
              onChange={e => setTitle(e.target.value)}
              placeholder="Event title"
              className="w-full px-4 py-3 rounded-lg border focus:outline-none focus:ring-2"
              style={{ borderColor: M.bg3 }}
            />

            <div className="grid grid-cols-2 gap-3">
              <input
                type="date"
                value={date}
                onChange={e => setDate(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none"
                style={{ borderColor: M.bg3 }}
              />
              <input
                type="time"
                value={time}
                onChange={e => setTime(e.target.value)}
                className="px-4 py-3 rounded-lg border focus:outline-none"
                style={{ borderColor: M.bg3 }}
              />
            </div>

            <button
              onClick={addEvent}
              className="w-full py-3 rounded-lg text-white font-medium flex items-center justify-center gap-2 hover:shadow-lg"
              style={{ background: M.primary }}
            >
              <Plus className="w-5 h-5" />
              Add Event
            </button>
          </div>
        </div>

        {/* 📅 Events List */}
        <div className="bg-white rounded-2xl p-5 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold mb-4" style={{ color: M.text }}>
            Upcoming Events
          </h3>

          {events.length === 0 ? (
            <div className="text-center py-8">
              <Calendar className="w-12 h-12 mx-auto mb-3" style={{ color: M.muted }} />
              <p style={{ color: M.muted }}>No events scheduled</p>
            </div>
          ) : (
            <div className="space-y-3">
              {events.map(ev => (
                <div
                  key={ev.id}
                  className="flex items-center justify-between p-4 rounded-xl"
                  style={{ background: M.bg1 }}
                >
                  <div className="flex items-center gap-3">
                    <div className="w-12 h-12 rounded-lg flex items-center justify-center" style={{ background: M.primary }}>
                      <Calendar className="w-6 h-6 text-white" />
                    </div>
                    <div>
                      <p className="font-medium" style={{ color: M.text }}>{ev.title}</p>
                      <p className="text-sm flex items-center gap-2" style={{ color: M.muted }}>
                        <Clock className="w-3 h-3" />
                        {ev.date} · {ev.time} ({ev.duration})
                      </p>
                    </div>
                  </div>

                  <button
                    onClick={() => deleteEvent(ev.id)}
                    className="p-2 rounded-lg hover:bg-red-50 text-red-500"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>

      </main>
    </div>
  );
}
