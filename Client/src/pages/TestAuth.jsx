import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import LogoutButton from '../components/LogoutButton';
import UserInfo from '../components/UserInfo';
import { GraduationCap } from 'lucide-react';

export default function TestAuth() {
  const navigate = useNavigate();
  const { user, isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div className="min-h-screen bg-[#F6FFF8] flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-[#6B9080] mx-auto mb-4"></div>
          <p className="text-[#6B9080]">Loading...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F6FFF8] p-8">
      <div className="max-w-4xl mx-auto">
        {/* Header */}
        <div className="bg-white rounded-2xl shadow-lg p-6 mb-6 border border-[#A4C3B2]">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="w-12 h-12 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
                <GraduationCap className="w-7 h-7 text-white" />
              </div>
              <div>
                <h1 className="text-2xl font-bold text-[#2C3E3F]">Authentication Test Page</h1>
                <p className="text-[#6B9080] text-sm">Verify your login system</p>
              </div>
            </div>
            <LogoutButton variant="default" />
          </div>
        </div>

        {/* Authentication Status */}
        <div className="bg-white rounded-2xl shadow-lg p-6 mb-6 border border-[#A4C3B2]">
          <h2 className="text-xl font-semibold text-[#2C3E3F] mb-4">Authentication Status</h2>
          <div className="space-y-3">
            <div className="flex items-center justify-between p-3 bg-[#F6FFF8] rounded-lg">
              <span className="text-[#6B9080] font-medium">Is Authenticated:</span>
              <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                isAuthenticated 
                  ? 'bg-green-100 text-green-700' 
                  : 'bg-red-100 text-red-700'
              }`}>
                {isAuthenticated ? '? Yes' : '? No'}
              </span>
            </div>
            <div className="flex items-center justify-between p-3 bg-[#F6FFF8] rounded-lg">
              <span className="text-[#6B9080] font-medium">Loading:</span>
              <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                loading 
                  ? 'bg-yellow-100 text-yellow-700' 
                  : 'bg-green-100 text-green-700'
              }`}>
                {loading ? '? Yes' : '? No'}
              </span>
            </div>
          </div>
        </div>

        {/* User Information */}
        {isAuthenticated && user && (
          <div className="bg-white rounded-2xl shadow-lg p-6 mb-6 border border-[#A4C3B2]">
            <h2 className="text-xl font-semibold text-[#2C3E3F] mb-4">User Information</h2>
            <UserInfo className="mb-4 p-4 bg-[#F6FFF8] rounded-lg" />
            <div className="space-y-2">
              <div className="p-3 bg-[#F6FFF8] rounded-lg">
                <span className="text-[#6B9080] font-medium">User ID:</span>
                <p className="text-[#2C3E3F] font-mono text-sm mt-1">{user.userId || 'N/A'}</p>
              </div>
              <div className="p-3 bg-[#F6FFF8] rounded-lg">
                <span className="text-[#6B9080] font-medium">Email:</span>
                <p className="text-[#2C3E3F] mt-1">{user.email || 'N/A'}</p>
              </div>
              <div className="p-3 bg-[#F6FFF8] rounded-lg">
                <span className="text-[#6B9080] font-medium">First Name:</span>
                <p className="text-[#2C3E3F] mt-1">{user.firstName || 'N/A'}</p>
              </div>
              <div className="p-3 bg-[#F6FFF8] rounded-lg">
                <span className="text-[#6B9080] font-medium">Last Name:</span>
                <p className="text-[#2C3E3F] mt-1">{user.lastName || 'N/A'}</p>
              </div>
            </div>
          </div>
        )}

        {/* Stored Tokens */}
        <div className="bg-white rounded-2xl shadow-lg p-6 mb-6 border border-[#A4C3B2]">
          <h2 className="text-xl font-semibold text-[#2C3E3F] mb-4">Stored Tokens</h2>
          <div className="space-y-3">
            <div className="p-3 bg-[#F6FFF8] rounded-lg">
              <span className="text-[#6B9080] font-medium">Access Token:</span>
              <div className="mt-2 p-2 bg-white rounded border border-[#A4C3B2] overflow-hidden">
                <p className="text-[#2C3E3F] font-mono text-xs break-all">
                  {localStorage.getItem('accessToken') || 'Not found'}
                </p>
              </div>
            </div>
            <div className="p-3 bg-[#F6FFF8] rounded-lg">
              <span className="text-[#6B9080] font-medium">Refresh Token:</span>
              <div className="mt-2 p-2 bg-white rounded border border-[#A4C3B2] overflow-hidden">
                <p className="text-[#2C3E3F] font-mono text-xs break-all">
                  {localStorage.getItem('refreshToken') || 'Not found'}
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Test Actions */}
        <div className="bg-white rounded-2xl shadow-lg p-6 border border-[#A4C3B2]">
          <h2 className="text-xl font-semibold text-[#2C3E3F] mb-4">Test Navigation</h2>
          <div className="grid grid-cols-2 gap-4">
            <button
              onClick={() => navigate('/login')}
              className="px-4 py-3 bg-[#6B9080] hover:bg-[#577466] text-white rounded-lg transition"
            >
              Go to Login
            </button>
            <button
              onClick={() => navigate('/signup')}
              className="px-4 py-3 bg-[#6B9080] hover:bg-[#577466] text-white rounded-lg transition"
            >
              Go to SignUp
            </button>
            <button
              onClick={() => navigate('/dashboard')}
              className="px-4 py-3 bg-[#6B9080] hover:bg-[#577466] text-white rounded-lg transition"
            >
              Go to Dashboard
            </button>
            <button
              onClick={() => navigate('/profile')}
              className="px-4 py-3 bg-[#6B9080] hover:bg-[#577466] text-white rounded-lg transition"
            >
              Go to Profile
            </button>
          </div>
        </div>

        {/* Logout Variations */}
        <div className="bg-white rounded-2xl shadow-lg p-6 mt-6 border border-[#A4C3B2]">
          <h2 className="text-xl font-semibold text-[#2C3E3F] mb-4">Logout Button Variants</h2>
          <div className="flex flex-wrap gap-4 items-center">
            <div>
              <p className="text-sm text-[#6B9080] mb-2">Default</p>
              <LogoutButton variant="default" />
            </div>
            <div>
              <p className="text-sm text-[#6B9080] mb-2">Icon Only</p>
              <LogoutButton variant="icon" />
            </div>
            <div>
              <p className="text-sm text-[#6B9080] mb-2">Link Style</p>
              <LogoutButton variant="link" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
