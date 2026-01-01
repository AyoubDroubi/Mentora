import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useProfile } from '../contexts/ProfileContext';
import { FaCheckCircle, FaUniversity, FaGraduationCap, FaClock } from 'react-icons/fa';

/**
 * Profile Onboarding Banner
 * Shows when user hasn't completed their profile yet
 * Per SRS Module 2: Encourages completion of academic attributes
 */
const ProfileOnboarding = () => {
  const navigate = useNavigate();
  const { completion, needsOnboarding } = useProfile();

  // Don't show if profile is complete or doesn't need onboarding
  if (!needsOnboarding && completion >= 100) {
    return null;
  }

  // Only show if completion is below 100%
  if (completion >= 100) {
    return null;
  }

  return (
    <div className="bg-gradient-to-r from-indigo-500 to-purple-600 text-white rounded-lg p-6 shadow-lg mb-6">
      <div className="flex items-start justify-between">
        <div className="flex-1">
          <h3 className="text-xl font-bold mb-2 flex items-center">
            {needsOnboarding ? (
              <>Welcome! Let's set up your profile</>
            ) : (
              <>Complete Your Profile</>
            )}
          </h3>
          <p className="text-indigo-100 mb-4">
            {needsOnboarding
              ? "To get personalized career recommendations, please complete your academic profile with:"
              : `Your profile is ${completion}% complete. Add more information to unlock all features:`}
          </p>
          
          <div className="space-y-2 mb-4">
            <div className="flex items-center space-x-2">
              <FaUniversity className="text-indigo-200" />
              <span className="text-sm">University & Major (Required)</span>
            </div>
            <div className="flex items-center space-x-2">
              <FaGraduationCap className="text-indigo-200" />
              <span className="text-sm">Expected Graduation Year (Required)</span>
            </div>
            <div className="flex items-center space-x-2">
              <FaClock className="text-indigo-200" />
              <span className="text-sm">Timezone for Scheduling (Required)</span>
            </div>
          </div>

          <button
            onClick={() => navigate('/profile')}
            className="bg-white text-indigo-600 px-6 py-2 rounded-lg font-semibold hover:bg-indigo-50 transition inline-flex items-center space-x-2"
          >
            {needsOnboarding ? (
              <>
                <span>Complete Profile Now</span>
                <FaCheckCircle />
              </>
            ) : (
              <>
                <span>Update Profile</span>
                <FaCheckCircle />
              </>
            )}
          </button>
        </div>

        {/* Progress Circle */}
        <div className="ml-4">
          <div className="relative w-20 h-20">
            <svg className="transform -rotate-90" width="80" height="80">
              <circle
                cx="40"
                cy="40"
                r="35"
                stroke="rgba(255,255,255,0.2)"
                strokeWidth="6"
                fill="none"
              />
              <circle
                cx="40"
                cy="40"
                r="35"
                stroke="white"
                strokeWidth="6"
                fill="none"
                strokeDasharray={`${2 * Math.PI * 35}`}
                strokeDashoffset={`${2 * Math.PI * 35 * (1 - completion / 100)}`}
                strokeLinecap="round"
              />
            </svg>
            <div className="absolute inset-0 flex items-center justify-center">
              <span className="text-xl font-bold">{completion}%</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProfileOnboarding;
