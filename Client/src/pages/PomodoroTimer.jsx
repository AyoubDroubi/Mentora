import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { Clock, Play, Pause, RotateCcw, Save, TrendingUp } from 'lucide-react';
import { studySessionsService } from '../services';

export default function PomodoroTimer() {
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

  // Timer settings (in minutes)
  const DEFAULT_WORK_TIME = 25;
  const DEFAULT_SHORT_BREAK = 5;
  const DEFAULT_LONG_BREAK = 15;

  // State
  const [minutes, setMinutes] = useState(DEFAULT_WORK_TIME);
  const [seconds, setSeconds] = useState(0);
  const [isRunning, setIsRunning] = useState(false);
  const [mode, setMode] = useState('work'); // 'work', 'shortBreak', 'longBreak'
  const [sessionCount, setSessionCount] = useState(0);
  const [totalStudyTime, setTotalStudyTime] = useState(null);
  const [pauseCount, setPauseCount] = useState(0);
  const [sessionStartTime, setSessionStartTime] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [showSaveSuccess, setShowSaveSuccess] = useState(false);

  const intervalRef = useRef(null);

  // Load total study time on mount
  useEffect(() => {
    fetchTotalStudyTime();
  }, []);

  // Timer logic
  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(() => {
        if (seconds > 0) {
          setSeconds(seconds - 1);
        } else if (minutes > 0) {
          setMinutes(minutes - 1);
          setSeconds(59);
        } else {
          // Timer completed
          handleTimerComplete();
        }
      }, 1000);
    } else {
      clearInterval(intervalRef.current);
    }

    return () => clearInterval(intervalRef.current);
  }, [isRunning, minutes, seconds]);

  // Fetch total study time
  const fetchTotalStudyTime = async () => {
    try {
      const response = await studySessionsService.getSummary();
      if (response.success) {
        setTotalStudyTime(response.data);
      }
    } catch (err) {
      console.error('Error fetching study time:', err);
    }
  };

  // Handle timer complete
  const handleTimerComplete = async () => {
    setIsRunning(false);
    
    if (mode === 'work') {
      // Save work session
      await saveSession();
      
      // Increment session count
      const newCount = sessionCount + 1;
      setSessionCount(newCount);

      // Play completion sound (optional)
      playNotificationSound();

      // Show success message
      setShowSaveSuccess(true);
      setTimeout(() => setShowSaveSuccess(false), 3000);

      // Suggest break
      if (newCount % 4 === 0) {
        // After 4 sessions, suggest long break
        if (window.confirm('Great job! 4 sessions completed. Take a long break?')) {
          startLongBreak();
        } else {
          reset();
        }
      } else {
        // Suggest short break
        if (window.confirm('Session complete! Take a short break?')) {
          startShortBreak();
        } else {
          reset();
        }
      }
    } else {
      // Break completed
      playNotificationSound();
      if (window.confirm('Break finished! Ready to study?')) {
        startWorkSession();
      } else {
        reset();
      }
    }
  };

  // Save completed session
  const saveSession = async () => {
    try {
      setLoading(true);
      setError(null);

      const durationMinutes = mode === 'work' ? DEFAULT_WORK_TIME : 0;

      const response = await studySessionsService.saveSession({
        durationMinutes,
        startTime: sessionStartTime || new Date().toISOString(),
        pauseCount,
        focusScore: calculateFocusScore()
      });

      if (response.success) {
        // Refresh total study time
        await fetchTotalStudyTime();
        
        // Reset pause count
        setPauseCount(0);
        setSessionStartTime(null);
      }
    } catch (err) {
      console.error('Error saving session:', err);
      setError(err.response?.data?.message || 'Failed to save session');
    } finally {
      setLoading(false);
    }
  };

  // Calculate focus score based on pause count
  const calculateFocusScore = () => {
    if (pauseCount === 0) return 100;
    if (pauseCount === 1) return 90;
    if (pauseCount === 2) return 80;
    if (pauseCount === 3) return 70;
    return Math.max(50, 100 - (pauseCount * 10));
  };

  // Play notification sound
  const playNotificationSound = () => {
    // You can add audio file here
    // const audio = new Audio('/notification.mp3');
    // audio.play();
  };

  // Timer controls
  const startWorkSession = () => {
    setMode('work');
    setMinutes(DEFAULT_WORK_TIME);
    setSeconds(0);
    setIsRunning(true);
    setSessionStartTime(new Date().toISOString());
  };

  const startShortBreak = () => {
    setMode('shortBreak');
    setMinutes(DEFAULT_SHORT_BREAK);
    setSeconds(0);
    setIsRunning(true);
  };

  const startLongBreak = () => {
    setMode('longBreak');
    setMinutes(DEFAULT_LONG_BREAK);
    setSeconds(0);
    setIsRunning(true);
  };

  const toggleTimer = () => {
    if (!isRunning && !sessionStartTime) {
      setSessionStartTime(new Date().toISOString());
    }
    
    if (isRunning) {
      // Pausing
      setPauseCount(prev => prev + 1);
    }
    
    setIsRunning(!isRunning);
  };

  const reset = () => {
    setIsRunning(false);
    setPauseCount(0);
    setSessionStartTime(null);
    
    switch (mode) {
      case 'work':
        setMinutes(DEFAULT_WORK_TIME);
        break;
      case 'shortBreak':
        setMinutes(DEFAULT_SHORT_BREAK);
        break;
      case 'longBreak':
        setMinutes(DEFAULT_LONG_BREAK);
        break;
    }
    setSeconds(0);
  };

  // Format time display
  const formatTime = (mins, secs) => {
    return `${String(mins).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
  };

  // Get progress percentage
  const getProgress = () => {
    const totalSeconds = mode === 'work' 
      ? DEFAULT_WORK_TIME * 60 
      : (mode === 'shortBreak' ? DEFAULT_SHORT_BREAK : DEFAULT_LONG_BREAK) * 60;
    const currentSeconds = minutes * 60 + seconds;
    return ((totalSeconds - currentSeconds) / totalSeconds) * 100;
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <Clock className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Pomodoro Timer</span>
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

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />

      <main className="container mx-auto px-4 mt-6 max-w-4xl">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Success Message */}
        {showSaveSuccess && (
          <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded-lg mb-4 flex items-center gap-2">
            <Save className="w-5 h-5" />
            Session saved successfully! Great work! ??
          </div>
        )}

        {/* Total Study Time Card */}
        {totalStudyTime && (
          <div className="bg-white rounded-2xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-3 mb-2">
              <TrendingUp className="w-6 h-6" style={{ color: M.primary }} />
              <h3 className="font-bold text-lg" style={{ color: M.text }}>Total Study Time</h3>
            </div>
            <div className="text-4xl font-bold" style={{ color: M.primary }}>
              {totalStudyTime.formatted}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>
              {totalStudyTime.totalMinutes} minutes total
            </p>
          </div>
        )}

        {/* Timer Card */}
        <div className="bg-white rounded-2xl p-8 shadow-lg border" style={{ borderColor: M.bg3 }}>
          {/* Mode Selector */}
          <div className="flex gap-2 mb-6">
            <button
              onClick={startWorkSession}
              disabled={isRunning}
              className={`flex-1 px-4 py-2 rounded-lg font-medium transition-all ${
                mode === 'work' ? 'text-white shadow-md' : 'text-[#5A7A6B]'
              }`}
              style={mode === 'work' ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
            >
              Work (25min)
            </button>
            <button
              onClick={startShortBreak}
              disabled={isRunning}
              className={`flex-1 px-4 py-2 rounded-lg font-medium transition-all ${
                mode === 'shortBreak' ? 'text-white shadow-md' : 'text-[#5A7A6B]'
              }`}
              style={mode === 'shortBreak' ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
            >
              Short Break (5min)
            </button>
            <button
              onClick={startLongBreak}
              disabled={isRunning}
              className={`flex-1 px-4 py-2 rounded-lg font-medium transition-all ${
                mode === 'longBreak' ? 'text-white shadow-md' : 'text-[#5A7A6B]'
              }`}
              style={mode === 'longBreak' ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
            >
              Long Break (15min)
            </button>
          </div>

          {/* Timer Display */}
          <div className="text-center mb-8">
            <div className="relative inline-block">
              {/* Progress Circle */}
              <svg className="transform -rotate-90" width="300" height="300">
                <circle
                  cx="150"
                  cy="150"
                  r="140"
                  stroke={M.bg3}
                  strokeWidth="12"
                  fill="none"
                />
                <circle
                  cx="150"
                  cy="150"
                  r="140"
                  stroke={M.primary}
                  strokeWidth="12"
                  fill="none"
                  strokeDasharray={`${2 * Math.PI * 140}`}
                  strokeDashoffset={`${2 * Math.PI * 140 * (1 - getProgress() / 100)}`}
                  strokeLinecap="round"
                  className="transition-all duration-1000"
                />
              </svg>

              {/* Time Display */}
              <div className="absolute inset-0 flex items-center justify-center">
                <div>
                  <div className="text-6xl font-bold" style={{ color: M.text }}>
                    {formatTime(minutes, seconds)}
                  </div>
                  <div className="text-sm" style={{ color: M.muted }}>
                    {mode === 'work' ? 'Focus Time' : 
                     mode === 'shortBreak' ? 'Short Break' : 'Long Break'}
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Controls */}
          <div className="flex gap-3 justify-center">
            <button
              onClick={toggleTimer}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
              style={{ background: M.primary }}
            >
              {isRunning ? <Pause className="w-5 h-5" /> : <Play className="w-5 h-5" />}
              {isRunning ? 'Pause' : 'Start'}
            </button>
            <button
              onClick={reset}
              className="px-8 py-4 rounded-lg border font-medium hover:shadow-md transition-all flex items-center gap-2"
              style={{ borderColor: M.bg3, color: M.text }}
            >
              <RotateCcw className="w-5 h-5" />
              Reset
            </button>
          </div>

          {/* Session Info */}
          <div className="mt-6 grid grid-cols-3 gap-4 text-center">
            <div>
              <p className="text-sm" style={{ color: M.muted }}>Sessions Today</p>
              <p className="text-2xl font-bold" style={{ color: M.primary }}>{sessionCount}</p>
            </div>
            <div>
              <p className="text-sm" style={{ color: M.muted }}>Pause Count</p>
              <p className="text-2xl font-bold" style={{ color: M.primary }}>{pauseCount}</p>
            </div>
            <div>
              <p className="text-sm" style={{ color: M.muted }}>Focus Score</p>
              <p className="text-2xl font-bold" style={{ color: M.primary }}>{calculateFocusScore()}%</p>
            </div>
          </div>
        </div>

        {/* Tips */}
        <div className="bg-blue-50 rounded-lg p-4 mt-6">
          <h4 className="font-semibold mb-2 text-blue-900">Pomodoro Technique Tips:</h4>
          <ul className="text-sm text-blue-800 space-y-1">
            <li>• Work for 25 minutes with full focus</li>
            <li>• Take a 5-minute break after each session</li>
            <li>• After 4 sessions, take a longer 15-minute break</li>
            <li>• Minimize distractions during work sessions</li>
            <li>• Your focus score decreases with each pause</li>
          </ul>
        </div>
      </main>
    </div>
  );
}
