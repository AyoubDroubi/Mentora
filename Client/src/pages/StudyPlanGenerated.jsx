import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAssessment } from '../contexts/AssessmentContext';
import SharedHeader from '../components/SharedHeader';
import LoadingSpinner from '../components/LoadingSpinner';
import { 
  CheckCircleIcon, 
  SparklesIcon, 
  ClockIcon,
  ChartBarIcon,
  ArrowRightIcon,
  AcademicCapIcon
} from '@heroicons/react/24/outline';

export default function StudyPlanGenerated() {
  const navigate = useNavigate();
  const { currentStudyPlan, loading } = useAssessment();
  const [celebrating, setCelebrating] = useState(true);

  useEffect(() => {
    // Stop celebrating after 3 seconds
    const timer = setTimeout(() => {
      setCelebrating(false);
    }, 3000);

    return () => clearTimeout(timer);
  }, []);

  useEffect(() => {
    // If no study plan, redirect to assessment
    if (!loading && !currentStudyPlan) {
      navigate('/assessment');
    }
  }, [currentStudyPlan, loading, navigate]);

  const handleViewPlan = () => {
    if (currentStudyPlan) {
      navigate(`/study-plan/${currentStudyPlan.id}`);
    }
  };

  const handleViewAllPlans = () => {
    navigate('/my-study-plans');
  };

  if (loading || !currentStudyPlan) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-green-50 via-white to-blue-50">
        <SharedHeader />
        <div className="flex items-center justify-center h-[calc(100vh-80px)]">
          <LoadingSpinner />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-green-50 via-white to-blue-50">
      <SharedHeader />

      <div className="max-w-4xl mx-auto px-4 py-12">
        {/* Success Animation */}
        {celebrating && (
          <div className="fixed inset-0 flex items-center justify-center z-50 pointer-events-none">
            <div className="animate-bounce">
              <SparklesIcon className="w-32 h-32 text-yellow-500" />
            </div>
          </div>
        )}

        {/* Success Card */}
        <div className="bg-white rounded-2xl shadow-xl p-8 mb-6">
          {/* Success Icon */}
          <div className="flex justify-center mb-6">
            <div className="p-6 bg-green-100 rounded-full">
              <CheckCircleIcon className="w-16 h-16 text-green-600" />
            </div>
          </div>

          {/* Success Message */}
          <div className="text-center mb-8">
            <h1 className="text-4xl font-bold text-gray-900 mb-3">
              ?? Study Plan Generated!
            </h1>
            <p className="text-xl text-gray-600">
              Your personalized AI-powered study plan is ready
            </p>
          </div>

          {/* Plan Summary */}
          <div className="bg-gradient-to-r from-indigo-50 to-purple-50 rounded-xl p-6 mb-6">
            <h2 className="text-2xl font-bold text-gray-900 mb-2">
              {currentStudyPlan.title}
            </h2>
            <p className="text-gray-700 mb-4">
              {currentStudyPlan.summary}
            </p>

            {/* Plan Stats */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              {/* Total Steps */}
              <div className="bg-white rounded-lg p-4 text-center">
                <div className="flex justify-center mb-2">
                  <AcademicCapIcon className="w-8 h-8 text-indigo-600" />
                </div>
                <p className="text-2xl font-bold text-gray-900">
                  {currentStudyPlan.steps?.length || 0}
                </p>
                <p className="text-sm text-gray-600">Study Steps</p>
              </div>

              {/* Estimated Hours */}
              <div className="bg-white rounded-lg p-4 text-center">
                <div className="flex justify-center mb-2">
                  <ClockIcon className="w-8 h-8 text-blue-600" />
                </div>
                <p className="text-2xl font-bold text-gray-900">
                  {currentStudyPlan.estimatedHours}h
                </p>
                <p className="text-sm text-gray-600">Total Hours</p>
              </div>

              {/* Skills to Learn */}
              <div className="bg-white rounded-lg p-4 text-center">
                <div className="flex justify-center mb-2">
                  <ChartBarIcon className="w-8 h-8 text-purple-600" />
                </div>
                <p className="text-2xl font-bold text-gray-900">
                  {currentStudyPlan.requiredSkills?.length || 0}
                </p>
                <p className="text-sm text-gray-600">Skills</p>
              </div>
            </div>
          </div>

          {/* Difficulty Badge */}
          <div className="flex justify-center mb-6">
            <span className={`px-4 py-2 rounded-full text-sm font-semibold ${
              currentStudyPlan.difficultyLevel === 'Beginner' ? 'bg-green-100 text-green-800' :
              currentStudyPlan.difficultyLevel === 'Intermediate' ? 'bg-yellow-100 text-yellow-800' :
              currentStudyPlan.difficultyLevel === 'Advanced' ? 'bg-orange-100 text-orange-800' :
              'bg-red-100 text-red-800'
            }`}>
              {currentStudyPlan.difficultyLevel} Level
            </span>
          </div>

          {/* Key Features */}
          <div className="mb-8">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">
              What's Included:
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
              <div className="flex items-start">
                <CheckCircleIcon className="w-5 h-5 text-green-600 mr-2 mt-0.5 flex-shrink-0" />
                <span className="text-gray-700">Step-by-step learning path</span>
              </div>
              <div className="flex items-start">
                <CheckCircleIcon className="w-5 h-5 text-green-600 mr-2 mt-0.5 flex-shrink-0" />
                <span className="text-gray-700">Curated learning resources</span>
              </div>
              <div className="flex items-start">
                <CheckCircleIcon className="w-5 h-5 text-green-600 mr-2 mt-0.5 flex-shrink-0" />
                <span className="text-gray-700">Progress tracking</span>
              </div>
              <div className="flex items-start">
                <CheckCircleIcon className="w-5 h-5 text-green-600 mr-2 mt-0.5 flex-shrink-0" />
                <span className="text-gray-700">Skill gap analysis</span>
              </div>
            </div>
          </div>

          {/* Action Buttons */}
          <div className="flex flex-col sm:flex-row gap-4">
            <button
              onClick={handleViewPlan}
              className="flex-1 flex items-center justify-center px-6 py-4 bg-gradient-to-r from-indigo-600 to-purple-600 text-white font-semibold rounded-lg hover:from-indigo-700 hover:to-purple-700 transition-all shadow-lg"
            >
              <SparklesIcon className="w-5 h-5 mr-2" />
              View My Study Plan
              <ArrowRightIcon className="w-5 h-5 ml-2" />
            </button>

            <button
              onClick={handleViewAllPlans}
              className="px-6 py-4 bg-white text-gray-700 font-semibold rounded-lg border-2 border-gray-300 hover:bg-gray-50 transition-all"
            >
              View All Plans
            </button>
          </div>
        </div>

        {/* Tips Card */}
        <div className="bg-blue-50 border border-blue-200 rounded-xl p-6">
          <h3 className="text-lg font-semibold text-blue-900 mb-3">
            ?? Getting Started Tips:
          </h3>
          <ul className="space-y-2 text-blue-800">
            <li className="flex items-start">
              <span className="mr-2">1.</span>
              <span>Review your study plan and understand each step</span>
            </li>
            <li className="flex items-start">
              <span className="mr-2">2.</span>
              <span>Mark your plan as "Active" to start tracking progress</span>
            </li>
            <li className="flex items-start">
              <span className="mr-2">3.</span>
              <span>Check off checkpoints as you complete them</span>
            </li>
            <li className="flex items-start">
              <span className="mr-2">4.</span>
              <span>Explore recommended learning resources</span>
            </li>
            <li className="flex items-start">
              <span className="mr-2">5.</span>
              <span>Track your skill development over time</span>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
}
