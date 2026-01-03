import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import {
  Target,
  Award,
  CheckSquare,
  TrendingUp,
  Clock,
  Calendar,
  Trophy,
  CheckCircle,
  AlertCircle,
  Clock as ClockIcon,
  Star,
} from 'lucide-react';

export default function CareerProgress() {
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

  const [skillsData, setSkillsData] = useState(null);
  const [progressData, setProgressData] = useState(null);

  useEffect(() => {
    const fetchSkills = async () => {
      try {
        const response = await fetch('/api/ai/career-skills', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            careerGoal: user.careerGoal,
            skills: user.skills,
            level: user.level,
          }),
        });
        if (response.ok) {
          const data = await response.json();
          setSkillsData(data);
        } else {
          console.error('Failed to fetch skills');
          // Provide mock data when API fails
          const mockData = {
            topSkills: [
              { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
              { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
              { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
              { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
              { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 }
            ],
            allSkills: [
              { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
              { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
              { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
              { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
              { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 },
              { name: 'Communication', type: 'Soft', status: 'Achieved', priority: 6 },
              { name: 'Problem Solving', type: 'Soft', status: 'In Progress', priority: 7 },
              { name: 'Team Work', type: 'Soft', status: 'Achieved', priority: 8 },
              { name: 'Time Management', type: 'Soft', status: 'In Progress', priority: 9 },
              { name: 'Adaptability', type: 'Soft', status: 'Missing', priority: 10 }
            ]
          };
          setSkillsData(mockData);
        }
      } catch (error) {
        console.error('Error fetching skills:', error);
        // Provide mock data when API fails
        const mockData = {
          topSkills: [
            { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
            { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
            { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
            { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
            { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 }
          ],
          allSkills: [
            { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
            { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
            { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
            { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
            { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 },
            { name: 'Communication', type: 'Soft', status: 'Achieved', priority: 6 },
            { name: 'Problem Solving', type: 'Soft', status: 'In Progress', priority: 7 },
            { name: 'Team Work', type: 'Soft', status: 'Achieved', priority: 8 },
            { name: 'Time Management', type: 'Soft', status: 'In Progress', priority: 9 },
            { name: 'Adaptability', type: 'Soft', status: 'Missing', priority: 10 }
          ]
        };
        setSkillsData(mockData);
      }
    };

    fetchSkills();
  }, [user]);

  useEffect(() => {
    if (skillsData) {
      // Calculate overall progress based on skills
      const achievedSkills = skillsData.allSkills.filter(skill => skill.status === 'Achieved').length;
      const totalSkills = skillsData.allSkills.length;
      const overallProgress = Math.round((achievedSkills / totalSkills) * 100);

      // Generate dynamic milestones based on skills
      const milestones = [
        {
          id: 1,
          title: 'Foundation Skills',
          description: 'Master core programming fundamentals',
          progress: calculateMilestoneProgress(skillsData.allSkills, ['JavaScript/TypeScript', 'Problem Solving']),
          status: getMilestoneStatus(calculateMilestoneProgress(skillsData.allSkills, ['JavaScript/TypeScript', 'Problem Solving'])),
          deadline: '2024-02-01',
          skills: ['JavaScript/TypeScript', 'Problem Solving']
        },
        {
          id: 2,
          title: 'Web Development',
          description: 'Build modern web applications',
          progress: calculateMilestoneProgress(skillsData.allSkills, ['React/Vue.js', 'Node.js', 'API Development']),
          status: getMilestoneStatus(calculateMilestoneProgress(skillsData.allSkills, ['React/Vue.js', 'Node.js', 'API Development'])),
          deadline: '2024-04-01',
          skills: ['React/Vue.js', 'Node.js', 'API Development']
        },
        {
          id: 3,
          title: 'Database & Backend',
          description: 'Handle data storage and server-side logic',
          progress: calculateMilestoneProgress(skillsData.allSkills, ['Database Design']),
          status: getMilestoneStatus(calculateMilestoneProgress(skillsData.allSkills, ['Database Design'])),
          deadline: '2024-05-01',
          skills: ['Database Design']
        },
        {
          id: 4,
          title: 'Professional Skills',
          description: 'Develop essential soft skills for career success',
          progress: calculateMilestoneProgress(skillsData.allSkills, ['Communication', 'Team Work', 'Time Management', 'Adaptability']),
          status: getMilestoneStatus(calculateMilestoneProgress(skillsData.allSkills, ['Communication', 'Team Work', 'Time Management', 'Adaptability'])),
          deadline: '2024-06-01',
          skills: ['Communication', 'Team Work', 'Time Management', 'Adaptability']
        }
      ];

      setProgressData({
        overallProgress,
        milestones
      });
    }
  }, [skillsData]);

  const calculateMilestoneProgress = (allSkills, requiredSkills) => {
    const relevantSkills = allSkills.filter(skill => requiredSkills.includes(skill.name));
    const achievedCount = relevantSkills.filter(skill => skill.status === 'Achieved').length;
    return Math.round((achievedCount / relevantSkills.length) * 100);
  };

  const getMilestoneStatus = (progress) => {
    if (progress === 100) return 'completed';
    if (progress > 0) return 'in_progress';
    return 'pending';
  };

  if (progressData === null) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24 flex items-center justify-center">
        <SharedHeader title="Mentora - Career Progress" />
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900 mx-auto mb-4"></div>
          <p style={{ color: M.text }}>Loading career progress...</p>
        </div>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader title="Mentora - Career Progress" />
      <main className="container mx-auto px-4 mt-6">
        {/* Progress Overview */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Progress Overview</h2>

          <div className="flex items-center justify-between mb-4">
            <span className="text-lg font-medium" style={{ color: M.text }}>Overall Progress</span>
            <span className="text-2xl font-bold" style={{ color: M.primary }}>{progressData.overallProgress}%</span>
          </div>

          <div className="w-full bg-gray-200 rounded-full h-4 mb-6">
            <div
              className="h-4 rounded-full transition-all duration-500"
              style={{ width: `${progressData.overallProgress}%`, background: M.primary }}
            ></div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="text-center">
              <div className="text-2xl font-bold mb-1" style={{ color: M.primary }}>
                {progressData.milestones.filter(m => m.status === 'completed').length}
              </div>
              <p className="text-sm" style={{ color: M.muted }}>Milestones Completed</p>
            </div>
            <div className="text-center">
              <div className="text-2xl font-bold mb-1" style={{ color: M.primary }}>
                {progressData.milestones.filter(m => m.status === 'in_progress').length}
              </div>
              <p className="text-sm" style={{ color: M.muted }}>In Progress</p>
            </div>
          </div>
        </div>

        {/* Career Milestones */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Milestones</h2>
          <div className="space-y-6">
            {progressData.milestones.map((milestone) => (
              <div key={milestone.id} className="border rounded-lg p-4" style={{ borderColor: M.bg3 }}>
                <div className="flex items-start justify-between mb-3">
                  <div className="flex-1">
                    <h3 className="text-lg font-semibold mb-1" style={{ color: M.text }}>{milestone.title}</h3>
                    <p className="text-sm mb-2" style={{ color: M.muted }}>{milestone.description}</p>
                    <div className="flex items-center gap-2 mb-2">
                      <Calendar className="w-4 h-4" style={{ color: M.muted }} />
                      <span className="text-xs" style={{ color: M.muted }}>Deadline: {new Date(milestone.deadline).toLocaleDateString()}</span>
                    </div>
                  </div>
                  <div className={`flex items-center gap-1 px-3 py-1 rounded-full text-xs font-medium ${getStatusColor(milestone.status)}`}>
                    {getStatusIcon(milestone.status)}
                    {milestone.status.replace('_', ' ').toUpperCase()}
                  </div>
                </div>

                <div className="mb-3">
                  <div className="flex justify-between text-sm mb-1">
                    <span style={{ color: M.text }}>Progress</span>
                    <span style={{ color: M.primary }}>{milestone.progress}%</span>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-2">
                    <div
                      className="h-2 rounded-full transition-all duration-500"
                      style={{ width: `${milestone.progress}%`, background: M.primary }}
                    ></div>
                  </div>
                </div>

                <div className="flex flex-wrap gap-2">
                  {milestone.skills.map((skill, index) => (
                    <span
                      key={index}
                      className="px-2 py-1 bg-blue-50 text-blue-700 rounded text-xs font-medium"
                    >
                      {skill}
                    </span>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center gap-4">
          <button
            onClick={() => navigate('/career-skills')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            View Skills Analysis
          </button>
          <button
            onClick={() => navigate('/career-plan')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.secondary }}
          >
            View Career Plan
          </button>
          <button
            onClick={() => navigate('/career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            Back to Career Builder
          </button>
        </div>
      </main>
    </div>
  );
}
