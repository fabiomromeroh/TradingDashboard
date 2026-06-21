import { NavLink } from 'react-router-dom';
import {  ArrowLeftRight, Upload, Users } from 'lucide-react';

const navItems = [
  { to: '/users', icon: Users, label: 'Users' },
  { to: '/trades', icon: ArrowLeftRight, label: 'Trades' },
  { to: '/import', icon: Upload, label: 'Import' },
];

export default function Sidebar() {
  return (
    <aside className="w-56 h-screen bg-zinc-900 flex flex-col shrink-0">
      {/* Logo */}
      <div className="px-5 py-4 border-b border-zinc-800">
        <span className="text-white font-bold text-base tracking-tight">
          Trading <span className="text-blue-400">Dashboard</span>
        </span>
      </div>

      {/* Nav */}
      <nav className="flex-1 px-3 py-4 space-y-0.5">
        {navItems.map(({ to, icon: Icon, label }) => (
          <NavLink
            key={to}
            to={to}
            end={to === '/'}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors ${
                isActive
                  ? 'bg-blue-600 text-white'
                  : 'text-zinc-400 hover:bg-zinc-800 hover:text-zinc-100'
              }`
            }
          >
            <Icon size={16} />
            {label}
          </NavLink>
        ))}
      </nav>

      {/* Footer */}
      <div className="px-5 py-4 border-t border-zinc-800">
        <p className="text-xs text-zinc-500">v0.1.0</p>
      </div>
    </aside>
  );
}
