import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {Edit2, Save, Mail, Phone, MapPin, Award, Flame, Settings, ChevronRight, LogOut, Camera} from 'lucide-react';

export default function ProfilePage() {
  const navigate = useNavigate();

  
  const M = {
    primary: '#6B9080',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // User state
  const [user, setUser] = useState({
    name: 'roaaa',
    email: 'kumarbijayebehera07@gmail.com',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Bijaya',
    phone: '+962 79 123 4567',
    location: 'Amman, Jordan',
    joinDate: 'January 2024',
    studyStreak: 7,
     totalHours: 124,
     completedTasks: 45,
    bio: 'Computer Science student passionate about learning and personal growth.',
  });

  const [isEditing, setIsEditing] = useState(false);
  const [profileEdit, setProfileEdit] = useState({ ...user });
    const [selectedFile, setSelectedFile] = useState(null);
 
  useEffect(() => {
    setProfileEdit({ ...user });
  }, [user]);

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        setProfileEdit(prev => ({ ...prev, avatar: e.target.result }));
      };
      reader.readAsDataURL(file);
    }
  };

  const saveProfile = () => {
     setUser(profileEdit);
    setIsEditing(false);
    alert('Profile updated successfully!');
  };

   const handleLogout = () => {
     alert('Logged out successfully!');
 
  };

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen p-6">
      {/* Profile Header */}
      <div className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
        <div className="flex flex-col md:flex-row items-center gap-6">
          <div className="relative">
            <img
                src={profileEdit.avatar}
              alt="avatar"
                className="w-32 h-32 rounded-full border-4 shadow-lg"
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
            {isEditing ? (
                <div className="space-y-3">
                <input 
                  value={profileEdit.name} 
                  onChange={e => setProfileEdit(p => ({ ...p, name: e.target.value }))}
                  className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                  placeholder="Name"
                />
                <textarea
                  value={profileEdit.bio}
                  onChange={e => setProfileEdit(p => ({ ...p, bio: e.target.value }))}
                  className="w-full px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                  style={{ borderColor: M.bg3 }}
                  placeholder="Bio"
                  rows={2}
                />
              </div>
            ) : (
              <>
                <h2 className="text-2xl font-bold text-[#2C3E3F]">{user.name}</h2>
                <p className="text-[#5A7A6B] mt-1">{user.bio}</p>
              </>
            )}
             <div className="flex flex-wrap gap-3 mt-3 justify-center md:justify-start">
                 <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium flex items-center gap-2">
                   <Flame className="w-4 h-4 text-orange-500" />
                   {user.studyStreak} day streak
                </span>
                 <span className="px-3 py-1 rounded-full bg-[#F6FFF8] text-sm font-medium">
                Joined {user.joinDate}
                 </span>
               </div>
             </div>
              <div className="flex gap-2">
               {isEditing ? (
              <>
                <button 
                  onClick={saveProfile}
                  className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
                  style={{ background: M.primary }}
                >
                    <Save className="w-4 h-4" />
                     Save
                   </button>
                    <button 
                     onClick={() => setIsEditing(false)}
                     className="px-4 py-2 rounded-lg border font-medium hover:shadow-md transition-all"
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
      </div>

      {/* Contact Info */}
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
                   value={profileEdit.email} 
                   onChange={e => setProfileEdit(p => ({ ...p, email: e.target.value }))}
                   className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                 style={{ borderColor: M.bg3 }}
                  placeholder="Email"
               />
             </div>
              <div className="flex items-center gap-3">
                <Phone className="w-5 h-5 text-[#5A7A6B]" />
                 <input 
                  value={profileEdit.phone} 
                 onChange={e => setProfileEdit(p => ({ ...p, phone: e.target.value }))}
                 className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                    placeholder="Phone"
               />
               </div>
                <div className="flex items-center gap-3">
                 <MapPin className="w-5 h-5 text-[#5A7A6B]" />
                <input 
                   value={profileEdit.location} 
                  onChange={e => setProfileEdit(p => ({ ...p, location: e.target.value }))}
                 className="flex-1 px-4 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]" 
                  style={{ borderColor: M.bg3 }}
                  placeholder="Location"
                 />
               </div>
             </div>
           ) : (
            <div className="space-y-3">
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                <Mail className="w-5 h-5 text-[#5A7A6B]" />
                 <div>
                   <p className="text-xs text-[#5A7A6B]">Email</p>
                   <p className="font-medium text-[#2C3E3F]">{user.email}</p>
                 </div>
              </div>
              <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                 <Phone className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                  <p className="text-xs text-[#5A7A6B]">Phone</p>
                  <p className="font-medium text-[#2C3E3F]">{user.phone}</p>
                   </div>
            </div>
               <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
              <MapPin className="w-5 h-5 text-[#5A7A6B]" />
                <div>
                <p className="text-xs text-[#5A7A6B]">Location</p>
                  <p className="font-medium text-[#2C3E3F]">{user.location}</p>
              </div>
            </div>
            </div>
        )}
      </div>

      {/* Study Statistics */}
      <div className="bg-white rounded-2xl p-6 shadow-lg mt-6 border" style={{ borderColor: M.bg3 }}>
        <h3 className="font-bold text-[#2C3E3F] mb-4 flex items-center gap-2">
          <Award className="w-5 h-5" style={{ color: M.primary }} />
          Study Statistics
                </h3>
               <div className="grid grid-cols-2 gap-4">
           <div className="p-4 rounded-xl text-center" style={{ background: M.bg1 }}>
            <p className="text-3xl font-bold" style={{ color: M.primary }}>{user.totalHours}</p>
              <p className="text-sm text-[#5A7A6B]">Total Hours</p>
          </div>
              <div className="p-4 rounded-xl text-center" style={{ background: M.bg1 }}>
            <p className="text-3xl font-bold" style={{ color: M.primary }}>{user.completedTasks}</p>
                     <p className="text-sm text-[#5A7A6B]">Tasks Done</p>
          </div>
          <div className="p-4 rounded-xl text-center" style={{ background: M.bg1 }}>
             <p className="text-3xl font-bold" style={{ color: M.primary }}>{user.studyStreak}</p>
             <p className="text-sm text-[#5A7A6B]">Day Streak</p>
              </div>
        </div>
      </div>

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