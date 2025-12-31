import React from 'react';
import { useAuth } from '../contexts/AuthContext';
import { FiUser } from 'react-icons/fi';

const UserInfo = ({ className = '' }) => {
  const { user, isAuthenticated } = useAuth();

  if (!isAuthenticated || !user) {
    return null;
  }

  return (
    <div className={`flex items-center gap-3 ${className}`}>
      <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-full flex items-center justify-center">
        <FiUser className="w-5 h-5 text-white" />
      </div>
      <div className="text-left">
        <p className="text-[#2C3E3F] font-medium">
          {user.firstName && user.lastName
            ? `${user.firstName} ${user.lastName}`
            : user.email}
        </p>
        {user.firstName && user.lastName && (
          <p className="text-[#6B9080] text-sm">{user.email}</p>
        )}
      </div>
    </div>
  );
};

export default UserInfo;
