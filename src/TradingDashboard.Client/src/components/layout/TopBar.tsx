import { useLocation } from 'react-router-dom';

const pageTitles: Record<string, string> = {
  '/': 'Dashboard',
  '/users': 'Users',
  '/trades': 'Trades',
  '/import': 'Trading Accounts',
};

export default function TopBar() {
  const { pathname } = useLocation();
  const title = pageTitles[pathname] ?? 'Trading Dashboard';

  return (
    <header className="h-13 border-b border-zinc-200  flex items-center px-6 shrink-0">
      <h2 className="text-sm  text-zinc-700">{title}</h2>
    </header>
  );
}
