import React, { useState, useEffect } from 'react';
import { ChevronLeft, ChevronRight, Calendar as CalendarIcon, CheckSquare, Circle } from 'lucide-react';

/**
 * Full Functional Calendar Component
 * Displays events and tasks on a monthly calendar view
 * Features:
 * - Month navigation
 * - Event indicators
 * - Task indicators
 * - Click to view day details
 * - Today highlighting
 */
const Calendar = ({ events = [], tasks = [], onDayClick }) => {
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [currentDate, setCurrentDate] = useState(new Date());
  const [selectedDay, setSelectedDay] = useState(null);

  // Get calendar data
  const year = currentDate.getFullYear();
  const month = currentDate.getMonth();
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  // Get first day of month and total days
  const firstDayOfMonth = new Date(year, month, 1);
  const lastDayOfMonth = new Date(year, month + 1, 0);
  const daysInMonth = lastDayOfMonth.getDate();
  const startDayOfWeek = firstDayOfMonth.getDay(); // 0 = Sunday

  // Month names
  const monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];

  // Day names
  const dayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  // Generate calendar days
  const calendarDays = [];
  
  // Previous month days (to fill the first week)
  const prevMonthDays = new Date(year, month, 0).getDate();
  for (let i = startDayOfWeek - 1; i >= 0; i--) {
    calendarDays.push({
      day: prevMonthDays - i,
      isCurrentMonth: false,
      date: new Date(year, month - 1, prevMonthDays - i)
    });
  }

  // Current month days
  for (let i = 1; i <= daysInMonth; i++) {
    calendarDays.push({
      day: i,
      isCurrentMonth: true,
      date: new Date(year, month, i)
    });
  }

  // Next month days (to fill the last week)
  const remainingDays = 42 - calendarDays.length; // 6 weeks * 7 days
  for (let i = 1; i <= remainingDays; i++) {
    calendarDays.push({
      day: i,
      isCurrentMonth: false,
      date: new Date(year, month + 1, i)
    });
  }

  // Get events and tasks for a specific day
  const getDayData = (date) => {
    const dateStr = date.toISOString().split('T')[0];
    
    const dayEvents = events.filter(e => {
      const eventDate = new Date(e.eventDateTime).toISOString().split('T')[0];
      return eventDate === dateStr;
    });

    const dayTasks = tasks.filter(t => {
      const taskDate = new Date(t.createdAt).toISOString().split('T')[0];
      return taskDate === dateStr;
    });

    const completedTasks = dayTasks.filter(t => t.isCompleted).length;
    const attendedEvents = dayEvents.filter(e => e.isAttended).length;

    return {
      events: dayEvents,
      tasks: dayTasks,
      completedTasks,
      attendedEvents,
      hasActivity: dayEvents.length > 0 || dayTasks.length > 0
    };
  };

  // Check if date is today
  const isToday = (date) => {
    return date.toDateString() === today.toDateString();
  };

  // Navigate months
  const previousMonth = () => {
    setCurrentDate(new Date(year, month - 1, 1));
    setSelectedDay(null);
  };

  const nextMonth = () => {
    setCurrentDate(new Date(year, month + 1, 1));
    setSelectedDay(null);
  };

  const goToToday = () => {
    setCurrentDate(new Date());
    setSelectedDay(null);
  };

  // Handle day click
  const handleDayClick = (dayData) => {
    if (!dayData.isCurrentMonth) return;
    
    setSelectedDay(dayData.date);
    
    if (onDayClick) {
      const data = getDayData(dayData.date);
      onDayClick(dayData.date, data);
    }
  };

  return (
    <div className="bg-white rounded-2xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
      {/* Header */}
      <div className="flex items-center justify-between mb-6">
        <div className="flex items-center gap-3">
          <CalendarIcon className="w-6 h-6" style={{ color: M.primary }} />
          <h2 className="text-xl font-bold" style={{ color: M.text }}>
            {monthNames[month]} {year}
          </h2>
        </div>
        <div className="flex items-center gap-2">
          <button
            onClick={previousMonth}
            className="p-2 rounded-lg hover:bg-gray-100 transition-colors"
            title="Previous month"
          >
            <ChevronLeft className="w-5 h-5" style={{ color: M.primary }} />
          </button>
          <button
            onClick={goToToday}
            className="px-4 py-2 rounded-lg font-medium hover:shadow-md transition-all text-sm"
            style={{ background: M.bg1, color: M.primary }}
          >
            Today
          </button>
          <button
            onClick={nextMonth}
            className="p-2 rounded-lg hover:bg-gray-100 transition-colors"
            title="Next month"
          >
            <ChevronRight className="w-5 h-5" style={{ color: M.primary }} />
          </button>
        </div>
      </div>

      {/* Day names */}
      <div className="grid grid-cols-7 gap-2 mb-2">
        {dayNames.map(name => (
          <div
            key={name}
            className="text-center font-semibold text-sm py-2"
            style={{ color: M.muted }}
          >
            {name}
          </div>
        ))}
      </div>

      {/* Calendar grid */}
      <div className="grid grid-cols-7 gap-2">
        {calendarDays.map((dayData, index) => {
          const data = getDayData(dayData.date);
          const isTodayDay = isToday(dayData.date);
          const isSelected = selectedDay && dayData.date.toDateString() === selectedDay.toDateString();

          return (
            <button
              key={index}
              onClick={() => handleDayClick(dayData)}
              disabled={!dayData.isCurrentMonth}
              className={`
                relative min-h-[80px] p-2 rounded-lg border transition-all
                ${dayData.isCurrentMonth ? 'hover:shadow-md hover:scale-105' : 'opacity-40 cursor-not-allowed'}
                ${isTodayDay ? 'ring-2 ring-blue-500' : ''}
                ${isSelected ? 'ring-2 ring-green-500' : ''}
              `}
              style={{
                borderColor: isTodayDay ? '#3B82F6' : M.bg3,
                background: data.hasActivity ? M.bg1 : 'white'
              }}
            >
              {/* Day number */}
              <div
                className={`text-sm font-semibold mb-1 ${
                  isTodayDay ? 'text-blue-600' : dayData.isCurrentMonth ? '' : 'text-gray-400'
                }`}
                style={{ color: dayData.isCurrentMonth && !isTodayDay ? M.text : undefined }}
              >
                {dayData.day}
              </div>

              {/* Indicators */}
              {dayData.isCurrentMonth && data.hasActivity && (
                <div className="space-y-1">
                  {/* Events indicator */}
                  {data.events.length > 0 && (
                    <div className="flex items-center gap-1">
                      <Circle
                        className="w-2 h-2"
                        style={{
                          fill: data.attendedEvents > 0 ? '#4CAF50' : M.primary,
                          color: data.attendedEvents > 0 ? '#4CAF50' : M.primary
                        }}
                      />
                      <span className="text-xs" style={{ color: M.muted }}>
                        {data.events.length}E
                      </span>
                    </div>
                  )}

                  {/* Tasks indicator */}
                  {data.tasks.length > 0 && (
                    <div className="flex items-center gap-1">
                      <CheckSquare
                        className="w-2 h-2"
                        style={{
                          color: data.completedTasks > 0 ? '#4CAF50' : M.muted
                        }}
                      />
                      <span className="text-xs" style={{ color: M.muted }}>
                        {data.completedTasks}/{data.tasks.length}T
                      </span>
                    </div>
                  )}
                </div>
              )}

              {/* Today badge */}
              {isTodayDay && (
                <div
                  className="absolute top-1 right-1 w-2 h-2 rounded-full"
                  style={{ background: '#3B82F6' }}
                />
              )}
            </button>
          );
        })}
      </div>

      {/* Legend */}
      <div className="mt-4 pt-4 border-t flex items-center justify-center gap-6 text-sm" style={{ borderColor: M.bg3 }}>
        <div className="flex items-center gap-2">
          <Circle className="w-3 h-3" style={{ fill: M.primary, color: M.primary }} />
          <span style={{ color: M.muted }}>Events</span>
        </div>
        <div className="flex items-center gap-2">
          <CheckSquare className="w-3 h-3" style={{ color: M.muted }} />
          <span style={{ color: M.muted }}>Tasks</span>
        </div>
        <div className="flex items-center gap-2">
          <Circle className="w-3 h-3" style={{ fill: '#4CAF50', color: '#4CAF50' }} />
          <span style={{ color: M.muted }}>Completed</span>
        </div>
        <div className="flex items-center gap-2">
          <div className="w-3 h-3 rounded-full" style={{ background: '#3B82F6' }} />
          <span style={{ color: M.muted }}>Today</span>
        </div>
      </div>

      {/* Selected day details */}
      {selectedDay && (
        <div className="mt-4 pt-4 border-t" style={{ borderColor: M.bg3 }}>
          <h3 className="font-semibold mb-2" style={{ color: M.text }}>
            {selectedDay.toLocaleDateString('en-US', { 
              weekday: 'long', 
              year: 'numeric', 
              month: 'long', 
              day: 'numeric' 
            })}
          </h3>
          {(() => {
            const data = getDayData(selectedDay);
            return (
              <div className="space-y-2">
                {data.events.length > 0 && (
                  <div>
                    <p className="text-sm font-medium mb-1" style={{ color: M.muted }}>
                      Events ({data.events.length})
                    </p>
                    <ul className="space-y-1">
                      {data.events.map((event, i) => (
                        <li
                          key={i}
                          className="text-sm pl-4 border-l-2"
                          style={{
                            borderColor: event.isAttended ? '#4CAF50' : M.primary,
                            color: M.text
                          }}
                        >
                          {event.title} {event.isAttended && '?'}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
                {data.tasks.length > 0 && (
                  <div>
                    <p className="text-sm font-medium mb-1" style={{ color: M.muted }}>
                      Tasks ({data.completedTasks}/{data.tasks.length} completed)
                    </p>
                    <ul className="space-y-1">
                      {data.tasks.map((task, i) => (
                        <li
                          key={i}
                          className="text-sm pl-4 border-l-2"
                          style={{
                            borderColor: task.isCompleted ? '#4CAF50' : M.muted,
                            color: task.isCompleted ? M.muted : M.text,
                            textDecoration: task.isCompleted ? 'line-through' : 'none'
                          }}
                        >
                          {task.title}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
                {!data.hasActivity && (
                  <p className="text-sm" style={{ color: M.muted }}>
                    No events or tasks on this day
                  </p>
                )}
              </div>
            );
          })()}
        </div>
      )}
    </div>
  );
};

export default Calendar;
