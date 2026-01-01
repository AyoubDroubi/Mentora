import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useProfile } from '../contexts/ProfileContext';
import { useAuth } from '../contexts/AuthContext';
import {
  Edit2, Save, X, Mail, Phone, MapPin, Calendar, University, 
  GraduationCap, Clock, Linkedin, Github, Camera, LogOut, 
  AlertCircle, CheckCircle, User, ArrowLeft, Home
} from 'lucide-react';

const STUDY_LEVELS = [
  { value: 'Freshman', label: 'Freshman (Year 1)' },
  { value: 'Sophomore', label: 'Sophomore (Year 2)' },
  { value: 'Junior', label: 'Junior (Year 3)' },
  { value: 'Senior', label: 'Senior (Year 4)' },
  { value: 'Graduate', label: 'Graduate' }
];

export default function ProfilePage() {
  const navigate = useNavigate();
  const { user, logout } = useAuth();
  const { profile, loading, completion, updateProfile, getTimezones } = useProfile();
  
  const [isEditing, setIsEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [timezones, setTimezones] = useState(['UTC']);
  const [uploadingAvatar, setUploadingAvatar] = useState(false);
  
  // Form state
  const [formData, setFormData] = useState({
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

  // Load profile data into form
  useEffect(() => {
    if (profile) {
      setFormData({
        bio: profile.bio || '',
        location: profile.location || '',
        phoneNumber: profile.phoneNumber || '',
        dateOfBirth: profile.dateOfBirth ? profile.dateOfBirth.split('T')[0] : '',
        university: profile.university || '',
        major: profile.major || '',
        expectedGraduationYear: profile.expectedGraduationYear || new Date().getFullYear(),
        currentLevel: profile.currentLevelName || 'Freshman',
        timezone: profile.timezone || 'UTC',
        linkedInUrl: profile.linkedInUrl || '',
        gitHubUrl: profile.gitHubUrl || '',
        avatarUrl: profile.avatarUrl || ''
      });
    }
  }, [profile]);

  // Load timezones when location changes
  useEffect(() => {
    loadTimezones();
  }, [formData.location]);

  const loadTimezones = async () => {
    const tzs = await getTimezones(formData.location || null);
    setTimezones(tzs);
  };

  const handleChange = (field, value) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setError('');
  };

  const validateForm = () => {
    const errors = [];

    if (!formData.university?.trim()) {
      errors.push('University is required');
    }
    if (!formData.major?.trim()) {
      errors.push('Major is required');
    }
    if (formData.expectedGraduationYear < 2024 || formData.expectedGraduationYear > 2050) {
      errors.push('Graduation year must be between 2024 and 2050');
    }
    if (!formData.currentLevel) {
      errors.push('Study level is required');
    }
    if (!formData.timezone?.trim()) {
      errors.push('Timezone is required');
    }

    // Validate URLs if provided
    const urlPattern = /^https?:\/\/.+/;
    if (formData.linkedInUrl && !urlPattern.test(formData.linkedInUrl)) {
      errors.push('LinkedIn URL must start with http:// or https://');
    }
    if (formData.gitHubUrl && !urlPattern.test(formData.gitHubUrl)) {
      errors.push('GitHub URL must start with http:// or https://');
    }

    // Validate phone if provided
    if (formData.phoneNumber && !formData.phoneNumber.match(/^\+?[1-9]\d{1,14}$/)) {
      errors.push('Phone number must be in international format (e.g., +962791234567)');
    }

    return errors;
  };

  const handleSave = async () => {
    setError('');
    setSuccess('');

    // Validate
    const validationErrors = validateForm();
    if (validationErrors.length > 0) {
      setError(validationErrors.join('\n'));
      return;
    }

    setSaving(true);

    const result = await updateProfile(formData);

    setSaving(false);

    if (result.success) {
      setSuccess('Profile updated successfully!');
      setIsEditing(false);
      setTimeout(() => setSuccess(''), 3000);
    } else {
      setError(result.error || 'Failed to update profile');
    }
  };

  const handleCancel = () => {
    setIsEditing(false);
    setError('');
    setSuccess('');
    // Reload profile data
    if (profile) {
      setFormData({
        bio: profile.bio || '',
        location: profile.location || '',
        phoneNumber: profile.phoneNumber || '',
        dateOfBirth: profile.dateOfBirth ? profile.dateOfBirth.split('T')[0] : '',
        university: profile.university || '',
        major: profile.major || '',
        expectedGraduationYear: profile.expectedGraduationYear || new Date().getFullYear(),
        currentLevel: profile.currentLevelName || 'Freshman',
        timezone: profile.timezone || 'UTC',
        linkedInUrl: profile.linkedInUrl || '',
        gitHubUrl: profile.gitHubUrl || '',
        avatarUrl: profile.avatarUrl || ''
      });
    }
  };

  const handleAvatarChange = (event) => {
    const file = event.target.files?.[0];
    if (!file) return;

    // Validate file type
    if (!file.type.startsWith('image/')) {
      setError('Please select an image file');
      return;
    }

    // Validate file size (max 2MB)
    if (file.size > 2 * 1024 * 1024) {
      setError('Image size must be less than 2MB');
      return;
    }

    setUploadingAvatar(true);
    setError('');

    const reader = new FileReader();
    reader.onload = (e) => {
      const result = e.target?.result;
      if (typeof result === 'string') {
        handleChange('avatarUrl', result);
        setUploadingAvatar(false);
        setSuccess('Avatar updated! Remember to click Save to apply changes.');
        setTimeout(() => setSuccess(''), 3000);
      }
    };
    reader.onerror = () => {
      setError('Failed to read image file');
      setUploadingAvatar(false);
    };
    reader.readAsDataURL(file);
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
  
  // Fix avatar URL - use formData.avatarUrl or generate from user info
  const avatarUrl = formData.avatarUrl || 
                    user?.avatarUrl || 
                    `https://api.dicebear.com/7.x/avataaars/svg?seed=${encodeURIComponent(displayName)}`;
  
  // Fix graduation display - don't show if already graduated (0 or negative years)
  const showGraduationYears = profile?.yearsUntilGraduation !== undefined && 
                               profile.yearsUntilGraduation > 0;

  return (
    <div className="min-h-screen bg-gradient-to-br from-[#F6FFF8] via-[#EAF4F4] to-[#E8F3E8] p-6">
      <div className="max-w-4xl mx-auto space-y-6">
        {/* Back to Dashboard Button */}
        <button
          onClick={() => navigate('/dashboard')}
          className="flex items-center gap-2 text-[#6B9080] hover:text-[#577466] transition-colors group"
        >
          <ArrowLeft className="w-5 h-5 group-hover:-translate-x-1 transition-transform" />
          <span className="font-medium">Back to Dashboard</span>
        </button>

        {/* Success Message */}
        {success && (
          <div className="bg-green-50 border border-green-200 rounded-lg p-4 flex items-center gap-3 text-green-800">
            <CheckCircle className="w-5 h-5 flex-shrink-0" />
            <p className="font-medium">{success}</p>
          </div>
        )}

        {/* Error Message */}
        {error && (
          <div className="bg-red-50 border border-red-200 rounded-lg p-4 text-red-800">
            <div className="flex items-start gap-3">
              <AlertCircle className="w-5 h-5 flex-shrink-0 mt-0.5" />
              <div>
                <p className="font-semibold mb-1">Please fix the following errors:</p>
                <pre className="text-sm whitespace-pre-wrap">{error}</pre>
              </div>
            </div>
          </div>
        )}

        {/* Profile Header */}
        <div className="bg-white rounded-2xl shadow-lg border border-[#A4C3B2] p-6">
          <div className="flex flex-col md:flex-row items-center gap-6">
            <div className="relative">
              <img
                src={avatarUrl}
                alt="avatar"
                className="w-32 h-32 rounded-full border-4 border-[#6B9080] shadow-lg object-cover"
                onError={(e) => {
                  // Fallback if image fails to load
                  e.currentTarget.src = `https://api.dicebear.com/7.x/avataaars/svg?seed=${encodeURIComponent(displayName)}`;
                }}
              />
              {isEditing && (
                <>
                  <label 
                    htmlFor="avatar-upload"
                    className="absolute bottom-0 right-0 bg-white rounded-full p-2 shadow-lg border-2 border-[#6B9080] cursor-pointer hover:bg-[#F6FFF8] transition-colors"
                  >
                    {uploadingAvatar ? (
                      <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-[#6B9080]"></div>
                    ) : (
                      <Camera className="w-4 h-4 text-[#6B9080]" />
                    )}
                  </label>
                  <input
                    id="avatar-upload"
                    type="file"
                    accept="image/*"
                    onChange={handleAvatarChange}
                    className="hidden"
                    disabled={uploadingAvatar}
                  />
                  {uploadingAvatar && (
                    <div className="absolute inset-0 bg-black bg-opacity-30 rounded-full flex items-center justify-center">
                      <div className="text-white text-xs">Uploading...</div>
                    </div>
                  )}
                </>
              )}
            </div>
            
            <div className="flex-1 text-center md:text-left">
              <h2 className="text-2xl font-bold text-[#2C3E3F]">{user ? `${user.firstName} ${user.lastName}` : 'User'}</h2>
              <p className="text-[#5A7A6B] mt-1">{user?.email || ''}</p>
              
              {/* Bio Section - More prominent */}
              <div className="mt-4">
                {isEditing ? (
                  <div className="space-y-2">
                    <label className="text-sm font-medium text-[#6B9080] flex items-center gap-2">
                      <Edit2 className="w-4 h-4" />
                      About Me
                    </label>
                    <textarea
                      value={formData.bio}
                      onChange={(e) => handleChange('bio', e.target.value)}
                      placeholder="Tell us about yourself... (e.g., Computer Science student passionate about AI and web development)"
                      maxLength={500}
                      className="w-full p-3 rounded-lg border-2 border-[#A4C3B2] focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none resize-none transition-all"
                      rows={4}
                    />
                    <div className="flex justify-between items-center">
                      <span className="text-xs text-[#5A7A6B]">
                        {formData.bio?.length || 0}/500 characters
                      </span>
                      <span className="text-xs text-[#5A7A6B] italic">
                        ?? Tip: Describe your studies, interests, and goals
                      </span>
                    </div>
                  </div>
                ) : (
                  <div className="bg-[#F6FFF8] rounded-lg p-4 border border-[#E8F3E8]">
                    {formData.bio ? (
                      <>
                        <p className="text-sm font-medium text-[#6B9080] mb-2 flex items-center gap-2">
                          <User className="w-4 h-4" />
                          About Me
                        </p>
                        <p className="text-[#2C3E3F] leading-relaxed">{formData.bio}</p>
                      </>
                    ) : (
                      <div className="text-center py-4">
                        <p className="text-[#5A7A6B] italic mb-2">No bio yet</p>
                        <p className="text-xs text-[#5A7A6B]">
                          Click "Edit Profile" to add your bio
                        </p>
                      </div>
                    )}
                  </div>
                )}
              </div>
              
              <div className="flex flex-wrap gap-2 mt-4 justify-center md:justify-start">
                <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium text-[#6B9080]">
                  {completion}% Complete
                </span>
                {showGraduationYears && (
                  <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium text-[#6B9080]">
                    {profile.yearsUntilGraduation} {profile.yearsUntilGraduation === 1 ? 'year' : 'years'} to graduation
                  </span>
                )}
                {profile?.yearsUntilGraduation !== undefined && profile.yearsUntilGraduation <= 0 && (
                  <span className="px-3 py-1 rounded-full bg-green-100 text-sm font-medium text-green-700">
                    Graduated
                  </span>
                )}
              </div>
            </div>
            
            <div className="flex gap-2">
              {isEditing ? (
                <>
                  <button
                    onClick={handleSave}
                    disabled={saving}
                    className="px-4 py-2 bg-[#6B9080] text-white rounded-lg font-medium hover:bg-[#577466] transition flex items-center gap-2 disabled:opacity-50"
                  >
                    {saving ? (
                      <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white"></div>
                    ) : (
                      <Save className="w-4 h-4" />
                    )}
                    {saving ? 'Saving...' : 'Save'}
                  </button>
                  <button
                    onClick={handleCancel}
                    disabled={saving}
                    className="px-4 py-2 border border-[#A4C3B2] rounded-lg font-medium hover:bg-gray-50 transition flex items-center gap-2 disabled:opacity-50"
                  >
                    <X className="w-4 h-4" />
                    Cancel
                  </button>
                </>
              ) : (
                <>
                  <button
                    onClick={() => navigate('/dashboard')}
                    className="px-4 py-2 border border-[#A4C3B2] text-[#6B9080] rounded-lg font-medium hover:bg-[#F6FFF8] transition flex items-center gap-2"
                    title="Back to Dashboard"
                  >
                    <Home className="w-4 h-4" />
                    <span className="hidden md:inline">Dashboard</span>
                  </button>
                  <button
                    onClick={() => setIsEditing(true)}
                    className="px-4 py-2 bg-[#6B9080] text-white rounded-lg font-medium hover:bg-[#577466] transition flex items-center gap-2"
                  >
                    <Edit2 className="w-4 h-4" />
                    Edit Profile
                  </button>
                </>
              )}
            </div>
          </div>

          {/* Completion Bar */}
          <div className="mt-6">
            <div className="flex items-center justify-between mb-2">
              <span className="text-sm text-[#5A7A6B]">Profile Completion</span>
              <span className="text-sm font-semibold text-[#6B9080]">{completion}%</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2">
              <div
                className="bg-[#6B9080] h-2 rounded-full transition-all duration-300"
                style={{ width: `${completion}%` }}
              ></div>
            </div>
          </div>
        </div>

        {/* Contact Information */}
        <FormSection
          title="Contact Information"
          icon={<Mail className="w-5 h-5 text-[#6B9080]" />}
        >
          <FormField
            icon={<Phone className="w-5 h-5" />}
            label="Phone Number"
            value={formData.phoneNumber}
            onChange={(e) => handleChange('phoneNumber', e.target.value)}
            placeholder="+962791234567"
            disabled={!isEditing}
          />
          <FormField
            icon={<MapPin className="w-5 h-5" />}
            label="Location"
            value={formData.location}
            onChange={(e) => handleChange('location', e.target.value)}
            placeholder="Amman, Jordan"
            disabled={!isEditing}
          />
          <FormField
            icon={<Calendar className="w-5 h-5" />}
            label="Date of Birth"
            type="date"
            value={formData.dateOfBirth}
            onChange={(e) => handleChange('dateOfBirth', e.target.value)}
            disabled={!isEditing}
          />
        </FormSection>

        {/* Academic Information */}
        <FormSection
          title="Academic Information"
          icon={<University className="w-5 h-5 text-[#6B9080]" />}
        >
          <FormField
            icon={<University className="w-5 h-5" />}
            label="University"
            value={formData.university}
            onChange={(e) => handleChange('university', e.target.value)}
            placeholder="University of Jordan"
            disabled={!isEditing}
            required
          />
          <FormField
            icon={<GraduationCap className="w-5 h-5" />}
            label="Major"
            value={formData.major}
            onChange={(e) => handleChange('major', e.target.value)}
            placeholder="Computer Science"
            disabled={!isEditing}
            required
          />
          <div className="grid grid-cols-2 gap-4">
            <FormField
              label="Graduation Year"
              type="number"
              value={formData.expectedGraduationYear}
              onChange={(e) => handleChange('expectedGraduationYear', parseInt(e.target.value))}
              min="2024"
              max="2050"
              disabled={!isEditing}
              required
            />
            <FormField
              label="Study Level"
              type="select"
              value={formData.currentLevel}
              onChange={(e) => handleChange('currentLevel', e.target.value)}
              options={STUDY_LEVELS}
              disabled={!isEditing}
              required
            />
          </div>
        </FormSection>

        {/* System Configuration */}
        <FormSection
          title="System Configuration"
          icon={<Clock className="w-5 h-5 text-[#6B9080]" />}
        >
          <FormField
            icon={<Clock className="w-5 h-5" />}
            label="Timezone"
            type="select"
            value={formData.timezone}
            onChange={(e) => handleChange('timezone', e.target.value)}
            options={timezones.map(tz => ({ value: tz, label: tz }))}
            disabled={!isEditing}
            required
          />
          <p className="text-xs text-[#5A7A6B] mt-1">
            Used for scheduling tasks and notifications in your local time
          </p>
        </FormSection>

        {/* Social Links */}
        <FormSection
          title="Social Links"
          icon={<Linkedin className="w-5 h-5 text-[#6B9080]" />}
        >
          <FormField
            icon={<Linkedin className="w-5 h-5 text-blue-600" />}
            label="LinkedIn"
            value={formData.linkedInUrl}
            onChange={(e) => handleChange('linkedInUrl', e.target.value)}
            placeholder="https://linkedin.com/in/username"
            disabled={!isEditing}
          />
          <FormField
            icon={<Github className="w-5 h-5" />}
            label="GitHub"
            value={formData.gitHubUrl}
            onChange={(e) => handleChange('gitHubUrl', e.target.value)}
            placeholder="https://github.com/username"
            disabled={!isEditing}
          />
        </FormSection>

        {/* Logout */}
        <div className="bg-white rounded-2xl shadow-lg border border-[#A4C3B2] p-6">
          <button
            onClick={handleLogout}
            className="w-full flex items-center justify-between p-3 rounded-lg hover:bg-red-50 transition text-red-500"
          >
            <div className="flex items-center gap-3">
              <LogOut className="w-5 h-5" />
              <span className="font-medium">Logout</span>
            </div>
          </button>
        </div>
      </div>
    </div>
  );
}

// Helper Components
function FormSection({ title, icon, children }) {
  return (
    <div className="bg-white rounded-2xl shadow-lg border border-[#A4C3B2] p-6">
      <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
        {icon}
        {title}
      </h3>
      <div className="space-y-4">
        {children}
      </div>
    </div>
  );
}

function FormField({ icon, label, type = 'text', value, onChange, disabled, required, placeholder, options, min, max }) {
  const inputClasses = `w-full px-4 py-2 rounded-lg border border-[#A4C3B2] focus:outline-none focus:ring-2 focus:ring-[#6B9080] disabled:bg-gray-50 disabled:text-gray-500 ${icon ? 'pl-11' : ''}`;

  return (
    <div>
      {label && (
        <label className="text-sm text-[#5A7A6B] mb-1 block">
          {label} {required && <span className="text-red-500">*</span>}
        </label>
      )}
      <div className="relative">
        {icon && (
          <div className="absolute left-3 top-1/2 -translate-y-1/2 text-[#5A7A6B]">
            {icon}
          </div>
        )}
        {type === 'select' ? (
          <select
            value={value}
            onChange={onChange}
            disabled={disabled}
            className={inputClasses}
          >
            {options?.map(opt => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        ) : (
          <input
            type={type}
            value={value}
            onChange={onChange}
            disabled={disabled}
            placeholder={placeholder}
            min={min}
            max={max}
            className={inputClasses}
          />
        )}
      </div>
    </div>
  );
}