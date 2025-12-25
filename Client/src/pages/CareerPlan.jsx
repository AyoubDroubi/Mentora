import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import {
  Target,
  ChevronRight,
  Award,
  CheckSquare,
  BookOpen,
  TrendingUp,
  Clock,
  BookMarked,
  RotateCcw,
  ArrowLeft,
  ArrowRight,
  Brain,
  Home as HomeIcon,
  User as UserIcon,
  LayoutDashboard,
  FileText,
  Download,
  Lightbulb,
  ListChecks,
  Zap,
  FileText as FileTextIcon,
  ExternalLink,
  BarChart3,
  CheckCircle,
  AlertCircle,
  Clock as ClockIcon,
  Star,
  Code,
  Users,
  Briefcase,
  GraduationCap,
  TrendingUp as TrendingUpIcon,
} from 'lucide-react';

export default function CareerPlan() {
  const navigate = useNavigate();
  const { user } = useUser();
  const { careerPlan } = user;

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const planStatus = careerPlan ? 'Generated' : 'Not Generated';

  const Header = () => (
       <header
         className="px-6 py-4 flex items-center justify-between shadow-lg"
         style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
       >
         <div className="flex items-center gap-3">
           <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
             <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
           </div>
           <span className="text-white text-xl font-bold">Mentora - plan</span>
         </div>
   
         <div className="flex items-center gap-4">
           <img
           onClick={() => navigate('/profile')}
             src={user.avatar}
             alt="Profile"
             className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
           />
         </div>
       </header>
     );
   


  const getStatusBadge = () => {
    switch (planStatus) {
      case 'Not Generated':
        return (
          <div className="flex items-center gap-2 px-4 py-2 bg-gray-100 text-gray-700 rounded-full">
            <AlertCircle className="w-4 h-4" />
            <span className="text-sm font-medium">Not Generated</span>
          </div>
        );
      case 'Generated':
        return (
          <div className="flex items-center gap-2 px-4 py-2 bg-green-100 text-green-700 rounded-full">
            <CheckCircle className="w-4 h-4" />
            <span className="text-sm font-medium">Generated</span>
          </div>
        );
      case 'Outdated':
        return (
          <div className="flex items-center gap-2 px-4 py-2 bg-yellow-100 text-yellow-700 rounded-full">
            <ClockIcon className="w-4 h-4" />
            <span className="text-sm font-medium">Outdated</span>
          </div>
        );
      default:
        return null;
    }
  };

  if (!careerPlan) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <h1 className="text-3xl font-bold mb-4" style={{ color: M.text }}>No Career Plan Generated</h1>
            <p className="text-lg mb-6" style={{ color: M.muted }}>
              You haven't generated a career plan yet. Please take the career assessment to create your personalized plan.
            </p>
            <button
              onClick={() => navigate('/create-career-builder')}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all text-lg"
              style={{ background: M.primary }}
            >
              Take Career Assessment
            </button>
          </div>
        </main>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Plan Status Badge */}
        <div className="flex justify-center mb-6">
          {getStatusBadge()}
        </div>

        {/* Career Timeline */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Timeline</h2>
          <div className="space-y-6">
            {careerPlan.timeline.map((stage, index) => {
              const IconComponent = stage.icon;
              return (
                <div key={index} className="flex items-start gap-4">
                  <div className={`flex-shrink-0 w-12 h-12 rounded-full flex items-center justify-center ${
                    stage.completed ? 'bg-green-100' : 'bg-gray-100'
                  }`}>
                    <IconComponent className={`w-6 h-6 ${
                      stage.completed ? 'text-green-600' : 'text-gray-400'
                    }`} />
                  </div>
                  <div className="flex-1">
                    <div className="flex items-center gap-2 mb-2">
                      <h3 className="text-lg font-semibold" style={{ color: M.text }}>{stage.stage}</h3>
                      <span className="text-sm px-2 py-1 bg-gray-100 rounded-full" style={{ color: M.muted }}>
                        {stage.duration}
                      </span>
                    </div>
                    <p className="text-sm mb-3" style={{ color: M.muted }}>{stage.description}</p>
                    <div className="flex flex-wrap gap-2">
                      {stage.skills.map((skill, skillIndex) => (
                        <span
                          key={skillIndex}
                          className="px-3 py-1 bg-blue-50 text-blue-700 rounded-full text-xs font-medium"
                        >
                          {skill}
                        </span>
                      ))}
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        </div>

        {/* Skills Preview */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Skills Preview</h2>

          <div className="grid md:grid-cols-2 gap-6">
            {/* Technical Skills */}
            <div>
              <div className="flex items-center gap-2 mb-4">
                <Code className="w-5 h-5" style={{ color: M.primary }} />
                <h3 className="text-lg font-semibold" style={{ color: M.text }}>Technical Skills</h3>
              </div>
              <div className="flex flex-wrap gap-2">
                {careerPlan.skillsPreview.technical.map((skill, index) => (
                  <span
                    key={index}
                    className="px-3 py-2 bg-green-50 text-green-700 rounded-lg text-sm font-medium border border-green-200"
                  >
                    {skill}
                  </span>
                ))}
              </div>
            </div>

            {/* Soft Skills */}
            <div>
              <div className="flex items-center gap-2 mb-4">
                <Users className="w-5 h-5" style={{ color: M.primary }} />
                <h3 className="text-lg font-semibold" style={{ color: M.text }}>Soft Skills</h3>
              </div>
              <div className="flex flex-wrap gap-2">
                {careerPlan.skillsPreview.soft.map((skill, index) => (
                  <span
                    key={index}
                    className="px-3 py-2 bg-purple-50 text-purple-700 rounded-lg text-sm font-medium border border-purple-200"
                  >
                    {skill}
                  </span>
                ))}
              </div>
            </div>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center gap-4">
          <button
            onClick={() => navigate('/create-career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            Retake Assessment
          </button>
          <button
            onClick={() => navigate('/career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.secondary }}
          >
            Back to Career Builder
          </button>
          <button
            onClick={() => {}}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            View Career Plan
          </button>
        </div>
        </main>
      </div>
    );
  }
