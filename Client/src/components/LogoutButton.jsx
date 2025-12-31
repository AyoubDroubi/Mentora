import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FiLogOut } from 'react-icons/fi';
import { useAuth } from '../contexts/AuthContext';

const LogoutButton = ({ className = '', variant = 'default' }) => {
  const navigate = useNavigate();
  const { logout } = useAuth();
  const [isLoading, setIsLoading] = useState(false);

  const handleLogout = async () => {
    if (window.confirm('Are you sure you want to logout?')) {
      setIsLoading(true);
      try {
        await logout();
        navigate('/login');
      } catch (error) {
        console.error('Logout error:', error);
      } finally {
        setIsLoading(false);
      }
    }
  };

  // Default button style
  if (variant === 'default') {
    return (
      <button
        onClick={handleLogout}
        disabled={isLoading}
        className={`flex items-center gap-2 px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-lg transition disabled:opacity-50 disabled:cursor-not-allowed ${className}`}
      >
        <FiLogOut className="w-5 h-5" />
        <span>{isLoading ? 'Logging out...' : 'Logout'}</span>
      </button>
    );
  }

  // Icon only style
  if (variant === 'icon') {
    return (
      <button
        onClick={handleLogout}
        disabled={isLoading}
        title="Logout"
        className={`p-2 text-red-500 hover:text-red-600 hover:bg-red-50 rounded-lg transition disabled:opacity-50 disabled:cursor-not-allowed ${className}`}
      >
        <FiLogOut className="w-5 h-5" />
      </button>
    );
  }

  // Text link style
  if (variant === 'link') {
    return (
      <button
        onClick={handleLogout}
        disabled={isLoading}
        className={`text-red-500 hover:text-red-600 underline disabled:opacity-50 disabled:cursor-not-allowed ${className}`}
      >
        {isLoading ? 'Logging out...' : 'Logout'}
      </button>
    );
  }

  return null;
};

export default LogoutButton;
