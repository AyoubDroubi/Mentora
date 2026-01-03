import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import {
  Lightbulb,
  ListChecks,
  FileText as FileTextIcon,
  ExternalLink,
  HelpCircle,
  TrendingUp,
  Award,
  BarChart3,
} from 'lucide-react';

export default function CareerBuilder() {
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

  /* ================= COMPONENTS ================= */

  /* ================= MAIN VIEW ================= */

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader title="Mentora - Career Builder" />
      <main className="container mx-auto px-4 mt-6">
        {/* Hero Section */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="text-left">
            <h1 className="text-3xl font-bold mb-4" style={{ color: M.text }}>Build Your Career Path</h1>
            <p className="text-lg mb-6" style={{ color: M.muted }}>
              Discover your ideal career with our comprehensive tools and personalized guidance.
              Take assessments, create plans, and track your progress towards success.
            </p>
          </div>
        </div>
        {/* Career Tools Section */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-xl font-bold mb-4" style={{ color: M.text }}>Career Tools</h2>
          <div className="grid grid-cols-2 gap-4">
            <button onClick={() => navigate('/create-career-builder')} className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all" style={{ borderColor: M.bg3 }}>
              <HelpCircle className="w-8 h-8" style={{ color: M.primary }} />
              <span className="text-sm font-medium">Career Quiz</span>
              <span className="text-xs text-[#5A7A6B]">Assessment</span>
            </button>
            <button onClick={() => navigate('/career-plan')} className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all" style={{ borderColor: M.bg3 }}>
              <FileTextIcon className="w-8 h-8" style={{ color: M.primary }} />
              <span className="text-sm font-medium">Career Plan</span>
              <span className="text-xs text-[#5A7A6B]">AI Generated</span>
            </button>
            <button onClick={() => navigate('/career-progress')} className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all" style={{ borderColor: M.bg3 }}>
              <TrendingUp className="w-8 h-8" style={{ color: M.primary }} />
              <span className="text-sm font-medium">Career Progress</span>
              <span className="text-xs text-[#5A7A6B]">Track Goals</span>
            </button>
            <button onClick={() => navigate('/career-skills')} className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all" style={{ borderColor: M.bg3 }}>
              <BarChart3 className="w-8 h-8" style={{ color: M.primary }} />
              <span className="text-sm font-medium">Skills Snapshot</span>
              <span className="text-xs text-[#5A7A6B]">Analysis</span>
            </button>
          </div>
        </div>

        {/* Career Resources Section */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          {/* Career Writing Tips */}
          <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-3 mb-4">
              <Lightbulb className="w-6 h-6" style={{ color: M.primary }} />
              <h3 className="text-xl font-bold" style={{ color: M.text }}>Career Writing Tips</h3>
            </div>
            <ul className="space-y-2">
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Tailor your resume for each job application
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Use action verbs to describe your achievements
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Quantify your accomplishments with numbers
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Keep your resume to one page for entry-level positions
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Include relevant keywords from the job description
              </li>
            </ul>
          </div>

          {/* Career Checklist */}
          <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-3 mb-4">
              <ListChecks className="w-6 h-6" style={{ color: M.primary }} />
              <h3 className="text-xl font-bold" style={{ color: M.text }}>Career Checklist</h3>
            </div>
            <ul className="space-y-2">
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Complete your professional profile on LinkedIn
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Build a portfolio website or GitHub repository
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Network with professionals in your field
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Take relevant online courses or certifications
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Gain practical experience through projects or volunteering
              </li>
            </ul>
          </div>

          {/* Cover Letter Guidance */}
          <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-3 mb-4">
              <FileTextIcon className="w-6 h-6" style={{ color: M.primary }} />
              <h3 className="text-xl font-bold" style={{ color: M.text }}>Cover Letter Guidance</h3>
            </div>
            <ul className="space-y-2">
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Address the hiring manager by name when possible
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Explain why you're interested in the specific company
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Highlight relevant experience and achievements
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Show how your skills match the job requirements
              </li>
              <li className="flex items-start gap-2 text-sm" style={{ color: M.muted }}>
                <div className="w-2 h-2 rounded-full mt-2 flex-shrink-0" style={{ background: M.primary }} />
                Keep it to 3-4 paragraphs
              </li>
            </ul>
          </div>

          {/* Career Resources */}
          <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-3 mb-4">
              <ExternalLink className="w-6 h-6" style={{ color: M.primary }} />
              <h3 className="text-xl font-bold" style={{ color: M.text }}>Career Resources</h3>
            </div>
            <div className="grid grid-cols-1 gap-3">
              <a href="https://www.linkedin.com/learning" target="_blank" rel="noopener noreferrer" className="flex items-center gap-3 p-2 rounded-lg border hover:shadow-md transition-all" style={{ borderColor: M.bg3, color: M.text }}>
                <ExternalLink className="w-4 h-4" style={{ color: M.primary }} />
                <span className="font-medium text-sm">LinkedIn Learning</span>
              </a>
              <a href="https://www.coursera.org" target="_blank" rel="noopener noreferrer" className="flex items-center gap-3 p-2 rounded-lg border hover:shadow-md transition-all" style={{ borderColor: M.bg3, color: M.text }}>
                <ExternalLink className="w-4 h-4" style={{ color: M.primary }} />
                <span className="font-medium text-sm">Coursera</span>
              </a>
              <a href="https://www.udemy.com" target="_blank" rel="noopener noreferrer" className="flex items-center gap-3 p-2 rounded-lg border hover:shadow-md transition-all" style={{ borderColor: M.bg3, color: M.text }}>
                <ExternalLink className="w-4 h-4" style={{ color: M.primary }} />
                <span className="font-medium text-sm">Udemy</span>
              </a>
              <a href="https://www.glassdoor.com" target="_blank" rel="noopener noreferrer" className="flex items-center gap-3 p-2 rounded-lg border hover:shadow-md transition-all" style={{ borderColor: M.bg3, color: M.text }}>
                <ExternalLink className="w-4 h-4" style={{ color: M.primary }} />
                <span className="font-medium text-sm">Glassdoor</span>
              </a>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
