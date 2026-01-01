import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useProfile } from '../contexts/ProfileContext';
import { useAuth } from '../contexts/AuthContext';
import {
  Edit2, Save, Mail, Phone, MapPin, Award, Flame, Settings, 
  ChevronRight, LogOut, Camera, University, GraduationCap, 
  Clock, Linkedin, Github, Calendar
} from 'lucide-react';

export default function ProfilePage() {
  const navigate = useNavigate();
  const { user, logout } = useAuth();
  const { profile, loading, completion, updateProfile, getTimezones } = useProfile();
  
  const M = {
    primary: '#6B9080',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [isEditing, setIsEditing] = useState(false);
  const [profileEdit, setProfileEdit] = useState({
    bio: '',
    location: '',
    phoneNumber: '',
    dateOfBirth: '',
    university: '',
    major: '',
    expectedGraduationYear: new Date().getFullYear(),
    currentLevel: 'Freshman',
    timezone: 'UTC',
    linkedInUrl: '',
    gitHubUrl: '',
    avatarUrl: ''
  });
  const [timezones, setTimezones] = useState([]);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  // Study Level options per SRS 2.1.2
  const studyLevels = [
    { value: 'Freshman', label: 'Freshman (Year 1)' },
    { value: 'Sophomore', label: 'Sophomore (Year 2)' },
    { value: 'Junior', label: 'Junior (Year 3)' },
    { value: 'Senior', label: 'Senior (Year 4)' },
    { value: 'Graduate', label: 'Graduate' }
  ];

  useEffect(() => {
    if (profile) {
      setProfileEdit({
        bio: profile.bio || '',
        location: profile.location || '',
        phoneNumber: profile.phoneNumber || '',
        dateOfBirth: profile.dateOfBirth ? profile.dateOfBirth.split('T')[0] : '',
        university: profile.university || '',
        major: profile.major || '',
        expectedGraduationYear: profile.expectedGraduationYear || new Date().getFullYear(),
        currentLevel: profile.currentLevel || 'Freshman',
        timezone: profile.timezone || 'UTC',
        linkedInUrl: profile.linkedInUrl || '',
        gitHubUrl: profile.gitHubUrl || '',
        avatarUrl: profile.avatarUrl || ''
      });
    }
  }, [profile]);

  useEffect(() => {
    loadTimezones();
  }, [profileEdit.location]);

  const loadTimezones = async () => {
    const tzs = await getTimezones(profileEdit.location || null);
    setTimezones(tzs);
  };

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        setProfileEdit(prev => ({ ...prev, avatarUrl: e.target.result }));
      };
      reader.readAsDataURL(file);
    }
  };

  const saveProfile = async () => {
    setSaving(true);
    setError('');
    
    const result = await updateProfile(profileEdit);
    
    setSaving(false);
    
    if (result.success) {
      setIsEditing(false);
      alert('Profile updated successfully!');
    } else {
      setError(result.error || 'Failed to update profile');
      alert(result.error || 'Failed to update profile');
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (loading && !profile) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-[#6B9080]"></div>
      </div>
    );
  }

  const displayName = user ? `${user.firstName} ${user.lastName}` : 'User';
  const displayEmail = user?.email || '';
  const avatarUrl = profileEdit.avatarUrl || user?.avatarUrl || `https://api.dicebear.com/7.x/avataaars/svg?seed=${displayName}`;

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen p-6">
      {/* Profile Header */}
      <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
        <div className="flex flex-col md:flex-row items-center gap-6">
          <div className="relative">
            <img
              src={avatarUrl}
              alt="avatar"
              className="w-32 h-32 rounded-full border-4 shadow-lg object-cover"
              style={{ borderColor: M.primary }}
            />
            {isEditing && (
              <button
                onClick={() => document.getElementById('avatar-input').click()}
                className="absolute bottom-0 right-0 bg-white rounded-full p-2 shadow-lg border-2"
                style={{ borderColor: M.primary }}
              >
                <Camera className="w-4 h-4" style={{ color: M.primary }} />
              </button>
            )}
            <input
              id="avatar-input"
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              className="hidden"
            />
          </div>
          
          <div className="flex-1 text-center md:text-left">
            <h2 className="text-2xl font-bold text-[#2C3E3F]">{displayName}</h2>
            <p className="text-[#5A7A6B] mt-1">{displayEmail}</p>
            {profile && (
              <p className="text-[#5A7A6B] mt-1">{profileEdit.bio || 'No bio yet'}</p>
            )}
            
            <div className="flex flex-wrap gap-3 mt-3 justify-center md:justify-start">
              <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium">
                Profile {completion}% Complete
              </span>
              {profile?.yearsUntilGraduation !== undefined && (
                <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium">
                  {profile.yearsUntilGraduation} years to graduation
                </span>
              )}
            </div>
          </div>
          
          <div className="flex gap-2">
            {isEditing ? (
              <>
                <button 
                  onClick={saveProfile}
                  disabled={saving}
                  className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2 disabled:opacity-50"
                  style={{ background: M.primary }}
                >
                  {saving ? (
                    <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white"></div>
                  ) : (
                    <Save className="w-4 h-4" />
                  )}
                  {saving ? 'Saving...' : 'Save'}
                </button>
                <button 
                  onClick={() => setIsEditing(false)}
                  disabled={saving}
                  className="px-4 py-2 rounded-lg border font-medium hover:shadow-md transition-all disabled:opacity-50"
                  style={{ borderColor: M.bg3 }}
                >
                  Cancel
                </button>
              </>
            ) : (
              <button 
                onClick={() => setIsEditing(true)}
                className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
                style={{ background: M.primary }}
              >
                <Edit2 className="w-4 h-4" />
                Edit
              </button>
            )}
          </div>
        </div>

        {/* Profile Completion Bar */}
        <div className="mt-4">
          <div className="flex items-center justify-between mb-2">
            <span className="text-sm text-[#5A7A6B]">Profile Completion</span>
            <span className="text-sm font-semibold" style={{ color: M.primary }}>{completion}%</span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-2">
            <div
              className="h-2 rounded-full transition-all duration-300"
              style={{ width: `${completion}%`, background: M.primary }}
            ></div>
          </div>
        </div>
      </div>

      {/* Contact & Personal Info */}
      <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
        <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
          <Mail className="w-5 h-5" style={{ color: M.primary }} />
          Contact Information
        </h3>
        {isEditing ? (
          <div className="space-y-3">
            <div className="flex items-center gap-3">
              <Mail className="w-5 h-5 text-[#5A7A6B]" />
              <input 
                value={displayEmail}
                disabled
                className="flex-1 px-4 py-2 rounded-lg border bg-gray-50" 
                style={{ borderColor: M.bg3 }}
              />
            </div>
            <div className="flex items-center gap-3">
              <Phone className="w-5 h-5 text-[#5A7A6B]" />
              <input 
                value={profileEdit.phoneNumber} 
                onChange={e => setProfileEdit(p => ({ ...p, phoneNumber: e.target.value }))}
                className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                style={{ borderColor: M.bg3 }}
                placeholder="+962 79 123 4567"
              />
            </div>
            <div className="flex items-center gap-3">
              <MapPin className="w-5 h-5 text-[#5A7A6B]" />
              <input 
                value={profileEdit.location} 
                onChange={e => setProfileEdit(p => ({ ...p, location: e.target.value }))}
                className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                style={{ borderColor: M.bg3 }}
                placeholder="Amman, Jordan"
              />
            </div>
            <div className="flex items-center gap-3">
              <Calendar className="w-5 h-5 text-[#5A7A6B]" />
              <input 
                type="date"
                value={profileEdit.dateOfBirth} 
                onChange={e => setProfileEdit(p => ({ ...p, dateOfBirth: e.target.value }))}
                className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                style={{ borderColor: M.bg3 }}
              />
            </div>
          </div>
        ) : (
          <div className="space-y-3">
            <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
              <Mail className="w-5 h-5 text-[#5A7A6B]" />
              <div>
                <p className="text-xs text-[#5A7A6B]">Email</p>
                <p className="font-medium text-[#2C3E3F]">{displayEmail}</p>
              </div>
            </div>
            {profile?.phoneNumber && (
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <Phone className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Phone</p>
                  <p className="font-medium text-[#2C3E3F]">{profile.phoneNumber}</p>
                </div>
              </div>
            )}
            {profile?.location && (
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <MapPin className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Location</p>
                  <p className="font-medium text-[#2C3E3F]">{profile.location}</p>
                </div>
              </div>
            )}
            {profile?.age && (
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <Calendar className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Age</p>
                  <p className="font-medium text-[#2C3E3F]">{profile.age} years old</p>
                </div>
              </div>
            )}
          </div>
        )}
      </div>

      {/* Academic Information (SRS 2.1.1 & 2.1.2) */}
      <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
        <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
          <University className="w-5 h-5" style={{ color: M.primary }} />
          Academic Information
        </h3>
        {isEditing ? (
          <div className="space-y-3">
            <div>
              <label className="text-sm text-[#5A7A6B] mb-1 block">
                University <span className="text-red-500">*</span>
              </label>
              <input 
                value={profileEdit.university} 
                onChange={e => setProfileEdit(p => ({ ...p, university: e.target.value }))}
                required
                className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                style={{ borderColor: M.bg3 }}
                placeholder="University of Jordan"
              />
            </div>
            <div>
              <label className="text-sm text-[#5A7A6B] mb-1 block">
                Major <span className="text-red-500">*</span>
              </label>
              <input 
                value={profileEdit.major} 
                onChange={e => setProfileEdit(p => ({ ...p, major: e.target.value }))}
                required
                className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                style={{ borderColor: M.bg3 }}
                placeholder="Computer Science"
              />
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div>
                <label className="text-sm text-[#5A7A6B] mb-1 block">
                  Graduation Year <span className="text-red-500">*</span>
                </label>
                <input 
                  type="number"
                  value={profileEdit.expectedGraduationYear} 
                  onChange={e => setProfileEdit(p => ({ ...p, expectedGraduationYear: parseInt(e.target.value) }))}
                  required
                  min="2024"
                  max="2050"
                  className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                />
              </div>
              <div>
                <label className="text-sm text-[#5A7A6B] mb-1 block">
                  Study Level <span className="text-red-500">*</span>
                </label>
                <select
                  value={profileEdit.currentLevel}
                  onChange={e => setProfileEdit(p => ({ ...p, currentLevel: e.target.value }))}
                  required
                  className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                  style={{ borderColor: M.bg3 }}
                >
                  {studyLevels.map(level => (
                    <option key={level.value} value={level.value}>{level.label}</option>
                  ))}
                </select>
              </div>
            </div>
          </div>
        ) : profile ? (
          <div className="space-y-3">
            <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
              <University className="w-5 h-5 text-[#5A7A6B]" />
              <div>
                <p className="text-xs text-[#5A7A6B]">University</p>
                <p className="font-medium text-[#2C3E3F]">{profile.university}</p>
              </div>
            </div>
            <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
              <Award className="w-5 h-5 text-[#5A7A6B]" />
              <div>
                <p className="text-xs text-[#5A7A6B]">Major</p>
                <p className="font-medium text-[#2C3E3F]">{profile.major}</p>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <GraduationCap className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Graduation</p>
                  <p className="font-medium text-[#2C3E3F]">{profile.expectedGraduationYear}</p>
                </div>
              </div>
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <Award className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Level</p>
                  <p className="font-medium text-[#2C3E3F]">{profile.currentLevelName}</p>
                </div>
              </div>
            </div>
          </div>
        ) : (
          <p className="text-[#5A7A6B]">Please complete your academic profile</p>
        )}
      </div>

      {/* System Configuration (SRS 2.2.1) */}
      <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
        <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
          <Clock className="w-5 h-5" style={{ color: M.primary }} />
          System Configuration
        </h3>
        {isEditing ? (
          <div>
            <label className="text-sm text-[#5A7A6B] mb-1 block">
              Timezone (IANA Format) <span className="text-red-500">*</span>
            </label>
            <select
              value={profileEdit.timezone}
              onChange={e => setProfileEdit(p => ({ ...p, timezone: e.target.value }))}
              required
              className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
              style={{ borderColor: M.bg3 }}
            >
              {timezones.map(tz => (
                <option key={tz} value={tz}>{tz}</option>
              ))}
            </select>
            <p className="mt-1 text-xs text-[#5A7A6B]">
              Used for scheduling tasks and notifications in your local time
            </p>
          </div>
        ) : profile ? (
          <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
            <Clock className="w-5 h-5 text-[#5A7A6B]" />
            <div>
              <p className="text-xs text-[#5A7A6B]">Timezone</p>
              <p className="font-medium text-[#2C3E3F]">{profile.timezone}</p>
            </div>
          </div>
        ) : (
          <p className="text-[#5A7A6B]">No timezone set</p>
        )}
      </div>

      {/* Social Links */}
      {(isEditing || (profile?.linkedInUrl || profile?.gitHubUrl)) && (
        <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] mb-4">Social Links</h3>
          {isEditing ? (
            <div className="space-y-3">
              <div className="flex items-center gap-3">
                <Linkedin className="w-5 h-5 text-blue-600" />
                <input 
                  type="url"
                  value={profileEdit.linkedInUrl} 
                  onChange={e => setProfileEdit(p => ({ ...p, linkedInUrl: e.target.value }))}
                  className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                  placeholder="https://linkedin.com/in/username"
                />
              </div>
              <div className="flex items-center gap-3">
                <Github className="w-5 h-5" />
                <input 
                  type="url"
                  value={profileEdit.gitHubUrl} 
                  onChange={e => setProfileEdit(p => ({ ...p, gitHubUrl: e.target.value }))}
                  className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                  placeholder="https://github.com/username"
                />
              </div>
            </div>
          ) : (
            <div className="space-y-3">
              {profile?.linkedInUrl && (
                <a 
                  href={profile.linkedInUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors"
                >
                  <Linkedin className="w-5 h-5 text-blue-600" />
                  <span className="font-medium text-[#2C3E3F]">LinkedIn Profile</span>
                  <ChevronRight className="w-4 h-4 ml-auto" />
                </a>
              )}
              {profile?.gitHubUrl && (
                <a 
                  href={profile.gitHubUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors"
                >
                  <Github className="w-5 h-5" />
                  <span className="font-medium text-[#2C3E3F]">GitHub Profile</span>
                  <ChevronRight className="w-4 h-4 ml-auto" />
                </a>
              )}
            </div>
          )}
        </div>
      )}

      {/* Account Actions */}
      <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
        <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
          <Settings className="w-5 h-5" style={{ color: M.primary }} />
          Account Settings
        </h3>
        <button
          onClick={() => navigate('/study-planner')}
          className="w-full flex items-center justify-between p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors text-[#2C3E3F] mb-2"
        >
          <div className="flex items-center gap-3">
            <Award className="w-5 h-5" style={{ color: M.primary }} />
            <span className="font-medium">Study Planner</span>
          </div>
          <ChevronRight className="w-5 h-5" />
        </button>
        <button
          onClick={handleLogout}
          className="w-full flex items-center justify-between p-3 rounded-lg hover:bg-red-50 transition-colors text-red-500"
        >
          <div className="flex items-center gap-3">
            <LogOut className="w-5 h-5" />
            <span className="font-medium">Logout</span>
          </div>
          <ChevronRight className="w-5 h-5" />
        </button>
      </div>
    </div>
  );
}