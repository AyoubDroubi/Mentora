import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { BookOpen } from 'lucide-react';
import PropTypes from 'prop-types';

/**
 * Shared Header Component
 * Unified header for all pages in the application
 * Maintains consistent theme and styling
 */
const SharedHeader = ({ title = 'Mentora', showNav = true }) => {
  const navigate = useNavigate();
  const { user } = useUser();

  // Unified Theme Colors
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  return (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      {/* Logo & Title */}
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">{title}</span>
      </div>

      {/* Navigation & Profile */}
      <div className="flex items-center gap-4">
        {showNav && (
          <nav className="hidden md:flex items-center gap-4">
            <button
              onClick={() => navigate('/dashboard')}
              className="text-white font-medium hover:bg-white/20 transition-all duration-300 px-3 py-2 rounded-lg"
            >
              Dashboard
            </button>
            <button
              onClick={() => navigate('/study-planner')}
              className="text-white font-medium hover:bg-white/20 transition-all duration-300 px-3 py-2 rounded-lg"
            >
              Study Planner
            </button>
            <button
              onClick={() => navigate('/career-builder')}
              className="text-white font-medium hover:bg-white/20 transition-all duration-300 px-3 py-2 rounded-lg"
            >
              Career Builder
            </button>
            <button
              onClick={() => navigate('/skills')}
              className="text-white font-medium hover:bg-white/20 transition-all duration-300 px-3 py-2 rounded-lg"
            >
              My Skills
            </button>
          </nav>
        )}

        {/* Profile Avatar */}
        {user?.avatar ? (
          <img
            onClick={() => navigate('/profile')}
            src={user.avatar}
            alt="Profile"
            className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
          />
        ) : (
          <div
            onClick={() => navigate('/profile')}
            className="w-10 h-10 rounded-full border-2 border-white flex items-center justify-center bg-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
            style={{ color: M.primary }}
          >
            <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
            </svg>
          </div>
        )}
      </div>
    </header>
  );
};

SharedHeader.propTypes = {
  title: PropTypes.string,
  showNav: PropTypes.bool
};

export default SharedHeader;
