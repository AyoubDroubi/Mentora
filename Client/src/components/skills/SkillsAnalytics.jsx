import { useState, useEffect } from 'react';
import skillsService from '../../services/skillsService';
import {
  ChartBarIcon,
  AcademicCapIcon,
  TrophyIcon,
  ClockIcon,
  CheckCircleIcon,
  ExclamationCircleIcon,
  LightBulbIcon,
  CodeBracketIcon,
  ChatBubbleLeftRightIcon,
  BriefcaseIcon,
  SparklesIcon,
  ArrowTrendingUpIcon
} from '@heroicons/react/24/outline';

const SkillsAnalytics = () => {
  // Theme colors
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [summary, setSummary] = useState(null);
  const [distribution, setDistribution] = useState(null);
  const [coverage, setCoverage] = useState(null);
  const [timeline, setTimeline] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadAnalytics();
  }, []);

  const loadAnalytics = async () => {
    try {
      setLoading(true);
      const [summaryRes, distributionRes, coverageRes, timelineRes] = await Promise.all([
        skillsService.getSummary(),
        skillsService.getDistribution(),
        skillsService.getCoverage(),
        skillsService.getTimeline()
      ]);

      setSummary(summaryRes.data);
      setDistribution(distributionRes.data);
      setCoverage(coverageRes.data);
      setTimeline(timelineRes.data);
    } catch (error) {
      console.error('Error loading analytics:', error);
    } finally {
      setLoading(false);
    }
  };

  const getCategoryIcon = (category) => {
    const icons = {
      Technical: CodeBracketIcon,
      Soft: ChatBubbleLeftRightIcon,
      Business: BriefcaseIcon,
      Creative: SparklesIcon
    };
    const Icon = icons[category] || CodeBracketIcon;
    return <Icon className="w-5 h-5" style={{ color: M.muted }} />;
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2" style={{ borderColor: M.primary }}></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white rounded-3xl p-6 border shadow-lg" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <ChartBarIcon className="w-8 h-8" style={{ color: M.primary }} />
            <span className="text-3xl font-bold" style={{ color: M.text }}>{summary?.totalSkills || 0}</span>
          </div>
          <h3 className="font-semibold" style={{ color: M.text }}>Total Skills</h3>
          <p className="text-sm mt-1" style={{ color: M.muted }}>
            {100 - (summary?.totalSkills || 0)} more available
          </p>
        </div>

        <div className="bg-white rounded-3xl p-6 border shadow-lg" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <AcademicCapIcon className="w-8 h-8" style={{ color: M.secondary }} />
            <span className="text-3xl font-bold" style={{ color: M.text }}>{summary?.featuredSkillsCount || 0}</span>
          </div>
          <h3 className="font-semibold" style={{ color: M.text }}>Featured Skills</h3>
          <p className="text-sm mt-1" style={{ color: M.muted }}>
            {10 - (summary?.featuredSkillsCount || 0)} spots remaining
          </p>
        </div>

        <div className="bg-white rounded-3xl p-6 border shadow-lg" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <ClockIcon className="w-8 h-8" style={{ color: M.primary }} />
            <span className="text-3xl font-bold" style={{ color: M.text }}>{summary?.totalExperienceYears || 0}</span>
          </div>
          <h3 className="font-semibold" style={{ color: M.text }}>Years Experience</h3>
          <p className="text-sm mt-1" style={{ color: M.muted }}>
            Total across all skills
          </p>
        </div>

        <div className="bg-white rounded-3xl p-6 border shadow-lg" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <TrophyIcon className="w-8 h-8" style={{ color: M.secondary }} />
            <span className="text-3xl font-bold" style={{ color: M.text }}>{coverage?.coverageScore || 0}</span>
          </div>
          <h3 className="font-semibold" style={{ color: M.text }}>Coverage Score</h3>
          <p className="text-sm mt-1" style={{ color: M.muted }}>
            Out of 100
          </p>
        </div>
      </div>

      {/* Charts Row */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Proficiency Distribution */}
        <div className="bg-white rounded-3xl shadow-lg p-6 border" style={{ borderColor: M.bg3 }}>
          <h3 className="text-lg font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
            <ArrowTrendingUpIcon className="w-5 h-5" />
            Proficiency Distribution
          </h3>
          <div className="space-y-4">
            {Object.entries(distribution?.byProficiency || {}).map(([level, percentage]) => (
              <div key={level}>
                <div className="flex items-center justify-between mb-2">
                  <span className="text-sm font-medium" style={{ color: M.text }}>{level}</span>
                  <span className="text-sm" style={{ color: M.muted }}>{percentage.toFixed(1)}%</span>
                </div>
                <div className="w-full bg-gray-200 rounded-full h-3">
                  <div
                    className="h-3 rounded-full transition-all"
                    style={{
                      background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})`,
                      width: `${percentage}%`
                    }}
                  ></div>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Category Breakdown */}
        <div className="bg-white rounded-3xl shadow-lg p-6 border" style={{ borderColor: M.bg3 }}>
          <h3 className="text-lg font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
            <ChartBarIcon className="w-5 h-5" />
            Category Breakdown
          </h3>
          <div className="space-y-4">
            {Object.entries(summary?.categoryBreakdown || {}).map(([category, count]) => {
              const total = summary?.totalSkills || 1;
              const percentage = (count / total) * 100;
              return (
                <div key={category}>
                  <div className="flex items-center justify-between mb-2">
                    <span className="text-sm font-medium flex items-center gap-2" style={{ color: M.text }}>
                      {getCategoryIcon(category)}
                      {category}
                    </span>
                    <span className="text-sm" style={{ color: M.muted }}>{count} skills</span>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-3">
                    <div
                      className="h-3 rounded-full transition-all"
                      style={{
                        background: `linear-gradient(90deg, ${M.secondary}, ${M.primary})`,
                        width: `${percentage}%`
                      }}
                    ></div>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>

      {/* Coverage Analysis */}
      {coverage && (
        <div className="bg-white rounded-3xl shadow-lg p-6 border" style={{ borderColor: M.bg3 }}>
          <h3 className="text-lg font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
            <TrophyIcon className="w-5 h-5" />
            Skills Coverage Analysis
          </h3>
          
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* Strong Categories */}
            <div>
              <div className="flex items-center gap-2 mb-3">
                <CheckCircleIcon className="w-6 h-6 text-green-600" />
                <h4 className="font-medium" style={{ color: M.text }}>Strong Areas</h4>
              </div>
              <div className="space-y-2">
                {coverage.strongCategories.length > 0 ? (
                  coverage.strongCategories.map((cat) => (
                    <div key={cat} className="px-3 py-2 rounded-lg text-sm flex items-center gap-2" style={{ background: M.bg3, color: M.text }}>
                      {getCategoryIcon(cat)}
                      {cat}
                    </div>
                  ))
                ) : (
                  <p className="text-sm italic" style={{ color: M.muted }}>No strong areas yet</p>
                )}
              </div>
            </div>

            {/* Weak Categories */}
            <div>
              <div className="flex items-center gap-2 mb-3">
                <ExclamationCircleIcon className="w-6 h-6 text-yellow-600" />
                <h4 className="font-medium" style={{ color: M.text }}>Areas to Improve</h4>
              </div>
              <div className="space-y-2">
                {coverage.weakCategories.length > 0 ? (
                  coverage.weakCategories.map((cat) => (
                    <div key={cat} className="px-3 py-2 bg-yellow-50 text-yellow-800 rounded-lg text-sm flex items-center gap-2">
                      {getCategoryIcon(cat)}
                      {cat}
                    </div>
                  ))
                ) : (
                  <p className="text-sm italic" style={{ color: M.muted }}>No weak areas</p>
                )}
              </div>
            </div>

            {/* Recommendations */}
            <div>
              <div className="flex items-center gap-2 mb-3">
                <LightBulbIcon className="w-6 h-6 text-blue-600" />
                <h4 className="font-medium" style={{ color: M.text }}>Recommendations</h4>
              </div>
              <div className="space-y-2">
                {coverage.recommendations.length > 0 ? (
                  coverage.recommendations.map((rec, index) => (
                    <div key={index} className="px-3 py-2 rounded-lg text-sm" style={{ background: M.bg1, color: M.text, border: `1px solid ${M.bg3}` }}>
                      {rec}
                    </div>
                  ))
                ) : (
                  <p className="text-sm italic" style={{ color: M.muted }}>You're doing great!</p>
                )}
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Timeline */}
      {timeline?.timeline && timeline.timeline.length > 0 && (
        <div className="bg-white rounded-3xl shadow-lg p-6 border" style={{ borderColor: M.bg3 }}>
          <h3 className="text-lg font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
            <ClockIcon className="w-5 h-5" />
            Skills Learning Timeline
          </h3>
          <div className="space-y-6">
            {timeline.timeline.map((year) => (
              <div key={year.year} className="relative pl-6" style={{ borderLeft: `2px solid ${M.secondary}` }}>
                <div className="absolute -left-2 top-0 w-4 h-4 rounded-full" style={{ background: M.primary }}></div>
                <h4 className="font-semibold mb-3" style={{ color: M.text }}>{year.year}</h4>
                <div className="space-y-3">
                  {year.months.map((month) => (
                    <div key={month.month} className="rounded-lg p-3" style={{ background: M.bg1 }}>
                      <div className="text-sm font-medium mb-2" style={{ color: M.muted }}>
                        {new Date(2000, month.month - 1).toLocaleString('default', { month: 'long' })}
                      </div>
                      <div className="flex flex-wrap gap-2">
                        {month.skills.map((skill, idx) => (
                          <span
                            key={idx}
                            className="px-3 py-1 bg-white border rounded-full text-sm flex items-center gap-2"
                            style={{ borderColor: M.bg3 }}
                          >
                            <AcademicCapIcon className="w-4 h-4" />
                            {skill.name} - {skill.proficiency}
                          </span>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default SkillsAnalytics;
